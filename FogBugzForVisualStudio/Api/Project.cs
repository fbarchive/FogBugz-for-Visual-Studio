using System;
using System.Collections.Generic;
using System.Text;

namespace FogBugzForVisualStudio.Api
{
    public class Project
    {
        public readonly int ixProject;
        public readonly string sProject;

        public Project(int ixProject, string sProject)
        {
            this.ixProject = ixProject;
            this.sProject = sProject;
        }

        public override string ToString()
        {
            return sProject;
        }
    }
}
