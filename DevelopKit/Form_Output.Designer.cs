namespace DevelopKit
{
    partial class Form_Output
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
            this.UserIdlabel = new System.Windows.Forms.Label();
            this.DevelopNumberLabel = new System.Windows.Forms.Label();
            this.OutputPathlabel = new System.Windows.Forms.Label();
            this.DeveloperTextBox = new System.Windows.Forms.TextBox();
            this.DevelopNumberTextBox = new System.Windows.Forms.TextBox();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.DevelopWarningLabel = new System.Windows.Forms.Label();
            this.DevelopNumberWarningLabel = new System.Windows.Forms.Label();
            this.OutputPathWarningLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UserIdlabel
            // 
            this.UserIdlabel.AutoSize = true;
            this.UserIdlabel.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserIdlabel.Location = new System.Drawing.Point(132, 108);
            this.UserIdlabel.Margin = new System.Windows.Forms.Padding(0);
            this.UserIdlabel.Name = "UserIdlabel";
            this.UserIdlabel.Size = new System.Drawing.Size(88, 25);
            this.UserIdlabel.TabIndex = 0;
            this.UserIdlabel.Text = "开发者：";
            // 
            // DevelopNumberLabel
            // 
            this.DevelopNumberLabel.AutoSize = true;
            this.DevelopNumberLabel.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DevelopNumberLabel.Location = new System.Drawing.Point(132, 192);
            this.DevelopNumberLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DevelopNumberLabel.Name = "DevelopNumberLabel";
            this.DevelopNumberLabel.Size = new System.Drawing.Size(69, 25);
            this.DevelopNumberLabel.TabIndex = 1;
            this.DevelopNumberLabel.Text = "序号：";
            // 
            // OutputPathlabel
            // 
            this.OutputPathlabel.AutoSize = true;
            this.OutputPathlabel.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutputPathlabel.Location = new System.Drawing.Point(132, 275);
            this.OutputPathlabel.Margin = new System.Windows.Forms.Padding(0);
            this.OutputPathlabel.Name = "OutputPathlabel";
            this.OutputPathlabel.Size = new System.Drawing.Size(107, 25);
            this.OutputPathlabel.TabIndex = 2;
            this.OutputPathlabel.Text = "输出路径：";
            // 
            // DeveloperTextBox
            // 
            this.DeveloperTextBox.Location = new System.Drawing.Point(253, 108);
            this.DeveloperTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.DeveloperTextBox.Name = "DeveloperTextBox";
            this.DeveloperTextBox.Size = new System.Drawing.Size(382, 26);
            this.DeveloperTextBox.TabIndex = 3;
            // 
            // DevelopNumberTextBox
            // 
            this.DevelopNumberTextBox.Location = new System.Drawing.Point(253, 190);
            this.DevelopNumberTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.DevelopNumberTextBox.Name = "DevelopNumberTextBox";
            this.DevelopNumberTextBox.Size = new System.Drawing.Size(382, 26);
            this.DevelopNumberTextBox.TabIndex = 4;
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(253, 275);
            this.OutputTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(382, 26);
            this.OutputTextBox.TabIndex = 5;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(535, 357);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(100, 28);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "确认";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(385, 357);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(100, 28);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // DevelopWarningLabel
            // 
            this.DevelopWarningLabel.AutoSize = true;
            this.DevelopWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.DevelopWarningLabel.Location = new System.Drawing.Point(675, 111);
            this.DevelopWarningLabel.Name = "DevelopWarningLabel";
            this.DevelopWarningLabel.Size = new System.Drawing.Size(107, 20);
            this.DevelopWarningLabel.TabIndex = 8;
            this.DevelopWarningLabel.Text = "开发者不能为空";
            // 
            // DevelopNumberWarningLabel
            // 
            this.DevelopNumberWarningLabel.AutoSize = true;
            this.DevelopNumberWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.DevelopNumberWarningLabel.Location = new System.Drawing.Point(675, 193);
            this.DevelopNumberWarningLabel.Name = "DevelopNumberWarningLabel";
            this.DevelopNumberWarningLabel.Size = new System.Drawing.Size(107, 20);
            this.DevelopNumberWarningLabel.TabIndex = 9;
            this.DevelopNumberWarningLabel.Text = "序列号不能为空";
            // 
            // OutputPathWarningLabel
            // 
            this.OutputPathWarningLabel.AutoSize = true;
            this.OutputPathWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.OutputPathWarningLabel.Location = new System.Drawing.Point(675, 278);
            this.OutputPathWarningLabel.Name = "OutputPathWarningLabel";
            this.OutputPathWarningLabel.Size = new System.Drawing.Size(121, 20);
            this.OutputPathWarningLabel.TabIndex = 10;
            this.OutputPathWarningLabel.Text = "输出路径不能为空";
            // 
            // Form_Output
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(905, 494);
            this.ControlBox = false;
            this.Controls.Add(this.OutputPathWarningLabel);
            this.Controls.Add(this.DevelopNumberWarningLabel);
            this.Controls.Add(this.DevelopWarningLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.DevelopNumberTextBox);
            this.Controls.Add(this.DeveloperTextBox);
            this.Controls.Add(this.OutputPathlabel);
            this.Controls.Add(this.DevelopNumberLabel);
            this.Controls.Add(this.UserIdlabel);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(911, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(911, 500);
            this.Name = "Form_Output";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_Output_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserIdlabel;
        private System.Windows.Forms.Label DevelopNumberLabel;
        private System.Windows.Forms.Label OutputPathlabel;
        private System.Windows.Forms.TextBox DeveloperTextBox;
        private System.Windows.Forms.TextBox DevelopNumberTextBox;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label DevelopWarningLabel;
        private System.Windows.Forms.Label DevelopNumberWarningLabel;
        private System.Windows.Forms.Label OutputPathWarningLabel;
    }
}