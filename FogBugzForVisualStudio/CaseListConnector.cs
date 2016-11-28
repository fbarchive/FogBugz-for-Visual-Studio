using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

using FogBugzForVisualStudio.Api;

namespace FogBugzForVisualStudio
{
    public class CaseListConnector
    {
        private Dictionary<int, bool> ixBugInFilter;
        private Dictionary<int, CaseListRow> allRows;
        private List<CaseListRow> visibleRows;
        private List<CaseListRow> rootRows;
        private bool loaded;

        private string sortField;
        private SortOrder sortOrder;

        public string SortField
        {
            get
            {
                return sortField;
            }
            set
            {
                this.Sort(value, sortOrder);
            }
        }

        public SortOrder SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                this.Sort(sortField, value);
            }
        }

        internal FogBugzClient FogBugzClient { get; private set; }

        /// <summary>
        /// Construct a CaseListConnector.
        /// </summary>
        /// <param name="fb">A logged-on FogBugzClient.</param>
        /// <param name="casesInFilter">Cases that are in the user's current filter, as returned from FogBugzClient.ListCases</param>
        /// <param name="supplementaryCases">A list of cases that are not in the filter, but are parent cases or subcases to cases that are.</param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="expandedCases">Optional: a dictionary matching cases to whether they are expanded or collapsed.  Pass the value from a previous CaseListConnector's GetCasesExpanded() to save view state.</param>
        /// <param name="casesShowingSubcases">Optional: a dictionary matching cases to whether they show subcases not in the filter. Pass the value from a previous CaseListConnector's GetCasesShowingSubcases() to save view state.</param>
        public CaseListConnector(FogBugzClient fb, List<Case> casesInFilter, List<Case> supplementaryCases, string sortField, SortOrder sortOrder, Dictionary<int, bool> expandedCases = null, Dictionary<int, bool> casesShowingSubcases = null)
        {
            FogBugzClient = fb;
            this.sortField = sortField;
            this.sortOrder = sortOrder;

            ixBugInFilter = new Dictionary<int, bool>();
            allRows = new Dictionary<int, CaseListRow>();

            // Compute a list of root cases, which have no parent subcase, from casesInFilter.
            var allCases = new Dictionary<int, Case>();
            foreach (var c in casesInFilter)
            {
                allCases[c.ixBug] = c;
                ixBugInFilter[c.ixBug] = true;
            }
            foreach (var c in supplementaryCases)
            {
                allCases[c.ixBug] = c;
                ixBugInFilter[c.ixBug] = false;
            }

            var dictRootCases = new Dictionary<int, bool>();
            foreach (var c in casesInFilter)
            {
                var i = c.ixBug;
                while (allCases[i].ixBugParent.HasValue)
                {
                    i = allCases[i].ixBugParent.Value;
                }
                dictRootCases[i] = true;
            }

            rootRows = new List<CaseListRow>();
            foreach (var ixBug in dictRootCases.Keys)
            {
                rootRows.Add(createCaseRow(allCases[ixBug], 0, allCases, expandedCases, casesShowingSubcases));
            }

            PopulateVisibleRows();

            loaded = true;
        }

        public Dictionary<int, bool> GetCasesExpanded()
        {
            var expanded = new Dictionary<int, bool>();
            foreach (var c in allRows.Values)
            {
                expanded[c.Bug.ixBug] = c.Expanded;
            }
            return expanded;
        }

        public Dictionary<int, bool> GetCasesShowingSubcases()
        {
            var showing = new Dictionary<int, bool>();
            foreach (var c in allRows.Values)
            {
                showing[c.Bug.ixBug] = c.ShowsSubcasesNotInFilter;
            }
            return showing;
        }

        private CaseListRow createCaseRow(Case c, int i, Dictionary<int, Case> allCases, Dictionary<int, bool> expandedCases, Dictionary<int, bool> casesShowingSubcases)
        {
            var row = new CaseListRow(c, i, this);
            if (row.Expandable)
            {
                row.Expanded = (expandedCases != null && expandedCases.ContainsKey(c.ixBug)) ? expandedCases[c.ixBug] : containsSubcaseInFilter(c, allCases);
            }
            if (casesShowingSubcases != null && casesShowingSubcases.ContainsKey(c.ixBug))
            {
                row.ShowsSubcasesNotInFilter = casesShowingSubcases[c.ixBug];
            }
            allRows[c.ixBug] = row;

            foreach (var ixBugChild in c.ixBugChildren)
            {
                createCaseRow(allCases[ixBugChild], i + 1, allCases, expandedCases, casesShowingSubcases);
            }

            return row;
        }

        private bool containsSubcaseInFilter(Case c, Dictionary<int, Case> allCases)
        {
            foreach (var ixBug in c.ixBugChildren)
            {
                if (ixBugInFilter[ixBug]) return true;
                if (containsSubcaseInFilter(allCases[ixBug], allCases)) return true;
            }
            return false;
        }

        private void PopulateVisibleRows()
        {
            visibleRows = new List<CaseListRow>();

            if (sortField != null) rootRows.Sort(new CaseListRowComparer(sortField, sortOrder));

            foreach (var row in rootRows)
            {
                visibleRows.Add(row);
                if (row.Expanded) ExpandRow(row);
            }
        }

        public int RowCount
        {
            get
            {
                return visibleRows.Count;
            }
        }

        public CaseListRow Row(int index)
        {
            return visibleRows[index];
        }

        public void Sort(string sortField, SortOrder order)
        {
            this.sortField = sortField;
            this.sortOrder = order;
            PopulateVisibleRows();
        }

        private List<CaseListRow> getSubrows(CaseListRow row)
        {
            var subrows = new List<CaseListRow>();
            var hiddenSubrows = new List<CaseListRow>();
            foreach (var ixBug in row.Bug.ixBugChildren)
            {
                // Rows that are in the filter are always shown. Otherwise we're dependent on ShowsSubcasesNotInFilter.
                if (ixBugInFilter[ixBug] || row.ShowsSubcasesNotInFilter)
                {
                    subrows.Add(allRows[ixBug]);
                }
                else
                {
                    hiddenSubrows.Add(allRows[ixBug]);
                }
            }

            if (subrows.Count == 0 && hiddenSubrows.Count > 0)
            {
                // In this case, override and don't show a "X other cases hidden" row.
                return hiddenSubrows;
            }

            if (hiddenSubrows.Count > 0)
            {
                // Add a row to display "[ x cases were hidden because they don't match your filter]"
                subrows.Add(row.HiddenCasesRow);
            }

            return subrows;
        }

        private int ExpandRow(CaseListRow row)
        {
            var subrows = getSubrows(row);

            if (sortField != null) subrows.Sort(new CaseListRowComparer(sortField, sortOrder));

            var ixRowParent = visibleRows.IndexOf(row);
            var c = 0;
            foreach (var subrow in subrows)
            {
                visibleRows.Insert(ixRowParent + 1 + c++, subrow);
                if (subrow.Expanded)
                {
                    c += ExpandRow(subrow);
                }
            }

            return c;
        }

        private void CollapseRow(CaseListRow row)
        {
            var subrows = getSubrows(row);

            foreach (var subrow in subrows)
            {
                visibleRows.Remove(subrow);
                if (subrow.Expanded) CollapseRow(subrow);
            }

            if (visibleRows.Contains(row.HiddenCasesRow)) 
            {
                visibleRows.Remove(row.HiddenCasesRow);
                }
        }

        internal void RowExpandedChanged(CaseListRow row)
        {
            if (!loaded) return;

            if (row.Expanded)
            {
                ExpandRow(row);
            }
            else
            {
                CollapseRow(row);
            }

            OnRowCountChanged(this, new EventArgs());
            OnViewStateChanged(this, new EventArgs());
        }

        internal void RowShowsSubcasesNotInFilterChanged(CaseListRow caseListRow)
        {
            if (!loaded) return;

            CollapseRow(caseListRow);
            ExpandRow(caseListRow);

            OnRowCountChanged(this, new EventArgs());
            OnViewStateChanged(this, new EventArgs());
        }

        internal void ShowHiddenSubcases(CaseListHiddenCasesRow caseListHiddenCasesRow, int ixBugParent)
        {
            allRows[ixBugParent].ShowsSubcasesNotInFilter = true;  // Will raise OnRowCountChanged
        }

        internal bool BugInFilter(int ixBug)
        {
            return ixBugInFilter[ixBug];
        }

        public event EventHandler OnRowCountChanged;

        public event EventHandler OnViewStateChanged;

        public event FogBugzForVisualStudio.FogBugzCtl.ShowUrlHandler OnShowUrl;

        internal void ShowUrl(string p)
        {
            this.OnShowUrl(p);
        }
    }

    public class CaseListRow
    {
        public CaseListRow(Case c, int i, CaseListConnector parent)
        {
            Bug = c;
            Indent = i;
            _parentConnector = new WeakReference(parent);
        }

        private WeakReference _parentConnector;
        protected CaseListConnector parentConnector
        {
            get
            {
                if (_parentConnector == null || !_parentConnector.IsAlive)
                {
                    return null;
                }
                return (CaseListConnector)_parentConnector.Target;
            }
        }

        public virtual Case Bug { get; private set; }

        public int Indent { get; private set; }

        public virtual bool Expandable
        {
            get
            {
                return Bug.ixBugChildren.Count > 0;
            }
        }

        private bool _expanded;

        public virtual bool Expanded 
        { 
            get
            {
                return _expanded;
            }

            set
            {
                if (Bug.ixBugChildren.Count == 0 && value)
                {
                    throw new InvalidOperationException();
                }

                _expanded = value;
                if (parentConnector != null)
                {
                    parentConnector.RowExpandedChanged(this);
                }
            }
        }

        private bool _showsSubcasesNotInFilter;

        public virtual bool ShowsSubcasesNotInFilter
        {
            get
            {
                return _showsSubcasesNotInFilter;
            }

            set
            {
                if (Bug.ixBugChildren.Count == 0 && value)
                {
                    throw new InvalidOperationException();
                }

                _showsSubcasesNotInFilter = value;
                if (parentConnector != null)
                {
                    parentConnector.RowShowsSubcasesNotInFilterChanged(this);
                }
            }
        }

        public virtual bool InFilter
        {
            get
            {
                if (parentConnector != null) return parentConnector.BugInFilter(Bug.ixBug);
                return true;
            }
        }
        
        public virtual void OnClicked(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (parentConnector == null) return;

            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "sTitle":
                case "ixBug":
                    parentConnector.ShowUrl(parentConnector.FogBugzClient.GetDefaultUrl() + Bug.ixBug);
                    Bug.Unread = false;
                    grid.InvalidateRow(e.RowIndex);
                    break;

                case "hrsCurrEst":
                    var estimateForm = new frmSetEstimate();
                    estimateForm.lblEstimate.Text = "Enter an estimate for Case " + Bug.ixBug + ", " + Bug.sTitle + ".";
                    estimateForm.fldEstimate.Text = Bug.hrsCurrEst.ToString();
                    if (estimateForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            parentConnector.FogBugzClient.SetCaseEstimate(Bug, estimateForm.fldEstimate.Text);
                            grid.InvalidateRow(e.RowIndex); // invalidate all cells as remaining time might change
                        }
                        catch (FogBugzErrorException exp)
                        {
                            BugScout.ReportException(exp);
                            MessageBox.Show(String.IsNullOrEmpty(exp.Message) ? ("FogBugz returned an unknown error (" + exp.Code + ")") : (exp.Message + " (" + exp.Code + ")"));
                        }
                    }
                    estimateForm.Dispose();
                    break;
            }
        }

        public virtual object GetCellValue(string columnName)
        {
            return typeof(Case).GetProperty(columnName).GetValue(Bug, null);
        }

        public virtual void CellFormatting(DataGridViewCellFormattingEventArgs e, DataGridView grid)
        {
            if (grid.Columns[e.ColumnIndex].Name == "sTitle")
            {
                e.CellStyle.Padding = new Padding(32 * Indent + (Expandable ? 14 : 0), 0, 0, 0);
            }

            if (grid.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType() == typeof(DataGridViewLinkCell))
            {
                bool selected = grid.Rows[e.RowIndex].Selected;
                DataGridViewLinkCell linkCell = (DataGridViewLinkCell)grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (selected)
                {
                    linkCell.LinkColor = System.Drawing.SystemColors.HighlightText;
                }
                else
                {
                    if (this.InFilter)
                    {
                        bool Unread = (grid.Columns[e.ColumnIndex].Name == "hrsCurrEst") ? (Bug.hrsElapsed.hrs == Decimal.Zero) : Bug.Unread;
                        linkCell.LinkColor = Unread ? Color.Blue : Color.Purple;
                    }
                    else
                    {
                        linkCell.LinkColor = Color.Gray;
                    }
                }
            }

            if (!this.InFilter)
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        private CaseListHiddenCasesRow hiddenCasesRow;
        public CaseListHiddenCasesRow HiddenCasesRow
        {
            get
            {
                if (hiddenCasesRow == null)
                {
                    var hide = 0;
                    foreach (var ixBugChild in Bug.ixBugChildren)
                    {
                        if (!parentConnector.BugInFilter(ixBugChild)) hide++;
                    }

                    hiddenCasesRow = new CaseListHiddenCasesRow(Bug.ixBug, hide, Indent + 1, parentConnector);
                }
                return hiddenCasesRow;
            }
        }
    }

    public class CaseListHiddenCasesRow : CaseListRow
    {
        private int ixBugParent;
        private int cHiddenCases;

        public CaseListHiddenCasesRow(int ixBugParent, int cHiddenCases, int i, CaseListConnector parent) : base(null, i, parent)
        {
            this.ixBugParent = ixBugParent;
            this.cHiddenCases = cHiddenCases;
        }

        public override Case Bug
        {
            get
            {
                return null;
            }
        }

        public override bool Expandable
        {
            get
            {
                return false;
            }
        }

        public override bool Expanded
        {
            get
            {
                return false;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public override bool ShowsSubcasesNotInFilter
        {
            get
            {
                return false;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public override void OnClicked(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (parentConnector != null)
            {
                parentConnector.ShowHiddenSubcases(this, ixBugParent);
            }
        }

        public override object GetCellValue(string columnName)
        {
            if (columnName == "sTitle")
            {
                return "[ " + cHiddenCases + " subcases not matching filter ]";
            }
            else if (columnName == "categoryImage")
            {
                var manager = new ResourceManager("FogBugzForVisualStudio.Resource1", GetType().Assembly);
                return (Bitmap)manager.GetObject("icon_none");
            }
            return null;
        }

        public override bool InFilter
        {
            get
            {
                return false;
            }
        }
    }

    internal class CaseListRowComparer : IComparer<CaseListRow>
    {
        private string field;
        private SortOrder sortOrder;

        public CaseListRowComparer(string field, SortOrder sortOrder)
        {
            this.field = field;
            this.sortOrder = sortOrder;
        }

        public int Compare(CaseListRow x, CaseListRow y)
        {
            if (x is CaseListHiddenCasesRow) return 1;
            if (y is CaseListHiddenCasesRow) return -1;

            var xValue = x.GetCellValue(field);
            var yValue = y.GetCellValue(field);
            var result = 0;

            if (xValue == null && yValue == null)
            {
                result = 0;
            }
            else if (xValue == null)
            {
                result = -1;
            }
            else if (yValue == null)
            {
                result = 1;
            }
            else
            {
                if (xValue is IComparable && yValue is IComparable)
                {
                    result = ((IComparable)xValue).CompareTo(yValue);
                }
                // If values don't implement IComparer but are equivalent
                else if (xValue.Equals(yValue))
                {
                    result = 0;
                }
                // Values don't implement IComparer and are not equivalent, so compare as string values
                else result = xValue.ToString().CompareTo(yValue.ToString());
            }

            // ixBug as a secondary sort so that the order is consistent
            if (result == 0)
            {
                var ixX = x.Bug.ixBug;
                var ixY = y.Bug.ixBug;

                result = ixX.CompareTo(ixY);
            }

            if (sortOrder == SortOrder.Descending)
            {
                result = -result;
            }
            return result;
        }
    }
}
