﻿// Microsoft Office Outlook 2007 Add-in Sample Code
//
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// 

// Added 7/2010 by Adam Ernst. Note:
// If Microsoft makes any code marked as “sample” available on this Web Site 
// without a License Agreement, then that code is licensed to you under 
// the terms of the Microsoft Limited Public License.
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class PictureDispConverter
{
    //IPictureDisp guid
    public static Guid iPictureDispGuid = typeof(stdole.IPictureDisp).GUID;

    /// Converts an Icon into a IPictureDisp
    public static stdole.IPictureDisp ToIPictureDisp(Icon icon)
    {
        PICTDESC.Icon pictIcon = new PICTDESC.Icon(icon);
        return PictureDispConverter.OleCreatePictureIndirect(pictIcon, ref iPictureDispGuid, true);
    }

    /// Converts an image into a IPictureDisp
    public static stdole.IPictureDisp ToIPictureDisp(Image image)
    {
        Bitmap bitmap = (image is Bitmap) ? (Bitmap)image : new Bitmap(image);
        PICTDESC.Bitmap pictBit = new PICTDESC.Bitmap(bitmap);
        return PictureDispConverter.OleCreatePictureIndirect(pictBit, ref iPictureDispGuid, true);
    }


    [DllImport("OleAut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
    private static extern stdole.IPictureDisp OleCreatePictureIndirect([MarshalAs(UnmanagedType.AsAny)] object picdesc, ref Guid iid, bool fOwn);

    private readonly static HandleCollector handleCollector = new HandleCollector("Icon handles", 1000);

    // WINFORMS COMMENT:
    // PICTDESC is a union in native, so we'll just
    // define different ones for the different types
    // the "unused" fields are there to make it the right
    // size, since the struct in native is as big as the biggest
    // union.
    private static class PICTDESC
    {
        //Picture Types
        public const short PICTYPE_UNINITIALIZED = -1;
        public const short PICTYPE_NONE = 0;
        public const short PICTYPE_BITMAP = 1;
        public const short PICTYPE_METAFILE = 2;
        public const short PICTYPE_ICON = 3;
        public const short PICTYPE_ENHMETAFILE = 4;

        [StructLayout(LayoutKind.Sequential)]
        public class Icon
        {
            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(PICTDESC.Icon));
            internal int picType = PICTDESC.PICTYPE_ICON;
            internal IntPtr hicon = IntPtr.Zero;
            internal int unused1 = 0;
            internal int unused2 = 0;

            internal Icon(System.Drawing.Icon icon)
            {
                this.hicon = icon.ToBitmap().GetHicon();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class Bitmap
        {
            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(PICTDESC.Bitmap));
            internal int picType = PICTDESC.PICTYPE_BITMAP;
            internal IntPtr hbitmap = IntPtr.Zero;
            internal IntPtr hpal = IntPtr.Zero;
            internal int unused = 0;

            internal Bitmap(System.Drawing.Bitmap bitmap)
            {
                this.hbitmap = bitmap.GetHbitmap();
            }
        }
    }
}
