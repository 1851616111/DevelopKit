﻿using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class Form1_Util
    {

        public static void OpenImageForm(string filepath, TabControl tabcontrol1, FormDelegate formDelegate)
        {

            Image image;
            try
            {
                Stream s = File.Open(filepath, FileMode.Open);
                image = Bitmap.FromStream(s);
                s.Close();
                s.Dispose();
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("内存不足", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("Form1 openImage()", "catch OutOfMemoryException", ex.ToString());
                return;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("该文件已不存在, 请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("Form1 openImage()", "catch FileNotFoundException", ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开图片文件失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("Form1 openImage()", "catch exception", ex.ToString());
                return;
            }

            string filename = StringUtil.GetFileName(filepath);

            TabPage tabPage = new TabPage
            {
                Name = filepath,
                Text = filename,
                Padding = new Padding(6),
                ToolTipText = filepath,
                Tag = new Hashtable
                {
                    ["filetype"] = FileType.Image,
                    ["filename"] = filename,
                    ["filepath"] = filepath
                }
            };

            Form1_Image innerForm = new Form1_Image
            {
                filepath = filepath,
                filename = filename,
                Name = filepath,
                TopLevel = false,     //设置为非顶级控件
                Dock = DockStyle.Fill,
                FormBorderStyle = FormBorderStyle.None,
                formDelegateHandler = formDelegate,

            };

            //将tabpage 添加到 tabcontroll中
            tabPage.Controls.Add(innerForm);
            tabcontrol1.TabPages.Add(tabPage);
            tabcontrol1.SelectTab(tabPage);
            innerForm.Show();

            //重新计算图片放置位置
            innerForm.pictureBox1.Location = new Point((innerForm.Width - image.Size.Width) / 2 + innerForm.Location.X, (innerForm.Height - image.Size.Height) / 2 - 100);
            innerForm.pictureBox1.Size = image.Size;
            innerForm.pictureBox1.Name = filename;
            innerForm.pictureBox1.Image = image;
        }

        public static void OpenTxtForm(string projectDir, string filepath, TabControl tabcontrol1, FormDelegate formDelegate)
        {
            string filename = StringUtil.GetFileName(filepath);
            Form1_Txt form;

            try
            {
                FileUtil.ReadText(filepath, out string text);

                //2.在tabpage绑定一个form
                form = new Form1_Txt
                {
                    ProjectUserDir = projectDir,
                    Name = filepath,
                    filepath = filepath,
                    filename = filepath,
                    TopLevel = false,     //设置为非顶级控件
                    Dock = DockStyle.Fill,
                    FormBorderStyle = FormBorderStyle.None,
                    formDelegateHandler = formDelegate,
                    OriginalRichTextBoxData = text
                };

            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("内存不足", "打开文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("该文件已不存在, 请重新选择", "打开文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开文件失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("Form1.OpenTxt()", "打开文件失败", ex.ToString());

                return;
            }

            //1.创建一个tabpage
            TabPage tabPage = new TabPage
            {
                Text = filename,
                Name = filepath,
                Padding = new Padding(6),
                ToolTipText = filepath,
            };

            tabPage.Controls.Add(form);
            tabcontrol1.TabPages.Add(tabPage);
            tabcontrol1.SelectTab(tabPage);

            form.Show();
        }
    }
}
