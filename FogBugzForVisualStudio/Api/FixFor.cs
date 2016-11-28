using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class FixFor
    {
        public readonly int ixFixFor;
        public readonly string sFixFor;

        public FixFor(int ixFixFor, string sFixFor)
        {
            this.ixFixFor = ixFixFor;
            this.sFixFor = sFixFor;
        }

        public override string ToString()
        {
            return sFixFor;
        }
    }
}
