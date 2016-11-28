using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using FogBugzForVisualStudio.Api;
using System.Runtime.Serialization.Formatters.Binary;
using System.Resources;

namespace FogBugzForVisualStudio
{
    public partial class FogBugzCtl : UserControl
    {
        /// <summary>
        /// Event fires when the control wants its container to show a URL.
        /// </summary>
        /// <param name="sURL"></param>
        public delegate void ShowUrlHandler(string sURL);
        public event ShowUrlHandler OnShowUrl;

        private FogBugzClient fb;
        private string sURL;
        private string sUser;
        private FogBugzUIState state = FogBugzUIState.LoggedOff;
        private string sFilterLableText = "";

        private bool settingsLoaded;

        private ToolStripMenuItem toolReply, toolEdit, toolAssign, toolResolve, toolClose, toolAddSubcase, toolReactivate, toolReopen, toolWorkOn;

        public FogBugzCtl()
        {
            InitializeComponent();

            sURL = RegistryHelper.FogBugzVSKey.GetValue("URL") as string;
            sUser = RegistryHelper.FogBugzVSKey.GetValue("User") as string;

            var sApiUrl = RegistryHelper.FogBugzVSKey.GetValue("ApiUrl") as string;
            var sToken = RegistryHelper.FogBugzVSKey.GetValue("Token") as string;

            if (!String.IsNullOrEmpty(sURL) && !String.IsNullOrEmpty(sToken))
            {
                fb = new FogBugzClient(sApiUrl, sToken);
                // Although the token was saved, we are not logged on yet. 
                // People, statuses, categories need to be downloaded.
                // We'll get a LoggedOn event when that is complete.
                State(FogBugzUIState.LoggingOn);
            }
            else
            {
                fb = new FogBugzClient();
                State(FogBugzUIState.LoggedOff);
            }

            fb.OnLogon += LoggedOn;
            fb.OnListIntervals += ListIntervalsResult;
            fb.OnListCases += ListCasesResult;
            fb.OnUserLoggedOff += UserLoggedOff;
            fb.OnListFilters += ListFiltersResult;
            fb.OnStop += Stopped;

            grid.AutoGenerateColumns = false;

            // Restore columns
            try
            {
                var str = new MemoryStream((byte[])RegistryHelper.FogBugzVSKey.GetValue("Columns"));
                var visibleColumns = (new BinaryFormatter()).Deserialize(str) as List<KeyValuePair<string, int>>;
                if (visibleColumns != null)
                {
                    foreach (DataGridViewColumn col in grid.Columns)
                    {
                        col.Visible = false;
                    }

                    int i = 0;
                    foreach (var c in visibleColumns)
                    {
                        DataGridViewColumn col = grid.Columns[c.Key];
                        col.Visible = true;
                        col.DisplayIndex = i++;
                        col.Width = c.Value;
                    }
                }
            }
            catch
            {
                // Ignore errors.
            }

            var gridMenu = new ContextMenuStrip();
            gridMenu.Items.Add(showColumnsMenu());
            gridMenu.Items.Add(new ToolStripSeparator());

            // Case actions.
            gridMenu.Items.Add(toolReply = new ToolStripMenuItem("Reply", null, toolReply_Click));
            gridMenu.Items.Add(toolEdit = new ToolStripMenuItem("Edit", null, toolEdit_Click));
            gridMenu.Items.Add(toolAssign = new ToolStripMenuItem("Assign", null, toolAssign_Click));
            gridMenu.Items.Add(toolResolve = new ToolStripMenuItem("Resolve", null, toolResolve_Click));
            gridMenu.Items.Add(toolClose = new ToolStripMenuItem("Close", null, toolClose_Click));
            gridMenu.Items.Add(toolAddSubcase = new ToolStripMenuItem("Add Subcase", null, toolAddSubcase_Click));
            gridMenu.Items.Add(toolReactivate = new ToolStripMenuItem("Reactivate", null, toolReactivate_Click));
            gridMenu.Items.Add(toolReopen = new ToolStripMenuItem("Reopen", null, toolReopen_Click));
            gridMenu.Items.Add(new ToolStripSeparator());
            gridMenu.Items.Add(toolWorkOn = new ToolStripMenuItem("Work On", null, toolWorkOn_Click));

            grid.ContextMenuStrip = gridMenu;

            settingsLoaded = true;
        }

