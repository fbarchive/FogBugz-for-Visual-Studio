using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Interval
    {
        public int ixInterval { get; private set; }

        public Person Person { get; private set; }

        public int ixBug { get; private set; }
        public string sTitle { get; private set; }

        public DateTime? dtStart { get; private set; }
        public DateTime? dtEnd { get; private set; }

        public bool fDeleted { get; private set; }

        public Interval(Dictionary<String, String> fields, FogBugzClient parentClient)
        {
            this.ixInterval = Convert.ToInt32(fields["ixInterval"]);

            this.Person = parentClient.GetPerson(Convert.ToInt32(fields["ixPerson"]));

            this.ixBug = Convert.ToInt32(fields["ixBug"]);
            this.sTitle = fields["sTitle"];

            this.dtStart = Util.ParseApiDate(fields["dtStart"]);
            this.dtEnd = Util.ParseApiDate(fields["dtEnd"]);

            this.fDeleted = Convert.ToBoolean(fields["fDeleted"]);
        }
    }
}
