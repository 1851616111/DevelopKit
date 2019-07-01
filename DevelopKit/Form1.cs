using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Threading;

namespace DevelopKit
{
    public partial class Form1 : Form
    {
        static private int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度

        ParameterizedThreadStart pts;
        Thread t;
        private Hashtable treeViewLeafNodeTag;

        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = @"Resources\EighteenColor1.ssk";

            Log.Init(Path.Combine(System.Environment.CurrentDirectory, "log.txt"));
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
                    project.NextStatus();
                    showOpenedProject();
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
            preInstallContent();

            pts = new ParameterizedThreadStart(ProjectSyncTools.Sync);
            t = new Thread(pts);
            t.Start(GlobalProject);
        }

        private void hideOpenedProject()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            tabControl1.Visible = false;
            splitter1.Visible = false;
            splitter2.Visible = false;

            treeViewLeafNodeTag = new Hashtable();
            treeViewLeafNodeTag["is_leaf_node"] = true;
            GlobalProject = null;
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

        private void OpenImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string error;
                bool ok = GlobalProject.NewOpenImage(openFileDialog.FileName, out error);
                if (!ok)
                {
                    MessageBox.Show(Errors.ProjectFileAlreadyExist, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (openFileDialog.FileName.StartsWith(GlobalProject.GetUserSpaceDir()))
                {
                    openImage(openFileDialog.FileName, true);
                }
                else
                {
                    openImage(openFileDialog.FileName, false);
                }
            }
        }

