using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace FogBugzForVisualStudio.Api
{
    public class Category
    {
        private class IconTypeImageAttribute : Attribute
        {
            public readonly string FileName;

            public IconTypeImageAttribute(String fileName)
            {
                this.FileName = fileName;
            }
        }

        public enum IconType
        {
            [IconTypeImageAttribute("icon_none")] 
            None = 0,
            [IconTypeImageAttribute("icon_bug")] 
            Bug = 1,
            [IconTypeImageAttribute("icon_feature")]
            Feature = 2,
            [IconTypeImageAttribute("icon_inquiry")]
            Inquiry = 3,
            [IconTypeImageAttribute("icon_scheduleitem")]
            ScheduleItem = 4,
            [IconTypeImageAttribute("icon_bug_error")]
            BugError = 5,
            [IconTypeImageAttribute("icon_wrench")]
            Wrench = 6,
            [IconTypeImageAttribute("icon_magnifier")]
            Magnifier = 7,
            [IconTypeImageAttribute("icon_key")]
            Key = 8,
            [IconTypeImageAttribute("icon_error")]
            Error = 9
        }

        public int ixCategory { get; private set; }
        public string sCategory { get; private set; }
        public bool fDeleted { get; private set; }
        public int ixStatusDefault { get; private set; }
        public IconType nIconType { get; private set; }

        public Category(Dictionary<String, String> fields)
        {
            this.ixCategory = Convert.ToInt32(fields["ixCategory"]);
            this.sCategory = fields["sCategory"];
            this.fDeleted = Convert.ToBoolean(fields["fDeleted"]);
            this.ixStatusDefault = Convert.ToInt32(fields["ixStatusDefault"]);
            try
            {
                this.nIconType = (IconType)Enum.Parse(typeof(IconType), fields["nIconType"]);
            }
            catch
            {
                // Ignore in case more icons are added later.
            }
        }

        public override string ToString()
        {
            return sCategory;
        }

        private static Dictionary<IconType, Bitmap> imageCache;

        /// <summary>
        /// Get the Category image. Properly belongs in the view, but .NET's bindings give us grief
        /// if we try to format an image in the DataGridView.CellFormatting event. Just give in and 
        /// put it in the model.
        /// </summary>
        public Bitmap Image
        {
            get
            {
                if (imageCache == null)
                {
                    imageCache = new Dictionary<IconType, Bitmap>();
                    ResourceManager manager = new ResourceManager("FogBugzForVisualStudio.Resource1", GetType().Assembly);
                    
                    foreach (FieldInfo field in typeof(IconType).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
                    {
                        object[] attrs = field.GetCustomAttributes(true);
                        string fileName = null;
                        foreach (object attr in attrs)
                        {
                            if (attr.GetType().Equals(typeof(IconTypeImageAttribute)))
                            {
                                fileName = ((IconTypeImageAttribute)attr).FileName;
                                break;
                            }
                        }

                        if (fileName != null)
                        {
                            imageCache[(IconType)field.GetValue(null)] = (Bitmap)manager.GetObject(fileName);
                        }
                    }
                }
                return imageCache[nIconType];
            }
        }
    }
}
