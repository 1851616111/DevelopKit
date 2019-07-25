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

    public class CenterBoardData
    {
        public Image PictureBoxImage;
    }

    public class CenterBoardController
    {
        private int OpenedSceneId;
        private int minPictureBoxWidth;
        private Panel RightPenel;
        private PictureBox CenterBoardPictureBox;
        private ToolStripLabel CenterBoardImageSizeLabel;
        private ToolStripLabel CenterBoardPictureBoxSizeLabel;

        private Dictionary<int, PictureBox> PropertyPictureBoxCache;
        private Dictionary<int, FlowLayoutPanel> SceneFlowLayoutPanelMap;
        private Dictionary<int, CenterBoardData> SceneCenterBoardDataMap;
        private Dictionary<int, SortedDictionary<int, GroupCache>> GroupLayerCache;

        public CenterBoardController() { }
        public CenterBoardController(PictureBox centerPb, Panel penel, ToolStripLabel imageSizeLabel, ToolStripLabel pbSizeLabel, int width)
        {
            minPictureBoxWidth = width;
            RightPenel = penel;
            CenterBoardPictureBox = centerPb;
            CenterBoardImageSizeLabel = imageSizeLabel;
            CenterBoardPictureBoxSizeLabel = pbSizeLabel;
            SetCenterBoardPictureBoxWidth(width);

            PropertyPictureBoxCache = new Dictionary<int, PictureBox>();
            SceneFlowLayoutPanelMap = new Dictionary<int, FlowLayoutPanel>();
            SceneCenterBoardDataMap = new Dictionary<int, CenterBoardData>();
            GroupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();
        }

        public void ScrollUpCenterBoardPictureBox()
        {
            if (CenterBoardPictureBox.Image == null || CenterBoardPictureBox.Image.Width == CenterBoardPictureBox.Width)
                return;
    
            int StepWidth = 100;

            if (CenterBoardPictureBox.Image.Width - CenterBoardPictureBox.Width >= StepWidth)
                SetCenterBoardPictureBoxWidth(CenterBoardPictureBox.Width + StepWidth);
            else
                SetCenterBoardPictureBoxWidth(CenterBoardPictureBox.Image.Width);
        }

        public void ScrollDownCenterBoardPictureBox()
        {
            if (CenterBoardPictureBox.Image == null || CenterBoardPictureBox.Width == minPictureBoxWidth)
                return;

            int StepWidth = 100;

            if (CenterBoardPictureBox.Width - minPictureBoxWidth >= StepWidth)
                SetCenterBoardPictureBoxWidth(CenterBoardPictureBox.Width - StepWidth);
            else
                SetCenterBoardPictureBoxWidth(minPictureBoxWidth);
        }


        public void SetCenterBoardPictureBoxWidth(int width)
        {
            CenterBoardPictureBox.Width = width;
            CenterBoardPictureBox.Refresh();
            if (CenterBoardPictureBox.Image == null)
            {
                CenterBoardImageSizeLabel.Text = "";
                CenterBoardPictureBoxSizeLabel.Text = "";
            }
            else
            {
                SetCenterBoardSizeLabel();
            }
        }

        public void SetCenterBoardPictureBoxImage(Image image)
        {
            CenterBoardPictureBox.Image = image;
            CenterBoardPictureBox.Refresh();

            if (CenterBoardPictureBox.Image == null)
            {
                CenterBoardImageSizeLabel.Text = "";
                CenterBoardPictureBoxSizeLabel.Text = "";
            }
            else
            {
                SetCenterBoardSizeLabel();
            }
        }

        private void SetCenterBoardSizeLabel()
        {
            float percent = (float)CenterBoardPictureBox.Image.Height / CenterBoardPictureBox.Image.Width;
            CenterBoardImageSizeLabel.Text = string.Format("图片尺寸:{0}*{1}", CenterBoardPictureBox.Image.Width, CenterBoardPictureBox.Image.Height);
            CenterBoardPictureBoxSizeLabel.Text = string.Format("显示尺寸:{0}*{1}", CenterBoardPictureBox.Width, (int)(CenterBoardPictureBox.Width * percent));
        }

        public void DoubleClickScene(int newSceneID)
        {
            if (newSceneID == OpenedSceneId)
                return;

            //隐藏上一次打开的场景
            if (OpenedSceneId > 0 && SceneFlowLayoutPanelMap.ContainsKey(OpenedSceneId))
            {
                SceneFlowLayoutPanelMap[OpenedSceneId].Visible = false;
                SceneFlowLayoutPanelMap[OpenedSceneId].Enabled = false;
                CenterBoardPictureBox.Image = null;
                CenterBoardPictureBox.Refresh();
            }

            if (SceneFlowLayoutPanelMap.ContainsKey(newSceneID))
            {
                SceneFlowLayoutPanelMap[newSceneID].Visible = true;
                SceneFlowLayoutPanelMap[newSceneID].Enabled = true;
                ShowCenterBoard(newSceneID);
            }
            else
            {
                SceneFlowLayoutPanelMap.Add(newSceneID, loadSceneFlowLayoutPanel(newSceneID));
            }

            OpenedSceneId = newSceneID;
        }

        private FlowLayoutPanel loadSceneFlowLayoutPanel(int sceneId)
        {
            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(sceneId);
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();

            RightPenel.Controls.Add(flowLayoutPanel);

            bool setFlow = false;
            foreach (Group group in GlobalConfig.Project.CarConfig.SceneIdToGroupsMapping[scene.Id])
            {
                TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                flowLayoutPanel.Controls.Add(tableLayoutPanel);
                if (!setFlow)
                {
                    Form1_FlowPanel.LoadFlowPanelConfig(flowLayoutPanel);
                    setFlow = true;
                }
                Form1_FlowPanel.LoadGroupTablePanelConfig(tableLayoutPanel, flowLayoutPanel.Width, group);
                Form1_FlowPanel.LoadGroupTablePanelData(group, tableLayoutPanel, GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id], GlobalConfig.UiConfig.PropertyRowHeight);
            }

            return flowLayoutPanel;
        }

        public void ShowCenterBoard(int sceneID)
        {
            if (!SceneCenterBoardDataMap.ContainsKey(sceneID))
                return;

            SetCenterBoardPictureBoxImage(SceneCenterBoardDataMap[sceneID].PictureBoxImage);
            CenterBoardImageSizeLabel.Text = string.Format("{0}*{1}", CenterBoardPictureBox.Width, CenterBoardPictureBox.Height);
        }

        private void UpdateCenterBoard(int sceneID, Image image)
        {
            if (image != null)
            {
                if (!SceneCenterBoardDataMap.ContainsKey(sceneID))
                {
                    SceneCenterBoardDataMap.Add(sceneID, new CenterBoardData
                    {
                        PictureBoxImage = CenterBoardPictureBox.Image,
                    });
                }
                else
                {
                    SceneCenterBoardDataMap[sceneID].PictureBoxImage = CenterBoardPictureBox.Image;
                }

                CenterBoardImageSizeLabel.Text = string.Format("{0}*{1}", CenterBoardPictureBox.Width, CenterBoardPictureBox.Height);
            }

            SetCenterBoardPictureBoxImage(image);
            OpenedSceneId = sceneID;
        }

        public void SetPictureBox(int key, PictureBox pb)
        {
            PropertyPictureBoxCache[key] = pb;
        }

        public PictureBox GetPictureBox(int key)
        {
            if (PropertyPictureBoxCache.ContainsKey(key))
            {
                return PropertyPictureBoxCache[key];
            }
            else
            {
                return null;
            }
        }

        public void ShowGroupOnCenterBoard(TableLayoutPanel tabPanel, Group group)
        {
            List<PngUtil.MergeImageParams> ps = ListGroupImages(tabPanel, group);
            HideGroupOnCenterBoard(group, ps);
        }

        public void HideGroupOnCenterBoard(Group group, List<PngUtil.MergeImageParams> ps)
        {
            if (!GroupLayerCache.ContainsKey(group.Sceneid))
            {
                GroupLayerCache[group.Sceneid] = new SortedDictionary<int, GroupCache>();
            }

            if (ps == null)
            {
                GroupLayerCache[group.Sceneid].Remove(group.LayerIndex);
            }
            else
            {
                GroupLayerCache[group.Sceneid][group.LayerIndex] = new GroupCache
                {
                    GroupLayerId = group.LayerIndex,
                    GroupSize = group.Size,
                    GroupPropertiesImages = ps,
                };
            }

            if (GroupLayerCache[group.Sceneid].Count == 0)
            {
                UpdateCenterBoard(group.Sceneid, null);
            }
            else
            {
                List<PngUtil.MergeImageParams> resultList = null;
                GroupSize maxGroupSize = null;
                foreach (GroupCache groupCache in GroupLayerCache[group.Sceneid].Values)
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
                UpdateCenterBoard(group.Sceneid, PngUtil.MergeImageList(resultList, maxGroupSize.Width, maxGroupSize.Height));
            }
        }

        private List<PngUtil.MergeImageParams> ListGroupImages(TableLayoutPanel tabPanel, Group group)
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
