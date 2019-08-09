using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public class RightController
    {
        private Controller controller;
        public Controller Controller { get => controller; set => controller = value; }


        private Panel RightPanel;

        private Dictionary<int, FlowLayoutPanel> ScenePanelMapping;
        private Dictionary<int, TableLayoutPanel> GroupPanelMapping;

        public Dictionary<int, SortedDictionary<int, Group>> OpenedGroupMapping;


        public RightController(Panel rightPanel)
        {
            RightPanel = rightPanel;

            ScenePanelMapping = new Dictionary<int, FlowLayoutPanel>();
            GroupPanelMapping = new Dictionary<int, TableLayoutPanel>();

            OpenedGroupMapping = new Dictionary<int, SortedDictionary<int, Group>>();
        }

        public void Show(Scene scene)
        {
            if (scene.children.Count == 0)
                ShowSceneFlowPanel(scene.Id);
            else if (controller.ShareCache.OpenedSceneID > 0 )
            {
                ScenePanelMapping[controller.ShareCache.OpenedSceneID].Visible = false;
                controller.ShareCache.OpenedSceneID = 0;
            }
                    
            //隐藏
        }

        public void ShowSceneFlowPanel(int sid)
        {
            if (sid == controller.ShareCache.OpenedSceneID)
            {
                return;
            }

            if (ScenePanelMapping.ContainsKey(sid))
            {
                if (controller.ShareCache.OpenedSceneID == 0)
                {
                    ScenePanelMapping[sid].Visible = true;
                    controller.ShareCache.OpenedSceneID = sid;
                }
                else
                {
                    ScenePanelMapping[sid].Visible = true;
                    ScenePanelMapping[controller.ShareCache.OpenedSceneID].Visible = false;
                    controller.ShareCache.OpenedSceneID = sid;
                }
            }
        }

        public void ClickGroup(Group group)
        {
            TableLayoutPanel groupPanel = GetGroupTablePanel(group.Id); ;
            if (FormUtil.IsGroupHide(groupPanel))
            {
                HideBrotherGroups(group);

                GlobalConfig.Controller.Right.RegisterGroupOnClick(group);
                FormUtil.ShowGroup(groupPanel);
                //若有同层级的Group则需要隐藏
            }
            else
                HideGroup(group, groupPanel);
            Controller.Center.Show(group.SearchTopScene());
        }


        public void HideBrotherGroups(Group group)
        {
            List<Group> brotherGroups = GlobalConfig.Project.CarConfig.ListGroupByLayerId(group.Sceneid, group.LayerIndex);
            foreach (Group brotherGroup in brotherGroups)
            {
                if (group.Id != brotherGroup.Id)
                {
                    TableLayoutPanel brotherGroupPanel = GlobalConfig.Controller.Right.GetGroupTablePanel(brotherGroup.Id);
                    HideGroup(brotherGroup, brotherGroupPanel);
                }
            }
        }

        private void HideGroup(Group group, TableLayoutPanel groupPanel)
        {
            GlobalConfig.Controller.Right.DegisterGroup(group);
            FormUtil.HideGroupPanel(groupPanel);
        }

        public void LoadSceneResources(Scene scene)
        {
            FlowLayoutPanel flowLayoutPanel = FlowLayoutPanelUtil.CreateFlowLayoutPanel(scene, RightPanel);
            if (flowLayoutPanel == null)
            {
                return;
            }
        }

        public void RegisterScenePanel(int sid, FlowLayoutPanel panel)
        {
            ScenePanelMapping[sid] = panel;
        }

        //若同层已注册的话，返回false 否则返回true
        public bool RegisterGroupOnload(Group group, TableLayoutPanel panel)
        {
            GroupPanelMapping[group.Id] = panel;

            if (!OpenedGroupMapping.ContainsKey(group.Sceneid))
            {
                OpenedGroupMapping[group.Sceneid] = new SortedDictionary<int, Group>();
            }
            if (!OpenedGroupMapping[group.Sceneid].ContainsKey(group.LayerIndex))
            {
                OpenedGroupMapping[group.Sceneid][group.LayerIndex] = group;
                return true;
            }
            else
            {
                return false;
            }
        }

        public TableLayoutPanel GetGroupTablePanel(int gid)
        {
            return GroupPanelMapping[gid];
        }

        public void RegisterGroupOnClick(Group group)
        {
            if (OpenedGroupMapping[group.Sceneid].ContainsKey(group.LayerIndex))
                OpenedGroupMapping[group.Sceneid][group.LayerIndex] = group;
            else
            {
                OpenedGroupMapping[group.Sceneid].Add(group.LayerIndex, group);
            }
        }

        public void DegisterGroup(Group group)
        {
            OpenedGroupMapping[group.Sceneid].Remove(group.LayerIndex);
        }

        public SortedDictionary<int, Group> GetOpenedSceneGroups(int sceneID)
        {
            return OpenedGroupMapping.ContainsKey(sceneID) ? OpenedGroupMapping[sceneID] : null;
        }
    }
}
