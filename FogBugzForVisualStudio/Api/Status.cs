using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Status
    {
        public readonly int ixStatus;
        public readonly string sStatus;
        public readonly bool fDeleted;
        public readonly int ixCategory;
        public readonly bool fResolved;
        public readonly int iOrder;

        public Status(Dictionary<String, String> fields)
        {
            this.ixStatus = Convert.ToInt32(fields["ixStatus"]);
            this.sStatus = fields["sStatus"];
            this.fDeleted = Convert.ToBoolean(fields["fDeleted"]);
            this.ixCategory = Convert.ToInt32(fields["ixCategory"]);
            this.fResolved = Convert.ToBoolean(fields["fResolved"]);
            this.iOrder = Convert.ToInt32(fields["iOrder"]);
        }

        public override string ToString()
        {
            return sStatus;
        }
    }
}
