using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public class GroupCache
    {
        public int GroupLayerId;
        public PngUtil.MergeImageParams GroupImage;
    }

    public delegate void UpdateHandler(Image image);

    public static class CenterBoardController
    {
        public static UpdateHandler updateImageHandler;

        private static Dictionary<int, SortedDictionary<int, GroupCache>> groupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();

        public static void DrawGroupAndSceneView(TableLayoutPanel tabPanel, int sid, int gid, int glid)
        {
            Image image = DrawGroupView(tabPanel, gid);
            DrawSceneView(sid, glid, image);
        }

        //若image为空，则为隐藏group组件
        //glid group layer idx
        public static  void DrawSceneView(int sid, int glid, Image image)
        {
            if (!groupLayerCache.ContainsKey(sid))
            {
                groupLayerCache[sid] = new SortedDictionary<int, GroupCache>();
            }

            if (image == null)
            {
                groupLayerCache[sid].Remove(glid);
            }
            else
            {
                groupLayerCache[sid][glid] = new GroupCache
                {
                    GroupLayerId = glid,
                    GroupImage = new PngUtil.MergeImageParams
                    {
                        Image = image,
                        X = 0,
                        Y = 0,
                    },
                };
            }

            if (groupLayerCache[sid].Count == 0)
            {
                updateImageHandler(null);
            }
            else if (groupLayerCache[sid].Count == 1)
            {
                updateImageHandler(image);
            }
            else
            {
                List<PngUtil.MergeImageParams> ps = new List<PngUtil.MergeImageParams>();
                foreach (GroupCache groupCache in groupLayerCache[sid].Values)
                {
                    ps.Add(groupCache.GroupImage);
                }
                updateImageHandler(PngUtil.MergeImageList(ps));
            }
        }

        public static Image DrawGroupView(TableLayoutPanel tabPanel, int gid)
        {
            List<Property> properties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[gid];
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Property property in properties)
            {
                if (property.Type == PropertyType.Image)
                {
                    Control[] pbCtl = tabPanel.Controls.Find(property.GetPictureBoxId(), true);
                    if (pbCtl.Length == 0)
                    {
                        Console.WriteLine("Get group property picturebox nil, property id={0}", property.Id);
                    }
                    mergeParams.Add(new PngUtil.MergeImageParams
                    {
                        Image = ((PictureBox)pbCtl[0]).Image,
                        X = property.GetLocation().X,
                        Y = property.GetLocation().Y,
                    });
                }
            }

            return PngUtil.MergeImageList(mergeParams);
        }


        //public static void CraeteDrawingBoard(TableLayoutPanel panel, Property property)
        //{
        //    FlowLayoutPanel parentPanel = (FlowLayoutPanel)panel.Parent;
        //    List<Group> groups = GlobalConfig.Project.CarConfig.ListGroupsByIDAndIndex(property.SceneId, property.GroupId, property.GroupLayerIdx);

        //    List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
        //    foreach (Group group in groups)
        //    {
        //        Control[] brotherTablePanel = parentPanel.Controls.Find(group.GetTablePanelId(), true);
        //        if (brotherTablePanel.Length == 0)
        //        {
        //            Console.WriteLine("Get brother table panel nil, group id={0}", group.Id);
        //        }

        //        TableLayoutPanel tableLayout = (TableLayoutPanel)brotherTablePanel[0];

        //        List<Property> groupProperties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];

        //        foreach (Property groupProperty in groupProperties)
        //        {
        //            if (groupProperty.Type == PropertyType.Image)
        //            {
        //                Control[] pbCtl = tableLayout.Controls.Find(groupProperty.GetPictureBoxId(), true);
        //                if (pbCtl.Length == 0)
        //                {
        //                    Console.WriteLine("Get group property picturebox nil, property id={0}", groupProperty.Id);
        //                }
        //                mergeParams.Add(new PngUtil.MergeImageParams
        //                {
        //                    Image = ((PictureBox)pbCtl[0]).Image,
        //                    X = groupProperty.GetLocation().X,
        //                    Y = groupProperty.GetLocation().Y,
        //                });
        //            }
        //        }
        //    }

        //    if (mergeParams.Count >= 2)
        //    {
        //        //GlobalConfig.MainPictureBox.Image = PngUtil.MergeImageList(mergeParams);
        //    }
        //}


        public class CenterImageTemplate
        {
            public Image DrawImage()
            {
                return null;
            }
        }
    }
}
