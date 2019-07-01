﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public partial class Form1_Image : Form
    {
        public FormDelegate formDelegateHandler;
        public Form1_Image()
        {
            InitializeComponent();
        }

        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hashtable hashTable = (Hashtable)this.Tag;
            formDelegateHandler(new OperateFileReuqest(OperateFileType.Save, (string)hashTable["filepath"]));
        }

        private void Form_Image_Load(object sender, EventArgs e)
        {

        }

        private void CloseImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.关闭自身窗体
            this.Close();
            this.Dispose();

            //2. 通知主form 关闭tabpage
            Hashtable hashTable = (Hashtable)this.Tag;
            formDelegateHandler(new OperateFileReuqest(OperateFileType.Close, (string)hashTable["filepath"]));

        }
    }
}
