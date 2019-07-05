using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace DevelopKit
{
    public partial class Form1_Txt : Form
    {
        public FormDelegate formDelegateHandler;
        public string ProjectUserDir;
        public string OriginalRichTextBoxData;
        public string filepath;
        public string filename;

        public Form1_Txt()
        {
            InitializeComponent();
        }

        private void Form1_Txt_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = OriginalRichTextBoxData;
        }

        private void CloseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.关闭自身窗体
            this.Close();
            this.Dispose();

            formDelegateHandler(new FormRequest(RequestType.Close, FileType.Txt, filepath));
        }

        //1.另存为
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = ProjectUserDir,
                FileName = filename,
                Filter = "Txt|*.text|所有文件|*.*",
                RestoreDirectory = true,
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileUtil.WriteStringToFile(saveFileDialog.FileName, richTextBox1.Text);
            }

            OriginalRichTextBoxData = richTextBox1.Text;
            formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Txt, filepath));
        }

        //2. 保存到原文件
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filepath))
            {
                DialogResult dialogResult = MessageBox.Show("文件" + filepath + "已不存在，请否要保存", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    FileUtil.WriteStringToFile(filepath, richTextBox1.Text);
                }
            }
            else {
                if (!FileUtil.FlushStringToFile(filepath, richTextBox1.Text))
                {
                    Log.Error("Form1_txt", "SavefileToolStripMenuItem_Click", "保存文件内容失败");
                }
            }

            OriginalRichTextBoxData = richTextBox1.Text;
            formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Txt, filepath));
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text != OriginalRichTextBoxData)
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsChanged, FileType.Txt, filepath));
            }
            else
            {
                formDelegateHandler(new FormRequest(RequestType.MarkFileAsSaved, FileType.Txt, filepath));
            }
        }
    }
}
