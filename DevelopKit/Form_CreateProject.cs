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
    public partial class Form_CreateProject : Form
    {

        public Form_CreateProject()
        {
            InitializeComponent();
        }

        public delegate void UpdateGlobalProjectHandler(Project project);//声明委托， 用于将此页面的project数据传递回form1
        public UpdateGlobalProjectHandler updateGlobalProjectHandler;

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "我的皮肤项目";
            textBox1.Focus();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string vehicleType = comboBox2.SelectedItem.ToString();
            string version = comboBox2.SelectedIndex.ToString();
            string projectName = textBox1.Text;
            string projectPath = comboBox1.Text;
            string developor = textBox2.Text;


            Project newProject = new Project(vehicleType, version, projectName, projectPath, developor);
            bool overwrite = false;

        recreate:

            if (newProject.StartCreateProject(overwrite, out string error, out string errorDetails))
            {
                newProject.NextStatus();
                MessageBox.Show("创建项目成功", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newProject.SetStatusOpen();
                updateGlobalProjectHandler(newProject);
                this.Hide();
            }
            else if (Errors.IsProjectAlreadyExistErr(error))
            {
                DialogResult dialogResult = MessageBox.Show(error + ", 请选择是否覆盖", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {

                    try
                    {
                        Directory.Delete(newProject.GetUserSpaceDir(), true);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("项目正在被打开，请确认后再删除", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.Error("Form2.Button3_Click()", "清除项目文件失败", ex.ToString());
                        return;
                    }
                    overwrite = true;
                    goto recreate;
                }
            }
            else
            {
                MessageBox.Show(error, errorDetails, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Red;
            if (textBox1.Text.Length == 0)
            {
                button3.Enabled = false;
                label5.Visible = true;
            }
            else
            {
                if (!button3.Enabled)
                {
                    button3.Enabled = true;
                }
                if (label5.Visible)
                {
                    label5.Visible = false;
                }
            }
        }

        private void ComboBox1_Changed(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Red;
            if (comboBox1.Text.Length == 0 || !Directory.Exists(comboBox1.Text))  //1.输入文件夹路径为空 2输入文本路径不存在
            {
                button3.Enabled = false;
                label6.Visible = true;
            }
            else
            {
                if (!button3.Enabled)
                {
                    button3.Enabled = true;
                }
                if (label6.Visible)
                {
                    label6.Visible = false;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath))
                    {
                        MessageBox.Show("路径不存在, 请检查路径是否正确，然后重试", "选择文件夹", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch (System.ArgumentException ex)
                {
                    MessageBox.Show("路径不存在, 请检查路径是否正确，然后重试", "选择文件夹", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                comboBox1.Items.Add(folderBrowserDialog1.SelectedPath);
                comboBox1.SelectedItem = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