        private ToolStripMenuItem showColumnsMenu()
        {
            var showColumnsMenu = new ToolStripMenuItem("Show Columns");

            var columnMenuGroups = new Dictionary<String, List<String>>();
            columnMenuGroups["Date"] = new List<String> { "dtClosed", "dtOpened", "dtResolved", "dtDue", "dtLastUpdated" };
            columnMenuGroups["Estimate"] = new List<String> { "hrsElapsed", "hrsCurrEst", "hrsOrigEst", "hrsRemaining" };
            columnMenuGroups["Person"] = new List<String> { "ixPersonAssignedTo", "ixPersonClosedBy", "ixPersonLastEditedBy", "ixPersonOpenedBy", "ixPersonResolvedBy" };

            // Construct a list of all the columns that are in groups
            var allGroupedColumns = new List<string>();
            foreach (var columnList in columnMenuGroups.Values)
            {
                allGroupedColumns.AddRange(columnList);
            }

            // Get a list of all columns that *aren't* in groups
            var menuTopLevel = new List<ColumnOrGroupWrapper>();
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (!allGroupedColumns.Contains(col.Name))
                {
                    menuTopLevel.Add(new ColumnOrGroupWrapper(col));
                }
            }
            foreach (var groupName in columnMenuGroups.Keys)
            {
                menuTopLevel.Add(new ColumnOrGroupWrapper(groupName));
            }
            menuTopLevel.Sort();

            foreach (ColumnOrGroupWrapper col in menuTopLevel)
            {
                if (col.ColumnOrGroup is string)
                {
                    // We've reached a group. Add an item for the header:
                    var m = new ToolStripMenuItem((string)col.ColumnOrGroup);
                    m.Font = new Font(m.Font, FontStyle.Bold);
                    m.Enabled = false;
                    showColumnsMenu.DropDownItems.Add(m);

                    foreach (string colName in columnMenuGroups[(string)col.ColumnOrGroup])
                    {
                        var item = menuItemForColumn(grid.Columns[colName]);
                        item.Text = "      " + item.Text; // Ugly hack, but it works.
                        showColumnsMenu.DropDownItems.Add(item);
                    }
                }
                else
                {
                    showColumnsMenu.DropDownItems.Add(menuItemForColumn((DataGridViewColumn)col.ColumnOrGroup));
                }
            }

            return showColumnsMenu;
        }

        // We want to override certain menu items. Notably, the Category menu item doesn't have HeaderText since it's only 18px wide.
        private static Dictionary<string, string> columnMenuItemOverride = new Dictionary<string, string> { { "ixCategory", "Category" } };

        private ToolStripMenuItem backlogMenuItem;

        private ToolStripMenuItem menuItemForColumn(DataGridViewColumn col)
        {
            var text = col.HeaderText;
            if (columnMenuItemOverride.ContainsKey(col.Name)) text = columnMenuItemOverride[col.Name];

            var m = new ToolStripMenuItem(text);
            m.Checked = col.Visible;
            m.Tag = col;
            m.Click += showColumn_Click;
            if (col.Name == "iBacklog") backlogMenuItem = m;
            return m;
        }

        private class ColumnOrGroupWrapper : IComparable<ColumnOrGroupWrapper>
        {
            public object ColumnOrGroup { get; private set; }

            public ColumnOrGroupWrapper(DataGridViewColumn col)
            {
                ColumnOrGroup = col;
            }

            public ColumnOrGroupWrapper(string groupName)
            {
                ColumnOrGroup = groupName;
            }

            public int CompareTo(ColumnOrGroupWrapper other)
            {
                return this.ToString().CompareTo(other.ToString());
            }

