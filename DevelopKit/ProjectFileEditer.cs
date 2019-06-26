using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public class ProjectFileEditer
    {
        private LinkedList<ProjectFile> projectFileList;

        public ProjectFileEditer()
        {
            projectFileList = new LinkedList<ProjectFile>();
        }

        public bool IsFileInProjectDir(string filepath)
        {
            foreach (ProjectFile file in projectFileList)
            {
                if (file.filePath == filepath) return true;
            }

            return false;
        }

        //新打开一个图片或文件后，当没有保存到本地时，暂时不需要追踪内容一致性
        //保存到项目中后需要开始追踪状态
        public void OpenImage(string filepath)
        {
            ProjectFile projectFile = new ProjectFile();

            projectFile.fileName = StringUtil.GetFileName(filepath);
            projectFile.filePath = filepath;
            projectFile.fileType = FileType.Image;
            projectFile.ifOpen = true;
            projectFile.ifSaveToProject = false;
        }
    }

    public enum FileType
    {
        Txt = 1,
        Image = 2
    }

    public struct ProjectFile
    {
        public string fileName;  //read.txt write 
        public string filePath;  //C:\Programs\read.txt   
        public FileType fileType; // Txt 
        public bool ifOpen; //是否用户正在编辑这个图片
        public bool ifSaveToProject; //是否保存到项目制定目录中
        public System.Drawing.Bitmap savedBitmap; //最新的已保存的图片内容
    }
}