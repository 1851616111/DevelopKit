using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DevelopKit
{
    public partial class Form_Progress : Form
    {
        public Form_Progress(int progressMax)
        {
            InitializeComponent();
            progressBar1.Maximum = progressMax;
            ProgressContentLabel.Text = "";
        }

        public void AddProgressValue(int value, string sceneName)
        {
            if (progressBar1.Value + value <= progressBar1.Maximum)
            {
                progressBar1.Value += value;
                progressBar1.Refresh();

                ProgressContentLabel.Show();
                ProgressContentLabel.Text = string.Format("场景 {0} 已加载", sceneName);
                ProgressContentLabel.Update();
                Thread.Sleep(100);
            }
        }
    }
}
