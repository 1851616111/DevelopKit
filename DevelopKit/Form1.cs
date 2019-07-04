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
        static private readonly int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private readonly int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度

        ParameterizedThreadStart pts;
        Thread t;
        private static readonly Hashtable treeViewLeafNodeTag = new Hashtable
        {
            ["is_leaf_node"] = true
        };

        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = @"Resources\EighteenColor1.ssk";

            Log.Init(Path.Combine(System.Environment.CurrentDirectory, "log.txt"));
            HideOpenedProject();
            GlobalProject = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ControlBox = true;
            this.SetBounds(0, 0, displayWidth, displayHeight);
            this.menuStrip1.Items[0].MouseHover += new EventHandler(ToolStripMenuItem1_MouseOver);
        }

        private Project GlobalProject;

        public void SetGlobalProject(Project project)
        {
            if (project.Status == 0)
            {
                project.Status = ProjectStatus.StartOpenProject;
            }
            this.GlobalProject = project;

            ProjectStatusHandler(this.GlobalProject);
        }

        public Project GetGlobalProject()
        {
            return GlobalProject;
        }

        private void ProjectStatusHandler(Project project)
        {
            switch (project.Status)
            {
                case (ProjectStatus.StartOpenProject):
                    toolStripStatusLabel1.Text = "打开皮肤项目：" + project.ProjectName;
                    project.NextStatus();
                    ShowOpenedProject();
                    toolStripStatusLabel1.Text = "就绪";

                    break;
            }
        }

        private void ShowOpenedProject()
        {
            panel1.Visible = true;
            panel2.Visible = true;
            tabControl1.Visible = true;
            splitter1.Visible = true;
            splitter2.Visible = true;
            openImageToolStripMenuItem.Enabled = true;
            InstallTreeView();

            pts = new ParameterizedThreadStart(ProjectSyncTools.Sync);
            t = new Thread(pts);
            t.Start(this);
        }

        private void HideOpenedProject()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            tabControl1.Visible = false;
            splitter1.Visible = false;
            splitter2.Visible = false;
            openImageToolStripMenuItem.Enabled = false;

            GlobalProject = null;
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
                bool ok = GlobalProject.NewOpenFile(openFileDialog.FileName, out string error);
                if (!ok)
                {
                    MessageBox.Show(Errors.ProjectFileAlreadyExist, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Log.Error("Form1 OpenImageToolStripMenuItem_Click()", "GlobalProject.NewOpenImage()", error);
                    return;
                }

                if (openFileDialog.FileName.StartsWith(GlobalProject.GetUserSpaceDir()))
                {
                    Form1_Util.OpenImageForm(openFileDialog.FileName, true, tabControl1, Form_Image_Handler);
                }
                else
                {
                    Form1_Util.OpenImageForm(openFileDialog.FileName, false, tabControl1, Form_Image_Handler);
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
                    SaveImageByFilePath(request.filepath);
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
                            tabpage.Dispose();

                            GlobalProject.CloseFile(request.filepath);
                        }
                    }
                    break;
            }
        }

        //文件保存成功后， 需要移除*， 向用户标识该文件已经同步
        private bool ChangeFileWindowsTextAsSaved(string filePath)
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

        private void OpenFile(string filepath)
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
                Form1_Util.OpenImageForm(filepath, Saved, tabControl1, Form_Image_Handler);
            }
            else
            {
                Form1_Util.OpenTxtForm(filepath, Saved, tabControl1, Form_Image_Handler);
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
                Form1_Util.OpenImageForm(file.filePath, Saved, tabControl1, Form_Image_Handler);
            }
            else
            {
                Form1_Util.OpenTxtForm(file.filePath, Saved, tabControl1, Form_Image_Handler);
            }
        }


        //保存当前Tabpage页的图片 ,由form1 自上而下发起保存图片请求
        private void ToolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            TabPage selectedTabTage = tabControl1.SelectedTab;
            if (selectedTabTage == null)
            {
                return;
            }
            SaveImageInTabPage(selectedTabTage);
        }

        //子Form通过delegate 回调通知的filepath来保存图片， 需要便利所有tabpage
        private void SaveImageByFilePath(string filepath)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                Console.WriteLine(tabPage.Name + "-------->" + filepath);
                if (tabPage.Name == filepath)
                {
                    SaveImageInTabPage(tabPage);
                    return;
                }
            }
        }

        private void SaveImageInTabPage(TabPage tabpage)
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
                toolStripStatusLabel1.Text = string.Format("文件{0}已保存到项目中", filename);
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
                    if (childControl.GetType() != typeof(PictureBox))
                    {
                        continue;
                    }
                    SaveFileDialog fileDialog = new SaveFileDialog
                    {
                        Filter = "PNG|*.png|所有文件|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = true,
                        InitialDirectory = GlobalProject.GetUserSpaceDir()

                    };
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
                            else
                            {
                                MessageBox.Show("未知的文件类型");
                            }
                            ChangeFileWindowsTextAsSaved(filepath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存文件失败");
                            Log.Error("Form1 SaveImageInTabPage()", "保存文件失败", ex.ToString());
                        }
                    }
                }
            }
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl2.SelectedIndex == 0)
            {
                TreeView1_LoadCurrentProject(GlobalProject.GetUserSpaceDir());
            }
            else
            {

            }
        }

        private void InstallTreeView()
        {
            tabControl1.SelectedIndex = 0;

            TreeView1_LoadCurrentProject(GlobalProject.GetUserSpaceDir());
        }


        private void TreeView1_LoadCurrentProject(string projectdir)
        {
            Directory.SetCurrentDirectory(projectdir);

            string projectDir = GlobalProject.GetUserSpaceDir();
            string[] dirs = Directory.GetDirectories(projectdir);

            treeView1.Nodes.Clear();

            string[] files = Directory.GetFiles(projectdir);
            foreach (string longfile in files)
            {
                string relativePath = longfile.Remove(0, projectDir.Length + 1);
                treeView1.Nodes.Add(longfile, relativePath, (int)FileUtil.GetImageIndexByFileName(relativePath));
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
                treeView1.Nodes.Add(dir, relativePath, (int)FileUtil.ImageListIndexOfTreeView.Directory);
            }

            //防止单机图片抖动
            foreach (var node in treeView1.Nodes)
            {
                var Node = node as TreeNode;
                Node.SelectedImageIndex = Node.ImageIndex;
            }
        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && ((Hashtable)(e.Node.Tag))["is_leaf_node"] != null)
            {
                if (!GlobalProject.NewOpenFile(e.Node.Name, out string error))
                {
                    MessageBox.Show("打开文件失败: " + error, "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Log.Error("Form1.treeView1_NodeMouseDoubleClick()", "GlobalProject.NewOpenImag() " + e.Node.Name, error);
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
                            e.Node.Nodes.Add(longfile, longfile.Remove(0, e.Node.Name.Length + 1), (int)FileUtil.GetImageIndexByFileName(longfile));
                        }

                        //为每个新增的文件节点增加Tag， 用于点击相应双击事件的标识
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            node.Tag = treeViewLeafNodeTag;
                        }

                        string[] allDirectory = Directory.GetDirectories(e.Node.Name);
                        foreach (string dir in allDirectory)
                        {
                            e.Node.Nodes.Add(dir, dir.Remove(0, e.Node.Name.Length + 1), (int)FileUtil.ImageListIndexOfTreeView.Directory);
                        }

                        //防止单机图片抖动
                        foreach (var node in e.Node.Nodes)
                        {
                            var Node = node as TreeNode;
                            Node.SelectedImageIndex = Node.ImageIndex;
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
                if (projectProject == null)
                {
                    MessageBox.Show("读取项目配置文件失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SetGlobalProject((Project)projectProject);
                //ShowOpenedProject();

                foreach (ProjectFile pf in GlobalProject.filesEditer.projectFileList)
                {
                    OpenFile(pf);
                }
            }
        }

        //关闭项目
        private void CloseprojectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //清除左侧treewiew1

            treeView1.Nodes.Clear();

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

            HideOpenedProject();
            GlobalProject = null;
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

        //鼠标
        private void TabControl2_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}

