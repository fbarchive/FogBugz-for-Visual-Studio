using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;


namespace FogBugzForVisualStudio.Api
{
    /// <summary>
    /// Event fires when Logon is done.
    /// </summary>
    /// <param name="bSuccess">true if successful</param>
    /// <param name="sError">specific error message, if unsuccessful</param>
    /// <param name="rgNames">if unsuccessful because sEmail was ambiguous, a list of full names user must choose between</param>
    public delegate void LogonHandler(bool bSuccess, string sError, StringCollection rgNames);

    /// <summary>
    /// Event fires when List is done.
    /// </summary>
    /// <param name="bSuccess">true if successful</param>
    /// <param name="sError">specific error message, if unsuccessful</param>
    /// <param name="sListDescription">e.g. "All open cases assigned to Sophia"</param>
    /// <param name="rgCases">the cases themselves</param>
    public delegate void ListCasesHandler(bool bSuccess, string sError, string sListDescription, List<Case> rgCasesInCurrentFilter, List<Case>rgParentAndSubcases);

    /// <summary>
    /// Event fires if user is suddenly discovered to have logged off
    /// (logon token no longer works)
    /// </summary>
    public delegate void UserLoggedOffHandler();

    /// <summary>
    /// Event fires when ListFilters is done
    /// </summary>
    /// <param name="bSuccess"></param>
    /// <param name="sError"></param>
    /// <param name="rgFilters"></param>
    public delegate void ListFiltersHandler(bool bSuccess, string sError, List<Filter> rgFilters);

    /// <summary>
    /// Event fires when ListIntervals is done
    /// </summary>
    /// <param name="bSuccess"></param>
    /// <param name="rgIntervals"></param>
    public delegate void ListIntervalsHandler(bool bSuccess, string sError, List<Interval> rgIntervals);

    /// <summary>
    /// Event fires *instead of* the event you were expecting
    /// (LogonHandler, ListCasesHandler, UserLoggedOffHandler, etc)
    /// to tell you that your Stop() request has been honored
    /// </summary>
    public delegate void AsyncOperationCancelled();

    /// <summary>
    /// A C# class that implements a client to the FogBugz version 1 API.
    /// </summary>
    public class FogBugzClient
    {
        private string _sEmail;
        private string _sPassword;
        private string _sToken;
        private WebClient _wc = null;
        private string _lastCertError = "";
        private bool _retryWithHttp = false;
        
        public string sToken
        {
            get { return _sToken; }
        }

        /// <summary>
        /// The URL of FogBugz itself
        /// </summary>
        private string _url = "" ;
        private string _params = "";

        private Dictionary<int, Person> dictPeople;
        private Dictionary<int, Status> dictStatuses;
        private Dictionary<int, Category> dictCategories;

        public virtual Person GetPerson(int ixPerson)
        {
            return (dictPeople.ContainsKey(ixPerson)) ? dictPeople[ixPerson] : null;
        }

        public virtual Status GetStatus(int ixStatus)
        {
            return (dictStatuses.ContainsKey(ixStatus)) ? dictStatuses[ixStatus] : null;
        }

        public virtual Category GetCategory(int ixCategory)
        {
            return (dictCategories.ContainsKey(ixCategory)) ? dictCategories[ixCategory] : null;
        }

        /// <summary>
        /// The complete url of the API, e.g. "http://www.example.com/fogbugz/api.asp?"
        /// Not set until CheckAPIVersion has succeeded
        /// </summary>
        private string _urlApi = "";

        public string sApiUrl
        {
            get { return _urlApi; }
        }

        public bool? fHasBacklogPlugin;

        public event LogonHandler OnLogon;
        public event ListCasesHandler OnListCases;
        public event UserLoggedOffHandler OnUserLoggedOff;
        public event ListFiltersHandler OnListFilters;
        public event ListIntervalsHandler OnListIntervals;
        public event AsyncOperationCancelled OnStop;

        public FogBugzClient()
        {
        }

        public FogBugzClient(string sApiUrl, string sToken)
        {
            _urlApi = sApiUrl;
            _sToken = sToken;

            StartPostLogonInit();
        }

