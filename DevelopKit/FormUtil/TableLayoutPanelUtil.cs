using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class TableLayoutPanelUtil
    {
        public static void SetFields(TableLayoutPanel tablePanel, int flowWidth)
        {
            tablePanel.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            tablePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tablePanel.ColumnCount = 1;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tablePanel.Location = new Point(0, 0);
            //tablePanel.Name = group.GetTablePanelId();
            tablePanel.Size = new Size(flowWidth - 23, 0); //由于外层FlowLayoutPanel有垂直滚动条， 会占用一定宽度，这里减去后不会出先横向滚动条
            tablePanel.TabIndex = 0;
        }

        public static void SetData(TableLayoutPanel tabPanel, Group group, int rowHeight, bool init)
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
                FormEvent.ClickGroup(group);
            });

            LoadGroupProperties(tabPanel, group.GetProperties(), rowHeight);

            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GlobalConfig.UiConfig.PropertyTitleHeight));
            tabPanel.Height += GlobalConfig.UiConfig.PropertyTitleHeight;
            tabPanel.Controls.Add(titleBtn, 0, 0);
        }


        private static void LoadGroupProperties(TableLayoutPanel tabPanel, List<Property> properties, int rowHeight)
        {
            int rowIndex = 1;
            int maxRowIndex = properties.Count;
            foreach (Property property in properties)
            {
                if (property.ShowLabel)
                {
                    tabPanel.Height += rowHeight;
                    LoadProperty(tabPanel, rowIndex, maxRowIndex, property, rowHeight);
                    rowIndex++;
                }
                else
                {
                    LoadProperty(tabPanel, rowIndex, maxRowIndex, property, rowHeight);
                }
            }
        }

        //为属性创建单独的pannel用于方便展开和收缩
        private static void LoadProperty(TableLayoutPanel tabPanel, int index, int maxIndex, Property property, int propertyHeight)
        {
            switch (property.Type)
            {
                case PropertyType.Image:
                        SetRow(tabPanel, property, index, maxIndex, propertyHeight);
                    break;
                case PropertyType.TxtColor:
                    if (property.Value.Length > 0)
                    {
                        SetRow(tabPanel, property, index, maxIndex, propertyHeight);
                    }
                    break;
                case PropertyType.ImageAlpha:
                    if (property.Value.Length > 0)
                    {
                        SetRow(tabPanel, property, index, maxIndex, propertyHeight);
                    }
                    break;
                case PropertyType.Int:

                    //LoadGroupProperty(tabPanel, property, index, maxIndex, propertyHeight);
                    break;
                case PropertyType.Color:

                    //LoadGroupProperty(tabPanel, property, index, maxIndex, propertyHeight);
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

        private static void SetRow(TableLayoutPanel tabPanel, Property property, int index, int maxIndex, int rowHeight)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
            };
            tabPanel.Controls.Add(panel, 0, index);

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
                CacheImageProperty(property);
                panel.Controls.Add(button);

                button.Text = "更换图片";
                button.Click += new EventHandler(delegate (object _, EventArgs b)
                {
                    FormEvent.ReplaceImage(property);
                });

            }
            else if (property.OperateType == PropertyOperateType.AlphaWhiteImageSetColor)
            {
                TextBox textBox = CacheSetImageColorProperty(property, label.Width);

                panel.Controls.Add(textBox);
                panel.Controls.Add(button);

                button.Text = "设置颜色";
                button.Click += new EventHandler(delegate (object _, EventArgs b)
                {
                    FormEvent.SetImageColor(property, textBox);
                });
            }
            else if (property.OperateType == PropertyOperateType.AlphaWhiteImageSetAlpha)
            {
                TextBox textBox = CacheSetImageAlphaProperty(property, label.Width);
                panel.Controls.Add(button);
                panel.Controls.Add(textBox);

                string oldText = textBox.Text;
                button.Text = "确定";
                button.Click += new EventHandler(delegate (object _, EventArgs b)
                {
                    FormEvent.SetAlphaImageColor(property, ref oldText, textBox.Text);
                });
            }
            else if (property.OperateType == PropertyOperateType.ImageFilterColor)
            {
                TextBox textBox = new TextBox
                {
                    Width = 35,
                    Location = new Point(label.Width + 20, 0),
                    Height = 20,
                    Margin = new Padding(0, 0, 0, 0),
                    Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                    Text = "0"
                };
                TrackBar trackBar = CacheFilterImageColorProperty(property, label.Width);
                panel.Controls.Add(textBox);
                panel.Controls.Add(trackBar);

                trackBar.ValueChanged += new EventHandler(delegate (object sender, EventArgs e)
                {
                    textBox.Text = trackBar.Value.ToString();
                    FormEvent.FilterImageColor(property);
                });
            }
            else if (property.OperateType == PropertyOperateType.Nil)
            {
                CacheImageProperty(property);
            }
            else
                return;
            
            if (property.ShowLabel)
                tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            else if (index == maxIndex && maxIndex == 1)
                tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));

        }

        private static Image CacheImageProperty(Property property)
        {
            if (property.RefPropertyId == 0)
            {
                Image image = Image.FromFile(Path.Combine(GlobalConfig.Project.GetUserResourcesDir(), property.Value));
                GlobalConfig.Controller.ShareCache.ShareImage.Set(property.Id, image);
                return image;
            }
            else
            {
                GlobalConfig.Controller.ShareCache.ShareImage.Set(property.Id, property.RefPropertyId);
                return GlobalConfig.Controller.ShareCache.ShareImage.Get(property.Id);
            }
        }

        private static TextBox CacheSetImageColorProperty(Property property, int width)
        {
            TextBox textBox = new TextBox
            {
                AutoSize = false,
                Width = 35,
                Height = 25,
                Location = new Point(width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                Margin = new Padding(0, 0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle,
            };

            Image image = CacheImageProperty(property);

            if (property.DefaultValue != null && property.DefaultValue.Length > 0)
            {
                try
                {
                    textBox.BackColor = Color.FromArgb(
                        Convert.ToInt32(property.DefaultValue.Substring(2, 2), 16),
                        Convert.ToInt32(property.DefaultValue.Substring(4, 2), 16),
                        Convert.ToInt32(property.DefaultValue.Substring(6, 2), 16));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                PngUtil.SetAlphaWhilteImage((Bitmap)image, textBox.BackColor);
            }

            return textBox;
        }

        private static TextBox CacheSetImageAlphaProperty(Property property, int width)
        {
            Image image = CacheImageProperty(property);

            TextBox textBox = new TextBox
            {
                Width = 35,
                Location = new Point(width + GlobalConfig.UiConfig.PropertyLabelMargin, 0),
                Height = 20,
                Margin = new Padding(0, 0, 0, 0),
                Font = new Font("微软雅黑", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
            };
            if (property.DefaultValue == null || property.DefaultValue.Length == 0)
            {
                textBox.Text = "0";
            }
            else {
                PngUtil.SetAlphaWhilteImage((Bitmap)image, Convert.ToInt32(property.DefaultValue));
            }
            textBox.Text = property.DefaultValue;
            GlobalConfig.Controller.ShareCache.ShareTextBox.Set(property.Id, textBox);
            return textBox;
        }

        private static TrackBar CacheFilterImageColorProperty(Property property, int width)
        {
            TrackBar trackBar = new TrackBar
            {
                Location = new Point(width - 150, 0),
                Size = new Size(150, 45),
                Maximum = 180,
                Minimum = -180,
                Value = 0,
                TickFrequency = 30,
            };
            CacheImageProperty(property);
            GlobalConfig.Controller.ShareCache.ShareTrackBar.Set(property.Id, trackBar);
            return trackBar;
        }
    }
}
