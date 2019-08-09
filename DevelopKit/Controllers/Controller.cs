using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public class Controller
    {
        public ShareCache ShareCache;
        public LeftController Left;
        public RightController Right;
        public CenterController Center;

        public Controller(LeftController leftCtl, CenterController centerCtl, RightController rightCtl)
        {
            ShareCache = new ShareCache();
            Left = leftCtl;
            Right = rightCtl;
            Center = centerCtl;
            Left.Controller = this;
            Right.Controller = this;
            Center.Controller = this;
        }

        public void Show(Scene scene)
        {
            Right.Show(scene);

            Center.Show(scene.SearchTopScene());
        }

        public void LoadProjectWithProgress(Form_Progress form_Progress)
        {
            Left.Load(GlobalConfig.Project.CarConfig.Scenes, form_Progress);
        }
    }
}
