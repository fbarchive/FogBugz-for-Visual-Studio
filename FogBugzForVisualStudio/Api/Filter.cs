using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Filter
    {
        public enum FilterType
        {
            builtin = 1,
            saved = 2,
            shared = 4,
        }

        public FilterType type;
        public string codeFilter;   // opaque code
        public string s;            // user-visible name
        public bool fIsCurrent;  // is this the user's current filter?
    }
}
