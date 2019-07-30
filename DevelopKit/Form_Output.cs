using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{

    public partial class Form_OutPut : Form
    {
        public delegate void OutPutHandler();

        public OutPutHandler MainFormOutPutHandler;

        public Form_OutPut(string developer, string defaultPath, OutPutHandler handler)
        {
            InitializeComponent();

            MainFormOutPutHandler = handler;
            DevelopWarningLabel.Visible = false;
            DevelopNumberWarningLabel.Visible = false;
            OutputPathWarningLabel.Visible = false;

            DeveloperTextBox.Text = developer;
            OutputTextBox.Text = defaultPath;
            try
            {
                Directory.CreateDirectory(defaultPath);
            }
            catch (Exception)
            {
                MessageBox.Show("新建导出操作失败");
            }
           
        }

        private void Form_Output_Load(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (DeveloperTextBox.Text.Trim(' ').Length == 0)
            {
                DevelopWarningLabel.ForeColor = Color.Red;
                DevelopWarningLabel.Visible = true;
            }
          
            if (DevelopNumberTextBox.Text.Trim(' ').Length == 0)
            {
                DevelopNumberWarningLabel.ForeColor = Color.Red;
                DevelopNumberWarningLabel.Visible = true;
            }
            if (OutputTextBox.Text.Trim(' ').Length == 0)
            {
                OutputPathWarningLabel.ForeColor = Color.Red;
                OutputPathWarningLabel.Visible = true;
            }

            if(Directory.Exists(OutputTextBox.Text))
            {
                try
                {
                    Directory.CreateDirectory(OutputTextBox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("导出皮肤项目失败");
                }
            }

            MainFormOutPutHandler();

            this.Hide();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
