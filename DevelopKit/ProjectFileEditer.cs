using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace DevelopKit
{
    [Serializable]
    [XmlRoot("files_editer")]
    public class RecordFile
    {

        [XmlElementAttribute(ElementName = "file")]
        public List<ProjectFile> projectFileList;

        public RecordFile()
        {
            projectFileList = new List<ProjectFile>();
        }

        public bool IsFileInProjectDir(string filepath)
        {
            foreach (ProjectFile file in projectFileList)
            {
                if (file.filePath == filepath) return true;
            }

            return false;
        }

        //不论用户打开的是外部文件还是内部文件， 都需要存储FileEditor中， 已边下次打开Kit使用
        public void AddOpenedFile(string filepath)
        {
            ProjectFile projectFile = new ProjectFile(filepath);
          
            projectFileList.Add(projectFile);
        }

        public bool CloseOpenedFile(string filepath)
        {
            foreach (ProjectFile file in projectFileList)
            {
                if (filepath == file.filePath)
                {
                    projectFileList.Remove(file);
                    return true;
                }
            }
            return false;
        }
    }

    public enum FileType
    {
        Txt = 1,
        Image = 2
    }

    public class ProjectFile
    {
        [XmlElement("name")]
        public string fileName;  //read.txt write 
        [XmlElement("path")]
        public string filePath;  //C:\Programs\read.txt   
        [XmlElement("type")]
        public FileType fileType; // Txt Img

        public ProjectFile()
        { }

        public ProjectFile(string filepath)
        {
           fileName = StringUtil.GetFileName(filepath);
           filePath = filepath;


            if (FileUtil.IsFileImage(filepath))
            {
                fileType = FileType.Image;
            }
            else
            {
                fileType = FileType.Txt;
            }
        }
    }
}