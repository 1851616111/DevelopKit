﻿using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace DevelopKit
{
    public partial class Form_Main : Form
    {
        static private readonly int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private readonly int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度
        ParameterizedThreadStart pts;
        Thread t;
      
        public Form_Main()
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
        }

        public void SetGlobalProject(Project project)
        {
            if (project.Status == 0)
            {
                project.Status = ProjectStatus.StartOpenProject;
                project.CarConfig.MakeMappingCache();
            }

            GlobalConfig.Project = project;
            GlobalConfig.Controller = new CenterBoardController(panel2, tabPage1);

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
            loadScene(treeView2, GlobalConfig.Project.CarConfig);

            pts = new ParameterizedThreadStart(ProjectSyncTools.Sync);
            t = new Thread(pts);
            t.Start(this);
        }

        private void loadScene(TreeView treeview, CarConfig carConfig)
        {

            treeview.BeginUpdate();
            foreach (Scene scene in carConfig.Scenes)
            {
                TreeNode sceneNode = new TreeNode
                {
                    Name = scene.Id.ToString(),
                    Text = scene.Name
                };

                treeview.Nodes.Add(sceneNode);
            }
            treeview.EndUpdate();
        }

        private void HideOpenedProject()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            tabControl1.Visible = false;
            splitter1.Visible = false;
            splitter2.Visible = false;
            openImageToolStripMenuItem.Enabled = false;
            treeView2.Nodes.Clear();
            GlobalConfig.Project = null;
            GlobalConfig.Controller = null;
        }

        //创建项目
        private void ProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Create form2 = new Form_Create();
            form2.updateGlobalProjectHandler += SetGlobalProject;
            form2.SetDesktopBounds(Form_Main.displayWidth / 4, 80, Form_Main.displayWidth / 2, Form_Main.displayHeight / 2 + 150);
            form2.StartPosition = FormStartPosition.Manual;
            form2.Show();
            form2.Activate();
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
            }
        }

        //关闭项目
        private void CloseprojectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.Project.Editer.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(string.Format("存在{0}个已修改的属性未保存，是否关闭", GlobalConfig.Project.Editer.Count),  "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                    HideOpenedProject();
            }
            else {
                HideOpenedProject();
            }
        }

        //双击流程场景后在FlowLayout进行动态填充以及隐藏
        private void TreeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int sceneId = Convert.ToInt32(e.Node.Name);
            GlobalConfig.Controller.DoubleClickScene(sceneId);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            GlobalConfig.Controller.CenterBoardBarOnScroll();
        }

        //保存
        private void ToolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }
    }
}
