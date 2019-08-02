using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DevelopKit
{
    public class CenterBoardController
    {
        private int OpenedSceneId;
        private int minPictureBoxWidth;
        private Panel RightPenel;
        private PictureBox CenterBoardPictureBox;
        private ToolStripLabel CenterBoardImageSizeLabel;
        private ToolStripLabel CenterBoardPictureBoxSizeLabel;

        private Dictionary<string, PictureBox> PropertyPictureBoxCache;
        private Dictionary<int, FlowLayoutPanel> SceneIdToSceneFlowPanelCache;
        private Dictionary<int, CenterBoardData> SceneToCenterBoardDataCache;
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

            PropertyPictureBoxCache = new Dictionary<string, PictureBox>();
            SceneIdToSceneFlowPanelCache = new Dictionary<int, FlowLayoutPanel>();
            SceneToCenterBoardDataCache = new Dictionary<int, CenterBoardData>();  
            GroupLayerCache = new Dictionary<int, SortedDictionary<int, GroupCache>>();
        }

        public void InitScene(int newSceneID, bool init)
        {
            if (!init && newSceneID == OpenedSceneId)
                return;

            //隐藏上一次打开的场景
            if (OpenedSceneId > 0 && SceneIdToSceneFlowPanelCache.ContainsKey(OpenedSceneId))
            {
                SceneIdToSceneFlowPanelCache[OpenedSceneId].Visible = false;
                SceneIdToSceneFlowPanelCache[OpenedSceneId].Enabled = false;
                CenterBoardPictureBox.Image = null;
                CenterBoardPictureBox.Refresh();
            }

            if (SceneIdToSceneFlowPanelCache.ContainsKey(newSceneID))
            {
                SceneIdToSceneFlowPanelCache[newSceneID].Visible = true;
                SceneIdToSceneFlowPanelCache[newSceneID].Enabled = true;
                ShowCenterBoard(newSceneID);
            }
            else
            {
                FlowLayoutPanel flowLayoutPanel = loadSceneFlowLayoutPanel(newSceneID, init);
                if (flowLayoutPanel != null)
                {
                    SceneIdToSceneFlowPanelCache.Add(newSceneID, flowLayoutPanel);
                }
            }

            OpenedSceneId = newSceneID;
        }

        public FlowLayoutPanel GetSceneFlowLayoutPanel(int sceneId)
        {
            return SceneIdToSceneFlowPanelCache[sceneId];
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

        public void HideCenterBoardPictureBox()
        {
            CenterBoardPictureBox.Visible = false;
        }

        public void ShowCenterBoardPictureBox()
        {
            CenterBoardPictureBox.Visible = true;
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

        public void UpdateCenterBoardPictureBoxImage(Image image)
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


        private FlowLayoutPanel loadSceneFlowLayoutPanel(int sceneId, bool init)
        {
            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(sceneId);
            if (scene == null)
                return null;

            if (!GlobalConfig.Project.CarConfig.SceneIdToGroupsMapping.ContainsKey(scene.Id))
                return null;

            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            RightPenel.Controls.Add(flowLayoutPanel);
            flowLayoutPanel.Width = RightPenel.Width;
            flowLayoutPanel.Visible = false;

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
                Form1_FlowPanel.LoadGroupTablePanelData(group, tableLayoutPanel, GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id], GlobalConfig.UiConfig.PropertyRowHeight, init);
            }

            return flowLayoutPanel;
        }

        public void ShowCenterBoard(int sceneID)
        {
            if (!SceneToCenterBoardDataCache.ContainsKey(sceneID))
                return;

            UpdateCenterBoardPictureBoxImage(SceneToCenterBoardDataCache[sceneID].PictureBoxImage);
        }

        private void UpdateCenterBoard(int sceneID, Image image)
        {
            if (image != null)
            {
                if (!SceneToCenterBoardDataCache.ContainsKey(sceneID))
                {
                    SceneToCenterBoardDataCache.Add(sceneID, new CenterBoardData
                    {
                        PictureBoxImage = CenterBoardPictureBox.Image,
                    });
                }
                else
                {
                    SceneToCenterBoardDataCache[sceneID].PictureBoxImage = CenterBoardPictureBox.Image;
                }
            }

            UpdateCenterBoardPictureBoxImage(image);
            OpenedSceneId = sceneID;
        }

        public void SetPictureBox(string key, PictureBox pb)
        {
            PropertyPictureBoxCache[key] = pb;
        }

        public PictureBox GetPictureBox(string key)
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
            List<PngUtil.MergeImageParams> ps = ListGroupImageParams(tabPanel, group);
            UnionGroupsImageParams(group, ps);
        }

        public void UnionGroupsImageParams(Group group, List<PngUtil.MergeImageParams> ps)
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

        private List<PngUtil.MergeImageParams> ListGroupImageParams(TableLayoutPanel tabPanel, Group group)
        {
            List<Property> properties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Property property in properties)
            {
                if (property.Type == PropertyType.Image || property.OptType == PropertyOperateType.AlphaWhiteImageSetAlpha 
                    || property.OptType == PropertyOperateType.AlphaWhiteImageSetColor || property.OptType == PropertyOperateType.ImageFilterColor || PropertyOperateType.IsThirdPartType(property.OptType))
                {
                    Control[] pbCtl = tabPanel.Controls.Find(property.GetPictureBoxId(), true);
                    if (pbCtl.Length == 0)
                    {
                        continue;
                        Console.WriteLine("Get group property picturebox nil, property id={0}", property.Id);
                    }
                    PictureBox pb = (PictureBox)(pbCtl[0]);
                    if (!(bool)(pb).Enabled || pb.Image == null)  //checkbox 负责启用还是停用PictureBox
                        continue;

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


        public string StartOutput(Outputs outputs, Dictionary<int, Property> propertyIdMapping, string basePath, Form_Progress progressForm)
        {
            progressForm.SetProgressMax(outputs.ImageOutputs.Length + outputs.XmlOutputs.Length + outputs.MergeImageOutputs.Length);

            foreach (ImageOutput imageOutout in outputs.ImageOutputs)
            {
                if (!propertyIdMapping.ContainsKey(imageOutout.PropertyId))
                {
                    return Errors.PropertyIdNotExist;
                }

                Property property = propertyIdMapping[imageOutout.PropertyId];

                Control[] controls = GetSceneFlowLayoutPanel(property.SceneId).Controls.Find(property.GetPictureBoxId(), true);
                if (controls == null || controls.Length == 0)
                {
                    return Errors.UiPictureBoxNotExist;
                }

                try
                {
                    string newFile = Path.Combine(basePath, imageOutout.Target);
                    string dirName = newFile.Substring(0, newFile.LastIndexOf('\\'));

                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);

                    ((PictureBox)(controls[0])).Image.Save(newFile);
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                progressForm.AddProgressValue(1, string.Format("组件：{0},   属性: {1} 导出完毕", property.GroupName, property.Name));
            }

            foreach (XmlOutput xmlOutout in outputs.XmlOutputs)
            {
                if (!propertyIdMapping.ContainsKey(xmlOutout.PropertyId))
                {
                    return Errors.PropertyIdNotExist;
                }
                Property property = propertyIdMapping[xmlOutout.PropertyId];


                Control[] controls = null;
                if (property.OptType == PropertyOperateType.AlphaWhiteImageSetAlpha)
                {
                    controls = GetSceneFlowLayoutPanel(property.SceneId).Controls.Find(property.GetTextBoxAlphaID(), true);
                }
                else if (property.OptType == PropertyOperateType.AlphaWhiteImageSetColor)
                {
                    controls = GetSceneFlowLayoutPanel(property.SceneId).Controls.Find(property.GetTextBoxColorID(), true);
                }

                if (controls == null || controls.Length == 0)
                {
                    return Errors.UiPictureBoxNotExist;
                }
                progressForm.AddProgressValue(1, string.Format("组件：{0}, 属性: {1} 导出完毕", property.GroupName, property.Name));
            }


            List<PngUtil.MergeImageParams> list = new List<PngUtil.MergeImageParams>();
            foreach (MergeImageOutput MergeImageOutPut in outputs.MergeImageOutputs)
            {
                Dictionary<int, Property> map = MergeImageOutPut.GetPropertyMap();
                foreach (int proertyId in map.Keys)
                {
                    if (!propertyIdMapping.ContainsKey(proertyId))
                    {
                        return Errors.PropertyIdNotExist;
                    }

                    Property propertItem = propertyIdMapping[proertyId];
                    if (map.ContainsKey(propertItem.RefPropertyId))   //如果出现引用形式， 则此图不需要参与合成
                        continue;

                    Control[] controls = GetSceneFlowLayoutPanel(propertItem.SceneId).Controls.Find(propertItem.GetPictureBoxId(), true);
                    if (controls == null || controls.Length == 0)
                    {
                        return Errors.UiPictureBoxNotExist;
                    }


                    list.Add(new PngUtil.MergeImageParams
                    {
                        Image = ((PictureBox)(controls[0])).Image,
                    });
                };

                Image image = null;
                if (list.Count == 1)
                {
                    image = list[0].Image;
                }
                else
                {
                    image = PngUtil.MergeImageList(list, list[0].Image.Width, list[0].Image.Height);
                }

                try
                {
                    string newFile = Path.Combine(basePath, MergeImageOutPut.Target);
                    string dirName = newFile.Substring(0, newFile.LastIndexOf('\\'));

                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);

                    image.Save(newFile);
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }

                list.Clear();
                progressForm.AddProgressValue(1, string.Format("图片合成导出完毕"));
            }

            return null;
        }
    }

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
}
