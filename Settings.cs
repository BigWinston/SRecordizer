using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Text;

namespace SRecordizer
{
    class Settings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("white")]
        public Color BackgroundColor
        {
            get
            {
                return ((Color)this["BackgroundColor"]);
            }
            set
            {
                this["BackgroundColor"] = (Color)value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool SRecordView_ShowGridlines
        {
            get { return ((bool)this["SRecordView_ShowGridLines"]); }
            set { this["SRecordView_ShowGridLines"] = (bool)value;  }
        }
    }
}
