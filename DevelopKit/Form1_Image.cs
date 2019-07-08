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
        private Image image;
        private int formWidth;
        private int formHeight;
        private FormDelegate formDelegateHandler;
        private string filepath;
        private string filename;

        private int imageOriginalWidth;
        private int imageOriginalHeight;
        private Bitmap imageOriginalBitmap;
        private int resetSizeValue;
        private int resetWidth;
        private int resetHeight;
        private int resetColorValue;

        public Form1_Image(int w, int h, Image image, string img_filepath, string img_filename, FormDelegate delegateFn)
        {
            InitializeComponent();
            label1.Text = "100%";
            this.formWidth = w;
            this.formHeight = h;
            this.image = image;
            this.filepath = img_filepath;
            this.filename = img_filename;
            this.formDelegateHandler = delegateFn;

            pictureBox1.Image = image;
            imageOriginalWidth = image.Width;
            imageOriginalHeight = image.Height;
            imageOriginalBitmap = (Bitmap)pictureBox1.Image.Clone();
        }

        private void Form1_Image_Resize(object sender, EventArgs e)
        {
           
            pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, resetY((this.Height - pictureBox1.Height) / 2));
            hScrollBar1.Location = new Point((this.Width - hScrollBar1.Width) / 2, this.Height - 50);
            label1.Location = new Point(hScrollBar1.Location.X + hScrollBar1.Width + 20, this.Height - 55);
            hScrollBar2.Location = new Point(label1.Location.X + label1.Width + 100, this.Height - 50);
            label2.Location = new Point(hScrollBar2.Location.X - 45, this.Height - 55);

            this.Show();
        }
   
        private void resetForm()
        {

            if (image == null)
            {
                return;
            }
       
            FileInfo fi = new FileInfo(filepath);
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

            if (pictureBox1.Image.Width != imageOriginalWidth && pictureBox1.Image.Height != imageOriginalHeight)
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Image, filepath));
            }
            else
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Image, filepath));
            }
        }

        private void Form1_Image_Load(object sender, EventArgs e)
        {
            resetForm();
        }

        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(filepath);
                pictureBox1.Image.Save(filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存文件失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("From1_Image.SaveImageToolStripMenuItem_Click", "保存文件失败", ex.ToString());
            }
            formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Image, filepath));
        }

        private void CloseImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.关闭自身窗体
            this.Close();
            this.Dispose();

            //2. 通知主form 关闭tabpage
            Hashtable hashTable = (Hashtable)this.Tag;
            formDelegateHandler(new FormRequest(RequestType.Close, FileType.Image, filepath));
        }

        private void CopyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    Filter = "PNG|*.png|所有文件|*.*",
                    FileName = filename,
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
                        toolStripStatusLabel2.Text = string.Format("Alpha:{0}, RGB:({1},{2},{3})", color.A, color.R, color.G, color.B);
                    }
                }
                catch (Exception)
                {
                }

                FileInfo fi = new FileInfo(filepath);
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

            pictureBox1.Location = new Point((this.Width - resetWidth) / 2, resetY((this.Height - resetHeight) / 2));


            label1.Text = string.Format("{0}%", (MultipleFactor * 100).ToString());

            if (resetColorValue > 0)
            {
                Bitmap newBmp = PngUtil.RelativeChangeColor(imageOriginalBitmap, resetColorValue);
                pictureBox1.Image = KiResizeImage(newBmp, resetWidth, resetHeight);
            }
            else
            {
                pictureBox1.Image = KiResizeImage(imageOriginalBitmap, resetWidth, resetHeight);
            }
            resetSizeValue = hScrollBar1.Value;

            if (resetSizeValue != 0)
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Image, filepath));
            }
            else
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Image, filepath));
            }
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
            else
            {
                pictureBox1.Image = (Image)PngUtil.RelativeChangeColor(imageOriginalBitmap, hScrollBar2.Value);
            }
            resetColorValue = hScrollBar2.Value;

            if (resetColorValue != 0)
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Image, filepath));
            }
            else
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Image, filepath));
            }
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

        private void 纯白文字改色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = PngUtil.ChangeWhiteColor((Bitmap)pictureBox1.Image, colorDialog.Color);
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Image, filepath));
            }
        }

        private int resetY(int y)
        {
            if (y > 100)
            {
                return y > 0 ? y - y / 5 : 0;

            }
            else
            {
                return y > 0 ? y - y / 4 : 0;
            }
        }
    }
}
