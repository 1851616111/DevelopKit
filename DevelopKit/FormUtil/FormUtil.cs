using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class FormUtil
    {
        public static void HideGroupPanel(TableLayoutPanel groupPanel)
        {
            if (IsGroupHide(groupPanel))
            {
                return;
            }

            int rowHeight = GlobalConfig.UiConfig.PropertyRowHeight;

            groupPanel.Height -= (int)((groupPanel.RowStyles.Count - 1) * rowHeight);

            for (int rowIndex = 1; rowIndex <= groupPanel.RowStyles.Count - 1; rowIndex++)
            {
                groupPanel.RowStyles[rowIndex].Height = 0;
            }
        }

        public static void ShowGroup(TableLayoutPanel tabPanel)
        {
            int rowHeight = GlobalConfig.UiConfig.PropertyRowHeight;

            tabPanel.Height += (tabPanel.RowStyles.Count - 1) * rowHeight;

            for (int rowIndex = 1; rowIndex <= tabPanel.RowStyles.Count - 1; rowIndex++)
            {
                tabPanel.RowStyles[rowIndex].Height = rowHeight;
            }
        }


        public static bool IsGroupHide(TableLayoutPanel panel)
        {
            return panel.Height == GlobalConfig.UiConfig.PropertyTitleHeight;
        }
    }
}
