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
    public partial class Form_Create : Form
    {
        private App appConfig;

        public Form_Create()
        {
            InitializeComponent();
            appConfig = (App)FileUtil.DeserializeObjectFromFile(typeof(App), @"Resources\conf\app.xml");
            loadAppConfig(appConfig);
        }

        public delegate void UpdateGlobalProjectHandler(Project project);//声明委托， 用于将此页面的project数据传递回form1
        public UpdateGlobalProjectHandler updateGlobalProjectHandler;

        private void loadAppConfig(App app)
        {
            Log.Info("Form2", "读取应用配置", string.Format("获取厂商{0}个， 汽车车型{1}个", app.manufacturers.Length, app.carInfoList.Length));

            comboBox2.Items.Clear();
            foreach (Manufacturer manufacturer in app.manufacturers)
            {
                comboBox2.Items.Add(manufacturer.Name);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void ComboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            CarInfo[] cars = appConfig.ListCarsByManufacturer(comboBox2.SelectedItem.ToString());

            comboBox3.Items.Clear();
            foreach (CarInfo car in cars)
            {
                if (!comboBox3.Items.Contains(car.Name))
                {
                    comboBox3.Items.Add(car.Name);
                }
            }
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarInfo[] cars = appConfig.ListCarsByCarName(comboBox3.SelectedItem.ToString());
            foreach (CarInfo car in cars)
            {
                if (!comboBox4.Items.Contains(car.Version))
                {
                    comboBox4.Items.Add(car.Version);
                }
            }
            if (comboBox4.Items.Count > 0)
            {
                comboBox4.SelectedIndex = 0;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "我的皮肤项目";
            textBox1.Focus();
            comboBox2.SelectedIndex = 0;

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            CarInfo car = appConfig.GetCarInfo(comboBox2.SelectedItem.ToString(),
                comboBox3.SelectedItem.ToString(), comboBox4.SelectedItem.ToString());
            if (!car.Validate())
            {
                MessageBox.Show("验证车型配置文件失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CarConfig ccfg = car.GetCarConfig();

            string projectName = textBox1.Text;
            string projectPath = comboBox1.Text;
            string developor = textBox2.Text;


            Project newProject = new Project(car, ccfg, projectName, projectPath, developor);
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
