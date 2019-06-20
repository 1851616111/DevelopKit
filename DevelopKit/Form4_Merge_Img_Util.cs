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
    public partial class Form4_Merge_Img_Util : Form
    {
        private Image defaultImageBackgroud;
        private bool baseImageOK; //底图
        private bool upImageOK;    //上图
        private int baseImageWidth;
        private int baseImageHeight;

        public Form4_Merge_Img_Util()
        {
            InitializeComponent();
            defaultImageBackgroud = pictureBox1.Image; //记录下默认的+号图片内容
            disableUserOpt(true, true);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileAbsName = Path.GetFullPath(openFileDialog.FileName);
                    Image baseImage = Image.FromFile(fileAbsName);
                    label1.Text = string.Format("底图添加成功。({0}*{1}{2})", baseImage.Width, baseImage.Height, Path.GetExtension(openFileDialog.FileName));
                    pictureBox1.Image = baseImage;
                    enableUserOpt(true, false, baseImage.Width, baseImage.Height);
                    return;
                }
                else
                {
                    label1.Text = "底图添加失败";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加图片失败", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            pictureBox1.Image = defaultImageBackgroud;
            disableUserOpt(true, false);
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileAbsName = Path.GetFullPath(openFileDialog.FileName);
                    Image image = Image.FromFile(fileAbsName);
                    label2.Text = string.Format("上层图片添加成功。({0}*{1}{2})", image.Width, image.Height, Path.GetExtension(openFileDialog.FileName));
                    pictureBox2.Image = image;
                    enableUserOpt(false, true, image.Width, image.Height);
                    return;
                }
                else
                {
                    label2.Text = "上层图片添加失败";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("上层图片失败:" + ex.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            disableUserOpt(false, true);
            pictureBox2.Image = defaultImageBackgroud;
        }

        private void disableUserOpt(bool fromBaseImage, bool fromUpImage)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            if (fromBaseImage)
            {
                baseImageOK = false;
                baseImageWidth = 0;
                baseImageHeight = 0;
            }
            if (fromUpImage)
            {
                upImageOK = false;
            }

            label6.Text = "最大不超过底图宽度";
            label7.Text = "最大不超过底图高度";
        }

        private void enableUserOpt(bool fromBaseImage, bool fromUpImage, int maxX, int maxY)
        {
            if (fromBaseImage)
            {
                baseImageOK = true;
                if (maxX > 0)
                {
                    label6.Text = string.Format("最大不超过底图宽度{0}", maxX);
                }
                if (maxY > 0)
                {
                    label7.Text = string.Format("最大不超过底图高度{0}", maxY);
                }
                baseImageWidth = maxX;
                baseImageHeight = maxY;

            }
            if (fromUpImage)
            {
                upImageOK = true;
            }

            if (baseImageOK && upImageOK)
            {

                button1.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox1.Focus();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("请输入X坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("请输入Y坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!StringUtil.IsNumber(textBox1.Text))
            {
                MessageBox.Show("请输入整数的X坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!StringUtil.IsNumber(textBox2.Text))
            {
                MessageBox.Show("请输入整数的Y坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                PngUtil.favoriteImageObj img = new PngUtil.favoriteImageObj();
                img.image = pictureBox2.Image;

                img.x = Convert.ToInt32(textBox1.Text);
                img.y = Convert.ToInt32(textBox2.Text);
                if (img.x < 0 || img.x > baseImageWidth)
                {
                    MessageBox.Show("请输入合理的X坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (img.y < 0 || img.y > baseImageHeight)
                {
                    MessageBox.Show("请输入合理的Y坐标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                pictureBox3.Image = PngUtil.MergeImages2(pictureBox1.Image, img);

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("输入参数不合法, 请输入合理范围的整数", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("合成失败");
            }

            if (pictureBox3.Image != null)
            {
                button2.Enabled = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG|*.png|所有文件|*.*";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string target = saveFileDialog.FileName;
                pictureBox3.Image.Save(target);

                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }
    }
}
