using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public partial class Form4_Filter_Color2_Util : Form
    {
        private int mininumFormWidth = 500;
        private int miniumFormHeight = 400;

        bool startDraw;
        Point startPoint;
        Image originalImg;

        public Form4_Filter_Color2_Util()
        {
            InitializeComponent();
        }

        private void Form4_Filter_Color2_Util_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (originalImg == null) return;

            startDraw = true;
            startPoint = new Point(e.X, e.Y);

            toolStripStatusLabel2.Text = string.Format("起始 ({0},{1})", e.X, e.Y);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = string.Format("鼠标 ({0},{1})", e.X, e.Y);
            if (startDraw)
            {
                Image img = (Image)originalImg.Clone();
                Bitmap bmp = new Bitmap(img);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Point p2 = new Point(e.X, startPoint.Y);
                    Point p3 = new Point(startPoint.X, e.Y);

                    g.DrawLine(new Pen(Color.Black), startPoint, p2);
                    g.DrawLine(new Pen(Color.Black), startPoint, p3);
                    g.DrawLine(new Pen(Color.Black), new Point(e.X, e.Y), p2);
                    g.DrawLine(new Pen(Color.Black), new Point(e.X, e.Y), p3);
                    g.Dispose();
                }

                pictureBox1.Image = bmp;
            }
            
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            startDraw = false;
            toolStripStatusLabel3.Text = string.Format("宽高 ({0},{1})", 
                Math.Abs(e.X - startPoint.X), Math.Abs(e.Y - startPoint.Y));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImg = Image.FromFile(openFileDialog.FileName);

                    if (originalImg.Width < mininumFormWidth || originalImg.Height < miniumFormHeight)
                    {
                        pictureBox1.Width = mininumFormWidth;
                        pictureBox1.Height = miniumFormHeight;
                        this.Width = mininumFormWidth + 100;
                        this.Height = miniumFormHeight + 150;
                    }
                    else
                    {
                        pictureBox1.Width = originalImg.Width;
                        pictureBox1.Height = originalImg.Height;
                        this.Width = originalImg.Width + 100;
                        this.Height = originalImg.Height + 150;
                    }
                    pictureBox1.Image = originalImg;
                    this.Text += string.Format("  {0} ({1}*{2})", openFileDialog.SafeFileName, originalImg.Width, originalImg.Height);
                    toolStripStatusLabel2.Text = "";
                    toolStripStatusLabel3.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传图片失败, 请重新上传", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
