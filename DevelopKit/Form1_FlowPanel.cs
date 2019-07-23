﻿using System;
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
                    GlobalConfig.Controller.ShowGroupOnCenterBoard(tabPanel, group);
                }
                else if ((bool)((Hashtable)tabPanel.Tag)["hide"]) //上次为隐藏，再点击后更新为展开
                {
                    ShowGroupTablePanel(tabPanel, rowHeight);
                    HideGroupBrotherTablePanel(tabPanel, group);
                    GlobalConfig.Controller.ShowGroupOnCenterBoard(tabPanel, group);
                }
                else if (!(bool)((Hashtable)tabPanel.Tag)["hide"])//上次为显示，再点击后更新为隐藏
                {
                    HideGroupTablePanel(tabPanel);
                    GlobalConfig.Controller.HideGroupOnCenterBoard(group, null);
                }
                else
                {
                    Log.Error("Form1_FlowPanel", "table panel button click unknow status", "");
                }
            });

            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GlobalConfig.UiConfig.PropertyTitleHeight));
            tabPanel.Height += GlobalConfig.UiConfig.PropertyTitleHeight;
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

            int rowIndex = 1;
            foreach (Property property in properties)
            {
                if (property.ShowLabel)
                {
                    tabPanel.Height += rowHeight;
                    LoadProperty(tabPanel, rowIndex, property, rowHeight);
                    rowIndex++;
                }
                else
                {
                    LoadProperty(tabPanel, rowIndex, property, rowHeight);
                }
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
            if (tabPanel.Tag == null || (bool)((Hashtable)(tabPanel.Tag))["hide"] == true) //tabPanel 未初始化 
                return;

            ((Hashtable)tabPanel.Tag)["hide"] = true;
            if (tabPanel.RowStyles.Count == 1)
            {
                return;
            }

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

            switch (property.Type)
            {
                case PropertyType.Image:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, propertyHeight);
                    }
                    break;
                case PropertyType.TxtColor:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, propertyHeight);
                    }
                    break;
                case PropertyType.ImageAlpha:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, propertyHeight);
                    }
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

        private static void LoadImageProperty(TableLayoutPanel tabPanel, Property property, int index, int propertyHeight)
        {
            PictureBox cachedPb;
            Image image;
            if (property.RefPropertyId > 0)
            {
                cachedPb = GlobalConfig.Controller.GetPictureBox(property.RefPropertyId);
                if (cachedPb != null)
                {
                    image = cachedPb.Image;
                }
                else
                {
                    Log.Error("Flow1_FlowPanel", "LoadImageProperty", string.Format("property id={0} Get picture box nil", property.Id));
                    image = Image.FromFile(GlobalConfig.GetProjectResourcesDir() + property.Value);
                }
            }
            else
            {
                image = Image.FromFile(GlobalConfig.GetProjectResourcesDir() + property.Value);
            }

            PictureBox pictureBox = new PictureBox
            {
                Name = property.GetPictureBoxId(),
                Size = new Size(0, 0),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 0),
                BackColor = Color.Gray,
                Visible = false,

            };
            //每个Property的PictureBox都先注册到缓存中， 当有Property需要引用其他Property的图片时，直接取出 
            GlobalConfig.Controller.SetPictureBox(property.Id, pictureBox);

            if (!property.ShowLabel)
            {
                tabPanel.Controls.Add(pictureBox, 0, 0);  //若property不现实标签， 则直接挂载到table的第0行中
                return;
            }

            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, propertyHeight));
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
            };
            tabPanel.Controls.Add(panel, 0, index);
            panel.Controls.Add(pictureBox);

            Label label = new Label
            {
                Text = property.Name,
                Location = new Point(0, 0),
                Font = new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Height = 20,
                Width = 180,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            panel.Controls.Add(label);

            Button button = new Button
            {
                Location = new Point(panel.Width - 100, 0),
                Width = 100,
                Height = 26,
                Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                TextAlign = ContentAlignment.MiddleCenter
            };

            if (property.OptType == PropertyOperateType.ReplaceImage)
            {
                button.Text = "更换图片";
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
                            pictureBox.Refresh();

                            if (openFileDialog.FileName != property.Value)
                            {
                                Property propertyCopy = property.Clone();
                                propertyCopy.Value = openFileDialog.FileName;
                                GlobalConfig.Project.Editer.Add(property.Id, propertyCopy);
                            }
                            else
                            {
                                GlobalConfig.Project.Editer.Remove(property.Id);
                            }
                        }
                        GlobalConfig.Controller.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());

                    }
                    catch (Exception)
                    { }
                });
                panel.Controls.Add(button);
            }
            else if (property.OptType == PropertyOperateType.AlphaWhiteImageSetColor)
            {
                TextBox text = new TextBox
                {
                    AutoSize = false,
                    Width = 35,
                    Height = 25,
                    Location = new Point(label.Width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                    Margin = new Padding(0, 0, 0, 0),
                    BorderStyle = BorderStyle.FixedSingle,
                };

                if (property.DefaultValue != null && property.DefaultValue.Length > 0)
                {
                    try
                    {
                        text.BackColor = RGB(Convert.ToInt32(property.DefaultValue, 16));
                    }
                    catch (Exception ex)
                    {
                    }
                }

                button.Text = "设置颜色";
                button.Click += new EventHandler(delegate (object _, EventArgs b)
                {
                    ColorDialog dialog = new ColorDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (dialog.Color.ToArgb() != text.BackColor.ToArgb())
                        {
                            Property propertyCopy = property.Clone();
                            propertyCopy.DefaultValue = "Ox" + Convert.ToString(dialog.Color.ToArgb(), 16);
                            GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);
                        }
                        else
                        {
                            GlobalConfig.Project.Editer.Remove(property.Id);
                        }

                        PngUtil.SetAlphaWhilteImage((Bitmap)image, dialog.Color);
                        text.BackColor = dialog.Color;
                        pictureBox.Refresh();
                    }
                    GlobalConfig.Controller.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());

                });
                panel.Controls.Add(text);
                panel.Controls.Add(button);
            }
            else if (property.OptType == PropertyOperateType.AlphaWhiteImageSetAlpha)
            {
                TextBox text = new TextBox
                {
                    Text = "255",
                    Width = 35,
                    Location = new Point(label.Width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                    Height = 20,
                    Margin = new Padding(0, 0, 0, 0),
                    Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                };
                if (property.DefaultValue != null && property.DefaultValue.Length > 0)
                {
                    text.Text = property.DefaultValue;
                }

                string oldText = text.Text;
                button.Text = "确定";
                button.Click += new EventHandler(delegate (object _, EventArgs b)
                {
                    if (oldText != text.Text)
                    {
                        Property propertyCopy = property.Clone();
                        propertyCopy.DefaultValue = text.Text;
                        GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);
                    }
                    else {
                        GlobalConfig.Project.Editer.Remove(property.Id);
                    }
                    try
                    {
                        int alphaValue = Convert.ToInt32(text.Text);
                        PngUtil.SetAlphaWhilteImage((Bitmap)image, alphaValue);
                        pictureBox.Refresh();
                        GlobalConfig.Controller.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                    }
                    catch (Exception)
                    { }
                });
                panel.Controls.Add(button);
                panel.Controls.Add(text);
            }
        }

        public static uint ParseRGB(Color color)
        {
            return (uint)(((uint)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }


        private static Color RGB(int color)
        {
            int r = 0xFF & color;
            int g = 0xFF00 & color;
            g >>= 8;
            int b = 0xFF0000 & color;
            b >>= 16;
            return Color.FromArgb(r, g, b);
        }
    }
}