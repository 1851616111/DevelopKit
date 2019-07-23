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
        public int TrackBarMaxValue;
        public int TrackBarMinValue;
        public int TrackBarValue;
        public int PictureBoxWidth;
        public Image PictureBoxImage;
    }

    public static class CenterBoardController
    {
        private static int OpenedSceneId;
        private static Panel CenterBoardPenel;
        private static TabPage CenterBoardTabPage;
        private static PictureBox CenterBoardPictureBox;
        private static TrackBar CenterBoardTrackBar;
        private static Label CenterBoardLabel;
        private static StatusStrip CenterBoardToolStrip;

        private static Dictionary<int, FlowLayoutPanel> sceneFlowLayoutPanelMap = new Dictionary<int, FlowLayoutPanel>();
        private static Dictionary<int, CenterBoardData> SceneCenterBoardData = new Dictionary<int, CenterBoardData>();
        private static Dictionary<int, PictureBox> PictureBoxCache = new Dictionary<int, PictureBox>();         //注意PictureBox的声明周期，若无限绑定内存可能会溢出
        private static Dictionary<int, SortedDictionary<int, GroupCache>> groupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();

        public static void NewCenterBoardController(Panel centerPenel, TabPage tabPage)
        {
            CenterBoardPenel = centerPenel;
            CenterBoardTabPage = tabPage;

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
        }

        public static void DoubleClickScene(int newSceneID)
        {
            if (newSceneID == OpenedSceneId)
                return;

            //隐藏上一次打开的场景
            if (OpenedSceneId > 0 && sceneFlowLayoutPanelMap.ContainsKey(OpenedSceneId))
            {
                sceneFlowLayoutPanelMap[OpenedSceneId].Visible = false;
                sceneFlowLayoutPanelMap[OpenedSceneId].Enabled = false;
                CenterBoardPictureBox.Image = null;
                CenterBoardPictureBox.Refresh();
                CenterBoardController.ResetCenterBoard(false);
            }

            if (sceneFlowLayoutPanelMap.ContainsKey(newSceneID))
            {
                sceneFlowLayoutPanelMap[newSceneID].Visible = true;
                sceneFlowLayoutPanelMap[newSceneID].Enabled = true;
                CenterBoardController.ShowCenterBoard(newSceneID);
            }
            else
            {
                sceneFlowLayoutPanelMap.Add(newSceneID, loadSceneFlowLayoutPanel(newSceneID));
            }

            OpenedSceneId = newSceneID;
        }

        private static FlowLayoutPanel loadSceneFlowLayoutPanel(int sceneId)
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

        public static void ShowCenterBoard(int sceneID)
        {
            if (!SceneCenterBoardData.ContainsKey(sceneID))
                return;

            ResetCenterBoard(true);
            CenterBoardPictureBox.Image = SceneCenterBoardData[sceneID].PictureBoxImage;
            CenterBoardTrackBar.Maximum = SceneCenterBoardData[sceneID].TrackBarMaxValue;
            CenterBoardTrackBar.Minimum = SceneCenterBoardData[sceneID].TrackBarMinValue;
            CenterBoardTrackBar.Value = SceneCenterBoardData[sceneID].TrackBarValue;
            CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
            CenterBoardToolStrip.Items[0].Text = string.Format("{0}*{1}",
                  (int)(SceneCenterBoardData[sceneID].PictureBoxImage.Width * SceneCenterBoardData[sceneID].TrackBarValue / 10000F),
                  (int)(SceneCenterBoardData[sceneID].PictureBoxImage.Height * SceneCenterBoardData[sceneID].TrackBarValue / 10000F));
        }

        private static void UpdateCenterBoard(int sceneID, Image image)
        {
            if (image != null)
            {
                ResetCenterBoard(true);

                if (!SceneCenterBoardData.ContainsKey(sceneID))
                {
                    int percent100 = (int)(CenterBoardTabPage.Width * 10000F / image.Width);
                    CenterBoardTrackBar.Maximum = 10000;
                    CenterBoardTrackBar.Minimum = percent100;
                    CenterBoardTrackBar.Value = percent100;
                    CenterBoardTrackBar.TickFrequency = 500;
                    CenterBoardLabel.Location = new Point(CenterBoardTrackBar.Location.X + CenterBoardTrackBar.Width + 20, CenterBoardTrackBar.Location.Y);
                    CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
                    CenterBoardPictureBox.Width = CenterBoardTabPage.Width;

                    SceneCenterBoardData.Add(sceneID, new CenterBoardData
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
                    CenterBoardTrackBar.Maximum = SceneCenterBoardData[sceneID].TrackBarMaxValue;
                    CenterBoardTrackBar.Minimum = SceneCenterBoardData[sceneID].TrackBarMinValue;
                    CenterBoardTrackBar.Value = SceneCenterBoardData[sceneID].TrackBarValue;
                    CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100F).ToString("#") + "%";
                }

                SceneCenterBoardData[sceneID].PictureBoxImage = image;
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

        public static void CenterBoardBarOnScroll()
        {
            CenterBoardPictureBox.Width = (int)(CenterBoardPictureBox.Image.Width * (CenterBoardTrackBar.Value / 10000F));
            int imageHeight = (int)(CenterBoardPictureBox.Image.Height * (CenterBoardTrackBar.Value / 10000F));
            CenterBoardLabel.Text = (CenterBoardTrackBar.Value / 100).ToString() + "%";
            SceneCenterBoardData[OpenedSceneId].TrackBarValue = CenterBoardTrackBar.Value;
            CenterBoardToolStrip.Items[0].Text = string.Format("{0}*{1}", CenterBoardPictureBox.Width, imageHeight);

        }

        public static void ResetCenterBoard(bool visible)
        {
            CenterBoardTrackBar.Visible = visible;
            CenterBoardLabel.Visible = visible;
            CenterBoardToolStrip.Visible = visible;
        }


        public static void SetPictureBox(int key, PictureBox pb)
        {
            PictureBoxCache[key] = pb;
        }

        public static PictureBox GetPictureBox(int key)
        {
            if (PictureBoxCache.ContainsKey(key))
            {
                return PictureBoxCache[key];
            }
            else
            {
                return null;
            }
        }

        public static void ShowGroupOnCenterBoard(TableLayoutPanel tabPanel, Group group)
        {
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
                UpdateCenterBoard(group.Sceneid, null);
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
                UpdateCenterBoard(group.Sceneid, PngUtil.MergeImageList(resultList, maxGroupSize.Width, maxGroupSize.Height));
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
