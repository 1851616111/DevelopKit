using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace DevelopKit
{
    public static class Form1_FlowPanel
    {
        public static void LoadFlowPanelConfig(FlowLayoutPanel flowPanel)
        {
            flowPanel.Padding = new Padding(0, 0, 0, 0);
            flowPanel.Margin = new Padding(0, 0, 0, 0);
            flowPanel.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Location = new Point(0, 0);
            flowPanel.Name = "flowLayoutPanel1";
            flowPanel.TabIndex = 0;
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.White;
        }

        public static void LoadGroupTablePanelConfig(TableLayoutPanel tablePanel, int flowWidth, Group group)
        {
            tablePanel.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            tablePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tablePanel.ColumnCount = 1;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tablePanel.Location = new Point(0, 0);
            tablePanel.Name = group.GetTablePanelId();
            tablePanel.Size = new Size(flowWidth - 23, 0); //由于外层FlowLayoutPanel有垂直滚动条， 会占用一定宽度，这里减去后不会出先横向滚动条
            tablePanel.TabIndex = 0;
        }

        public static void LoadGroupTablePanelData(Group group, TableLayoutPanel tabPanel, List<Property> properties, int rowHeight)
        {

            Button titleBtn = new Button
            {
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Dock = DockStyle.Fill,
                Width = tabPanel.Width,
                Text = group.Name,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0)
            };
            titleBtn.Click += new EventHandler(delegate (object _, EventArgs b)
            {
                if (tabPanel.Tag == null)  //第一次点击展开需要初始化所有控件
                {
                    LoadGroups(tabPanel, properties, rowHeight);
                    CenterBoardController.DrawGroupAndSceneView(tabPanel, group.Sceneid, group.Id, group.LayerIndex);
                }
                else if ((bool)((Hashtable)tabPanel.Tag)["hide"]) //上次为隐藏，再点击后更新为展开
                {
                    ShowGroupTablePanel(tabPanel, rowHeight);
                    HideGroupBrotherTablePanel(tabPanel, group);
                    CenterBoardController.DrawGroupAndSceneView(tabPanel, group.Sceneid, group.Id, group.LayerIndex);
                }
                else if (!(bool)((Hashtable)tabPanel.Tag)["hide"])//上次为显示，再点击后更新为隐藏
                {
                    HideGroupTablePanel(tabPanel);
                    CenterBoardController.DrawSceneView(group.Sceneid, group.LayerIndex, null);
                }
                else
                {
                    Log.Error("Form1_FlowPanel", "table panel button click unknow status", "");
                }
            });

            int titleHeight = 30;
            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, titleHeight));
            tabPanel.RowCount++;
            tabPanel.Height += titleHeight;
            tabPanel.Controls.Add(titleBtn, 0, 0);
        }

        private static void LoadGroups(TableLayoutPanel tabPanel, List<Property> properties, int rowHeight)
        {
            if (tabPanel.Tag == null)
            {
                tabPanel.Tag = new Hashtable
                {
                    {"hide", false }
                };
            }

            tabPanel.Height += rowHeight * properties.Count;
            tabPanel.RowCount += properties.Count;

            int rowIndex = 1;
            foreach (Property property in properties)
            {
                LoadProperty(tabPanel, rowIndex, property, rowHeight);
                rowIndex++;
            }
        }

        private static void HideGroupBrotherTablePanel(TableLayoutPanel tabPanel, Group group)
        {
            //获取相同优先级的group
            List<Group> brotherGroups = GlobalConfig.Project.CarConfig.ListGroupByLayerId(group.Sceneid, group.LayerIndex);
            foreach (Group brotherGroup in brotherGroups)
            {
                if (brotherGroup.Id == group.Id)
                    continue;

                Control[] tabControls = ((FlowLayoutPanel)tabPanel.Parent).Controls.Find(brotherGroup.GetTablePanelId(), true);
                if (tabControls.Length == 0)
                {
                    Log.Error("Form1_FlowPanel", "获取优先级一致的兄弟组失败", "");
                    continue;
                }

                HideGroupTablePanel((TableLayoutPanel)tabControls[0]);
            }
        }

        private static void HideGroupTablePanel(TableLayoutPanel tabPanel)
        {
            if (Form1_Util.IsTablePanelHide(tabPanel)) //tabPanel 未初始化 
                return;

            ((Hashtable)tabPanel.Tag)["hide"] = true;
            tabPanel.Height -= (int)((tabPanel.RowStyles.Count - 1) * (tabPanel.RowStyles[1].Height));

            for (int rowIndex = 1; rowIndex <= tabPanel.RowStyles.Count - 1; rowIndex++)
            {
                tabPanel.RowStyles[rowIndex].Height = 0;
            }
        }

        private static void ShowGroupTablePanel(TableLayoutPanel tabPanel, int rowHeight)
        {
            ((Hashtable)tabPanel.Tag)["hide"] = false;
            tabPanel.Height += (tabPanel.RowStyles.Count - 1) * rowHeight;

            for (int rowIndex = 1; rowIndex <= tabPanel.RowStyles.Count - 1; rowIndex++)
            {
                tabPanel.RowStyles[rowIndex].Height = rowHeight;
            }
        }

        //为属性创建单独的pannel用于方便展开和收缩
        private static void LoadProperty(TableLayoutPanel tabPanel, int index, Property property, int propertyHeight)
        {
            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, propertyHeight));

            switch (property.Type)
            {
                case PropertyType.Image:
                    if (property.Value.Length > 0)
                    {
                        Panel panel = new Panel
                        {
                            Dock = DockStyle.Fill,
                        };
                        Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + @"Resources\project\" + property.Value);
                        PictureBox pictureBox = new PictureBox
                        {
                            Name = property.GetPictureBoxId(),
                            Size = new Size(50, 30),
                          
                            Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Resources\project\" + property.Value),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(10, 5),
                        };

                        Button button = new Button
                        {
                            Text = "更换",
                            Location = new Point(80, 5),
                        };

                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(button);

                        button.Click += new EventHandler(delegate (object _, EventArgs b)
                        {
                            try
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog
                                {
                                    Filter = "Png|*.png|Jpg|*.jpg"
                                };
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                                    GlobalConfig.Project.SetPropertyValueById(property.Id, openFileDialog.FileName);
                                }

                                CenterBoardController.DrawGroupAndSceneView(tabPanel, property.SceneId, property.GroupId, property.GroupLayerIdx);
                            }
                            catch (Exception)
                            { }
                        });
                        tabPanel.Controls.Add(panel, 0, index);
                    }

                    break;
                case PropertyType.TxtColor:
                    Button btn2 = new Button();
                    btn2.Text = "txt_color";
                    tabPanel.Controls.Add(btn2, 0, index);

                    break;
                case PropertyType.ImageAlpha:
                    Button btn3 = new Button();
                    btn3.Text = "txt_color";
                    tabPanel.Controls.Add(btn3, 0, index);
                    break;
                case PropertyType.Nil:
                    Button btn4 = new Button();
                    btn4.Text = "nil";
                    tabPanel.Controls.Add(btn4, 0, index);
                    break;
                default:
                    break;
            }
        }
    }
}
