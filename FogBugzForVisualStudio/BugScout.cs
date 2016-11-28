using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;

namespace FogBugzForVisualStudio
{
    class UrlParams : System.Collections.IEnumerable {
        private List<KeyValuePair<string, string>> parms = new List<KeyValuePair<string, string>>();

        public void Add(string name, string value) {
            parms.Add(new KeyValuePair<string, string>(name, value));
        }

        public override String ToString() {
            return String.Join("&", System.Array.ConvertAll(parms.ToArray(), (pair) => { 
                return String.Format("{0}={1}", Encode(pair.Key), Encode(pair.Value));
            }));
        }

        private String Encode(String s) {
            return HttpUtility.UrlEncode(s);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    class BugScout
    {
        public static void ReportException(Exception ex)
        {
            ReportException(ex, "");
        }

        public static void ReportException(Exception ex, String extra)
        {
            ReportError(ex.Message, String.Format(
                "[code]\n{0}\n[/code]\n\n{1}", 
                ex.ToString(), extra
            ));
        }

        public static void ReportError(String title)
        {
            ReportError(title, "");
        }

        public static void ReportError(String title, String extra){
            #if !DEBUG
                if (!Connect.ReportErrors)
                {
                    return;
                }
                // This is where errors can be reported to any system you prefer.
                // We use BugzScout, and have included some sample code to get
                // you started capturing error reports in FogBugz. You'll
                // need to update the URL and parameter values below. For more
                // information, please visit:
                // http://help.fogcreek.com/7566/bugzscout-for-automatic-crash-reporting
                //
                //var url = "https://your-site.fogbugz.com/scoutSubmit.asp";
                //var client = new WebClient();
                //client.DownloadStringAsync(new Uri(url + "?" + new UrlParams{
                //    {"ScoutUserName", "Your BugzScout User},
                //    {"ScoutProject", "Your BugzScout Project"},
                //    {"ScoutArea", "Your BugzScout Area"},
                //    {"description", "VS Add-In: " title},
                //    {"extra", extra}
                //}.ToString()));

            #endif
        }

        public delegate void Action();

        public static DownloadStringCompletedEventHandler CrashProof(DownloadStringCompletedEventHandler action)
        {
            return (a, b) =>
            {
                CatchAndReport(() => { 
                    action(a, b); 
                });
            };
        }

        public static void CatchAndReport(Action action){
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }
        }
    }
}
