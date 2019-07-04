using System;
using System.IO;
using System.Collections;
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
    public partial class Form1_Image : Form
    {
        public FormDelegate formDelegateHandler;
        private int imageOriginalWidth;
        private int imageOriginalHeight;
        private Bitmap imageOriginalBitmap;

        private int resetSizeValue;
        private int resetWidth;
        private int resetHeight;
        private int resetColorValue;

        public Form1_Image()
        {
            InitializeComponent();
            label1.Text = "100%";

        }

        private void Form1_Image_Load(object sender, EventArgs e)
        {
        }

        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hashtable hashTable = (Hashtable)this.Tag;
            formDelegateHandler(new OperateFileReuqest(OperateFileType.Save, (string)hashTable["filepath"]));
        }

        private void CloseImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.关闭自身窗体
            this.Close();
            this.Dispose();

            //2. 通知主form 关闭tabpage
            Hashtable hashTable = (Hashtable)this.Tag;
            formDelegateHandler(new OperateFileReuqest(OperateFileType.Close, (string)hashTable["filepath"]));

        }

        private void CopyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    Filter = "PNG|*.png|所有文件|*.*",
                    FilterIndex = 1,
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存文件失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("From1_Image.CopyImageToolStripMenuItem_Click", "保存文件失败", ex.ToString());
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image;

                try
                {
                    Color color = bmp.GetPixel(e.X, e.Y);
                    if (color != null)
                    {
                        toolStripStatusLabel2.Text = string.Format("{0},{1},{2} RGB {3}", color.R, color.G, color.B, color.A);
                    }
                }
                catch (Exception)
                {
                }

                Hashtable hs = (Hashtable)this.Tag;
                string fielpath = (string)hs["filepath"];
                FileInfo fi = new FileInfo(fielpath);
                float number = fi.Length; //B 字节
                string unit = "B";
                if (number > 1024)
                {
                    number /= 1024; //KB
                    unit = "KB";
                }
                if (number > 1024)
                {
                    number /= 1024;  //MB
                    unit = "MB";
                }
                if (number > 1024)
                {
                    number /= 1024;  //GB
                    unit = "GB";
                }

                toolStripStatusLabel1.Text = string.Format("{0},{1}像素", e.X, e.Y);
                toolStripStatusLabel3.Text = string.Format("{0} × {1}像素", pictureBox1.Image.Width, pictureBox1.Image.Height);
                toolStripStatusLabel4.Text = string.Format("大小:{0}{1}", number.ToString("#.#"), unit);
            }
        }

        private void PictureBox1_Resize(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                hScrollBar1.Location = new Point((this.Width - hScrollBar1.Width) / 2, this.Height - 50);
                label1.Location = new Point(hScrollBar1.Location.X + hScrollBar1.Width + 20, this.Height - 55);
                hScrollBar2.Location = new Point(label1.Location.X + label1.Width  + 100, this.Height - 50);
                label2.Location = new Point(hScrollBar2.Location.X - 45  , this.Height - 55);

                if (imageOriginalWidth == 0 && imageOriginalHeight == 0)
                {
                    imageOriginalWidth = pictureBox1.Image.Width;
                    imageOriginalHeight = pictureBox1.Image.Height;
                    imageOriginalBitmap = (Bitmap)pictureBox1.Image.Clone();
                }

                Hashtable hs = (Hashtable)this.Tag;

                string fielpath = (string)hs["filepath"];
                FileInfo fi = new FileInfo(fielpath);
                float number = fi.Length; //B 字节
                string unit = "B";
                if (number > 1024)
                {
                    number /= 1024; //KB
                    unit = "KB";
                }
                if (number > 1024)
                {
                    number /= 1024;  //MB
                    unit = "MB";
                }
                if (number > 1024)
                {
                    number /= 1024;  //GB
                    unit = "GB";
                }

                toolStripStatusLabel3.Text = string.Format("{0} × {1}像素", pictureBox1.Image.Width, pictureBox1.Image.Height);
                toolStripStatusLabel4.Text = string.Format("大小:{0}{1}", number.ToString("#.#"), unit);
            }
        }

        //图片放大操作
        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }

            float MultipleFactor = (100 + hScrollBar1.Value) / 100.0f;
            resetWidth = (int)(imageOriginalWidth * MultipleFactor);
            resetHeight = (int)(imageOriginalHeight * MultipleFactor);

            pictureBox1.Location = new Point((this.Width - resetWidth) / 2, (this.Height - resetHeight) / 2);


            label1.Text = string.Format("{0}%", (MultipleFactor * 100).ToString());

            if (resetColorValue > 0)
            {
                Bitmap newBmp = PngUtil.RelativeChangeColor(imageOriginalBitmap, resetColorValue);
                pictureBox1.Image = KiResizeImage(newBmp, resetWidth, resetHeight); 
            }
            else {
                pictureBox1.Image = KiResizeImage(imageOriginalBitmap, resetWidth, resetHeight);
            }
            resetSizeValue = hScrollBar1.Value;
        }

        //图片滤色选择操作
        private void HScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            if (hScrollBar2.Value < 0 || hScrollBar2.Value > 360)
            {
                return;
            }
            
            if (resetSizeValue > 0)
            {
                Bitmap newImage = KiResizeImage(imageOriginalBitmap, resetWidth, resetHeight);
                pictureBox1.Image = (Image)PngUtil.RelativeChangeColor(newImage, hScrollBar2.Value);
            }
            else {
                pictureBox1.Image = (Image)PngUtil.RelativeChangeColor(imageOriginalBitmap, hScrollBar2.Value);
            }
            resetColorValue = hScrollBar2.Value;
        }

        public Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap map = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(map);
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return map;
            }
            catch
            {
                return null;
            }
        }
    }
}
