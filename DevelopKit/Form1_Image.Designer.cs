namespace DevelopKit
{
    partial class Form1_Image
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operateImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageToolStripMenuItem,
            this.closeImageToolStripMenuItem,
            this.copyImageToolStripMenuItem,
            this.deleteImageToolStripMenuItem,
            this.operateImageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 114);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveImageToolStripMenuItem.Text = "保存";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.SaveImageToolStripMenuItem_Click);
            // 
            // closeImageToolStripMenuItem
            // 
            this.closeImageToolStripMenuItem.Name = "closeImageToolStripMenuItem";
            this.closeImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.closeImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.closeImageToolStripMenuItem.Text = "关闭";
            this.closeImageToolStripMenuItem.Click += new System.EventHandler(this.CloseImageToolStripMenuItem_Click);
            // 
            // copyImageToolStripMenuItem
            // 
            this.copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            this.copyImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.copyImageToolStripMenuItem.Text = "复制";
            // 
            // deleteImageToolStripMenuItem
            // 
            this.deleteImageToolStripMenuItem.Name = "deleteImageToolStripMenuItem";
            this.deleteImageToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.deleteImageToolStripMenuItem.Text = "删除";
            // 
            // operateImageToolStripMenuItem
            // 
            this.operateImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterColorToolStripMenuItem,
            this.wrToolStripMenuItem});
            this.operateImageToolStripMenuItem.Name = "operateImageToolStripMenuItem";
            this.operateImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.operateImageToolStripMenuItem.Text = "其他";
            // 
            // filterColorToolStripMenuItem
            // 
            this.filterColorToolStripMenuItem.Name = "filterColorToolStripMenuItem";
            this.filterColorToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.filterColorToolStripMenuItem.Text = "滤色";
            // 
            // wrToolStripMenuItem
            // 
            this.wrToolStripMenuItem.Name = "wrToolStripMenuItem";
            this.wrToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.wrToolStripMenuItem.Text = "写字";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form1_Image";
            this.Text = "Form_Image";
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operateImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wrToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
    }
}