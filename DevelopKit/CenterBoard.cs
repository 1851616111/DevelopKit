using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class CenterBoard
    {
        public static void CraeteDrawingBoard(TableLayoutPanel panel, Property property)
        {
            FlowLayoutPanel parentPanel = (FlowLayoutPanel)panel.Parent;
            List<Group> groups = GlobalConfig.Project.CarConfig.ListGroupsByIDAndIndex(property.SceneId, property.GroupId, property.GroupLayerIdx);

            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();
            foreach (Group group in groups)
            {
                Control[] brotherTablePanel = parentPanel.Controls.Find(group.GetTablePanelId(), true);
                if (brotherTablePanel.Length == 0)
                {
                    Console.WriteLine("Get brother table panel nil, group id={0}", group.Id);
                }

                TableLayoutPanel tableLayout = (TableLayoutPanel)brotherTablePanel[0];

                List<Property> groupProperties = GlobalConfig.Project.CarConfig.GroupIdToPropertyMapping[group.Id];

                foreach (Property groupProperty in groupProperties)
                {
                    if (groupProperty.Type == PropertyType.Image)
                    {
                        Control[] pbCtl = tableLayout.Controls.Find(groupProperty.GetPictureBoxId(), true);
                        if (pbCtl.Length == 0)
                        {
                            Console.WriteLine("Get group property picturebox nil, property id={0}", groupProperty.Id);
                        }
                            mergeParams.Add(new PngUtil.MergeImageParams
                            {
                                Image = ((PictureBox)pbCtl[0]).Image,
                                X = groupProperty.GetLocation().X,
                                Y = groupProperty.GetLocation().Y,
                            });
                    }
                }
            }

            if (mergeParams.Count >= 2)
            {
                GlobalConfig.MainPictureBox.Image = PngUtil.MergeImages2(mergeParams[0].Image, mergeParams[1]); 
            }
        }
    }

    public class CenterImageTemplate
    {
        public Image DrawImage()
        {
            return null;
        }
    }
}
