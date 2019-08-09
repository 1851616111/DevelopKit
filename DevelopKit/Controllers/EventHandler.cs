using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public class GroupEvent
    {
        public Group Group;
        public GroupEvent(Group group)
        {
            Group = group;
        }

        public void ClickGroup()
        {
            GlobalConfig.Controller.Right.ClickGroup(Group);
        }
    }

    public class PropertyEvent
    {
        public Property Prop;
        public Object Message;

        public PropertyEvent(Property property, Object message)
        {
            Prop = property;
            Message = message;
        }

        public void Send()
        {
            GlobalConfig.EventHandler.HandleProperty(this);
        }
    }

    public class PropertyEventHandler
    {
        public void HandleProperty(PropertyEvent Event)
        {
            if (Event.Prop.OperateType == PropertyOperateType.ReplaceImage)
            {
                GlobalConfig.Controller.ShareCache.ShareImage.Set(Event.Prop.Id, (Image)(Event.Message));
            }
            else if (Event.Prop.OperateType == PropertyOperateType.AlphaWhiteImageSetColor)
            {
                Image image = GlobalConfig.Controller.ShareCache.ShareImage.Get(Event.Prop.Id);
                PngUtil.SetAlphaWhilteImage((Bitmap)image, (Color)(Event.Message));
            }
            else if (Event.Prop.OperateType == PropertyOperateType.AlphaWhiteImageSetAlpha)
            {
                int alphaValue = Convert.ToInt32(Event.Message);
                Image image = GlobalConfig.Controller.ShareCache.ShareImage.Get(Event.Prop.Id);
                TextBox textBox = GlobalConfig.Controller.ShareCache.ShareTextBox.Get(Event.Prop.Id);
                PngUtil.SetAlphaWhilteImage((Bitmap)image, alphaValue);
            }
            else if (Event.Prop.OperateType == PropertyOperateType.ImageFilterColor)
            {
                TrackBar trackBar = GlobalConfig.Controller.ShareCache.ShareTrackBar.Get(Event.Prop.Id);
                Image image = GlobalConfig.Controller.ShareCache.ShareImage.Get(Event.Prop.Id);
                Bitmap bmp = PngUtil.RelativeChangeColor((Bitmap)image, trackBar.Value);
                GlobalConfig.Controller.ShareCache.ShareImage.Set(Event.Prop.Id, (Image)(bmp));
            }
            else if (PropertyOperateType.IsThirdPartType(Event.Prop.OperateType))
            {
                GlobalConfig.Controller.ShareCache.ThirdPartCaller.Draw(Event.Prop.OperateType, Event.Prop.PropertyLayerIdx, Event.Message);
            }

            GlobalConfig.Controller.Center.Refresh();
        }
    }
}
