using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DevelopKit
{
    public static class FormEvent
    {
        public static void ClickGroup(Group group)
        {
            GroupEvent groupEvent = new GroupEvent(group);
            groupEvent.ClickGroup();
        }

        public static void ReplaceImage(Property property)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Png|*.png|Jpg|*.jpg"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image image = Image.FromFile(openFileDialog.FileName);
                    image.Save(Path.Combine(GlobalConfig.Project.GetUserUploadImageDir(), openFileDialog.SafeFileName));

                    PropertyEvent Event = new PropertyEvent(property, image);
                    Event.Send();

                    if (openFileDialog.FileName != property.Value)
                    {
                        Property propertyCopy = property.Clone();
                        propertyCopy.Value = GlobalConfig.Project.GetPropertyImagePath(openFileDialog.SafeFileName);
                        GlobalConfig.Project.Editer.Add(property.Id, propertyCopy);
                    }
                    else
                    {
                        GlobalConfig.Project.Editer.Remove(property.Id);
                    }
                }
            }
            catch (Exception)
            { }
        }

        public static void SetColor(Property property, TextBox textBox)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Color.ToArgb() != textBox.BackColor.ToArgb())
                {
                    Property propertyCopy = property.Clone();
                    propertyCopy.DefaultValue = "0x" + dialog.Color.R.ToString("X2") + dialog.Color.G.ToString("X2") + dialog.Color.B.ToString("X2");
                    GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);
                }
                else
                {
                    GlobalConfig.Project.Editer.Remove(property.Id);
                }

                PropertyEvent Event = new PropertyEvent(property, dialog.Color);
                Event.Send();
                textBox.BackColor = dialog.Color;
            }
            dialog.Dispose();
        }

        public static void SetInt(Property property, TextBox textBox)
        {
            int i = Convert.ToInt32(textBox.Text);
            PropertyEvent Event = new PropertyEvent(property, i);
            Event.Send();
        }

        public static void SetAlphaImageColor(Property property, ref string oldText, string text)
        {
            if (oldText != text)
            {
                Property propertyCopy = property.Clone();
                propertyCopy.DefaultValue = text;
                GlobalConfig.Project.Editer.Set(property.Id, propertyCopy);
            }
            else
            {
                GlobalConfig.Project.Editer.Remove(property.Id);
            }
            try
            {
                PropertyEvent Event = new PropertyEvent(property, text);
                Event.Send();
            }
            catch (Exception)
            { }
        }

        public static void FilterImageColor(Property property)
        {
            PropertyEvent Event = new PropertyEvent(property, null);
            Event.Send();
        }

        public static void ThirdPartSetColor(Property property)
        {
            PropertyEvent Event = new PropertyEvent(property, null);
            Event.Send();
        }
    }
}
