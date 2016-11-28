## README

### About

FogBugz for Visual Studio is an add-in that allows you to browse your current cases from inside the 
Microsoft Visual Studio IDE. Add-ins were deprecated in Visual Studio 2013, in favor of VSPackage 
extensions. This project will not work with recent versions of Visual Studio.

FogBugz for Visual Studio shows you the same cases you would see from a web browser.

If you want, you can dock the tool window to any edge of the screen by dragging it. We suggest 
docking it to the bottom of Visual Studio, since it works best with a wide window.

If you close the FogBugz for Visual Studio window, you can get it back from the 
View | Other Windows menu.

(c) 2016 Fog Creek Software. All Rights Reserved. This software is licensed under an MIT-style 
license which can be found in the include LICENSE file. The LICENSE file shall be included in all 
copies or substantial portions of the Software.

### System Requirements

 - FogBugz 5.0 or later (FogBugz On Demand, FogBugz On Site, or FogBugz For Your Server)
 - Microsoft Visual Studio 2005/2008/2010/2012/2013

### Building

The environment necessary to build FogBugz for Visual Studio is included as a Visual Studio 2013 
solution. For other versions of Visual Studio, a solution will need to be set up first.

 - Open the "FogBugzForVisualStudio" solution file
 - Click Build > Build Solution

Once the "Build succeeded" banner appears, you should find the "FogBugzForVisualStudio" dll 
and addin files in the "FogBugzForVisualStudio\bin" directory

### Installation

 - Open a compatible version of Microsoft Visual Studio
 - Under Tools > Options, open Environment > Add-in Security
 - Idenitfy an add-in directory, or add a new directory to check for add-ins
 - Copy the built add-in files in the "bin" folder (see **Building**) to the directory identified 
   for add-in detection
 - Close Visual Studio, then re-open Visual Studio running as an Administrator by 
   right-clicking > Run as Administrator
 - Open the Add-in Manager (Tools > Add-in Manager on recent versions) and verify the 
   add-in has loaded

