using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FogBugzForVisualStudio.Api;

namespace FogBugzForVisualStudio.Test
{
    class Locale : IDisposable {
        private CultureInfo current;

        public Locale(CultureInfo culture) {
            current = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = current;
        }
    }

    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void CreateBug()
        {
            using(new Locale(CultureInfo.InvariantCulture))
            {
                var bug = new Case(1, Case.Op.resolve, new MockBug().Dict, new MockClient());
            }
        }

        [TestMethod]
        public void CreateBugLocalized()
        {
            using (new Locale(new CultureInfo("fr-FR")))
            {
                var bug = new Case(1, Case.Op.resolve, new MockBug{ 
                    {"hrsElapsed", "0.5"} 
                }.Dict, new MockClient());
            }
        }
    }
}
