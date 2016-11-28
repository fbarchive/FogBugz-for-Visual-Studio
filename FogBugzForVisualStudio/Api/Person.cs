using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Person
    {
        public readonly int ixPerson;
        public readonly string sFullName;
        public readonly string sEmail;

        public Person(Dictionary<String, String> fields)
        {
            this.ixPerson = Convert.ToInt32(fields["ixPerson"]);
            this.sFullName = fields["sFullName"];
            this.sEmail = fields["sEmail"];
        }

        public override string ToString()
        {
            return sFullName;
        }
    }
}
