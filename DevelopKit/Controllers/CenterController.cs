using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public class CenterController
    {
        private PictureBox PictureBox;

        private Controller controller;
        public Controller Controller { get => controller; set => controller = value; }

        public CenterController(PictureBox pb)
        {
            PictureBox = pb;
        }

        public void Show(Scene scene)
        {
            scene = scene.SearchTopScene();

            if (scene.children.Count == 0)
            {
                PictureBox.Image = PngUtil.MergeImageList(ListSceneImageParams(scene), 1920, 720);
            }
            else
            {
                List<PngUtil.MergeImageParams> result = ListSceneImageParams(scene);
                foreach (Scene childScene in scene.children)
                {
                    List<PngUtil.MergeImageParams> childResult = ListSceneImageParams(childScene);
                    if (childResult == null)
                        continue;

                    if (result == null)
                    {
                        result = childResult;
                        continue;
                    }

                    result = result.Union(childResult).ToList();
                }

                PictureBox.Image = PngUtil.MergeImageList(result, 1920, 720);
            }
            PictureBox.Refresh();
        }

        public void Refresh()
        {
            if (Controller.ShareCache.OpenedSceneID == 0)
                return;

            Scene scene = GlobalConfig.Project.CarConfig.GetSceneById(Controller.ShareCache.OpenedSceneID);

            Show(scene);
        }


        private List<PngUtil.MergeImageParams> ListSceneImageParams(Scene scene)
        {
            SortedDictionary<int, Group> sceneGroups = controller.Right.GetOpenedSceneGroups(scene.Id);
            if (sceneGroups == null)
            {
                return null;
            }

            List<PngUtil.MergeImageParams> result = new List<PngUtil.MergeImageParams>();

            foreach (Group group in sceneGroups.Values)
            {
                List<PngUtil.MergeImageParams> item = ListGroupImageParams(group);
                if (item == null)
                    continue;

                result = result.Union(item).ToList();
            }

            return result;
        }

        private List<PngUtil.MergeImageParams> ListGroupImageParams(Group group)
        {
            SortedDictionary<int, Property> properties = group.GetPropertiesByLayer();
            List<PngUtil.MergeImageParams> mergeParams = new List<PngUtil.MergeImageParams>();

            foreach (Property property in properties.Values)
            {
                if (property.Type == PropertyType.Image || property.OperateType == PropertyOperateType.AlphaWhiteImageSetAlpha
                    || property.OperateType == PropertyOperateType.AlphaWhiteImageSetColor || property.OperateType == PropertyOperateType.ImageFilterColor)
                {

                    Image image = GlobalConfig.Controller.ShareCache.ShareImage.Get(property.Id);

                    mergeParams.Add(new PngUtil.MergeImageParams
                    {
                        Image = image,
                        X = property.GetLocation().X,
                        Y = property.GetLocation().Y,
                    });
                } else if (PropertyOperateType.IsThirdPartType(property.OperateType))
                {
                    Image image = GlobalConfig.Controller.ShareCache.ThirdPartCaller.Get(property.OperateType);
                    if (image != null)
                    {
                        mergeParams.Add(new PngUtil.MergeImageParams
                        {
                            Image = image,
                            X = property.GetLocation().X,
                            Y = property.GetLocation().Y,
                        });
                    }
                }
            }
            return mergeParams;
        }
    }
}
