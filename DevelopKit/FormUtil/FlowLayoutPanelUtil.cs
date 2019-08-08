using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class FlowLayoutPanelUtil
    {
        public static FlowLayoutPanel CreateFlowLayoutPanel(Scene scene, Panel rightPanel)
        {
            List<Group> groups = scene.GetGroups();
            if (groups == null)
                return null;

            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                Width = rightPanel.Width,
                Visible = false
            };
            GlobalConfig.Controller.Right.RegisterScenePanel(scene.Id, flowLayoutPanel);
            rightPanel.Controls.Add(flowLayoutPanel);

            bool ifSetFields = false;
            foreach (Group group in groups)
            {
                TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                flowLayoutPanel.Controls.Add(tableLayoutPanel);
                bool ok = GlobalConfig.Controller.Right.RegisterGroupOnload(group, tableLayoutPanel);

                if (!ifSetFields)
                {
                    SetFields(flowLayoutPanel);
                    ifSetFields = true;
                }

                TableLayoutPanelUtil.SetFields(tableLayoutPanel, flowLayoutPanel.Width);
                TableLayoutPanelUtil.SetData(tableLayoutPanel, group, GlobalConfig.UiConfig.PropertyRowHeight, true);

                if (!ok)
                    FormUtil.HideGroupPanel(tableLayoutPanel);
            }

            return flowLayoutPanel;
        }

        private static void SetFields(FlowLayoutPanel flowPanel)
        {
            flowPanel.Padding = new Padding(0, 0, 0, 0);
            flowPanel.Margin = new Padding(0, 0, 0, 0);
            flowPanel.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Location = new Point(0, 0);
            //flowPanel.Name = "flowLayoutPanel1";
            flowPanel.TabIndex = 0;
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.White;
        }
    }
}
