using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DevelopKit
{
    public partial class Form1 : Form
    {
        static private int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度
        const int CLOSE_SIZE = 16; //Tabcontroll 的tagpage 页面 标签关闭按钮区域大小

        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = @"Resources\EighteenColor1.ssk";
            hideOpenedProject();
            //example.Example.Show();
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
            panel1.Visible = true;
            panel2.Visible = true;
            tabControl1.Visible = true;
            splitter1.Visible = true;
            splitter2.Visible = true;
        }

        private void hideOpenedProject()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            tabControl1.Visible = false;
            splitter1.Visible = false;
            splitter2.Visible = false;
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

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form4_Filter_Color_Util form4 = new Form4_Filter_Color_Util();
            form4.BringToFront();
            form4.Show();
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form4_Filter_Color2_Util form4 = new Form4_Filter_Color2_Util();
            form4.BringToFront();
            form4.Show();
        }

        //创建项目
        private void ProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.updateGlobalProjectHandler += SetGlobalProject;
            form2.SetDesktopBounds(Form1.displayWidth / 4, 80, Form1.displayWidth / 2, Form1.displayHeight / 2 + 150);
            form2.StartPosition = FormStartPosition.Manual;
            form2.Show();
            form2.Activate();
        }

        private void NewImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OpenImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string error;
                bool ok = GlobalProject.NewOpenImage(openFileDialog.FileName, out error);
                if (!ok)
                {

                    MessageBox.Show(Errors.ProjectFileAlreadyExist, "创建失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                NewOpenImage_FrontOpt(openFileDialog.FileName);
            }
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //Form_Image的所有操作请求均通过此函数出发 相应操作
        public void Form_Image_Handler(Object requestObj)
        {
            if (requestObj.GetType() == typeof(SaveImageReuqest))
            {
                SaveImageReuqest request = (SaveImageReuqest)requestObj;
                //设置默认文件类型显示顺序
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "PNG|*.png|所有文件|*.*";
                fileDialog.FilterIndex = 1;
                fileDialog.RestoreDirectory = true;
                fileDialog.InitialDirectory = GlobalProject.GetUserSpaceDir();
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    Image img = Image.FromFile(request.filepath);
                    img.Save(fileDialog.FileName);
                    if (!changeFileWindowsTextAsSaved(request.filepath))
                    {
                        MessageBox.Show("保存文件内部错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    MessageBox.Show("文件保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //文件保存成功后， 需要移除*， 向用户标识该文件已经同步
        private bool changeFileWindowsTextAsSaved(string filePath)
        {
            foreach (TabPage tabpage in tabControl1.TabPages)
            {
                if (tabpage.Name == filePath)
                {
                    tabpage.Text = StringUtil.markFileAsSaved(tabpage.Text);

                    return true;
                }
            }

            return false;
        }

        private void NewOpenImage_FrontOpt(string filepath)
        {
            System.Drawing.Image image;
            try
            {
                image = System.Drawing.Image.FromFile(filepath);
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("内存不足", "打开文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("该文件已不存在, 请重新选择", "打开文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开错误", "打开文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filename = StringUtil.GetFileName(filepath);
            Hashtable ht = new Hashtable();
            ht.Add("filename", filename);
            ht.Add("filepath", filepath);

            //创建一个tabpage
            TabPage tabPage = new TabPage();
            tabPage.Tag = ht;
            tabPage.Name = filepath;
            tabPage.Text = StringUtil.markFileAsUnsafed(filename);
            tabPage.Padding = new Padding(3);
            tabPage.ToolTipText = filepath;

            //将tabpage 添加到 tabcontroll中
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectTab(tabPage);

            //在tabpage绑定一个form
            Form1_Image form = new Form1_Image();
            form.Tag = ht;
            form.TopLevel = false;     //设置为非顶级控件
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.formDelegateHandler = Form_Image_Handler;
            tabPage.Controls.Add(form);

            //在form中创建一个picturebox

            PictureBox pictureBox1 = new PictureBox();
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Name = filename;
            pictureBox1.TabIndex = 0;
            pictureBox1.Image = image;

            form.Controls.Add(pictureBox1);
            pictureBox1.Show();
            form.Show();
        }
    }
}
