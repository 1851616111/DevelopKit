using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public delegate void FormDelegate(Object param);

    public enum OperateImageType
    {
        Save=1,
        Close=2
    }
    public struct OperateImageReuqest
    {

        public string filepath;
        public OperateImageType operatetype;

        public OperateImageReuqest(OperateImageType opttype, string srcFilePath)
        {
            operatetype = opttype;
            filepath = srcFilePath;
        }
    }
}
