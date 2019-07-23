using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public class CenterBoardController
    {
        public int OpenedSceneId;

        private Panel CenterBoardPenel;
        private TabPage CenterBoardTabPage;
        private PictureBox CenterBoardPictureBox;
        private TrackBar CenterBoardTrackBar;
        private Label CenterBoardLabel;
        private StatusStrip CenterBoardToolStrip;

        private Dictionary<int, PictureBox> PropertyPictureBoxCache;
        private Dictionary<int, FlowLayoutPanel> SceneFlowLayoutPanelMap;
        private Dictionary<int, CenterBoardData> SceneCenterBoardDataMap;
        private Dictionary<int, SortedDictionary<int, GroupCache>> GroupLayerCache;

        public CenterBoardController() { }
        public CenterBoardController(Panel centerPenel, TabPage tabPage)
        {

            CenterBoardPenel = centerPenel;
            CenterBoardTabPage = tabPage;
            PropertyPictureBoxCache = new Dictionary<int, PictureBox>();
            SceneFlowLayoutPanelMap = new Dictionary<int, FlowLayoutPanel>();
            SceneCenterBoardDataMap = new Dictionary<int, CenterBoardData>();
            GroupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();

            foreach (Control control in tabPage.Controls)
            {
                if (control.GetType() == typeof(PictureBox))
                {
                    CenterBoardPictureBox = (PictureBox)control;
                }
                else if (control.GetType() == typeof(TrackBar))
                {
                    CenterBoardTrackBar = (TrackBar)control;
                }
                else if (control.GetType() == typeof(Label))
                {
                    CenterBoardLabel = (Label)control;
                }
                else if (control.GetType() == typeof(StatusStrip))
                {
                    CenterBoardToolStrip = (StatusStrip)control;
                }
            }

            ResetCenterBoard(false);
        }

        public class GroupCache
        {
            public int GroupLayerId;
            public GroupSize GroupSize;
            public List<PngUtil.MergeImageParams> GroupPropertiesImages;
        }

        public class CenterBoardData
        {
            public int TrackBarMaxValue;
            public int TrackBarMinValue;
            public int TrackBarValue;
            public int PictureBoxWidth;
            public Image PictureBoxImage;
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
                ResetCenterBoard(false);
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

            CenterBoardPenel.Controls.Add(flowLayoutPanel);

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

            ResetCenterBoard(true);
            CenterBoardPictureBox.Image = SceneCenterBoardDataMap[sceneID].PictureBoxImage;
            CenterBoardTrackBar.Maximum = SceneCenterBoardDataMap[sceneID].TrackBarMaxValue;
            CenterBoardTrackBar.Minimum = SceneCenterBoardDataMap[sceneID].TrackBarMinValue;
            CenterBoardTrackBar.Value = SceneCenterBoardDataMap[sceneID].TrackBarValue;
            CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
            CenterBoardToolStrip.Items[0].Text = string.Format("{0}*{1}",
                  (int)(SceneCenterBoardDataMap[sceneID].PictureBoxImage.Width * SceneCenterBoardDataMap[sceneID].TrackBarValue / 10000F),
                  (int)(SceneCenterBoardDataMap[sceneID].PictureBoxImage.Height * SceneCenterBoardDataMap[sceneID].TrackBarValue / 10000F));
        }

        private void UpdateCenterBoard(int sceneID, Image image)
        {
            if (image != null)
            {
                ResetCenterBoard(true);

                if (!SceneCenterBoardDataMap.ContainsKey(sceneID))
                {
                    int percent100 = (int)(CenterBoardTabPage.Width * 10000F / image.Width);
                    CenterBoardTrackBar.Maximum = 10000;
                    CenterBoardTrackBar.Minimum = percent100;
                    CenterBoardTrackBar.Value = percent100;
                    CenterBoardTrackBar.TickFrequency = 500;
                    CenterBoardLabel.Location = new Point(CenterBoardTrackBar.Location.X + CenterBoardTrackBar.Width + 20, CenterBoardTrackBar.Location.Y);
                    CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
                    CenterBoardPictureBox.Width = CenterBoardTabPage.Width;

                    SceneCenterBoardDataMap.Add(sceneID, new CenterBoardData
                    {
                        PictureBoxImage = CenterBoardPictureBox.Image,
                        PictureBoxWidth = CenterBoardTabPage.Width,
                        TrackBarMaxValue = 10000,
                        TrackBarMinValue = percent100,
                        TrackBarValue = percent100,
                    });
                }
                else
                {
                    CenterBoardTrackBar.Maximum = SceneCenterBoardDataMap[sceneID].TrackBarMaxValue;
                    CenterBoardTrackBar.Minimum = SceneCenterBoardDataMap[sceneID].TrackBarMinValue;
                    CenterBoardTrackBar.Value = SceneCenterBoardDataMap[sceneID].TrackBarValue;
                    CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
                }

                SceneCenterBoardDataMap[sceneID].PictureBoxImage = image;
                CenterBoardPictureBox.Width = (int)((CenterBoardTrackBar.Value / 10000F) * image.Width);
                CenterBoardToolStrip.Items[0].Text = string.Format("{0}*{1}",
                    CenterBoardPictureBox.Width,
                    (int)(CenterBoardTrackBar.Value / 10000F * image.Height));
            }
            else
            {
                ResetCenterBoard(false);
            }
            CenterBoardPictureBox.Image = image;
            OpenedSceneId = sceneID;
        }

        public void CenterBoardBarOnScroll()
        {
            CenterBoardPictureBox.Width = (int)(CenterBoardPictureBox.Image.Width * (CenterBoardTrackBar.Value / 10000F));
            int imageHeight = (int)(CenterBoardPictureBox.Image.Height * (CenterBoardTrackBar.Value / 10000F));
            CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100).ToString() + "%";
            SceneCenterBoardDataMap[OpenedSceneId].TrackBarValue = CenterBoardTrackBar.Value;
            CenterBoardToolStrip.Items[0].Text = string.Format("{0}*{1}", CenterBoardPictureBox.Width, imageHeight);
        }

        public void ResetCenterBoard(bool visible)
        {
            CenterBoardTrackBar.Visible = visible;
            CenterBoardLabel.Visible = visible;
            CenterBoardToolStrip.Visible = visible;
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
