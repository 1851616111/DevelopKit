using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

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
