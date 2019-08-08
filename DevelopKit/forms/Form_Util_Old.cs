using System;
using System.IO;
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

        public static void LoadGroupTablePanelData(Group group, TableLayoutPanel tabPanel, List<Property> properties, int rowHeight, bool init)
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
                ClickGroupButton(group, tabPanel, properties, rowHeight);
            });

            //若是初始化，则直接加载
            if (init)
                ClickGroupButton(group, tabPanel, properties, rowHeight);

            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GlobalConfig.UiConfig.PropertyTitleHeight));
            tabPanel.Height += GlobalConfig.UiConfig.PropertyTitleHeight;
            tabPanel.Controls.Add(titleBtn, 0, 0);
        }

        private static void ClickGroupButton(Group group, TableLayoutPanel tabPanel, List<Property> properties, int rowHeight)
        {
            if (tabPanel.Tag == null)  //第一次点击展开需要初始化所有控件
            {
                //LoadGroups(tabPanel, properties, rowHeight);
                HideGroupBrotherTablePanel(tabPanel, group);
                GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, group);
            }
            else if ((bool)((Hashtable)tabPanel.Tag)["hide"]) //上次为隐藏，再点击后更新为展开
            {
                ShowGroupTablePanel(tabPanel, rowHeight);
                HideGroupBrotherTablePanel(tabPanel, group);
                GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, group);
            }
            else if (!(bool)((Hashtable)tabPanel.Tag)["hide"])//上次为显示，再点击后更新为隐藏
            {
                HideGroupTablePanel(tabPanel);
                GlobalConfig.CenterBoardController.UnionGroupsImageParams(group, null);
            }
            else
            {
                Log.Error("Form1_FlowPanel", "table panel button click unknow status", "");
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
        private static void LoadProperty(TableLayoutPanel tabPanel, int index, int maxIndex, Property property, int propertyHeight)
        {
            switch (property.Type)
            {
                case PropertyType.Image:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    }
                    break;
                case PropertyType.TxtColor:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    }
                    break;
                case PropertyType.ImageAlpha:
                    if (property.Value.Length > 0)
                    {
                        LoadImageProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    }
                    break;
                case PropertyType.Int:

                    LoadGroupProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    break;
                case PropertyType.Color:

                    LoadGroupProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    break;
                case PropertyType.Nil:
                    Button btn4 = new Button
                    {
                        Text = "nil"
                    };
                    tabPanel.Controls.Add(btn4, 0, index);
                    break;
                default:
                    break;
            }
        }

        private static void LoadGroupProperty(TableLayoutPanel tabPanel, Property property, int index, int maxIndex, int propertyHeight)
        {
            if (!PropertyOperateType.IsThirdPartType(property.OperateType))
                return;

            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
            };
            tabPanel.Controls.Add(panel, 0, index);

            //获取组件共享的PictureBox
            PictureBox GroupPictureBox = GlobalConfig.CenterBoardController.GetPictureBox(property.GetGroupPictureBoxId());
            if (GroupPictureBox == null)
            {
                GroupPictureBox = new PictureBox
                {
                    Name = property.GetPictureBoxId(),  //TODO to make sure
                    Size = new Size(0, 0),
                    Margin = new Padding(0, 0, 0, 0),
                    Padding = new Padding(0, 0, 0, 0),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(0, 0),
                    BackColor = Color.Gray,
                    Visible = false,
                };

                GlobalConfig.CenterBoardController.SetPictureBox(property.GetGroupPictureBoxId(), GroupPictureBox);
                GroupPictureBox.Tag = new ThirdPartApiClient(property.OperateType, GroupPictureBox, maxIndex + 1);
                panel.Controls.Add(GroupPictureBox);
            }
            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, propertyHeight));

            Label label = new Label
            {
                Text = property.Name,
                Location = new Point(14, 0),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
                Font = new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Height = 20,
                Width = 180,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            TextBox text = new TextBox
            {
                Name = property.GetTextBoxColorID(),
                AutoSize = false,
                Width = 35,
                Height = 25,
                Location = new Point(label.Width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                Margin = new Padding(0, 0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = property.Type
            };
            ((ThirdPartApiClient)(GroupPictureBox.Tag)).SetParam(index, text);
            panel.Controls.Add(label);
            panel.Controls.Add(text);


            if (property.Type == PropertyType.Int)
            {
                text.Text = property.DefaultValue;
                string lastText = text.Text;
                text.TextChanged += new EventHandler(delegate (object _, EventArgs b)
                {
                    if (text.Text.Trim(' ').Length == 0)
                    {
                        text.Text = lastText;
                        return;
                    }


                    int min, max;
                    bool ok = property.GetRangeAllowValue(out min, out max);

                    int textNumber;
                    if (!int.TryParse(text.Text, out textNumber))
                    {
                        if (ok)
                        {
                            MessageBox.Show(string.Format("请输入{0}-{1}的整数", min, max), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("请输入整数", "错误");
                        }

                        text.Text = lastText;
                        return;
                    }


                    if (ok && (textNumber < min || textNumber > max))
                    {
                        MessageBox.Show(string.Format("请输入{0}-{1}的整数", min, max), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        text.Text = lastText;
                        return;
                    }

                    if (text.Text == lastText)
                    {
                        GlobalConfig.Project.Editer.Remove(property.Id);
                    }
                    else
                    {
                        lastText = text.Text;
                        Property propertyCopy = property.Clone();
                        propertyCopy.DefaultValue = text.Text;
                        GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);

                        ((ThirdPartApiClient)(GroupPictureBox.Tag)).Call();
                        GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                    }
                });
            }
            else if (property.Type == PropertyType.Color)
            {
                Button button = new Button
                {
                    Location = new Point(panel.Width - 100, 0),
                    Width = 100,
                    Height = 26,
                    Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                if (property.DefaultValue != null && property.DefaultValue.Length > 0)
                {
                    try
                    {
                        text.BackColor = Color.FromArgb(
                            Convert.ToInt32(property.DefaultValue.Substring(2, 2), 16),
                            Convert.ToInt32(property.DefaultValue.Substring(4, 2), 16),
                            Convert.ToInt32(property.DefaultValue.Substring(6, 2), 16));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
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
                            propertyCopy.DefaultValue = "0x" + dialog.Color.R.ToString("X2") + dialog.Color.G.ToString("X2") + dialog.Color.B.ToString("X2");
                            GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);

                            text.BackColor = dialog.Color;
                            ((ThirdPartApiClient)(GroupPictureBox.Tag)).Call();
                            GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                        }
                        else
                        {
                            text.BackColor = dialog.Color;
                            GlobalConfig.Project.Editer.Remove(property.Id);
                        }
                    }
                });
                panel.Controls.Add(button);
            }

            if (index == maxIndex) //index 从1开始开始计数， 当是最后一个时
            {
                ((ThirdPartApiClient)(GroupPictureBox.Tag)).Call();
                GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
            }
        }



        private static void LoadImageProperty(TableLayoutPanel tabPanel, Property property, int index, int maxIndex, int propertyHeight)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
            };
            tabPanel.Controls.Add(panel, 0, index);

            PictureBox cachedPb;
            Image image;
            if (property.RefPropertyId > 0)
            {
                cachedPb = GlobalConfig.CenterBoardController.GetPictureBox(property.RefPropertyId.ToString());
                if (cachedPb != null)
                {
                    image = cachedPb.Image;
                }
                else
                {
                    Log.Error("Flow1_FlowPanel", "LoadImageProperty", string.Format("property id={0} Get picture box nil", property.Id));
                    image = Image.FromFile(Path.Combine(GlobalConfig.Project.GetUserResourcesDir(), property.Value));
                }
            }
            else
            {
                image = Image.FromFile(Path.Combine(GlobalConfig.Project.GetUserResourcesDir(), property.Value));
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
            panel.Controls.Add(pictureBox);

            SortedDictionary<int, Property> brothers = GlobalConfig.Project.CarConfig.GetGroupSameLayerProperties(property.GroupId, property);
            if (brothers.Count > 1) //包含自己所以大于1
            {
                CheckBox checkBox = new CheckBox
                {
                    Name = property.GetCheckBoxId(),
                    Margin = new Padding(0, 0, 0, 0),
                    Padding = new Padding(0, 0, 0, 0),
                    Checked = brothers.FirstOrDefault().Key == property.Id,
                    Width = 13,
                    Text = "",
                };
                if (!checkBox.Checked)
                    pictureBox.Enabled = false;
                checkBox.CheckedChanged += new System.EventHandler(delegate (object sender, EventArgs e)
                {
                    if (checkBox.Checked)
                    {
                        pictureBox.Enabled = true;
                        foreach (Property brotherProperty in brothers.Values)
                        {
                            if (brotherProperty.GetCheckBoxId() == checkBox.Name)
                                continue;

                            Control[] pbControls = tabPanel.Controls.Find(brotherProperty.GetPictureBoxId(), true);
                            foreach (PictureBox pb in pbControls)
                            {
                                pb.Enabled = false;
                            }

                            Control[] ckControls = tabPanel.Controls.Find(brotherProperty.GetCheckBoxId(), true);
                            foreach (CheckBox ck in ckControls)
                            {
                                ck.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        pictureBox.Enabled = false;
                    }

                    GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                });
                panel.Controls.Add(checkBox);
            }

            //每个Property的PictureBox都先注册到缓存中， 当有Property需要引用其他Property的图片时，直接取出 
            GlobalConfig.CenterBoardController.SetPictureBox(property.Id.ToString(), pictureBox);

            if (!property.ShowLabel)
            {
                tabPanel.Controls.Add(pictureBox, 0, 0);  //若property不现实标签， 则直接挂载到table的第0行中
                return;
            }

            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, propertyHeight));

            Label label = new Label
            {
                Text = property.Name,
                Location = new Point(14, 0),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
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

            if (property.OperateType == PropertyOperateType.ReplaceImage)
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
                            pictureBox.Image.Save(Path.Combine(GlobalConfig.Project.GetUserUploadImageDir(), openFileDialog.SafeFileName)); ;
                            pictureBox.Refresh();

                            if (openFileDialog.FileName != property.Value)
                            {
                                Property propertyCopy = property.Clone();
                                propertyCopy.Value = GlobalConfig.Project.GetPropertyImagePath(openFileDialog.SafeFileName);
                                GlobalConfig.Project.Editer.Add(property.Id, propertyCopy);
                            }
                            else
                            {
                                GlobalConfig.Project.Editer.Remove(property.Id);
                            }
                        }
                        GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());

                    }
                    catch (Exception)
                    { }


                });
                panel.Controls.Add(button);
            }
            else if (property.OperateType == PropertyOperateType.AlphaWhiteImageSetColor)
            {
                TextBox text = new TextBox
                {
                    Name = property.GetTextBoxColorID(),
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
                        text.BackColor = Color.FromArgb(
                            Convert.ToInt32(property.DefaultValue.Substring(2, 2), 16),
                            Convert.ToInt32(property.DefaultValue.Substring(4, 2), 16),
                            Convert.ToInt32(property.DefaultValue.Substring(6, 2), 16));

                        PngUtil.SetAlphaWhilteImage((Bitmap)image, text.BackColor);
                        pictureBox.Refresh();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
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
                            propertyCopy.DefaultValue = "0x" + dialog.Color.R.ToString("X2") + dialog.Color.G.ToString("X2") + dialog.Color.B.ToString("X2");
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
                    GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());

                });
                panel.Controls.Add(text);
                panel.Controls.Add(button);
            }
            else if (property.OperateType == PropertyOperateType.AlphaWhiteImageSetAlpha)
            {
                TextBox text = new TextBox
                {
                    Name = property.GetTextBoxAlphaID(),
                    Width = 35,
                    Location = new Point(label.Width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                    Height = 20,
                    Margin = new Padding(0, 0, 0, 0),
                    Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                };
                if (property.DefaultValue == null || property.DefaultValue.Length == 0)
                {
                    text.Text = "0";
                }
                else
                {
                    text.Text = property.DefaultValue;
                    PngUtil.SetAlphaWhilteImage((Bitmap)image, Convert.ToInt32(property.DefaultValue));
                    pictureBox.Refresh();
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
                    else
                    {
                        GlobalConfig.Project.Editer.Remove(property.Id);
                    }
                    try
                    {
                        int alphaValue = Convert.ToInt32(text.Text);
                        PngUtil.SetAlphaWhilteImage((Bitmap)image, alphaValue);
                        pictureBox.Refresh();
                        GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                    }
                    catch (Exception)
                    { }
                });
                panel.Controls.Add(button);
                panel.Controls.Add(text);
            }
            else if (property.OperateType == PropertyOperateType.ImageFilterColor)
            {
                //    TextBox textBox = new TextBox
                //    {
                //        Width = 35,
                //        Location = new Point(label.Width + 20, 0),
                //        Height = 20,
                //        Margin = new Padding(0, 0, 0, 0),
                //        Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                //        Text = "0"
                //    };

                //    trackBar.ValueChanged += new EventHandler(delegate (object sender, EventArgs e)
                //    {
                //        textBox.Text = trackBar.Value.ToString();
                //        pictureBox.Image = (Image)PngUtil.RelativeChangeColor((Bitmap)image, Convert.ToInt32(trackBar.Value) + 180);
                //        pictureBox.Refresh();
                //        GlobalConfig.CenterBoardController.ShowGroupOnCenterBoard(tabPanel, property.GetGroup());
                //    });
                //    panel.Controls.Add(textBox);
                //    panel.Controls.Add(trackBar);
                //}
            }
        }
    }
}