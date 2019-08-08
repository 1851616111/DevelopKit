using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DevelopKit
{
    public class LeftController
    {
        private Controller controller;
        public Controller Controller { get => controller; set => controller = value; }

        private TreeView TreeView;

        public LeftController(TreeView treeView)
        {
            TreeView = treeView;
        }

        public void Load(List<Scene> scenes, Form_Progress progressForm)
        {
            TreeView.BeginUpdate();
            foreach (Scene scene in scenes)
            {
                TreeNode sceneNode = new TreeNode
                {
                    Name = scene.Id.ToString(),
                    Text = scene.Name
                };

                controller.Right.LoadSceneResources(scene);
                progressForm.AddProgressValue(1, string.Format("场景 {0} 已加载", scene.Name));

                foreach (Scene childScene in scene.children)
                {
                    sceneNode.Nodes.Add(new TreeNode
                    {
                        Name = childScene.Id.ToString(),
                        Text = childScene.Name
                    });

                    controller.Right.LoadSceneResources(childScene);
                    progressForm.AddProgressValue(1, string.Format("场景 {0} 已加载", childScene.Name));
                }

                TreeView.Nodes.Add(sceneNode);
            }

            TreeView.EndUpdate();
        }
    }
}
