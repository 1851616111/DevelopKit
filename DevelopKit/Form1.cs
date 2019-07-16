using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Collections.Generic;
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
            GlobalConfig.Project = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ControlBox = true;
            this.SetBounds(0, 0, displayWidth, displayHeight);
            this.menuStrip1.Items[0].MouseHover += new EventHandler(ToolStripMenuItem1_MouseOver);
        }

        public void SetGlobalProject(Project project)
        {
            if (project.Status == 0)
            {
                project.Status = ProjectStatus.StartOpenProject;
            }
            GlobalConfig.Project = project;
            GlobalConfig.MainPictureBox = pictureBox1;

            ProjectStatusHandler(GlobalConfig.Project);
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
            tabControl2.SelectedIndex = 1;
            InstallTreeView();
            Form1_Car_Config.LoadScene(treeView2, GlobalConfig.Project.CarConfig);

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

            GlobalConfig.Project = null;
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
                bool ok = GlobalConfig.Project.NewOpenFile(openFileDialog.FileName, out string error);
                if (!ok)
                {
                    MessageBox.Show(Errors.ProjectFileAlreadyExist, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Log.Error("Form1 OpenImageToolStripMenuItem_Click()", "GlobalProject.NewOpenImage()", error);
                    return;
                }

                Form1_Util.OpenImageForm(openFileDialog.FileName, tabControl1, Form_Request_Handler);
            }
        }

        //Form_Image的所有操作请求均通过此函数出发 相应操作
        public void Form_Request_Handler(Object requestObj)
        {
            if (requestObj.GetType() != typeof(FormRequest))
            {
                return;
            }

            FormRequest request = (FormRequest)requestObj;
            switch (request.RequestType)
            {
                case RequestType.MarkFileAsChanged:
                    FormRequest_ChangeStatus(request);
                    break;
                case RequestType.MarkFileAsSaved:
                    FormRequest_ChangeStatus(request);
                    break;
                case RequestType.Close:
                    FormRequest_CloseImage(request);
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

            if (FileUtil.IsFileImage(filepath))
            {
                Form1_Util.OpenImageForm(filepath, tabControl1, Form_Request_Handler);
            }
            else
            {
                Form1_Util.OpenTxtForm(GlobalConfig.Project.GetUserSpaceDir(), filepath, tabControl1, Form_Request_Handler);
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
            SaveFileInTabPage(selectedTabTage);
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
                toolStripStatusLabel1.Text = string.Format("图片{0}已保存到项目中", filename);
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
                    if (childControl.GetType() != typeof(PictureBox))  //保存图片
                    {
                        continue;
                    }

                    SaveFileDialog fileDialog = new SaveFileDialog
                    {
                        Filter = "PNG|*.png|所有文件|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = true,
                        InitialDirectory = GlobalConfig.Project.GetUserSpaceDir()

                    };
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            ((PictureBox)childControl).Image.Save(fileDialog.FileName);
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

        private void SaveFileInTabPage(TabPage tabpage)
        {
            //if (tabpage.Tag == null)
            //{
            //    return;
            //}

            //Hashtable tag = (Hashtable)(tabpage.Tag);

            //if (tag["filetype"].GetType() != typeof(FileType))
            //{
            //    return;
            //}

            //string filename = (string)tag["filename"];
            //string filepath = (string)tag["filepath"];
            //FileType filetype = (FileType)tag["filetype"];


            //foreach (Control control in tabpage.Controls)  //
            //{
            //    foreach (Control childControl in control.Controls)
            //    {
            //        if (filetype == FileType.Image && childControl.GetType() == typeof(PictureBox))  //保存图片
            //        {
            //            SaveFileDialog fileDialog = new SaveFileDialog
            //            {
            //                Filter = "PNG|*.png|所有文件|*.*",
            //                FilterIndex = 1,
            //                RestoreDirectory = true,
            //                InitialDirectory = GlobalProject.GetUserSpaceDir()

            //            };
            //            if (fileDialog.ShowDialog() == DialogResult.OK)
            //            {
            //                try
            //                {
            //                    ((PictureBox)childControl).Image.Save(fileDialog.FileName);
            //                    ChangeFileWindowsTextAsSaved(filepath);

            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show("保存文件失败");
            //                    Log.Error("Form1 SaveImageInTabPage()", "保存文件失败", ex.ToString());
            //                }
            //            }

            //        }
            //        else if (filetype == FileType.Txt && childControl.GetType() == typeof(RichTextBox))
            //        {

            //            fileDialog.Filter = "Text|*.txt|所有文件|*.*";

            //            if (fileDialog.ShowDialog() == DialogResult.OK)
            //            {
            //                try
            //                {

            //                    ((RichTextBox)childControl).SaveFile(fileDialog.FileName);
            //                    ChangeFileWindowsTextAsSaved(filepath);

            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show("保存文件失败");
            //                    Log.Error("Form1 SaveImageInTabPage()", "保存文件失败", ex.ToString());
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl2.SelectedIndex == 0)
            {
                TreeView1_LoadCurrentProject(GlobalConfig.Project.GetUserSpaceDir());
            }
            else
            {
            }
        }

        private void InstallTreeView()
        {
            tabControl1.SelectedIndex = 0;

            TreeView1_LoadCurrentProject(GlobalConfig.Project.GetUserSpaceDir());
        }


        private void TreeView1_LoadCurrentProject(string projectdir)
        {
            Directory.SetCurrentDirectory(projectdir);

            string projectDir = GlobalConfig.Project.GetUserSpaceDir();
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
                if (!GlobalConfig.Project.NewOpenFile(e.Node.Name, out string error))
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

                foreach (ProjectFile pf in GlobalConfig.Project.filesEditer.projectFileList)
                {
                    OpenFile(pf.filePath);
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
            GlobalConfig.Project = null;
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


        //Form请求修改文件状态为已修改
        private void FormRequest_ChangeStatus(FormRequest request)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Name == request.FilePath)
                {
                    if (StringUtil.isFileSafed(tabPage.Text) && request.RequestType == RequestType.MarkFileAsChanged)
                    {
                        tabPage.Text = StringUtil.markFileAsUnsafed(tabPage.Text);
                    }
                    if (StringUtil.isFileUnSafed(tabPage.Text) && request.RequestType == RequestType.MarkFileAsSaved)
                    {
                        tabPage.Text = StringUtil.markFileAsSaved(tabPage.Text);
                    }
                    return;
                }
            }
        }

        //Form请求关闭图片
        private void FormRequest_CloseImage(FormRequest request)
        {
            foreach (TabPage tabpage in tabControl1.TabPages)
            {
                if (tabpage.Name == request.FilePath)
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

                    GlobalConfig.Project.CloseFile(request.FilePath);
                }
            }
        }

        //双击流程场景后在FlowLayout进行动态填充以及隐藏
        private void TreeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int sceneId = Convert.ToInt32(e.Node.Name);
            //加载右侧菜单页
            LoadFlowPanel(sceneId);

            //按照layer绘制中央区域
            LoadCenterImage(sceneId);
        }

        private void LoadFlowPanel(int sceneId)
        {
            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(sceneId);
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();

            panel2.Controls.Add(flowLayoutPanel);
            
            bool setFlow = false;
            foreach (Group group in scene.groups)
            {
                TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                flowLayoutPanel.Controls.Add(tableLayoutPanel);
                if (!setFlow)
                {
                    Form1_FlowPanel.LoadFlowPanelConfig(flowLayoutPanel);
                    setFlow = true;
                }

                Form1_FlowPanel.LoadGroupTablePanelConfig(tableLayoutPanel, flowLayoutPanel.Width, group);
                Form1_FlowPanel.LoadGroupTablePanelData(group.Name, tableLayoutPanel, GlobalConfig.Project.CarConfig.GetPropertiesByGroupId(group.Id), 50);
            }
        }

        private void LoadCenterImage(int sceneId)
        {
            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(sceneId);

        }
    }

    public static class GlobalConfig
    {
        private static Project project;
        private static PictureBox mainPicture;  

        public static Project Project { get => project; set => project = value; }
        public static PictureBox MainPictureBox { get => mainPicture; set => mainPicture = value; }
    }
}

