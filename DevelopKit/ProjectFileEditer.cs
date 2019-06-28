using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DevelopKit
{
    public class ProjectFileEditer
    {
        private LinkedList<ProjectFile> projectFileList;

        public ProjectFileEditer()
        {
            projectFileList = new LinkedList<ProjectFile>();
        }

        public XElement ToXElement()
        {
            XElement element = new XElement("project_files");
            foreach (Object ProjectFileobj in projectFileList)
            {
                ProjectFile pf = (ProjectFile)ProjectFileobj;

                element.Add(pf.ToXElement());
            }

            return element;
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
        public void OpenImage(string filepath)
        {
            ProjectFile projectFile = new ProjectFile();

            projectFile.fileName = StringUtil.GetFileName(filepath);
            projectFile.filePath = filepath;
            projectFile.fileType = FileType.Image;

            projectFileList.AddLast(projectFile);
        }
    }

    public enum FileType
    {
        Txt = 1,
        Image = 2
    }

    public class ProjectFile
    {
        public string fileName;  //read.txt write 
        public string filePath;  //C:\Programs\read.txt   
        public FileType fileType; // Txt Img

        public XElement ToXElement()
        {
            XElement element = new XElement("project_file");
            element.SetElementValue("name", fileName);
            element.SetElementValue("path", filePath);
            element.SetElementValue("type", fileType);

            return element;
        }
    }
}