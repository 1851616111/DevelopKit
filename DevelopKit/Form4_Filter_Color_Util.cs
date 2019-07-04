using System;
using System.IO;
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
    public partial class Form4_Filter_Color_Util : Form
    {
        public Form4_Filter_Color_Util()
        {
            InitializeComponent();
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
            flowLayoutPanel1.Controls.Add(pictureBox1);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    Image image = Image.FromFile(Path.GetFullPath(openFileDialog.FileName));

                    PictureBox pb = new PictureBox
                    {
                        Image = image,
                        BorderStyle = BorderStyle.FixedSingle,
                        Width = image.Width,
                        Height = image.Height,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
           
                    flowLayoutPanel1.Controls.Remove(pictureBox1);
                    flowLayoutPanel1.Controls.Add(pb);
                    flowLayoutPanel1.Controls.Add(pictureBox1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开文件失败" + ex.ToString());
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color targetColor = colorDialog.Color;
                foreach (PictureBox pb in flowLayoutPanel1.Controls)
                {
                    if (pb == pictureBox1)
                    {
                        continue;
                    }

                    Bitmap newbt = PngUtil.FilPic((Bitmap)pb.Image, targetColor);
                    pb.Image = (Image)newbt;
                }
            }
            else
            {
                MessageBox.Show("修改色值失败");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    int i = 1;
                    foreach (PictureBox pb in flowLayoutPanel1.Controls)
                    {
                        if (pb == pictureBox1)
                        {
                            continue;
                        }
                        string output_path = Path.Combine(folderBrowserDialog.SelectedPath, string.Format(@"{0}.png", i));

                        pb.Image.Save(output_path);
                        i++;
                    }
                }

                MessageBox.Show("批量保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败");
            }
        }

        private void Form4_Filter_Color_Util_Load(object sender, EventArgs e)
        {

        }
    }
}
