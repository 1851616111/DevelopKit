using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public partial class Form1_Image : Form
    {
        public FormDelegate formDelegateHandler;
        public Form1_Image()
        {
            InitializeComponent();
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

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image;

                Color color = bmp.GetPixel(e.X, e.Y);

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
                toolStripStatusLabel2.Text = string.Format("{0},{1},{2} RGB", color.R, color.G, color.B);
                toolStripStatusLabel3.Text = string.Format("{0} × {1}像素", pictureBox1.Image.Width, pictureBox1.Image.Height);
                toolStripStatusLabel4.Text = string.Format("大小:{0}{1}", number.ToString("#.#"), unit);
            }
        }

        private void PictureBox1_Resize(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
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
    }
}
