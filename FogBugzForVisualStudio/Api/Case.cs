using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FogBugzForVisualStudio.Api
{
    public class Case
    {
        [FlagsAttribute]
        public enum Op
        {
            edit = 0x0001,
            spam = 0x0002,
            assign = 0x0004,
            resolve = 0x0008,
            move = 0x0010,
            reactivate = 0x0020,
            close = 0x0040,
            reopen = 0x0080,
            remind = 0x0100,
            reply = 0x0200,
            forward = 0x0400,
            email = 0x0800
        }

        public int ixBug { get; private set; }
        public Op ops { get; private set; }

        public int? ixBugParent { get; private set; }
        public IList<int> ixBugChildren { get; private set; }

        public int ixBugEventLatest { get; private set; }
        public int ixBugEventLastView { get; private set; }

        public Category category { get; private set; }
        public Priority priority { get; private set; }
        public Status status { get; private set; }
        public Project project { get; private set; }
        public Area area { get; private set; }
        public FixFor fixfor { get; private set; }

        public string sTitle { get; private set; }

        public Person assignedTo { get; private set; }
        public Person openedBy { get; private set; }
        public Person resolvedBy { get; private set; }
        public Person closedBy { get; private set; }
        public Person lastEditedBy { get; private set; }

        public Estimate hrsOrigEst { get; private set; }
        public Estimate hrsCurrEst { get; private set; }
        public Estimate hrsElapsed { get; private set; }
        public Estimate hrsRemaining
        {
            get
            {
                return new Estimate(Math.Max(this.hrsCurrEst.hrs - this.hrsElapsed.hrs, Decimal.Zero));
            }
        }

        public Nullable<DateTime> dtOpened { get; private set; }
        public Nullable<DateTime> dtResolved { get; private set; }
        public Nullable<DateTime> dtClosed { get; private set; }
        public Nullable<DateTime> dtDue { get; private set; }
        public Nullable<DateTime> dtLastUpdated { get; private set; }

        public int? iBacklog { get; private set; }

        public static string BacklogFieldName = "plugin_projectbacklog_at_fogcreek_com_ibacklog";

        public Case(int ixBug, Op ops, Dictionary<String, String> fields, FogBugzClient parentClient)
        {
            this.ixBug = ixBug;
            this.ops = ops;

            if (!String.IsNullOrEmpty(fields["ixBugParent"]))
            {
                this.ixBugParent = Convert.ToInt32(fields["ixBugParent"]);
                if (this.ixBugParent <= 0) this.ixBugParent = null;
            }
            else
            {
                this.ixBugParent = null;
            }

            this.ixBugChildren = new List<int>();
            if (!String.IsNullOrEmpty(fields["ixBugChildren"]))
            {
                foreach (string ix in fields["ixBugChildren"].Split(','))
                {
                    this.ixBugChildren.Add(Convert.ToInt32(ix));
                }
            }

            this.ixBugEventLatest = Convert.ToInt32(fields["ixBugEventLatest"]);
            this.ixBugEventLastView = Convert.ToInt32(fields["ixBugEventLastView"]);

            this.category = parentClient.GetCategory(Convert.ToInt32(fields["ixCategory"]));
            this.priority = new Priority(Convert.ToInt32(fields["ixPriority"]), fields["sPriority"]);
            this.status = parentClient.GetStatus(Convert.ToInt32(fields["ixStatus"]));
            this.project = new Project(Convert.ToInt32(fields["ixProject"]), fields["sProject"]);
            this.area = new Area(Convert.ToInt32(fields["ixArea"]), fields["sArea"]);
            this.fixfor = new FixFor(Convert.ToInt32(fields["ixFixFor"]), fields["sFixFor"]);

            this.sTitle = fields["sTitle"];

            this.assignedTo = parentClient.GetPerson(Convert.ToInt32(fields["ixPersonAssignedTo"]));
            this.openedBy = parentClient.GetPerson(Convert.ToInt32(fields["ixPersonOpenedBy"]));
            this.resolvedBy = parentClient.GetPerson(Convert.ToInt32(fields["ixPersonResolvedBy"]));
            this.closedBy = parentClient.GetPerson(Convert.ToInt32(fields["ixPersonClosedBy"]));
            this.lastEditedBy = parentClient.GetPerson(Convert.ToInt32(fields["ixPersonLastEditedBy"]));

            this.hrsOrigEst = Estimate.Parse(fields["hrsOrigEst"]);
            this.hrsCurrEst = Estimate.Parse(fields["hrsCurrEst"]);
            this.hrsElapsed = Estimate.Parse(fields["hrsElapsed"]);

            this.dtOpened = Util.ParseApiDate(fields["dtOpened"]);
            this.dtResolved = Util.ParseApiDate(fields["dtResolved"]);
            this.dtClosed = Util.ParseApiDate(fields["dtClosed"]);
            this.dtDue = Util.ParseApiDate(fields["dtDue"]);
            this.dtLastUpdated = Util.ParseApiDate(fields["dtLastUpdated"]);

            if (fields.ContainsKey(BacklogFieldName) && !String.IsNullOrEmpty(fields[BacklogFieldName]))
            {
                this.iBacklog = Convert.ToInt32(fields[BacklogFieldName]);
            }
        }

        /// <summary>
        /// DataGridView's binding doesn't support subkeys. So cheat and add a shortcut.
        /// </summary>
        public Bitmap categoryImage
        {
            get
            {
                return category.Image;
            }
        }

        public bool Unread
        {
            get
            {
                return ixBugEventLastView < ixBugEventLatest;
            }
            set
            {
                // An exception to our usual immutability, since the user might view a case
                if (value)
                {
                    ixBugEventLastView = ixBugEventLatest;
                }
                else
                {
                    ixBugEventLastView = 0;
                }
            }
        }

        public void SetNewEstimate(Estimate e)
        {
            // A rare exception to Bug's immutability. Thus make a special wrapper method to set this.
            hrsCurrEst = e;
        }

        internal void WorkStarted()
        {
            hrsElapsed = new Estimate(0.01m);
        }
    }
}
