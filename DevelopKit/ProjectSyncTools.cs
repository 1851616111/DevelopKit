using System;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DevelopKit
{
    public static class ProjectSyncTools
    {
        public static void Sync(Object projectObj)
        {
    ParameterizedThreadStart:
            Project project = (Project)(projectObj);
            try
            {
                Console.WriteLine("project.GetConfigXml() ------> " + project.GetConfigXml());
                FileStream fileStream = new FileStream(project.GetConfigXml(), FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] fileData = new byte[fileStream.Length];
                try
                {
                    fileStream.Read(fileData, 0, (int)(fileStream.Length));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("project read xml error:" + ex.ToString());
                }

                string newDataStr = project.ToXElement().ToString();
                Console.WriteLine("fileData ------> ", fileData);
                Console.WriteLine("newDataStr ------> ", newDataStr);
                byte[] memData = Encoding.UTF8.GetBytes(newDataStr);

                if (!ByteUtil.Diff(fileData, memData))
                {
                    FileUtil.WriteStringToFile(project.GetConfigXml(), newDataStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("project sync xml error:" + ex.ToString());
            }

            Thread.Sleep(1000 * 5);
            Console.WriteLine("restart");
            goto ParameterizedThreadStart;
        }

        private static bool diffFile(Project project)
        {
            FileStream fileStream = new FileStream(project.GetConfigXml(), FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] fileData = new byte[fileStream.Length];
            try
            {
                fileStream.Read(fileData, 0, (int)(fileStream.Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine("project read xml error:" + ex.ToString());
            }

            byte[] memData = Encoding.UTF8.GetBytes(project.ToXElement().ToString());

            return ByteUtil.Diff(fileData, memData);
        }
    }
}
