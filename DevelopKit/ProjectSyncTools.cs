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
                byte[] fileData = project.ReadXmlFileBytes();
                byte[] memData = Encoding.UTF8.GetBytes(project.SerializeToStrInMemory());

                if (!ByteUtil.Diff(fileData, memData))
                {
                    Log.Error("Sync模块", "对比XML不同", "");
                    Log.Error("Sync模块", "原磁盘数据", Encoding.UTF8.GetString(fileData));
                    Log.Error("Sync模块", "新内存数据", Encoding.UTF8.GetString(memData));

                    if (!FileUtil.FlushBytesToFile(project.GetConfigXml(), memData))
                    {
                        Log.Error("Sync模块", "FlushBytesToFile", "failed");
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("project sync xml error:" + ex.ToString());
            }

            Thread.Sleep(1000 * 5);

            goto ParameterizedThreadStart;
        }
    }
}
