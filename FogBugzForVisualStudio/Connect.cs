using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;

using Extensibility;

using Microsoft.VisualStudio.CommandBars;

namespace FogBugzForVisualStudio
{

    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {

        private Mutex mut;

        private static string windowGuid = "{19D486CC-2C31-4251-A21E-3879483C94A8}";

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        private EnvDTE.Window FogBugzWindow()
        {
            string ctlProgID = "FogBugzForVisualStudio.FogBugzCtl";
            string asmPath = Assembly.GetExecutingAssembly().CodeBase;

            // Create and show the tool window
            EnvDTE80.Windows2 toolWins = (Windows2)_applicationObject.Windows;

            // might already exist (but user closed it) -- just make it visible.
            foreach (EnvDTE.Window w in toolWins)
            {
                if (w.ObjectKind == windowGuid)
                {
                    try
                    {
                        w.Visible = WindowVisible;
                        return w;
                    }
                    catch
                    {
                        // If we can't set Visible, we'll have to create
                        // it again. This happens if they removed the add-in
                        // using the add-in manager.
                    }
                }
            }

            object objTemp = null;
            EnvDTE.Window toolWin = toolWins.CreateToolWindow2(_addInInstance, asmPath, ctlProgID, "FogBugz", windowGuid, ref objTemp);

            if (toolWin == null)
            {
                return null;
            }

            FogBugzCtl ctl = (FogBugzCtl)objTemp;
            ctl.OnShowUrl += OnShowUrl;

            ResourceManager rm = new ResourceManager("FogBugzForVisualStudio.Resource1", GetType().Assembly);
            Bitmap img = (Bitmap)rm.GetObject("imgFogBugz");

            toolWin.SetTabPicture(PictureDispConverter.ToIPictureDisp(img));

            return toolWin;
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            // Create a mutex which will be recognized by the SETUP program to prevent
            // trying to run it while the add-in is loaded.
            try
            {
                mut = new Mutex(true, "FogBugzForVisualStudioRunning");
            }
            catch { }

            try
            {
                _applicationObject = (DTE2)application;
                _addInInstance = (AddIn)addInInst;

                // make a menu item to re-show the FogBugz add-in, in case they close it

                // Documentation suggests checking for connectMode == ext_ConnectMode.ext_cm_UISetup before you
                // do this. Unfortunately, we'll only see the flag once this way for each VS user.
                // See http://support.microsoft.com/kb/555321 for details. Instead, we always check if the command
                // exists, and add it if it doesn't.

                Commands2 cmds = (Commands2)_applicationObject.Commands;

                object[] ContextUIGuids = new object[] { };
                try
                {
                    _cmd = cmds.Item("FogBugzForVisualStudio.Connect.FogBugz", -1);
                }
                catch
                {
                    try
                    {
                        _cmd = cmds.AddNamedCommand2(_addInInstance,
                                                     "FogBugz",
                                                     "FogBugz",
                                                     "Browse cases from FogBugz in a tool window",
                                                     true,
                                                     8, // icon -- google "FaceID Browser" to see what the choices are
                                                     ref ContextUIGuids,
                                                     (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                                                     (int)vsCommandStyle.vsCommandStylePictAndText,
                                                     vsCommandControlType.vsCommandControlTypeButton);

                        Microsoft.VisualStudio.CommandBars.CommandBar mnu = ((CommandBars)_applicationObject.CommandBars)["MenuBar"];

                        // Find the "View | Other Windows" submenu, and add our control there.
                        // CommandBar.Name is not localized, so this should work in all localized versions of Visual Studio.
                        foreach (CommandBarControl ctl in mnu.Controls)
                        {
                            if (!(ctl is CommandBarPopup)) continue;
                            var popup = (CommandBarPopup)ctl;
                            if (!(popup.CommandBar.Name == "View")) continue;
                            foreach (CommandBarControl subctl in popup.Controls)
                            {
                                if (!(subctl is CommandBarPopup)) continue;
                                var subpopup = (CommandBarPopup)subctl;
                                if (subpopup.CommandBar.Name == "Other Windows")
                                {
                                    _cmd.AddControl(subpopup.CommandBar, 1);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        BugScout.ReportException(ex, FrmtDTE());
                        MessageBox.Show("Trouble adding command for FogBugz Add-In: " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex, FrmtDTE());
                MessageBox.Show("Trouble loading FogBugz Add-In: " + ex.Message);
            }
        }

        private void OnShowUrl(string s)
        {
            try
            {
                System.Diagnostics.Process.Start(s);
            }
            catch (Exception ex)
            {
                BugScout.ReportException(ex, FrmtDTE());
                MessageBox.Show("FogBugz Add-In had trouble loading a URL: " + ex.Message);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            try
            {
                // Create and show the tool window
                var window = FogBugzWindow();
                window.Visible = WindowVisible;
            }
            catch (Exception ex) 
            {
                BugScout.ReportException(ex, FrmtDTE());
                throw;
            }
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            try
            {
                // Remember whether the FogBugz tool was visible or not.
                // If it was invisible, we won't ShowFogBugzWindow the next time we start up.
                EnvDTE80.Windows2 toolWins = (Windows2)_applicationObject.Windows;
                foreach (EnvDTE.Window w in toolWins)
                {
                    if (w.ObjectKind == windowGuid)
                    {
                        try
                        {
                            WindowVisible = w.Visible;
                            return;
                        }
                        catch
                        {
                        }
                    }
                }
                WindowVisible = false;
            }
            catch(Exception ex)
            {
                BugScout.ReportException(ex, FrmtDTE());
            }
        }

        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            try
            {
                if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
                {
                    if (commandName == "FogBugzForVisualStudio.Connect.FogBugz")
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                BugScout.ReportException(ex, FrmtDTE());
            }
        }

        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            try
            {
                handled = false;
                if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
                {
                    if (commandName == "FogBugzForVisualStudio.Connect.FogBugz")
                    {
                        FogBugzWindow().Visible = true;
                        handled = true;
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                BugScout.ReportException(ex, FrmtDTE());
            }
        }

        private bool WindowVisible
        {
            get
            {
                return RegistryHelper.FogBugzVSKey.GetValue("Visible", true) as bool? ?? true;
            }

            set
            {
                RegistryHelper.FogBugzVSKey.SetValue("Visible", value);
            }
        }

        public static bool ReportErrors
        {
            get
            {
                var user = RegistryHelper.GetBool(RegistryHelper.FogBugzVSKey, "ReportErrors");
                if (user == null)
                {
                    return RegistryHelper.GetBool(RegistryHelper.FogBugzVSMachineKey, "ReportErrors") ?? false;
                }

                return user ?? false;
            }

            set
            {
                RegistryHelper.FogBugzVSKey.SetValue("ReportErrors", value);
            }
        }

        public String FrmtDTE() { 
            if(_applicationObject == null){
                return "";
            }

            return String.Format(
                "IDE Full Name: {0}\n" +
                "IDE Version: {1}\n" +
                "IDE Edition: {2}\n" +
                "IDE Locale: {3}",
                _applicationObject.FullName, _applicationObject.Version, 
                _applicationObject.Edition, _applicationObject.LocaleID
            );
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private Command _cmd;
    }
}