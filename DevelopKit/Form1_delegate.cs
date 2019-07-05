using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public delegate void FormDelegate(Object param);

    public enum RequestType
    {
        Close = 1,
        MarkFileAsChanged = 2,      
        MarkFileAsSaved = 3,
    }

    public class FormRequest
    {
        public FileType FileType;
        public string FilePath;
        public RequestType RequestType;

        public FormRequest(RequestType reqType, FileType reqFileType, string reqFilePath)
        {
            RequestType = reqType;
            FileType = reqFileType;
            FilePath = reqFilePath;
        }
    }
}