        public bool LoggedOn()
        {
            return !String.IsNullOrEmpty(_sToken) && !String.IsNullOrEmpty(_urlApi)
                && dictPeople != null && dictStatuses != null && dictCategories != null;
        }

        /// <summary>
        /// Stops any operation in progress
        /// </summary>
        public void Stop()
        {
            if (_wc != null)
            {
                _wc.CancelAsync();
            }
        }
        
        /// <summary>
        /// Starts the logon process asynchronously. Fires LogonCompleted when done.
        /// </summary>
        /// <param name="url">FogBugz URL</param>
        /// <param name="sEmail">email address or full name if email address is ambiguous</param>
        /// <param name="sPassword">password</param>
        public void Logon( string url, string sEmail, string sPassword )
        {
            _url = url;
            _url = _url.Replace("default.asp", "");
            _url = _url.Replace("default.php", "");
            if (_url.Contains("?"))
            {
                _params = _url.Substring(_url.IndexOf("?") + 1);
                _url = _url.Substring(0, _url.IndexOf("?"));
            }

            if (!_url.EndsWith("/")) _url += "/";

            _sEmail = sEmail;
            _sPassword = sPassword;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = OnRemoteCertificateValidation;

            if (Regex.IsMatch(_url, @"http://[^.]+\.fogbugz\.com/"))
            {
                _url = "https://" + _url.Substring("http://".Length);
                _retryWithHttp = true;
            }

            CheckAPIVersion();
        }

        /// <summary>
        /// Check that the server is really a FogBugz server with the right version of the API
        /// installed
        /// </summary>
        public void CheckAPIVersion()
        {
            InitiateDownload("", CheckAPIVersion_DownloadStringCompleted,
                             delegate(string sErrorMessage) { OnLogon(false, sErrorMessage, null); });
        }

