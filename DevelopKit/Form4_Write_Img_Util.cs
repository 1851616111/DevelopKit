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
    public partial class Form4_Write_Img_Util : Form
    {
        private Image image;
        private Font font;
        private Color color;

        public Form4_Write_Img_Util()
        {
            InitializeComponent();
            button3.Enabled = false;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileAbsName = Path.GetFullPath(openFileDialog.FileName);
                    image = Image.FromFile(fileAbsName);
                    label1.Text = string.Format("添加图片成功。({0}*{1}{2})", image.Width, image.Height, Path.GetExtension(openFileDialog.FileName));
                    pictureBox1.Image = image;
                    return;
                }
                else
                {
                    label1.Text = "添加图片失败";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加图片失败:" + ex.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                font = fontDialog.Font;
                textBox1.Font = font;
                enableButton();
            }
            else
            {
                MessageBox.Show("选择字体失败, 请重新选择");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog.Color;
                textBox1.ForeColor = color;
                enableButton();
            }
            else
            {
                MessageBox.Show("选择颜色失败, 请重新选择");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (text.Length == 0)
            {
                MessageBox.Show("文本内容不能为空");
                return;
            }

            int x, y, w, h;
            if (!parseIntStr(true, textBox2.Text, out x))
            {
                MessageBox.Show("文字起始坐标X格式不合法");
                return;
            }
            if (!parseIntStr(true, textBox3.Text, out y))
            {
                MessageBox.Show("文字起始坐标Y格式不合法");
                return;
            }
            if (!parseIntStr(false, textBox4.Text, out w))
            {
                MessageBox.Show("文字行宽度不合法");
                return;
            }
            if (!parseIntStr(false, textBox5.Text, out h))
            {
                MessageBox.Show("文字行高度不合法");
                return;
            }
            try
            {
                PngUtil.writeImage(image, new Rectangle(x, y, w, h), text, font, color);
                pictureBox1.Image = image;
                MessageBox.Show("写入成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入文本失败" + ex.ToString());
            }
        }

        private void enableButton()
        {
            if ((image == null) || (font == null) || (color == null))
            {
                return;
            }
            button3.Enabled = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "PNG|*.png|所有文件|*.*";

            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string targetFileName = saveFileDialog.FileName;

                    pictureBox1.Image.Save(targetFileName);
                    MessageBox.Show("保存图片成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存图片失败");
            }

        }

        private bool parseIntStr(bool allowZero, string input, out int output)
        {
            output = 0; 
            if (input.Length == 0)
            {
                return allowZero; //允许不输入。 默认为0
            }
            else if (!StringUtil.IsNumber(input))
            {
                return false;
            }
            else
            {
                output = Convert.ToInt32(input);
                return true;
            }
        }
    }
}
