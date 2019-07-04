using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public delegate void FormDelegate(Object param);

    public enum OperateFileType
    {
        Save=1,
        Close=2,
        SaveAs=3,
    }

    public struct OperateFileReuqest
    {

        public string filepath;
        public OperateFileType operatetype;

        public OperateFileReuqest(OperateFileType opttype, string srcFilePath)
        {
            operatetype = opttype;
            filepath = srcFilePath;
        }
    }
}
