using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Area
    {
        public readonly int ixArea;
        public readonly string sArea;

        public Area(int ixArea, string sArea)
        {
            this.ixArea = ixArea;
            this.sArea = sArea;
        }

        public override string ToString()
        {
            return sArea;
        }
    }
}
