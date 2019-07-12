using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class Form1_Car_Config
    {
        public static void LoadScene(TreeView treeview, CarConfig carConfig)
        {

            treeview.BeginUpdate();
            foreach (Scene scene in carConfig.Scenes)
            {
                TreeNode sceneNode = new TreeNode
                {
                    Name = scene.Id.ToString(),
                    Text = scene.Name
                
                };

                treeview.Nodes.Add(sceneNode);
            }
            treeview.EndUpdate();
        }

    }
}
