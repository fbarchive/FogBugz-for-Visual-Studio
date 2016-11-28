using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Priority
    {
        public readonly int ixPriority;
        public readonly string sPriority;

        public Priority(int ixPriority, string sPriority)
        {
            this.ixPriority = ixPriority;
            this.sPriority = sPriority;
        }

        public override string ToString()
        {
            return ixPriority.ToString() + " - " + sPriority;
        }
    }
}
