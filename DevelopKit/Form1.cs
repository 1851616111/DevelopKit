using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DevelopKit
{
    public partial class Form1 : Form
    {
        static private int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度

        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = @"Resources\EighteenColor1.ssk";
            hideOpenedProject();
        }

        private Project GlobalProject;
        public void SetGlobalProject(Project project)
        {
            this.GlobalProject = project;
            ProjectStatusHandler(project);
        }


        private void ProjectStatusHandler(Project project)
        {
            switch (project.Status)
            {
                case (ProjectStatus.StartOpenProject):
                    toolStripStatusLabel1.Text = "打开皮肤项目：" + project.ProjectName;
                    showOpenedProject();
                    project.NextStatus();
                    toolStripStatusLabel1.Text = "就绪";
                    break;
            }
        }

        private void showOpenedProject()
        {
            groupBox3.Enabled = true;
            groupBox3.Visible = true;
            groupBox2.Enabled = true;
            groupBox2.Visible = true;
            tabControl1.Enabled = true;
            tabControl1.Visible = true;
        }

        private void hideOpenedProject()
        {
            groupBox3.Enabled = false;
            groupBox3.Visible = false;
            groupBox2.Enabled = false;
            groupBox2.Visible = false;
            tabControl1.Enabled = false;
            tabControl1.Visible = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.ControlBox = true;
            this.SetBounds(0, 0, displayWidth, displayHeight);
            this.menuStrip1.Items[0].MouseHover += new EventHandler(ToolStripMenuItem1_MouseOver);
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolBar1.Show();
        }
        private void ToolStripMenuItem1_MouseOver(object sender, EventArgs e)
        {
            toolBar1.Visible = true;
            //this.menuStrip1.Items[0].BackColor = Color.Black;
        }

        private void ToolBar1_ButtonClick_1(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.ImageIndex)
            {
                case 0:
                    foreach (Form formItem in this.MdiChildren)
                    {
                        if (formItem.Name == "Form2" && formItem.Visible)
                        {
                            return; //避免重复打开
                        }
                    }

                    Form2 form2 = new Form2();
                    form2.updateGlobalProjectHandler += SetGlobalProject;
                    //form2.MdiParent = this;
                    form2.SetDesktopBounds(Form1.displayWidth / 4, 80, Form1.displayWidth / 2, Form1.displayHeight / 2 + 150);
                    form2.StartPosition = FormStartPosition.Manual;
                    form2.Show();
                    form2.Activate();


                    break;
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TabPage1_Click(object sender, EventArgs e)
        {

        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4_Merge_Img_Util form4 = new Form4_Merge_Img_Util();
            form4.BringToFront();
            form4.Show();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form4_Write_Img_Util form4 = new Form4_Write_Img_Util();
            form4.BringToFront();
            form4.Show();
        }
    }
}
