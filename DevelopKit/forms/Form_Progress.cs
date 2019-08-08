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
        private bool WithDetail;
        public Form_Progress(int progressMax, bool withDetail)
        {
            InitializeComponent();
            progressBar1.Maximum = progressMax;
            ProgressContentLabel.Text = "";
            if (!withDetail)
            {
                listBox1.Visible = false;
                this.Height = 115;
            }

            WithDetail = withDetail;
        }

        public void SetProgressMax(int max)
        {
            progressBar1.Maximum = max;
        }

        public void AddProgressValue(int value, string label)
        {
            if (progressBar1.Value + value <= progressBar1.Maximum)
            {
                progressBar1.Value += value;
                progressBar1.Refresh();

                if (WithDetail)
                {
                    AppendDetails(label);
                }
                else {
                    ProgressContentLabel.Show();
                    ProgressContentLabel.Text = label;
                    ProgressContentLabel.Update();
                }
                
                Thread.Sleep(80);
            }
        }

        private void AppendDetails(string detailItem)
        {
            listBox1.Items.Add(detailItem);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = -1;
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
