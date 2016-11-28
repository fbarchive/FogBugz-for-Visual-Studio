using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace FogBugzForVisualStudio
{
    static class RegistryHelper
    {
        public static RegistryKey FogBugzVSKey
        {
            get
            {
                return Registry.CurrentUser.CreateSubKey(@"Software\Fog Creek Software\FogBugz For Visual Studio");
            }
        }

        public static RegistryKey FogBugzVSMachineKey
        {
            get 
            {
                return Registry.LocalMachine.CreateSubKey(@"Software\Fog Creek Software\FogBugz For Visual Studio");
            }
        }

        public static bool? GetBool(RegistryKey obj, String key) {
            var result = obj.GetValue(key, null);
            if (result == null) {
                return null;
            }

            return Convert.ToBoolean(result);
        }
    }
}
