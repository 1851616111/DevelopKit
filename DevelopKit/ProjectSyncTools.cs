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
        public static void Sync(Object form1Obj)
        {
        ParameterizedThreadStart:

        restart:
            Project project = GlobalConfig.Project;
            if (project == null)
            {
                Thread.Sleep(1000 * 5);
                goto restart;
            }

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
                Log.Error("Sync模块", "catch Exception", ex.ToString());
            }

            Thread.Sleep(1000 * 5);

            goto ParameterizedThreadStart;
        }
    }
}