        private void CheckAPIVersion_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error == null)
            {
                int iVersion = 0;
                int iMinVersion = 0;
                int iClientVersionOnServer = 0;
                string sURL = "";
                XmlReader xml;

                try
                {
                    xml = GetXmlReader(e.Result);
                }
                catch 
                {
                    var fFBOD = _url.EndsWith(".fogbugz.com/");
                    var msg = fFBOD ?
                        "FogBugz On Demand is currently undergoing maintenance." :
                        "Malformed api.xml.";

                    OnLogon(false, msg, null);
                    return;
                }
                

                bool fContinue = xml.Read();
                while (fContinue)
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        switch (xml.Name)
                        {
                            case "version":
                                iVersion = xml.ReadElementContentAsInt();
                                break;

                            case "minversion":
                                iMinVersion = xml.ReadElementContentAsInt();
                                break;

                            case "url":
                                sURL = xml.ReadElementContentAsString();
                                break;

                            case "private":
                                if (xml.GetAttribute("id") == "FogBugz for Visual Studio")
                                {
                                    iClientVersionOnServer = Convert.ToInt32(xml.GetAttribute("version"));
                                }
                                fContinue = xml.Read();
                                break;

                            default:
                                fContinue = xml.Read();
                                break;
                        }
                    }
                    else
                    {
                        fContinue = xml.Read();
                    }
                }

                if (iVersion == 0 || iMinVersion == 0 || sURL.Length == 0)
                {
                    OnLogon(false, "Incomplete or missing api.xml", null);
                }
                else if (iMinVersion > 1)
                {
                    OnLogon(false, "That version of FogBugz is too new. Please re-install from the \"Extras\" menu in FogBugz", null);
                }
                else if (iClientVersionOnServer > 2)
                {
                    OnLogon(false, "Newer version of add-in available. Please re-install from the \"Extras\" menu in FogBugz", null);
                }
                else
                {
                    _urlApi = _url + sURL;
                    StartLogon();
                }
            }
            else
            {
                if (_retryWithHttp && Regex.IsMatch(_url, @"https://[^.]+\.fogbugz\.com/"))
                {
                    _url = "http://" + _url.Substring("https://".Length);
                    _retryWithHttp = false;
                    CheckAPIVersion();
                }
                else if (e.Error is WebException)
                {
                    WebException we = (WebException)e.Error;
                    if (we.Status == WebExceptionStatus.TrustFailure)
                    {
                        // we can get better error messages for SSL failures.
                        OnLogon(false, _lastCertError, null);
                    }
                    else if (e.Error.Message.Contains("404"))
                    {
                        OnLogon(false, e.Error.Message + " FogBugz API not installed on server.", null);
                    }
                    else
                    {
                        OnLogon(false, e.Error.Message, null);
                    }
                }
                else
                {
                    OnLogon(false, e.Error.Message, null);
                }
            }
        }

        /// <summary>
        /// Passed version check; start logging on
        /// </summary>
        private void StartLogon()
        {
            _sToken = ""; // clear any old tokens - otherwise logon won't happen if we're passing an invalid token
            InitiateDownload( UrlParams("cmd","logon","email",_sEmail,"password",_sPassword),
                              StartLogon_DownloadStringCompleted,
                              delegate(string sErrorMessage) { OnLogon(false, sErrorMessage, null); } );
        }

        private void StartLogon_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnLogon(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result.Trim());

            bool fAmbiguousLogon = false;
            StringCollection rgsFullNames = new StringCollection();
            string sErrAmbiguous = "";

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element)
                {
                    xml.Read();
                    continue;
                }
                
                switch (xml.Name)
                {
                    case "token":
                        _sToken = xml.ReadElementContentAsString();
                        
                        // We've successfully logged on, so download a list of people.
                        StartPostLogonInit();

                        return;

                    case "error":
                        switch (xml.GetAttribute("code"))
                        {
                            case "1":
                                OnLogon(false, xml.ReadElementContentAsString(), null);
                                return;
                            case "2":
                                sErrAmbiguous = xml.ReadElementContentAsString();
                                fAmbiguousLogon = true;
                                continue;
                            default:
                                OnLogon(false, "Unknown Error: " + xml.ReadElementContentAsString(), null);
                                return;
                        }

                    case "person":
                        rgsFullNames.Add(xml.ReadElementContentAsString());
                        break;

                    default:
                        xml.Read();
                        break;
                }
            }

            if (fAmbiguousLogon)
            {
                OnLogon(false, sErrAmbiguous, rgsFullNames);
            }
            else
            {
                OnLogon(false, "Unknown error; unrecognized XML response.", null);
            }
        }

        private void StartPostLogonInit()
        {
            InitiateDownload(UrlParams("cmd", "listPeople", "fIncludeNormal", "1", "fIncludeVirtual", "1"),
                              ListPeople_DownloadStringCompleted,
                              delegate(string s) { OnLogon(false, "Unable to download list of people.", null); });
        }

        private void ListPeople_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnLogon(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);

            dictPeople = new Dictionary<int, Person>();

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element || xml.Name != "person")
                {
                    xml.Read();
                    continue;
                }

                try
                {
                    var fields = ReadSubElementsAsDictionary(xml);
                    var person = new Person(fields);
                    dictPeople[person.ixPerson] = person;
                }
                catch (Exception ex)
                {
                    BugScout.ReportException(ex);
                    OnLogon(false, "API returned invalid <person>", null);
                    return;
                }
            }

            InitiateDownload(UrlParams("cmd", "listStatuses"),
                              ListStatuses_DownloadStringCompleted,
                              delegate(string s) { OnLogon(false, "Unable to download a list of statuses", null); });
        }

        private void ListStatuses_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnLogon(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);

            dictStatuses = new Dictionary<int, Status>();

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element || xml.Name != "status")
                {
                    xml.Read();
                    continue;
                }

                try
                {
                    var fields = ReadSubElementsAsDictionary(xml);
                    var status = new Status(fields);
                    dictStatuses[status.ixStatus] = status;
                }
                catch (Exception ex)
                {
                    BugScout.ReportException(ex);
                    OnLogon(false, "API returned invalid <status>", null);
                    return;
                }
            }

            InitiateDownload(UrlParams("cmd", "listCategories"),
                              ListCategories_DownloadStringCompleted,
                              delegate(string s) { OnLogon(false, "Unable to download a list of categories", null); });
        }

        private void ListCategories_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnLogon(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);

            dictCategories = new Dictionary<int, Category>();

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element || xml.Name != "category")
                {
                    xml.Read();
                    continue;
                }

                try
                {
                    var fields = ReadSubElementsAsDictionary(xml);
                    var category = new Category(fields);
                    dictCategories[category.ixCategory] = category;
                }
                catch (Exception ex)
                {
                    BugScout.ReportException(ex);
                    OnLogon(false, "API returned invalid <category>", null);
                    return;
                }
            }

            OnLogon(true, "", null);
        }

        public void LogOff()
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            InitiateDownload(UrlParams("cmd", "logoff"),
                             null,
                             null);
            _sToken = "";
        }

        public void SetCaseEstimate(Case c, string estimate)
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();

            var response = SynchronousDownload(UrlParams("cmd", "edit", "ixBug", c.ixBug.ToString(), "hrsCurrEst", estimate, "cols", "hrsCurrEst"));

            XmlReader xml = GetXmlReader(response);

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType == XmlNodeType.Element && xml.Name == "hrsCurrEst")
                {
                    c.SetNewEstimate(new Estimate(xml.ReadElementContentAsDecimal()));
                    return;
                }
                else if (xml.NodeType == XmlNodeType.Element && xml.Name == "error")
                {
                    throw new FogBugzErrorException(xml.ReadElementContentAsString(), Convert.ToInt32(xml.GetAttribute("code")));
                }
                else xml.Read();
            }
        }

        private void CheckForErrors(string response)
        {
            var xml = GetXmlReader(response);
            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType == XmlNodeType.Element && xml.Name == "error")
                {
                    throw new FogBugzErrorException(xml.ReadElementContentAsString(), Convert.ToInt32(xml.GetAttribute("code")));
                }
                else xml.Read();
            }
        }

        public void StartWork(Case c)
        {
            StartWork(c.ixBug);
            c.WorkStarted();
        }

        public void StartWork(int ixBug)
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            CheckForErrors(SynchronousDownload(UrlParams("cmd", "startWork", "ixBug", ixBug.ToString())));
        }

        public void StopWork()
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            CheckForErrors(SynchronousDownload(UrlParams("cmd", "stopWork")));
        }

        public void ListIntervals(DateTime? dtStart, DateTime? dtEnd)
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            InitiateDownload(UrlParams("cmd", "listIntervals", "cols", sListCols, "dtStart", dtStart.HasValue ? Util.FormatApiDate(dtStart.Value) : "", "dtEnd", dtEnd.HasValue ? Util.FormatApiDate(dtEnd.Value) : ""),
                              delegate(Object sender, DownloadStringCompletedEventArgs e) { ListIntervals_DownloadStringCompleted(sender, e); },
                              delegate(string s) { OnListIntervals(false, s, null); });
        }

        private void ListIntervals_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnListIntervals(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);

            var rgIntervals = new List<Interval>();

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element)
                {
                    xml.Read();
                    continue;
                }
                switch (xml.Name)
                {
                    case "error":
                        if (xml.GetAttribute("code") == "3")
                        {
                            OnListIntervals(false, xml.ReadElementContentAsString(), null);
                            OnUserLoggedOff();
                        }
                        else
                        {
                            OnListIntervals(false, xml.ReadElementContentAsString(), null);
                        }
                        return;

                    case "interval":
                        try
                        {
                            rgIntervals.Add(new Interval(ReadSubElementsAsDictionary(xml), this));
                        }
                        catch (Exception ex)
                        {
                            BugScout.ReportException(ex);
                            OnListIntervals(false, "API returned invalid <interval>", null);
                            return;
                        }
                        break;

                    default:
                        xml.Read();
                        break;
                }
            }

            OnListIntervals(true, "", rgIntervals);
        }

        private static string sListCols = "ixBug,ixBugParent,ixBugChildren,sTitle,ixProject,sProject,ixArea,sArea,ixPersonAssignedTo,ixPersonOpenedBy,ixPersonResolvedBy,ixPersonClosedBy,ixPersonLastEditedBy,ixStatus,ixPriority,sPriority,ixFixFor,sFixFor,hrsOrigEst,hrsCurrEst,hrsElapsed,ixCategory,dtOpened,dtResolved,dtClosed,dtLastUpdated,dtDue,ixBugEventLatest,ixBugEventLastView,plugin";

        /// <summary>
        /// Starts downloading a list of cases in the user's current filter.
        /// </summary>
        public void ListCases()
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            InitiateDownload( UrlParams("cmd", "search", "cols", sListCols),
                              delegate(Object sender, DownloadStringCompletedEventArgs e) { ListCases_DownloadStringCompleted(sender, e, null, null, null); },
                              delegate(string s) { OnListCases(false, s, null, null, null); });
        }

        /// <summary>
        /// Responds to a payload from a list cases command. This may be the main call from ListCases, listing the cases in the 
        /// user's current filter, or from a followup call that is resolving subcases and parent cases. The two cases can be
        /// differentiated by rgCasesInCurrentFilter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="rgCasesInCurrentFilter">If null, the payload contains list of cases in current filter. Otherwise, payload contains assorted parent cases and subcases for the cases in this array.</param>
        /// <param name="sDescription">If null, fill in from payload. Otherwise this is a supplementary call loading parent and subcases, and the value is already loaded.</param>
        /// <param name="rgParentAndSubcases">It may take multiple passes to get a complete tree of cases (resolve all parent and subcases). If so, this list holds an intermediate array.</param>
        private void ListCases_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e, List<Case> rgCasesInCurrentFilter, string sDescription, List<Case> rgParentAndSubcases)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnListCases(false, e.Error.Message, null, null, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);

            if (rgParentAndSubcases == null)
            {
                rgParentAndSubcases = new List<Case>();
            }

            List<Case> rgCaseDestination;
            if (rgCasesInCurrentFilter == null)
            {
                rgCasesInCurrentFilter = new List<Case>();
                rgCaseDestination = rgCasesInCurrentFilter;
            }
            else
            {
                rgCaseDestination = rgParentAndSubcases;
            }

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType != XmlNodeType.Element)
                {
                    xml.Read();
                    continue;
                }
                switch (xml.Name)
                {
                    case "error":
                        if (xml.GetAttribute("code") == "3")
                        {
                            OnListCases(false, xml.ReadElementContentAsString(), null, null, null);
                            OnUserLoggedOff();
                        }
                        else
                        {
                            OnListCases(false, xml.ReadElementContentAsString(), null, null, null);
                        }
                        return;

                    case "description":
                        sDescription = xml.ReadElementContentAsString();
                        break;

                    case "case":
                        try
                        {
                            var ixBug = Convert.ToInt32(xml.GetAttribute("ixBug"));
                            var ops = (Case.Op)Enum.Parse(typeof(Case.Op), xml.GetAttribute("operations"));
                            var els = ReadSubElementsAsDictionary(xml);
                            fHasBacklogPlugin = els.ContainsKey(Case.BacklogFieldName);
                            rgCaseDestination.Add(new Case(ixBug, ops, els, this));
                        }
                        catch (Exception ex)
                        {
                            BugScout.ReportException(ex);
                            OnListCases(false, "API returned invalid <case>", null, null, null);
                            return;
                        }
                        break;

                    default:
                        xml.Read();
                        break;
                }
            }

            var rgIxBugUnresolved = ComputeUnresolvedCases(rgCasesInCurrentFilter, rgParentAndSubcases);
            if (rgIxBugUnresolved.Count > 0)
            {
                string sIxBugQuery = "";
                foreach (int ixBug in rgIxBugUnresolved)
                {
                    sIxBugQuery += "," + Convert.ToString(ixBug);
                }
                sIxBugQuery = sIxBugQuery.Substring(1);

                Debug.WriteLine("Requesting bugs " + sIxBugQuery);

                InitiateDownload( UrlParams("cmd", "search", "cols", sListCols, "q", sIxBugQuery),
                              delegate(Object sender2, DownloadStringCompletedEventArgs e2) { ListCases_DownloadStringCompleted(sender2, e2, rgCasesInCurrentFilter, sDescription, rgParentAndSubcases); },
                              delegate(string s) { OnListCases(false, s, null, null, null); });
            }
            else
            {
                OnListCases(true, "", sDescription, rgCasesInCurrentFilter, rgParentAndSubcases);
            }
        }

        private List<int> ComputeUnresolvedCases(List<Case> rgCasesInCurrentFilter, List<Case> rgParentAndSubcases)
        {
            var dictAllCases = new Dictionary<int, Case>();

            var queue = new Queue<Case>();
            foreach (var c in rgCasesInCurrentFilter) 
            {
                queue.Enqueue(c);
                dictAllCases[c.ixBug] = c;
            }
            foreach (var c in rgParentAndSubcases)
            {
                queue.Enqueue(c);
                dictAllCases[c.ixBug] = c;
            }

            // Dictionary with a dummy value: the poor man's Set class.
            var dictUnresolved = new Dictionary<int, int>();
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.ixBugParent.HasValue && !dictAllCases.ContainsKey((int)c.ixBugParent)) dictUnresolved[(int)c.ixBugParent] = 0;
                foreach (var ixBugChild in c.ixBugChildren)
                {
                    if (!dictAllCases.ContainsKey(ixBugChild)) dictUnresolved[ixBugChild] = 0;
                }
            }

            var rgUnresolved = new List<int>();
            foreach (var ixBug in dictUnresolved.Keys)
            {
                rgUnresolved.Add(ixBug);
            }
            return rgUnresolved;
        }

        private Dictionary<String,String> ReadSubElementsAsDictionary(XmlReader xml)
        {
            var fields = new Dictionary<String, String>();

            if (xml.NodeType != XmlNodeType.Element)
            {
                throw new InvalidOperationException("ReadSubElementsAsDictionary was called when not positioned on an element");
            }

            String elementName = xml.Name;

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == elementName)
                {
                    xml.Read(); // Advance past end element, just like ReadElementContentAsString
                    return fields;
                }

                if (xml.NodeType == XmlNodeType.Element)
                    fields[xml.Name] = xml.ReadElementContentAsString();
                else
                    xml.Read();
            }

            throw new XmlException("ReadSubElementsAsDictionary reached EOF without reaching end of element.");
        }

        public void ListFilters()
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            InitiateDownload(UrlParams("cmd", "listFilters"),
                             ListFilters_DownloadStringCompleted,
                             delegate(string s) { OnListFilters(false, s, null); });
                              
        }

        private void ListFilters_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error != null)
            {
                OnListFilters(false, e.Error.Message, null);
                return;
            }

            XmlReader xml = GetXmlReader(e.Result);
            List<Filter> rgFilters = new List<Filter>();

            xml.Read();
            while (!xml.EOF)
            {
                if (xml.NodeType == XmlNodeType.Element && xml.Name == "filter")
                {
                    Filter f = new Filter();
                    f.codeFilter = xml.GetAttribute("sFilter");
                    f.type = (Filter.FilterType)Enum.Parse(typeof(Filter.FilterType), xml.GetAttribute("type"));
                    f.fIsCurrent = (xml.GetAttribute("status") == "current");
                    f.s = xml.ReadElementContentAsString().Trim();
                    rgFilters.Add(f);
                }
                else
                {
                    xml.Read();
                }
            }
            OnListFilters(true, "", rgFilters);
        }

        public void SaveFilterAndList(string codeFilter)
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            InitiateDownload(UrlParams("cmd", "setCurrentFilter", "sFilter", codeFilter),
                             SaveFilter_DownloadStringCompleted,
                             delegate(string s) { OnListCases(false, s, null, null, null); });
                             
        }

        private void SaveFilter_DownloadStringCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnStop();
                return;
            }

            if (e.Error == null)
            {
                ListCases();
            }
        }

        public string GetDefaultUrl()
        {
            if (!LoggedOn()) throw new FogBugzNotLoggedOnException();
            return _urlApi.Replace("api.", "default.");
        }

        //
        //  UTILITY FUNCTIONS
        //
        
        private delegate void InitiateDownloadFailureHandler( string sErrorMessage );

        /// <summary>
        /// Initiates a download.
        /// </summary>
        /// <param name="sParams">GET params, for example, "cmd=logoff"; use UrlParams to generate. If blank, downloads api.xml.</param>
        /// <param name="h">handler to call when string is received</param>
        /// <param name="hFail">handler to call if exception is raised even before download begins</param>
        private void InitiateDownload(string sParams, 
                                      DownloadStringCompletedEventHandler h,
                                      InitiateDownloadFailureHandler hFail
                                      )
        {
            _wc = new WebClient();
            _wc.UseDefaultCredentials = true;
            _wc.Proxy.Credentials = CredentialCache.DefaultCredentials;
            _wc.Encoding = Encoding.UTF8;
            
            if (h != null)
            {
                _wc.DownloadStringCompleted += BugScout.CrashProof(h);
            }

            try
            {
                string surl = "";
                if (sParams.Length == 0)
                {
                    surl = _url + "api.xml?" + _params;
                }
                else
                {
                    surl = _urlApi;
                    if (_params.Length > 0)
                    {
                        surl += _params + "&";
                    }
                    surl += sParams;
                }

                _wc.DownloadStringAsync(new Uri(surl));
            }
            catch(Exception e)
            {
                BugScout.ReportException(e);
                if (hFail != null)
                {
                    BugScout.CatchAndReport(() => {
                        hFail(e.Message);
                    });
                }
            }
        }

        private string SynchronousDownload(string sParams)
        {
            _wc = new WebClient();
            _wc.UseDefaultCredentials = true;
            _wc.Proxy.Credentials = CredentialCache.DefaultCredentials;
            _wc.Encoding = Encoding.UTF8;

            string surl = _urlApi;
            if (_params.Length > 0)
            {
                surl += _params + "&";
            }
            surl += sParams;

            return _wc.DownloadString(new Uri(surl));
        }

        private string UrlParams(params string[] rgs)
        {
            Debug.Assert(rgs.Length % 2 == 0);

            string s = "token=" + HttpUtility.UrlEncode(_sToken);
            for (int i = 0; i < rgs.Length; i += 2)
            {
                s += "&" + HttpUtility.UrlEncode(rgs[i]) + "=" + HttpUtility.UrlEncode(rgs[i + 1]);
            }
            return s;
        }

        private XmlReader GetXmlReader(string sxml)
        {
            XmlReaderSettings st = new XmlReaderSettings();
            st.IgnoreComments = true;
            st.IgnoreWhitespace = true;
            st.ConformanceLevel = ConformanceLevel.Fragment;
            st.ValidationType = ValidationType.None;

            XmlReader xml = XmlReader.Create(new StringReader(sxml.Trim()), st);  // Trim() removes BOM so <?xml is at the beginning

            xml.MoveToContent();

            return xml;
        }

        public bool OnRemoteCertificateValidation(Object sender,
                                                  X509Certificate certificate,
                                                  X509Chain chain,
                                                  SslPolicyErrors sslPolicyErrors)
        {
            switch (sslPolicyErrors)
            {
                case SslPolicyErrors.None:
                    return true;

                case SslPolicyErrors.RemoteCertificateChainErrors:
                    _lastCertError = "SSL Error: " + chain.ChainElements[0].ChainElementStatus[0].StatusInformation.Trim();
                    break;

                case SslPolicyErrors.RemoteCertificateNameMismatch:
                    _lastCertError = "SSL Error: The certificate name does not match the name in the URL.";
                    break;

                case SslPolicyErrors.RemoteCertificateNotAvailable:
                    _lastCertError = "SSL Error: The remote certificate is not available.";
                    break;
            }

            return false;
        }
    }

    class FogBugzNotLoggedOnException : Exception { }

    class FogBugzErrorException : Exception
    {
        public int Code { get; private set; }

        public FogBugzErrorException(string msg, int code) : base(msg)
        {
            Code = code;
        }
    }
}