        //Form_Image的所有操作请求均通过此函数出发 相应操作
        public void Form_Image_Handler(Object requestObj)
        {
            if (requestObj.GetType() != typeof(OperateFileReuqest))
            {
                return;
            }

            OperateFileReuqest request = (OperateFileReuqest)requestObj;
            switch (request.operatetype)
            {
                case OperateFileType.Save:
                    saveImageByFilePath(request.filepath);
                    break;
                case OperateFileType.Close:
                    foreach (TabPage tabpage in tabControl1.TabPages)
                    {
                        if (tabpage.Name == request.filepath)
                        {
                            int index = tabControl1.TabPages.IndexOf(tabpage);
                            int newSelectIndex;

                            if (tabControl1.TabPages.Count == 1) //如果只有一个
                            {
                                tabControl1.TabPages.Remove(tabpage);
                                return;
                            }

                            if (index == tabControl1.TabPages.Count - 1)    //如果需要删除的tabpage是最后一个打开的
                            {
                                newSelectIndex = tabControl1.TabPages.Count - 2;
                            }
                            else
                            {
                                newSelectIndex = tabControl1.TabPages.Count;
                            }

                            tabControl1.SelectedIndex = newSelectIndex;
                            tabControl1.TabPages.Remove(tabpage);
                            return;
                        }
                    }
                    break;
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

        private void OpenFile(string filepath )
        {
            bool Saved;
            if (filepath.StartsWith(GlobalProject.GetUserSpaceDir()))
            {
                Saved = true;
            }
            else
            {
                Saved = false;
            }
            if (FileUtil.IsFileImage(filepath))
            {
                openImage(filepath, Saved);
            }
            else {
                openTxt(filepath, Saved);
            }
        }

        private void OpenFile(ProjectFile file)
        {
            bool Saved;

            if (file.filePath.StartsWith(GlobalProject.GetUserSpaceDir()))
            {
                Saved = true;
            }
            else
            {
                Saved = false;
            }

            if (file.fileType == FileType.Image)
            {
                openImage(file.filePath, Saved);
            }
            else
            {
                openTxt(file.filePath, Saved);
            }
        }

        private void openImage(string filepath, bool saved)
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
            ht.Add("filetype", FileType.Image);
            ht.Add("filename", filename);
            ht.Add("filepath", filepath);


            //创建一个tabpage
            TabPage tabPage = new TabPage();
            tabPage.Tag = ht;
            tabPage.Name = filepath;
            if (saved)
            {
                tabPage.Text = filename;
            }
            else
            {
                tabPage.Text = StringUtil.markFileAsUnsafed(filename);
            }

            tabPage.Padding = new Padding(6);
            tabPage.ToolTipText = filepath;

            //将tabpage 添加到 tabcontroll中
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectTab(tabPage);

            //在tabpage绑定一个form
            Form1_Image form = new Form1_Image();
            form.Name = filepath;
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

        private void openTxt(string filepath, bool saved)
        {
            RichTextBox richTextBox = new RichTextBox();
            try
            {
                byte[] bytes = File.ReadAllBytes(filepath);
                // richTextBox.Text = File.
                richTextBox.Text = Encoding.UTF8.GetString(bytes);
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
            ht.Add("filetype", FileType.Txt);
            ht.Add("filename", filename);
            ht.Add("filepath", filepath);


            //创建一个tabpage
            TabPage tabPage = new TabPage();
            tabPage.Tag = ht;
            tabPage.Name = filepath;
            if (saved)
            {
                tabPage.Text = filename;
            }
            else
            {
                tabPage.Text = StringUtil.markFileAsUnsafed(filename);
            }

            tabPage.Padding = new Padding(6);
            tabPage.ToolTipText = filepath;

            //将tabpage 添加到 tabcontroll中
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectTab(tabPage);

            //在tabpage绑定一个form
            Form1_Image form = new Form1_Image();
            form.Name = filepath;
            form.Tag = ht;
            form.TopLevel = false;     //设置为非顶级控件
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.formDelegateHandler = Form_Image_Handler;
            tabPage.Controls.Add(form);

            //在form中创建一个picturebox

            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Name = filename;
            richTextBox.TabIndex = 0;

            form.Controls.Add(richTextBox);
            richTextBox.Show();
            form.Show();
        }


        //保存当前Tabpage页的图片 ,由form1 自上而下发起保存图片请求
        private void ToolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            TabPage selectedTabTage = tabControl1.SelectedTab;
            if (selectedTabTage == null)
            {
                return;
            }
            saveImageInTabPage(selectedTabTage);
        }

        //子Form通过delegate 回调通知的filepath来保存图片， 需要便利所有tabpage
        private void saveImageByFilePath(string filepath)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Name == filepath)
                {
                    saveImageInTabPage(tabPage);
                    return;
                }
            }
        }

        private void saveImageInTabPage(TabPage tabpage)
        {
            if (tabpage.Tag == null)
            {
                return;
            }

            Hashtable tag = (Hashtable)(tabpage.Tag);

            if (tag["filetype"].GetType() != typeof(FileType))
            {
                return;
            }

            string filename = (string)tag["filename"];
            string filepath = (string)tag["filepath"];
            FileType filetype = (FileType)tag["filetype"];
            if (StringUtil.isFileSafed(tabpage.Text))
            {
                MessageBox.Show(filename);
                return; //已保存暂时不做处理
            }

            foreach (Control control in tabpage.Controls)  //
            {
                if (control.GetType() != typeof(Form1_Image))
                {
                    continue;
                }

                foreach (Control childControl in control.Controls)
                {

                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.Filter = "PNG|*.png|所有文件|*.*";
                    fileDialog.FilterIndex = 1;
                    fileDialog.RestoreDirectory = true;
                    fileDialog.InitialDirectory = GlobalProject.GetUserSpaceDir();
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (filetype == FileType.Image)
                            {
                                ((PictureBox)childControl).Image.Save(fileDialog.FileName);
                            }
                            else if (filetype == FileType.Txt)
                            {
                                ((RichTextBox)childControl).SaveFile(fileDialog.FileName);
                            }
                            else {
                                MessageBox.Show("未知的文件类型");
                            }
                            changeFileWindowsTextAsSaved(filepath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存文件失败");
                        }
                    }
                }
            }
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl2.SelectedIndex == 0)
            {
                treeView1_LoadCurrentProject(GlobalProject.GetUserSpaceDir());
            }
            else
            {

            }
        }

        private void preInstallContent()
        {
            tabControl1.SelectedIndex = 0;


            treeView1_LoadCurrentProject(GlobalProject.GetUserSpaceDir());
        }



        private void treeView1_LoadCurrentProject(string projectdir)
        {
            Directory.SetCurrentDirectory(projectdir);

            string projectDir = GlobalProject.GetUserSpaceDir();
            string[] dirs = Directory.GetDirectories(projectdir);

            treeView1.Nodes.Clear();

            string[] files = Directory.GetFiles(projectdir);
            foreach (string longfile in files)
            {
                string relativePath = longfile.Remove(0, projectDir.Length + 1);
                treeView1.Nodes.Add(longfile, relativePath);

            }

            //为每个新增的文件节点增加Tag， 用于点击相应双击事件的标识
            foreach (TreeNode node in treeView1.Nodes)
            {
                node.Tag = treeViewLeafNodeTag;
            }

            foreach (string dir in dirs)
            {
                string relativePath = dir.Remove(0, projectDir.Length + 1);
                if (relativePath == ".kit")
                {
                    continue;
                }
                treeView1.Nodes.Add(dir, relativePath);
            }

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && ((Hashtable)(e.Node.Tag))["is_leaf_node"] != null)
            {
                string error = "";
                if (!GlobalProject.NewOpenImage(e.Node.Name, out error))
                {
                    MessageBox.Show("打开文件失败: " + error, "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                OpenFile(e.Node.Name);

               
            }
            else
            {
                ReadDir(e);
            }
        }

        private void ReadDir(TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.IsSelected)
                {
                    return;
                }
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
            }
            else
            {
                if (Directory.Exists(e.Node.Name))
                {
                    try
                    {
                        string[] allFiles = Directory.GetFiles(e.Node.Name);
                        foreach (string longfile in allFiles)
                        {
                            e.Node.Nodes.Add(longfile, longfile.Remove(0, e.Node.Name.Length + 1));
                        }

                        //为每个新增的文件节点增加Tag， 用于点击相应双击事件的标识
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            node.Tag = treeViewLeafNodeTag;
                        }

                        string[] allDirectory = Directory.GetDirectories(e.Node.Name);
                        foreach (string dir in allDirectory)
                        {
                            e.Node.Nodes.Add(dir, dir.Remove(0, e.Node.Name.Length + 1));
                        }
                    }
                    catch
                    {
                    }
                }
                e.Node.Expand();
            }
        }

        //打开已存在的项目
        private void OpenProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string userProjectDir = folderBrowserDialog.SelectedPath;
                if (!Directory.Exists(userProjectDir))
                {
                    MessageBox.Show("项目目录不存在，请确认后重新打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string projectInnerDir = Path.Combine(userProjectDir, Project.RuntimeConfigDirName);

                if (!Directory.Exists(projectInnerDir))
                {
                    MessageBox.Show("项目目录不存在，请确认后重新打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string projectConfigXML = Path.Combine(projectInnerDir, Project.RuntimeConfigXmlName);
                if (!File.Exists(projectConfigXML))
                {
                    MessageBox.Show("项目配置不存在，请确认后重新打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Object projectProject = FileUtil.DeserializeObjectFromFile(typeof(Project), projectConfigXML);
                SetGlobalProject((Project)projectProject);
                showOpenedProject();

                foreach (ProjectFile pf in GlobalProject.filesEditer.projectFileList)
                {
                    OpenFile(pf);
                }
            }
        }

        //关闭项目
        private void CloseprojectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //清除左侧tabcontrol
            foreach (TabPage tabpage in tabControl2.TabPages)
            {
                tabpage.Controls.Clear();
            }


            //清除中间tabcontrol
            int start = 0;
            foreach (TabPage tabpage in tabControl1.TabPages)
            {
                if (start != 0)
                {
                    tabControl1.TabPages.Remove(tabpage);
                    start++;
                }
                else
                {
                    start++;
                    continue;
                }
            }

            t.Abort();
            hideOpenedProject();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

