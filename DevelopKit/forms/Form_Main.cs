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
    public partial class Form_Main : Form
    {
        static private readonly int displayWidth = SystemInformation.WorkingArea.Width; //获取显示器工作区宽度
        static private readonly int displayHeight = SystemInformation.WorkingArea.Height; //获取显示器工作区高度
        private int CenterBoardWidth;
        ParameterizedThreadStart pts;
        Thread t;

        public Form_Main()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = @"Resources\EighteenColor1.ssk";
            HideOpenedProject();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetBounds(0, 0, displayWidth, displayHeight);
            CenterBoardWidth = splitter2.Location.X - centerBoardFlowPanel.Location.X;
            centerBoardFlowPanel.Width = CenterBoardWidth;
            //centerBoardToolStrip.MinimumSize.Width = splitter2.Location.X - centerBoardFlowPanel.Location.X;
            centerBoardToolStrip.Width = splitter2.Location.X - centerBoardFlowPanel.Location.X;
        }

        public void SetGlobalProject(Project project)
        {
            if (project.Status == 0)
            {
                project.RestoreEditedProperties(project.CarConfig.Properties);
                project.CarConfig.MakeMappingCache();
                project.Status = ProjectStatus.StartOpenProject;
            }


            GlobalConfig.Project = project;
            GlobalConfig.Controller = new Controller(new LeftController(treeView2), new CenterController(centerBoardPictuerBox), new RightController(rightPanel));
            GlobalConfig.CenterBoardController = new CenterBoardController(centerBoardPictuerBox, rightPanel,
                centerBoardImageRealSizeLabel, centerBoardPictureBoxSizeLabel, CenterBoardWidth);
            GlobalConfig.FrontCache = new ShareCache();
            GlobalConfig.EventHandler = new PropertyEventHandler();

            ProjectStatusHandler(GlobalConfig.Project);
        }

        private void ProjectStatusHandler(Project project)
        {
            switch (project.Status)
            {
                case (ProjectStatus.StartOpenProject):
                    project.NextStatus();
                    ShowOpenedProject();
                    break;
            }
        }

        private void ShowOpenedProject()
        {
            panel1.Visible = true;
            rightPanel.Visible = true;
            splitter1.Visible = true;
            splitter2.Visible = true;

            centerBoardPictureBoxSizeLabel.Visible = true;
            centerBoardImageRealSizeLabel.Visible = true;
            centerBoardRGBLabel.Visible = true;

            centerBoardFlowPanel.Visible = true;
            centerBoardPictuerBox.Visible = true;
            centerBoardToolStrip.Visible = true;
            //loadScene(treeView2, GlobalConfig.Project.CarConfig);
            LoadAllResources();

            pts = new ParameterizedThreadStart(ProjectSyncTools.Sync);
            t = new Thread(pts);
            t.Start(this);
        }

        private void LoadAllResources()
        {
            Form_Progress form_Progress = new Form_Progress(GlobalConfig.Project.CarConfig.GetTotalSceneNum(), false);
            form_Progress.Location = new Point((displayWidth - form_Progress.Width) / 2, (displayHeight - form_Progress.Height) / 2);
            form_Progress.Show();

            GlobalConfig.Controller.LoadProjectWithProgress(form_Progress);

            form_Progress.Close();
            form_Progress.Dispose();
        }


        //private void loadScene(TreeView treeview, CarConfig carConfig)
        //{
        //    Form_Progress form_Progress = new Form_Progress(carConfig.GetTotalSceneNum(), false);
        //    form_Progress.Location = new Point((displayWidth - form_Progress.Width) /2 , (displayHeight - form_Progress.Height) /2);
        //    form_Progress.Show();

        //    GlobalConfig.Controller.HideCenterBoardPictureBox();

        //    rightPanel.SuspendLayout();
        //    treeview.BeginUpdate();
        //    foreach (Scene scene in carConfig.Scenes)
        //    {
        //        TreeNode sceneNode = new TreeNode
        //        {
        //            Name = scene.Id.ToString(),
        //            Text = scene.Name
        //        };
        //        GlobalConfig.Controller.InitScene(scene.Id, true);

        //        foreach (Scene childScene in scene.children)
        //        {
        //            sceneNode.Nodes.Add(new TreeNode
        //            {
        //                Name = childScene.Id.ToString(),
        //                Text = childScene.Name
        //            });

        //            GlobalConfig.Controller.InitScene(childScene.Id, true);
        //            form_Progress.AddProgressValue(1, string.Format("场景 {0} 已加载", childScene.Name));
        //        }

        //        form_Progress.AddProgressValue(1, string.Format("场景 {0} 已加载", scene.Name));
        //        treeview.Nodes.Add(sceneNode);
        //    }

           
        //    treeview.EndUpdate();
        //    rightPanel.ResumeLayout();

        //    GlobalConfig.Controller.ShowCenterBoardPictureBox();

        //    form_Progress.Close();
        //    form_Progress.Dispose();
        //}

        private void HideOpenedProject()
        {
            centerBoardPictuerBox.Image = null;
            GlobalConfig.Project = null;
            GlobalConfig.CenterBoardController = null;

            centerBoardPictuerBox.Visible = false;
            centerBoardFlowPanel.Visible = false;
            centerBoardToolStrip.Visible = false;
            centerBoardPictureBoxSizeLabel.Visible = false;
            centerBoardImageRealSizeLabel.Visible = false;
            centerBoardRGBLabel.Visible = false;

            panel1.Visible = false;
            rightPanel.Visible = false;
            splitter1.Visible = false;
            splitter2.Visible = false;

            treeView2.Nodes.Clear();
            rightPanel.Controls.Clear();
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

        //导出皮肤
        private void OutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.Project != null)
            {
                Form_OutPut outputForm = new Form_OutPut(GlobalConfig.Project.Developer, 
                    GlobalConfig.Project.GetDefaultOutputPath(),
                    delegate () {
                        Form_Progress form_Progress = new Form_Progress(100, true);
                        form_Progress.Location = new Point((displayWidth - form_Progress.Width) / 2, (displayHeight - form_Progress.Height) / 2);
                        form_Progress.Show();

                        string err = GlobalConfig.CenterBoardController.StartOutput(
                           GlobalConfig.Project.CarConfig.outputs,
                           GlobalConfig.Project.CarConfig.PropertyIdMapping,
                           GlobalConfig.Project.GetDefaultOutputPath(),
                           form_Progress);
                        if (err != null)
                        {
                            Console.WriteLine("-------------------> result " + err);
                        }
                    });
                outputForm.SetDesktopBounds(Form_Main.displayWidth / 4, 80, Form_Main.displayWidth / 2, Form_Main.displayHeight / 2 + 150);
                outputForm.Show();
            }
        }

        //关闭项目
        private void CloseprojectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.Project.Editer.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(string.Format("存在{0}个已修改的属性未保存，是否关闭", GlobalConfig.Project.Editer.Count), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                    HideOpenedProject();
            }
            else
            {
                HideOpenedProject();
            }
        }

        //双击流程场景后在FlowLayout进行动态填充以及隐藏
        private void TreeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int sceneId = Convert.ToInt32(e.Node.Name);
            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(sceneId);
          
            GlobalConfig.Controller.Show(scene);
        }

        private void CloseProjectPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            CloseProjectPictureBox.BackColor = Color.White;
        }

        private void CloseProjectPictureBox_MouseLeave(object sender, EventArgs e)
        {
            CloseProjectPictureBox.BackColor = Color.Transparent;
        }

        private void CloseProjectPictureBox_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ScrollUpToolStripButton_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.CenterBoardController != null)
                GlobalConfig.CenterBoardController.ScrollUpCenterBoardPictureBox();
        }

        private void ScrollDownToolStripButton_Click(object sender, EventArgs e)
        {
            if (GlobalConfig.CenterBoardController != null)
                GlobalConfig.CenterBoardController.ScrollDownCenterBoardPictureBox();
        }

        private void CenterBoardPictuerBox_MouseMove(object sender, MouseEventArgs e)
        {
            centerBoardRGBLabel.Visible = centerBoardPictuerBox.Image != null;

            if (centerBoardRGBLabel.Visible)
            {
                Color pixel = ((Bitmap)(centerBoardPictuerBox.Image)).GetPixel(e.X, e.Y);
                centerBoardRGBLabel.Text = string.Format("RGB: ({0},{1},{2}),  Alpha: {3}", pixel.R, pixel.G, pixel.B, pixel.A);
            }
        }

        private void AdminToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PositionToolStripButton_Click(object sender, EventArgs e)
        {
            string position = GlobalConfig.FrontCache.GetPosition();
            if (position == "P")
            {
                GlobalConfig.FrontCache.SetPosition("");
                positionToolStripButton.Checked = false;
            }
            else
            {
                GlobalConfig.FrontCache.SetPosition("P");
                positionToolStripButton.Checked = true;
            }
        }
    }
}

