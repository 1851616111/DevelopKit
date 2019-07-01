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
    public class FilesEditer
    {

        [XmlElementAttribute(ElementName = "file")]
        public List<ProjectFile> projectFileList;

        public FilesEditer()
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
        public void RecordFile(string filepath)
        {
            ProjectFile projectFile = new ProjectFile();

            projectFile.fileName = StringUtil.GetFileName(filepath);
            projectFile.filePath = filepath;


            if (FileUtil.IsFileImage(filepath))
            {
                projectFile.fileType = FileType.Image;
            }
            else {
                projectFile.fileType = FileType.Txt;
            }
            Console.WriteLine("-------->>>rojectFileList.Add(projectFile " + filepath);
            projectFileList.Add(projectFile);
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
    }
}