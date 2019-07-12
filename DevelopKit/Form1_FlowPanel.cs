using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

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

        public static void LoadGroupTablePanelConfig(TableLayoutPanel tablePanel, int flowWidth)
        {
            tablePanel.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            tablePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tablePanel.ColumnCount = 1;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tablePanel.Location = new Point(0, 0);
            tablePanel.Name = "tableLayoutPanel1";
            tablePanel.Size = new Size(flowWidth - 23, 0); //由于外层FlowLayoutPanel有垂直滚动条， 会占用一定宽度，这里减去后不会出先横向滚动条
            tablePanel.TabIndex = 0;
            tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }

        public static void LoadGroupTablePanelData(string groupName, TableLayoutPanel tabPanel, List<Property> properties, int propertyHeight)
        {
            Button titleBtn = new Button
            {
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Dock = DockStyle.Fill,
                Width = tabPanel.Width,
                Text = groupName,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0)
            };
            titleBtn.Click += new EventHandler(delegate (object _, EventArgs b)
            {
                if (tabPanel.RowStyles.Count == 1) //展开
                {
                    LoadGroups(tabPanel, properties, propertyHeight);
                }
                else  //收缩合并
                {
                    UnloadGroups(tabPanel, properties, propertyHeight);
                }
            });

            int titleHeight = 30;
            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, titleHeight));
            tabPanel.Height += titleHeight;
            tabPanel.Controls.Add(titleBtn, 0, 0);
        }

        private static void LoadGroups(TableLayoutPanel tabPanel, List<Property> properties, int propertyHeight)
        {
            tabPanel.Height += propertyHeight * properties.Count;
            tabPanel.RowCount += properties.Count;

            int rowIndex = 1;
            foreach (Property property in properties)
            {
                LoadProperty(tabPanel, rowIndex, property, propertyHeight);
                rowIndex++;
            }
        }

        private static void UnloadGroups(TableLayoutPanel tabPanel, List<Property> properties, int propertyHeight)
        {
            tabPanel.Height -= propertyHeight * properties.Count;
            for (int i = 1; i <= properties.Count; i++)
            {
                tabPanel.Controls.RemoveAt(1);
                tabPanel.RowStyles.RemoveAt(1);
                tabPanel.RowCount -= 1;
            }
        }

        //为属性创建单独的pannel用于方便展开和收缩
        private static void LoadProperty(TableLayoutPanel tabPanel, int index, Property property, int propertyHeight)
        {
            tabPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, propertyHeight));
            
            switch (property.Type)
            {
                case PropertyType.Image:
                    Button btn = new Button();
                    btn.Text = "image";
                    tabPanel.Controls.Add(btn, 0, index);
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
                    Console.WriteLine("----------------" + property.Type);
                    break;
            }
        }
    }
}
