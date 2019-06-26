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
            this.contextMenuStrip1.SuspendLayout();
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
            // Form_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form_Image";
            this.Text = "Form_Image";
            this.Load += new System.EventHandler(this.Form_Image_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}