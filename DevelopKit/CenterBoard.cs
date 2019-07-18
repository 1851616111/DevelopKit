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
        public List<PngUtil.MergeImageParams> GroupPropertiesImages;
    }

    public delegate void UpdateHandler(Image image);

    public static class CenterBoardController
    {
        public static UpdateHandler updateImageHandler;

        private static Dictionary<int, SortedDictionary<int, GroupCache>> groupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();

        public static void DrawGroupAndSceneView(TableLayoutPanel tabPanel, Group group)
        {
            //Image image = DrawGroupView(tabPanel, group);
            List<PngUtil.MergeImageParams> ps = ListGroupImages(tabPanel, group);
            DrawSceneView(group, ps);
        }


        //若image为空，则为隐藏group组件
        //glid group layer idx
        public static void DrawSceneView(Group group, List<PngUtil.MergeImageParams> ps)
        {
            if (!groupLayerCache.ContainsKey(group.Sceneid))
            {
                groupLayerCache[group.Sceneid] = new SortedDictionary<int, GroupCache>();
            }

            if (ps == null)
            {
                groupLayerCache[group.Sceneid].Remove(group.LayerIndex);
            }
            else
            {
                groupLayerCache[group.Sceneid][group.LayerIndex] = new GroupCache
                {
                    GroupLayerId = group.LayerIndex,
                    GroupPropertiesImages = ps,
                };
            }

            if (groupLayerCache[group.Sceneid].Count == 0)
            {
                updateImageHandler(null);
            }
            else
            {
                List<PngUtil.MergeImageParams> resultList = null;
                foreach (GroupCache groupCache in groupLayerCache[group.Sceneid].Values)
                {
                    if (resultList == null)
                    {
                        resultList = groupCache.GroupPropertiesImages;
                    }
                    else
                    {
                        resultList = resultList.Union(groupCache.GroupPropertiesImages).ToList();
                    }   
                }
                updateImageHandler(PngUtil.MergeImageList(resultList));
            }
        }


        ////若image为空，则为隐藏group组件
        ////glid group layer idx
        //public static  void DrawSceneView(int sid, int glid, Image image)
        //{
        //    if (!groupLayerCache.ContainsKey(sid))
        //    {
        //        groupLayerCache[sid] = new SortedDictionary<int, GroupCache>();
        //    }

        //    if (image == null)
        //    {
        //        groupLayerCache[sid].Remove(glid);
        //    }
        //    else
        //    {
        //        groupLayerCache[sid][glid] = new GroupCache
        //        {
        //            GroupLayerId = glid,
        //            GroupPropertiesImages = new PngUtil.MergeImageParams
        //            {
        //                Image = image,
        //                X = 0,
        //                Y = 0,
        //            },
        //        };
        //    }

        //    if (groupLayerCache[sid].Count == 0)
        //    {
        //        updateImageHandler(null);
        //    }
        //    else
        //    {
        //        List<PngUtil.MergeImageParams> ps = new List<PngUtil.MergeImageParams>();
        //        foreach (GroupCache groupCache in groupLayerCache[sid].Values)
        //        {
        //            ps.Add(groupCache.GroupPropertiesImages);
        //        }
        //        updateImageHandler(PngUtil.MergeImageList(ps));
        //    }
        //}


        ////若image为空，则为隐藏group组件
        ////glid group layer idx
        //public static void DrawSceneView(int sid, int glid, Image image)
        //{
        //    if (!groupLayerCache.ContainsKey(sid))
        //    {
        //        groupLayerCache[sid] = new SortedDictionary<int, GroupCache>();
        //    }

        //    if (image == null)
        //    {
        //        groupLayerCache[sid].Remove(glid);
        //    }
        //    else
        //    {
        //        groupLayerCache[sid][glid] = new GroupCache
        //        {
        //            GroupLayerId = glid,
        //            GroupPropertiesImages = new PngUtil.MergeImageParams
        //            {
        //                Image = image,
        //                X = 0,
        //                Y = 0,
        //            },
        //        };
        //    }

        //    if (groupLayerCache[sid].Count == 0)
        //    {
        //        updateImageHandler(null);
        //    }
        //    else
        //    {
        //        List<PngUtil.MergeImageParams> ps = new List<PngUtil.MergeImageParams>();
        //        foreach (GroupCache groupCache in groupLayerCache[sid].Values)
        //        {
        //            ps.Add(groupCache.GroupPropertiesImages);
        //        }
        //        updateImageHandler(PngUtil.MergeImageList(ps));
        //    }
        //}


        public static Image DrawGroupView(TableLayoutPanel tabPanel, Group group)
        {
            List<Property> properties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Property property in properties)
            {
                if (property.Type == PropertyType.Image ||property.OptType == PropertyOperateType.FilterImageAlpha || property.OptType == PropertyOperateType.FilterImageColor)
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

            return PngUtil.MergeImageList(mergeParams, group.Size.Width, group.Size.Height);
        }

        public static List<PngUtil.MergeImageParams> ListGroupImages(TableLayoutPanel tabPanel, Group group)
        {
            List<Property> properties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Property property in properties)
            {
                if (property.Type == PropertyType.Image || property.OptType == PropertyOperateType.FilterImageAlpha || property.OptType == PropertyOperateType.FilterImageColor)
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

            return mergeParams;
        }


    }
}
