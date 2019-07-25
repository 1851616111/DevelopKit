namespace DevelopKit
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Project project;
        public void UpdateProject(Project project)
        {
            this.project = project; 
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeprojectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.projectToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.centerboardPictureBoxSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.centerBoardImageRealSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.centerBoardFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.centerBoardToolStrip = new System.Windows.Forms.ToolStrip();
            this.scrollUpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.scrollDownToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.centerBoardPictuerBox = new System.Windows.Forms.PictureBox();
            this.CloseProjectPictureBox = new System.Windows.Forms.PictureBox();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.rgbStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.centerBoardFlowPanel.SuspendLayout();
            this.centerBoardToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerBoardPictuerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseProjectPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.createToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(73, 25);
            this.toolStripMenuItem1.Text = "文件(F)";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem1});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.openToolStripMenuItem.Text = "打开(O)";
            // 
            // openProjectToolStripMenuItem1
            // 
            this.openProjectToolStripMenuItem1.Name = "openProjectToolStripMenuItem1";
            this.openProjectToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem1.Size = new System.Drawing.Size(221, 26);
            this.openProjectToolStripMenuItem1.Text = "项目";
            this.openProjectToolStripMenuItem1.Click += new System.EventHandler(this.OpenProjectToolStripMenuItem1_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createProjectToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.createToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.createToolStripMenuItem.Text = "新建(N)";
            // 
            // createProjectToolStripMenuItem
            // 
            this.createProjectToolStripMenuItem.Name = "createProjectToolStripMenuItem";
            this.createProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.createProjectToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
            this.createProjectToolStripMenuItem.Text = "项目";
            this.createProjectToolStripMenuItem.Click += new System.EventHandler(this.ProjectToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeprojectToolStripMenuItem});
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            this.closeToolStripMenuItem.Text = "关闭(C)";
            // 
            // closeprojectToolStripMenuItem
            // 
            this.closeprojectToolStripMenuItem.Name = "closeprojectToolStripMenuItem";
            this.closeprojectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.closeprojectToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.closeprojectToolStripMenuItem.Text = "项目";
            this.closeprojectToolStripMenuItem.Click += new System.EventHandler(this.CloseprojectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(57, 6);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(16, 48);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Margin = new System.Windows.Forms.Padding(0, 0, 38, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(36, 6, 6, 6);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1924, 37);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Highlight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripStatusLabel1,
            this.centerboardPictureBoxSizeLabel,
            this.centerBoardImageRealSizeLabel,
            this.rgbStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1039);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1924, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // projectToolStripStatusLabel1
            // 
            this.projectToolStripStatusLabel1.Name = "projectToolStripStatusLabel1";
            this.projectToolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.projectToolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // centerboardPictureBoxSizeLabel
            // 
            this.centerboardPictureBoxSizeLabel.Margin = new System.Windows.Forms.Padding(200, 3, 0, 2);
            this.centerboardPictureBoxSizeLabel.Name = "centerboardPictureBoxSizeLabel";
            this.centerboardPictureBoxSizeLabel.Size = new System.Drawing.Size(195, 17);
            this.centerboardPictureBoxSizeLabel.Text = "centerboardPictureBoxSizeLabel";
            // 
            // centerBoardImageRealSizeLabel
            // 
            this.centerBoardImageRealSizeLabel.Margin = new System.Windows.Forms.Padding(30, 3, 0, 2);
            this.centerBoardImageRealSizeLabel.Name = "centerBoardImageRealSizeLabel";
            this.centerBoardImageRealSizeLabel.Size = new System.Drawing.Size(196, 17);
            this.centerBoardImageRealSizeLabel.Text = "centerBoardImageRealSizeLabel";
            // 
            // skinEngine1
            // 
            this.skinEngine1.@__DrawButtonFocusRectangle = true;
            this.skinEngine1.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.skinEngine1.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.skinEngine1.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 1002);
            this.panel1.TabIndex = 17;
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(200, 1002);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeView2);
            this.tabPage3.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage3.Location = new System.Drawing.Point(32, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(164, 994);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "流程";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(3, 3);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(158, 988);
            this.treeView2.TabIndex = 0;
            this.treeView2.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView2_NodeMouseDoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(200, 37);
            this.splitter1.Margin = new System.Windows.Forms.Padding(0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(7, 1002);
            this.splitter1.TabIndex = 20;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter2.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(1497, 37);
            this.splitter2.Margin = new System.Windows.Forms.Padding(0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(7, 1002);
            this.splitter2.TabIndex = 21;
            this.splitter2.TabStop = false;
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(1504, 37);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rightPanel.MaximumSize = new System.Drawing.Size(420, 0);
            this.rightPanel.MinimumSize = new System.Drawing.Size(420, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(420, 1002);
            this.rightPanel.TabIndex = 18;
            // 
            // centerBoardFlowPanel
            // 
            this.centerBoardFlowPanel.BackColor = System.Drawing.SystemColors.MenuBar;
            this.centerBoardFlowPanel.Controls.Add(this.centerBoardToolStrip);
            this.centerBoardFlowPanel.Controls.Add(this.centerBoardPictuerBox);
            this.centerBoardFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerBoardFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.centerBoardFlowPanel.Location = new System.Drawing.Point(207, 37);
            this.centerBoardFlowPanel.Name = "centerBoardFlowPanel";
            this.centerBoardFlowPanel.Size = new System.Drawing.Size(1290, 1002);
            this.centerBoardFlowPanel.TabIndex = 26;
            // 
            // centerBoardToolStrip
            // 
            this.centerBoardToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerBoardToolStrip.GripMargin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.centerBoardToolStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.centerBoardToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrollUpToolStripButton,
            this.scrollDownToolStripButton});
            this.centerBoardToolStrip.Location = new System.Drawing.Point(0, 0);
            this.centerBoardToolStrip.MinimumSize = new System.Drawing.Size(1920, 36);
            this.centerBoardToolStrip.Name = "centerBoardToolStrip";
            this.centerBoardToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.centerBoardToolStrip.Size = new System.Drawing.Size(1920, 42);
            this.centerBoardToolStrip.TabIndex = 2;
            this.centerBoardToolStrip.Text = "toolStrip1";
            // 
            // scrollUpToolStripButton
            // 
            this.scrollUpToolStripButton.AutoSize = false;
            this.scrollUpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scrollUpToolStripButton.Image = global::DevelopKit.Properties.Resources.Loupe_Zoom2;
            this.scrollUpToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.scrollUpToolStripButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.scrollUpToolStripButton.Name = "scrollUpToolStripButton";
            this.scrollUpToolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.scrollUpToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.scrollUpToolStripButton.Text = "scrollUpToolStripButton";
            this.scrollUpToolStripButton.ToolTipText = "放大";
            this.scrollUpToolStripButton.Click += new System.EventHandler(this.ScrollUpToolStripButton_Click);
            // 
            // scrollDownToolStripButton
            // 
            this.scrollDownToolStripButton.AutoSize = false;
            this.scrollDownToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scrollDownToolStripButton.Image = global::DevelopKit.Properties.Resources.Loupe_Zoomback;
            this.scrollDownToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.scrollDownToolStripButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.scrollDownToolStripButton.Name = "scrollDownToolStripButton";
            this.scrollDownToolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.scrollDownToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.scrollDownToolStripButton.Text = "scrollDownToolStripButton";
            this.scrollDownToolStripButton.ToolTipText = "缩小";
            this.scrollDownToolStripButton.Click += new System.EventHandler(this.ScrollDownToolStripButton_Click);
            // 
            // centerBoardPictuerBox
            // 
            this.centerBoardPictuerBox.BackColor = System.Drawing.Color.LightSkyBlue;
            this.centerBoardPictuerBox.Location = new System.Drawing.Point(0, 42);
            this.centerBoardPictuerBox.Margin = new System.Windows.Forms.Padding(0);
            this.centerBoardPictuerBox.Name = "centerBoardPictuerBox";
            this.centerBoardPictuerBox.Size = new System.Drawing.Size(1800, 720);
            this.centerBoardPictuerBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.centerBoardPictuerBox.TabIndex = 1;
            this.centerBoardPictuerBox.TabStop = false;
            this.centerBoardPictuerBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CenterBoardPictuerBox_MouseMove);
            // 
            // CloseProjectPictureBox
            // 
            this.CloseProjectPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseProjectPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.CloseProjectPictureBox.Image = global::DevelopKit.Properties.Resources.close_project;
            this.CloseProjectPictureBox.Location = new System.Drawing.Point(1883, 0);
            this.CloseProjectPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.CloseProjectPictureBox.Name = "CloseProjectPictureBox";
            this.CloseProjectPictureBox.Size = new System.Drawing.Size(36, 36);
            this.CloseProjectPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CloseProjectPictureBox.TabIndex = 24;
            this.CloseProjectPictureBox.TabStop = false;
            this.CloseProjectPictureBox.Click += new System.EventHandler(this.CloseProjectPictureBox_Click);
            this.CloseProjectPictureBox.MouseLeave += new System.EventHandler(this.CloseProjectPictureBox_MouseLeave);
            this.CloseProjectPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CloseProjectPictureBox_MouseMove);
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(36, 36);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LogoPictureBox.TabIndex = 23;
            this.LogoPictureBox.TabStop = false;
            // 
            // rgbStripStatusLabel1
            // 
            this.rgbStripStatusLabel1.Image = global::DevelopKit.Properties.Resources.rgb;
            this.rgbStripStatusLabel1.Margin = new System.Windows.Forms.Padding(30, 3, 0, 2);
            this.rgbStripStatusLabel1.Name = "rgbStripStatusLabel1";
            this.rgbStripStatusLabel1.Size = new System.Drawing.Size(145, 17);
            this.rgbStripStatusLabel1.Text = "rgbStripStatusLabel1";
            // 
            // Form_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1924, 1061);
            this.Controls.Add(this.centerBoardFlowPanel);
            this.Controls.Add(this.CloseProjectPictureBox);
            this.Controls.Add(this.LogoPictureBox);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DevelopKit";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.centerBoardFlowPanel.ResumeLayout(false);
            this.centerBoardFlowPanel.PerformLayout();
            this.centerBoardToolStrip.ResumeLayout(false);
            this.centerBoardToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerBoardPictuerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseProjectPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel projectToolStripStatusLabel1;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeprojectToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.PictureBox CloseProjectPictureBox;
        private System.Windows.Forms.FlowLayoutPanel centerBoardFlowPanel;
        private System.Windows.Forms.PictureBox centerBoardPictuerBox;
        private System.Windows.Forms.ToolStrip centerBoardToolStrip;
        private System.Windows.Forms.ToolStripButton scrollDownToolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel centerboardPictureBoxSizeLabel;
        private System.Windows.Forms.ToolStripStatusLabel centerBoardImageRealSizeLabel;
        private System.Windows.Forms.ToolStripButton scrollUpToolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel rgbStripStatusLabel1;
    }
}

