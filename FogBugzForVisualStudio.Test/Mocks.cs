using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FogBugzForVisualStudio.Api;

namespace FogBugzForVisualStudio.Test
{
    class MockClient : FogBugzClient
    {
        public override Person GetPerson(int ixPerson)
        {
            return null;
        }

        public override Status GetStatus(int ixStatus)
        {
            return null;
        }

        public override Category GetCategory(int ixCategory)
        {
            return null;
        }
    }

    class MockBug : System.Collections.IEnumerable
    {

        private Dictionary<string, string> fields = new Dictionary<string, string>();
        public Dictionary<string, string> Dict { get { return fields; } }

        private void Fill(String s, string[] keys)
        {
            foreach (var key in keys)
            {
                fields[key] = s;
            }
        }

        public MockBug()
        {
            Fill("0", new string[]{
                "ixBugParent", "ixBugChildren", "ixBugEventLatest", "ixBugEventLastView", "ixCategory", 
                "ixPriority", "ixStatus", "ixProject", "ixArea", "ixFixFor", "ixPersonAssignedTo",
                "ixPersonOpenedBy", "ixPersonResolvedBy", "ixPersonClosedBy", "ixPersonLastEditedBy",
                Case.BacklogFieldName
            });

            Fill("", new string[] { "sProject", "sArea", "sFixFor", "sPriority", "sTitle" });
            Fill("0", new string[] { "hrsOrigEst", "hrsCurrEst", "hrsElapsed" });
            Fill(null, new string[] { "dtOpened", "dtResolved", "dtClosed", "dtDue", "dtLastUpdated" });
        }

        public void Add(string key, string value) 
        {
            fields[key] = value;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