            public override string ToString()
            {
                if (ColumnOrGroup is string)
                {
                    return (string)ColumnOrGroup;
                }
                else if (ColumnOrGroup is DataGridViewColumn)
                {
                    var col = (DataGridViewColumn)ColumnOrGroup;
                    if (columnMenuItemOverride.ContainsKey(col.Name)) return columnMenuItemOverride[col.Name];
                    else return col.HeaderText;
                }
                throw new InvalidOperationException("ColumnOrGroupWrapper has an object that is neither a group nor a column.");
            }
        }

        void showColumn_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem m = (ToolStripMenuItem)sender;
                DataGridViewColumn col = (DataGridViewColumn)m.Tag;
                m.Checked = !m.Checked;
                col.Visible = m.Checked;
            }
            catch (Exception ex) 
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void toolRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                State(FogBugzUIState.Busy);
                fb.ListCases();
                UpdateIntervals();
            }
            catch (Exception ex) 
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void SaveSettings()
        {
            if (!settingsLoaded) return;

            RegistryHelper.FogBugzVSKey.SetValue("URL", sURL ?? "");
            RegistryHelper.FogBugzVSKey.SetValue("User", sUser ?? "");

            RegistryHelper.FogBugzVSKey.SetValue("ApiUrl", fb.sApiUrl ?? "");
            RegistryHelper.FogBugzVSKey.SetValue("Token", fb.sToken ?? "");

            var formatter = new BinaryFormatter();
            MemoryStream str;

            if (Connector != null)
            {
                if (Connector.SortField != null)
                {
                    // Convert from DataPropertyName to Name
                    string columnName = null;
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        if (grid.Columns[i].DataPropertyName == Connector.SortField)
                        {
                            columnName = grid.Columns[i].Name;
                            break;
                        }
                    }
                    if (!String.IsNullOrEmpty(columnName))
                    {
                        RegistryHelper.FogBugzVSKey.SetValue("SortColumn", columnName);
                    }
                }
                else
                {
                    RegistryHelper.FogBugzVSKey.DeleteValue("SortColumn", false);
                }
                RegistryHelper.FogBugzVSKey.SetValue("SortOrder", Connector.SortOrder);

                str = new MemoryStream();
                formatter.Serialize(str, Connector.GetCasesExpanded());
                RegistryHelper.FogBugzVSKey.SetValue("CasesExpanded", str.ToArray(), Microsoft.Win32.RegistryValueKind.Binary);

                str = new MemoryStream();
                formatter.Serialize(str, Connector.GetCasesShowingSubcases());
                RegistryHelper.FogBugzVSKey.SetValue("CasesShowingSubcases", str.ToArray(), Microsoft.Win32.RegistryValueKind.Binary);
            }

            // Persist column state
            DataGridViewColumn col = grid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            var visibleColumns = new List<KeyValuePair<string, int>>();
            while (col != null)
            {
                visibleColumns.Add(new KeyValuePair<string, int>(col.Name, col.Width));
                col = grid.Columns.GetNextColumn(col, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
            }

            str = new MemoryStream();
            formatter.Serialize(str, visibleColumns);
            RegistryHelper.FogBugzVSKey.SetValue("Columns", str.ToArray(), Microsoft.Win32.RegistryValueKind.Binary);
        }

        private void Stopped()
        {
            State(FogBugzUIState.Normal);
        }

        private void LoggedOn(bool fSuccess, string message, StringCollection rgsNames)
        {
            if (fSuccess)
            {
                State(FogBugzUIState.Busy);
                fb.ListCases();
                UpdateIntervals();
                timUpdateWorkingOn.Enabled = true;
                SaveSettings(); // Save token and user
            }
            else
            {
                State(FogBugzUIState.LoggedOff);
                ShowLogOnWindow(message, rgsNames);
            }
        }

        private void UserLoggedOff()
        {
            timUpdateWorkingOn.Enabled = false;
            State(FogBugzUIState.LoggedOff);
            SaveSettings(); // In case this was an unprompted log off
        }

        private List<ToolStripMenuItem> recentCasesMenuItems;

        private void ListIntervalsResult(bool bSuccess, string sError, List<Interval> rgIntervals)
        {
            if (!bSuccess) return; // Fail silently

            var rgRecent = new List<Interval>();

            var foundCurrent = false;
            foreach (var interval in rgIntervals)
            {
                if (!interval.fDeleted && interval.dtStart.HasValue && !interval.dtEnd.HasValue)
                {
                    // Found current case
                    UpdateWorkingOn(interval.ixBug, interval.sTitle);
                    foundCurrent = true;
                }
                else if (!interval.fDeleted && interval.dtStart.HasValue && interval.dtEnd.HasValue)
                {
                    rgRecent.Add(interval);
                }
            }
            if (!foundCurrent)
            {
                UpdateWorkingOn(null, null);
            }

            rgRecent.Sort(delegate(Interval i1, Interval i2) { return i1.dtEnd.Value.CompareTo(i2.dtEnd.Value); });
            rgRecent.Reverse();

            if (recentCasesMenuItems != null)
            {
                foreach (ToolStripMenuItem item in recentCasesMenuItems)
                {
                    recentlyWorkedOnToolStripMenuItem.DropDownItems.Remove(item);
                    item.Dispose();
                }
            }
            recentCasesMenuItems = new List<ToolStripMenuItem>();

            int cRecentCases = 0;
            var dictRecent = new Dictionary<int, Interval>(); // Uniquify multiple intervals for same case
            foreach (var interval in rgRecent)
            {
                if (dictRecent.ContainsKey(interval.ixBug)) continue;

                var item = new ToolStripMenuItem("Case " + interval.ixBug + ": " + interval.sTitle, null, recentMenuItem_Click);
                item.Tag = interval;
                recentCasesMenuItems.Add(item);
                recentlyWorkedOnToolStripMenuItem.DropDownItems.Add(item);

                dictRecent[interval.ixBug] = interval;
                cRecentCases++;
                if (cRecentCases == 5) break;
            }

            menuNoRecentWorkedOn.Visible = (cRecentCases == 0);
        }

        private void recentMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var interval = ((Interval)((ToolStripMenuItem)sender).Tag);

                fb.StartWork(interval.ixBug);
                UpdateWorkingOn(interval.ixBug, interval.sTitle);
            }
            catch (FogBugzErrorException exp)
            {
                BugScout.ReportException(exp);
                MessageBox.Show(String.IsNullOrEmpty(exp.Message) ? ("FogBugz returned an unknown error (" + exp.Code + ")") : exp.Message);
            }
        }

        private CaseListConnector _connector;
        private CaseListConnector Connector
        {
            get
            {
                return _connector;
            }

            set
            {
                if (_connector != null)
                {
                    _connector.OnShowUrl -= OnShowUrl;
                    _connector.OnRowCountChanged -= Connector_OnRowCountChanged;
                    _connector.OnViewStateChanged -= Connector_OnViewStateChanged;
                }

                _connector = value;
                if (_connector != null)
                {
                    _connector.OnShowUrl += OnShowUrl;
                    _connector.OnRowCountChanged += Connector_OnRowCountChanged;
                    _connector.OnViewStateChanged += Connector_OnViewStateChanged;

                    grid.RowCount = _connector.RowCount;
                }
                else
                {
                    grid.RowCount = 0;
                }
                grid.Invalidate();
            }
        }

        private void ListCasesResult(bool bSuccess, string sError, string sListDescription, List<Case> rgCasesInCurrentFilter, List<Case> rgParentAndSubcases)
        {
            State(FogBugzUIState.Normal);

            if (bSuccess)
            {
                sFilterLableText = lblFilter.Text = sListDescription;
                lblFilter.ForeColor = SystemColors.ControlText;

                var formatter = new BinaryFormatter();
                Dictionary<int, bool> casesExpanded = null;
                Dictionary<int, bool> casesShowingSubcases = null;

                try
                {
                    var serCasesExpanded = (byte[])RegistryHelper.FogBugzVSKey.GetValue("CasesExpanded");
                    if (serCasesExpanded != null)
                    {
                        casesExpanded = formatter.Deserialize(new MemoryStream(serCasesExpanded)) as Dictionary<int, bool>;
                    }

                    var serCasesShowingSubcases = (byte[])RegistryHelper.FogBugzVSKey.GetValue("CasesShowingSubcases");
                    if (serCasesShowingSubcases != null)
                    {
                        casesShowingSubcases = formatter.Deserialize(new MemoryStream(serCasesShowingSubcases)) as Dictionary<int, bool>;
                    }
                }
                catch { }

                var sortColumn = RegistryHelper.FogBugzVSKey.GetValue("SortColumn") as string;
                var savedSortOrder = RegistryHelper.FogBugzVSKey.GetValue("SortOrder") as string;
                var sortOrder = (SortOrder)Enum.Parse(typeof(SortOrder), savedSortOrder ?? "Ascending");

                string colName = null;
                if (sortColumn != null && grid.Columns.Contains(sortColumn))
                {
                    colName = grid.Columns[sortColumn].DataPropertyName;
                }

                setSortGlyphColumn(sortColumn, sortOrder);
                Connector = new CaseListConnector(fb, rgCasesInCurrentFilter, rgParentAndSubcases, colName, sortOrder, casesExpanded, casesShowingSubcases);

                // fHasBacklogPlugin should now be set
                if (fb.fHasBacklogPlugin.HasValue)
                {
                    if (!fb.fHasBacklogPlugin.Value)
                    {
                        grid.Columns["iBacklog"].Visible = false;
                        backlogMenuItem.Visible = false;
                    }

                    backlogMenuItem.Visible = fb.fHasBacklogPlugin.Value;
                }
            }
            else
            {
                BugScout.ReportError(sError);
                lblFilter.Text = sError;
                lblFilter.ForeColor = Color.Red;
            }
        }

        void Connector_OnViewStateChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        void Connector_OnRowCountChanged(object sender, EventArgs e)
        {
            grid.RowCount = Connector.RowCount;
            grid.Invalidate();
        }

        private enum FogBugzUIState
        {
            LoggedOff,
            LoggingOn,
            Busy,
            Normal
        }

        /// <summary>
        /// Disables UI and displays wait cursor while busy
        /// </summary>
        /// <param name="s">The state UI is transitioning to.</param>
        private void State(FogBugzUIState s)
        {
            bool loggedOn = !(s == FogBugzUIState.LoggedOff || s == FogBugzUIState.LoggingOn);

            lblFilter.Visible = grid.Visible = loggedOn;
            lblNotLoggedOn.Visible = !loggedOn;
            lblNotLoggedOn.Text = (s == FogBugzUIState.LoggedOff ? "Log on to start using FogBugz for Visual Studio." : "Logging on...");

            toolStripButtonStop.Visible = (s == FogBugzUIState.Busy);
            toolRefresh.Visible = (s == FogBugzUIState.Normal);

            lblFilter.Text = sFilterLableText;
            if (s == FogBugzUIState.Busy)
            {
                grid.ClearSelection();
                lblFilter.Text = "Loading...";
            }
            else if (s == FogBugzUIState.LoggedOff)
            {
                grid.DataSource = null;
            }

            ddWorkingOn.Enabled = ddFilters.Enabled = grid.Enabled = (s == FogBugzUIState.Normal);

            btnLogOff.Text = loggedOn ? "Log Off" : "Log On";
            btnLogOff.Enabled = (s == FogBugzUIState.Normal || s == FogBugzUIState.LoggedOff);
            toolSendEmail.Enabled = toolNewCase.Enabled = toolRefresh.Enabled = loggedOn;

            Cursor.Current = (s == FogBugzUIState.Busy || s == FogBugzUIState.LoggingOn) ? Cursors.WaitCursor : Cursors.Default;
            reportErrorsAutomaticallyToolStripMenuItem.Checked = Connect.ReportErrors;

            state = s;
        }

        public void reportErrorsAutomaticallyToolStripMenuItem_CheckedChanged(object sender, object args)
        {
            Connect.ReportErrors = reportErrorsAutomaticallyToolStripMenuItem.Checked;
        }

        private void ShowLogOnWindow(String sError = null, StringCollection rgsNames = null)
        {
            var f = new frmLogOn();

            if (!String.IsNullOrEmpty(sError))
            {
                f.ShowError(sError);
            }

            f.fldURL.Text = sURL;
            if (rgsNames == null)
            {
                f.fldUser.Text = sUser;
            }
            else
            {
                f.fldUser.Visible = false;
                f.cmbUser.Visible = true;
                foreach (String s in rgsNames)
                {
                    f.cmbUser.Items.Add(s);
                }
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                sURL = f.fldURL.Text;
                sUser = (rgsNames == null) ? f.fldUser.Text : f.cmbUser.Text;

                State(FogBugzUIState.LoggingOn);
                fb.Logon(sURL, sUser, f.fldPassword.Text);
            }
            f.Dispose();
        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (fb.LoggedOn())
                {
                    State(FogBugzUIState.LoggedOff);
                    fb.LogOff();
                    Connector = null;
                    grid.Invalidate();
                    sUser = "";
                    SaveSettings();
                }
                else
                {
                    ShowLogOnWindow();
                }
            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            // determine what operations are possible
            Case.Op ops = (Case.Op)0xFFFF;
            var c = 0;
            if (Connector != null)
            {
                foreach (DataGridViewRow rw in grid.SelectedRows)
                {
                    var row = Connector.Row(rw.Index);
                    if (row.Bug != null) // Skip "Click to Show Hidden Subcases" rows
                    {
                        ops &= row.Bug.ops;
                        c++;
                    }
                }
            }

            if (c == 0) ops = (Case.Op)0;

            toolReply.Enabled = (ops & Case.Op.reply) != 0;
            toolEdit.Enabled = (ops & Case.Op.edit) != 0;
            toolAssign.Enabled = (ops & Case.Op.assign) != 0;
            toolClose.Enabled = (ops & Case.Op.close) != 0;
            toolReactivate.Enabled = (ops & Case.Op.reactivate) != 0;
            toolReopen.Enabled = (ops & Case.Op.reopen) != 0;
            toolResolve.Enabled = (ops & Case.Op.resolve) != 0;

            toolWorkOn.Enabled = toolAddSubcase.Enabled = (grid.SelectedRows.Count == 1);
        }

        private void ddFilters_DropDownOpening(object sender, EventArgs e)
        {
            fb.ListFilters();
        }

        private void ListFiltersResult(bool bSuccess, string sError, List<Filter> rgFilters)
        {
            if (bSuccess)
            {
                /*
                 * Optimization - check list of filters to see if
                 * it's the same as the currently showing list.
                 * If so, don't refresh, to avoid flashing.
                 */
                if (!NeedToRefreshList(rgFilters))
                {
                    return;
                }

                var ft = Filter.FilterType.builtin;
                ddFilters.DropDownItems.Clear();
                foreach (Filter f in rgFilters)
                {
                    if (ft != f.type)
                    {
                        ddFilters.DropDownItems.Add(new ToolStripSeparator());
                    }
                    ft = f.type;
                    ToolStripItem tsi = ddFilters.DropDownItems.Add(f.s);
                    tsi.Tag = f.codeFilter;
                    tsi.Font = new Font(tsi.Font, f.fIsCurrent ? FontStyle.Bold : FontStyle.Regular);
                }
            }
        }

        private bool NeedToRefreshList(List<Filter> rgFilters) {
            int ix = 0;
            var ft = Filter.FilterType.builtin;

            foreach (Filter f in rgFilters)
            {
                if (ft != f.type)
                {
                    ix++;
                }
                ft = f.type;

                var items = ddFilters.DropDownItems;
                if (items.Count - 1 < ix || items[ix].Text != f.s || items[ix].Tag.ToString() != f.codeFilter)
                {
                    return true;
                }

                ix++;
            }

            return (ddFilters.DropDownItems.Count - 1 >= ix);
        }

        private void ddFilters_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem.Tag.ToString() != "LoadingDoNotList")
                {
                    State(FogBugzUIState.Busy);

                    // Reset view state
                    Connector = null;
                    sFilterLableText = "";

                    RegistryHelper.FogBugzVSKey.DeleteValue("CasesExpanded", false);
                    RegistryHelper.FogBugzVSKey.DeleteValue("CasesShowingSubcases", false);

                    fb.SaveFilterAndList(e.ClickedItem.Tag.ToString());
                    //
                    // make current item bold
                    // UNDONE bug: if list is cancelled because you press
                    //             stop fast enough, the wrong item is boldened.
                    //
                    foreach (ToolStripItem tsi in ddFilters.DropDownItems)
                    {
                        tsi.Font = new Font(tsi.Font, (tsi == e.ClickedItem) ? FontStyle.Bold : FontStyle.Regular);
                    }
                }
            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void toolNewCase_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "command=new&pg=pgEditBug");
        }

        private void toolSendEmail_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "command=newemail&pg=pgEditBug");
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // Header click

                var sBugs = ListSelectedRows();
                if (!String.IsNullOrEmpty(sBugs))
                {
                    OnShowUrl(fb.GetDefaultUrl() + ListSelectedRows());
                }
            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Connector == null) return;

                if (e.RowIndex >= 0 && grid.Columns[e.ColumnIndex].Name != "hrsCurrEst") // Not headers; and not the estimate column (see CellMouseClick)
                {
                    Connector.Row(e.RowIndex).OnClicked(grid, e);
                }

            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private string ListSelectedRows()
        {
            if (Connector == null) return "";

            Debug.Assert(grid.SelectedRows.Count > 0);
            var rgsIxBug = new List<string>();
            foreach (DataGridViewRow rw in grid.SelectedRows)
            {
                var row = Connector.Row(rw.Index);
                if (row.Bug != null) rgsIxBug.Add(row.Bug.ixBug.ToString());
            }
            return String.Join(",", rgsIxBug.ToArray());
        }

        private void toolReply_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=reply&ixBug=" + ListSelectedRows());
        }

        private void toolEdit_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=edit&ixBug=" + ListSelectedRows());
        }

        private void toolAssign_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=assign&ixBug=" + ListSelectedRows());
        }

        private void toolResolve_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=resolve&ixBug=" + ListSelectedRows());
        }

        private void toolAddSubcase_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=new&ixBugParent=" + Connector.Row(grid.SelectedRows[0].Index).Bug.ixBug);
        }

        private void toolReactivate_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=reactivate&ixBug=" + ListSelectedRows());
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=close&ixBug=" + ListSelectedRows());
        }

        private void toolReopen_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgEditBug&command=reopen&ixBug=" + ListSelectedRows());
        }

        private void toolWorkOn_Click(object sender, EventArgs e)
        {
            if (Connector == null) return;

            try
            {
                var bug = Connector.Row(grid.SelectedRows[0].Index).Bug;

                if (bug.hrsCurrEst.hrs == 0.0m)
                {
                    // Set estimate first
                    var estimateForm = new frmSetEstimate();
                    estimateForm.lblEstimate.Text = "Enter an estimate to start working on Case " + bug.ixBug + ", " + bug.sTitle + ".";
                    if (estimateForm.ShowDialog() == DialogResult.OK)
                    {
                        Connector.FogBugzClient.SetCaseEstimate(bug, estimateForm.fldEstimate.Text);
                        grid.InvalidateRow(grid.SelectedRows[0].Index); // invalidate all cells as remaining time might change
                    }
                    else return;
                    estimateForm.Dispose();
                }

                fb.StartWork(bug);
                grid.InvalidateRow(grid.SelectedRows[0].Index);
                UpdateWorkingOn(bug.ixBug, bug.sTitle);
            }
            catch (FogBugzErrorException exp)
            {
                BugScout.ReportException(exp);
                MessageBox.Show(String.IsNullOrEmpty(exp.Message) ? ("FogBugz returned an unknown error (" + exp.Code + ")") : exp.Message);
            }
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            fb.Stop();
        }

        private void btnCancelLogon_Click(object sender, EventArgs e)
        {
            fb.Stop();
        }

        private bool IsValidRow(int index)
        {
            return Connector != null && 0 <= index && index < Connector.RowCount;
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (IsValidRow(e.RowIndex))
            {
                Connector.Row(e.RowIndex).CellFormatting(e, grid);
            }
        }

        private void grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveSettings();
        }

        private void grid_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            SaveSettings();
        }

        private void grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (IsValidRow(e.RowIndex))
            {
                e.Value = Connector.Row(e.RowIndex).GetCellValue(grid.Columns[e.ColumnIndex].DataPropertyName);
            }
        }

        private void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Connector == null) return;

                string col = grid.Columns[e.ColumnIndex].DataPropertyName;
                SortOrder order = SortOrder.Ascending;
                if (Connector.SortField == col)
                {
                    order = (Connector.SortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
                }

                Connector.Sort(col, order);
                setSortGlyphColumn(grid.Columns[e.ColumnIndex].Name, order);
                grid.Invalidate();

                SaveSettings();
            }
            catch (Exception ex) 
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void setSortGlyphColumn(string colName, SortOrder order)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            if (colName != null && grid.Columns.Contains(colName))
            {
                grid.Columns[colName].HeaderCell.SortGlyphDirection = order;
            }
        }

        private Bitmap t_d, t_r;
        private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Connector != null && grid.Columns[e.ColumnIndex].Name == "sTitle" && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, e.PaintParts);

                var row = Connector.Row(e.RowIndex);
                if (row.Expandable)
                {
                    if (t_d == null || t_r == null)
                    {
                        var manager = new ResourceManager("FogBugzForVisualStudio.Resource1", GetType().Assembly);
                        t_d = (Bitmap)manager.GetObject("t_d");
                        t_r = (Bitmap)manager.GetObject("t_r");
                    }

                    e.Graphics.DrawImage(row.Expanded ? t_d : t_r, e.CellBounds.Location.X + 32 * row.Indent + 5, e.CellBounds.Location.Y + 6);
                }

                e.Handled = true;
            }
        }

        private void grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Connector != null && e.RowIndex >= 0 && grid.Columns[e.ColumnIndex].Name == "sTitle")
                {
                    var row = Connector.Row(e.RowIndex);
                    if (row.Expandable && e.X >= row.Indent * 32 + 3 && e.X <= row.Indent * 32 + 4 + 9 && e.Y >= 6 && e.Y <= 6 + 9)
                    {
                        row.Expanded = !row.Expanded;
                    }
                }
                else if (e.RowIndex >= 0 && grid.Columns[e.ColumnIndex].Name == "hrsCurrEst")
                {
                    Connector.Row(e.RowIndex).OnClicked(grid, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            }
            catch (Exception ex) 
            {
                BugScout.ReportException(ex);
                throw;
            }
        }

        private void grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (!grid.Rows[e.RowIndex].Selected)
                {
                    grid.ClearSelection();
                    grid.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        private void menuEditTimesheet_Click(object sender, EventArgs e)
        {
            OnShowUrl(fb.GetDefaultUrl() + "pg=pgTimesheet");
        }

        private void menuWorkOnNothing_Click(object sender, EventArgs e)
        {
            try
            {
                fb.StopWork();
                UpdateWorkingOn(null, null);
            }
            catch (FogBugzErrorException ex)
            {
                BugScout.ReportException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateWorkingOn(int? ixBug, string sTitle)
        {
            ddWorkingOn.Text = (ixBug.HasValue ? "Working On " + ixBug : "Working On");
            menuViewCurrentCase.Text = (ixBug.HasValue ? "View Current Case: " + ixBug + ": " + sTitle : "View Current Case");
            menuViewCurrentCase.Enabled = ixBug.HasValue;
            menuViewCurrentCase.Tag = ixBug;
        }

        private void menuViewCurrentCase_Click(object sender, EventArgs e)
        {
            try
            {
                Nullable<int> ixBug = menuViewCurrentCase.Tag as Nullable<int>;
                if (!ixBug.HasValue) return;

                OnShowUrl(fb.GetDefaultUrl() + ixBug.Value);
            }
            catch (FogBugzErrorException ex)
            {
                BugScout.ReportException(ex);
                throw;
            }  

        }

        private void timUpdateWorkingOn_Tick(object sender, EventArgs e)
        {
            UpdateIntervals();
        }

        private void UpdateIntervals()
        {
            fb.ListIntervals(DateTime.Now - new TimeSpan(7, 0, 0, 0), null);
        }
    }
}
