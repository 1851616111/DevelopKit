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
        public GroupSize GroupSize;
        public List<PngUtil.MergeImageParams> GroupPropertiesImages;
    }

    public delegate void UpdateHandler(Image image);

    public static class CenterBoardCache
    {
        public static UpdateHandler SetCenterBoardImageHandler;
        //注意PictureBox的声明周期，若无限绑定内存可能会溢出
        private static Dictionary<int, PictureBox> PictureBoxCache = new Dictionary<int, PictureBox>();
        private static Dictionary<int, SortedDictionary<int, GroupCache>> groupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();

        public static void SetPictureBox(int key, PictureBox pb)
        {
            PictureBoxCache[key] = pb;
        }

        public static PictureBox GetPictureBox(int key)
        {
            if (PictureBoxCache.ContainsKey(key))
            {
                return PictureBoxCache[key];
            } else {
                return null;
            }
        }

        public static void ShowGroupOnCenterBoard(TableLayoutPanel tabPanel, Group group)
        {
            //Image image = DrawGroupView(tabPanel, group);
            List<PngUtil.MergeImageParams> ps = ListGroupImages(tabPanel, group);
            HideGroupOnCenterBoard(group, ps);
        }

        public static void HideGroupOnCenterBoard(Group group, List<PngUtil.MergeImageParams> ps)
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
                    GroupSize = group.Size,
                    GroupPropertiesImages = ps,
                };
            }

            if (groupLayerCache[group.Sceneid].Count == 0)
            {
                SetCenterBoardImageHandler(null);
            }
            else
            {
                List<PngUtil.MergeImageParams> resultList = null;
                GroupSize maxGroupSize = null;
                foreach (GroupCache groupCache in groupLayerCache[group.Sceneid].Values)
                {
                    if (maxGroupSize == null || groupCache.GroupSize.Width > maxGroupSize.Width)
                    {
                        maxGroupSize = groupCache.GroupSize;
                    } 
                        if (resultList == null)
                    {
                        resultList = groupCache.GroupPropertiesImages;
                    }
                    else
                    {
                        resultList = resultList.Union(groupCache.GroupPropertiesImages).ToList();
                    }   

                }
                SetCenterBoardImageHandler(PngUtil.MergeImageList(resultList, maxGroupSize.Width, maxGroupSize.Height));
            }
        }

        private static List<PngUtil.MergeImageParams> ListGroupImages(TableLayoutPanel tabPanel, Group group)
        {
            List<Property> properties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Property property in properties)
            {
                if (property.Type == PropertyType.Image || property.OptType == PropertyOperateType.AlphaWhiteImageSetAlpha || property.OptType == PropertyOperateType.AlphaWhiteImageSetColor)
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
