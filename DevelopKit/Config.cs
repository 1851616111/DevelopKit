using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class GlobalConfig
    {
        public static Project Project;
        public static CenterBoardController Controller;
        public static UIConfig UiConfig = new UIConfig {
            PropertyLabelMargin = 50,
            PropertyRowHeight = 35,
            PropertyTitleHeight = 30
        };

        public static string GetProjectResourcesDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory + Project.CarInfo.ResourcesDir;
        }
    }

    public class UIConfig
    {
        public int PropertyLabelMargin;
        public int PropertyTitleHeight;
        public int PropertyRowHeight;
    }
}
