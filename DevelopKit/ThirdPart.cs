using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;//提供高级的二维和矢量图像功能

namespace DevelopKit
{
    public class ThirdPartApiClient
    {
        PictureBox ImageContainer;
        private string NameSpace;
        private string Class;
        private string Method;

        private List<Object> Params;

        public ThirdPartApiClient(string CallInfo)
        {
            ImageContainer = new PictureBox
            {
                Size = new Size(0, 0),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 0),
                BackColor = Color.Gray,
                Visible = false,
            };

            string[] infos = CallInfo.Split('.');
            NameSpace = infos[0];
            Class = infos[1];
            Method = infos[2];

            Params = new List<Object>();
            Params.Add(ImageContainer);
        }

        public void AddParam(Object param)
        {
            Params.Add(param);
        }

        public Image Call()
        {
            Call(NameSpace, Class, Method, Params.ToArray());
            return ImageContainer.Image;
        }

        private void Call(string name_space, string class_name, string method_name, Object[] parameters)
        {
            Type type = Type.GetType(name_space + "." + class_name);
            Object obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(method_name);

            foreach (Object obj2 in parameters)
            {
                Console.WriteLine("--------------> " + obj2);
            }
            method.Invoke(obj, parameters);
        }
    }
}

namespace ThirdPart
{
    public class WM
    {
        public void DrawCircleHighSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            Draw_PM(pb, 0, 0, 399, 5, w, 270, 60, upColor, downColor);
        }

        public void DrawCircleMidSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            Draw_PM(pb, 0, 0, 399, 5, w, 180, 60, upColor, downColor);
        }

        public void DrawCircleLowSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            Draw_PM(pb, 0, 0, 399, 5, w, 90, 60, upColor, downColor);
        }

        public void DrawRectangleHightSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            int re_w = 399;
            int re_edge = 5;
            int angle = 270;
            int defalut_w = 60;
            Draw_PM_P(pb, 0, 0, re_w, re_edge, w, angle, defalut_w, downColor, upColor);
        }

        public void DrawRectangleMidSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            int re_w = 399;
            int re_edge = 5;
            int angle = 180;
            int defalut_w = 60;
            Draw_PM_P(pb, 0, 0, re_w, re_edge, w, angle, defalut_w, downColor, upColor);
        }

        public void DrawRectangleLowSpeed(PictureBox pb, Color upColor, Color downColor, int w)
        {
            int defalut_w = 60;
            Draw_PM_P(pb, 0, 0, 399, 5, w, 90, defalut_w, downColor, upColor);
        }

        //参数说明：默认x,默认y,默认矩形宽度，默认边距，可变宽度，高中低角度（270，150，60），最大填充色宽度，默认顶部颜色，默认底部颜色
        //-----默认底部颜色，默认顶部颜色 
        //高   Color.FromArgb(50, 232, 182), Color.FromArgb(138, 232, 87);
        //中   Color.FromArgb(255, 180, 2),  Color.FromArgb(255, 255, 36)
        //低   Color.FromArgb(255, 69, 69),   Color.FromArgb(255, 111, 111)
        private void Draw_PM(PictureBox pb, int x, int y, int w, int r_edge, int change_w, int change_angle, int defalut_w, Color FillColor_up, Color FillColor_down)
        {
            int r_in = w - change_w * 2;
            Color Color_bg = Color.FromArgb(45, 45, 45);
            if (change_w > defalut_w)
            {
                return;
            }

            if (change_w <= 0)
            {
                return;
            }
            //外部扇形

            // Graphics g = pb.CreateGraphics();
            Bitmap bpm = new Bitmap(w, w);
            Graphics g = Graphics.FromImage(bpm);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿                              

            Rectangle re_out = set_Rectangle(x + r_edge, y + r_edge, w - r_edge * 2);
            Rectangle re_in = set_Rectangle(x + change_w + r_edge, y + change_w + r_edge, w - change_w * 2 - r_edge * 2);
            Rectangle re_down = set_Rectangle(x + w / 2 - change_w / 2 + r_edge, y - r_edge + w - change_w, change_w);
            Rectangle re_up = set_upRectangle(re_out, change_w, change_angle);
            GraphicsPath myPath_bg = new GraphicsPath();
            Rectangle re_out_bg = set_Rectangle(x, y, w);
            Rectangle re_in_bg = set_Rectangle(x + change_w + r_edge * 2, y + change_w + r_edge * 2, w - change_w * 2 - r_edge * 4);
            myPath_bg.AddEllipse(re_out_bg);
            myPath_bg.AddEllipse(re_in_bg);
            g.DrawPath(new Pen(Color_bg, 1), myPath_bg);
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddEllipse(re_out);
            myPath.AddEllipse(re_in);
            g.DrawPath(new Pen(Color_bg, 1), myPath);
            SolidBrush brush_bg = new SolidBrush(Color_bg);
            g.FillPath(brush_bg, myPath);

            int LC_startAngle = set_litterCircleStartAngle(change_angle);
            GraphicsPath myPath_F = FillPath(re_out, re_in, re_up, re_down, change_angle, LC_startAngle, true);
            LinearGradientBrush brush = set_brush(re_out, FillColor_up, FillColor_down);
            g.FillPath(brush, myPath_F);

            pb.Image = (Image)bpm;
        }

        private void Draw_PM_P(PictureBox pb, int x, int y, int w, int r_edge, int change_w, int change_angle, int defalut_w, Color FillColor_down, Color FillColor_up)
        {

            int r_in = w - change_w * 2;
            int parma = 32;
            Color Color_bg = Color.FromArgb(45, 45, 45);
            if (change_w > defalut_w)
            {
                return;
            }

            if (change_w < 10)
            {
                return;
            }

            Bitmap bpm = new Bitmap(w, w);
            Graphics g = Graphics.FromImage(bpm);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿  
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;
            float tension = 0.4F;
            Point[] p_out = set_Points(x + parma, y + parma, w - parma * 2);
            Point[] p_in = set_Points(x + change_w + parma, y + change_w + parma, w - change_w * 2 - parma * 2);
            Point[] p_re_out = set_Points(x + r_edge + parma, y + r_edge + parma, w - r_edge * 2 - parma * 2);
            Point[] p_re_in = set_Points(x + change_w - r_edge + parma, y + change_w - r_edge + parma, w - change_w * 2 + r_edge * 2 - parma * 2);
            g.DrawClosedCurve(new Pen(Color_bg, 1), p_out, tension, FillMode.Winding);
            g.DrawClosedCurve(new Pen(Color_bg, 1), p_in, tension, FillMode.Winding);


            GraphicsPath myPath = new GraphicsPath();//建立GraphicsPath()类对象
            Rectangle re = new Rectangle(x, y, w, w);
            Region re_Region = new Region(re);

            if (change_angle == 270)
            {
                Rectangle re_out = new Rectangle(x + r_edge, y + r_edge, w - r_edge * 2, w - r_edge * 2);
                Region myRegion = new Region(re_out);
                Rectangle re_wai = new Rectangle(x + r_edge + w / 2, y + r_edge + w / 2, w / 2 - r_edge * 2 + 1, w / 2 - r_edge * 2);
                myPath.AddRectangle(re_wai);
                myRegion.Xor(myPath);//得到两个区域的交集
                myPath.AddClosedCurve(p_re_out, tension);
                myPath.AddClosedCurve(p_re_in, tension);
                myRegion.Intersect(myPath);//得到两个区域的交集
                re = re_out;
                re_Region = myRegion;

            }
            else if (change_angle == 180)
            {
                Rectangle re_out = new Rectangle(x + r_edge, y + r_edge, (w - r_edge * 2) / 2, w - r_edge * 2);
                Region myRegion = new Region(re_out);
                myPath.AddClosedCurve(p_re_out, tension);
                myPath.AddClosedCurve(p_re_in, tension);
                myRegion.Intersect(myPath);//得到两个区域的交集
                re = re_out;
                re_Region = myRegion;
            }
            else if (change_angle == 90)
            {
                Rectangle re_out = new Rectangle(x + r_edge, y + r_edge + w / 2, (w - r_edge * 2) / 2, w - r_edge * 2);
                Region myRegion = new Region(re_out);
                myPath.AddClosedCurve(p_re_out, tension);
                myPath.AddClosedCurve(p_re_in, tension);
                myRegion.Intersect(myPath);//得到两个区域的交集
                re = re_out;
                re_Region = myRegion;
            }
            GraphicsPath myPath_bg = new GraphicsPath();
            myPath_bg.AddClosedCurve(p_re_out, tension);
            myPath_bg.AddClosedCurve(p_re_in, tension);
            SolidBrush brush_bg = new SolidBrush(Color_bg);
            g.FillPath(brush_bg, myPath_bg);
            LinearGradientBrush brush = set_brush(re, FillColor_up, FillColor_down);
            g.FillRegion(brush, re_Region);//填充区域   
            g.DrawClosedCurve(new Pen(Color_bg, 1), p_re_out, tension, FillMode.Winding);
            g.DrawClosedCurve(new Pen(Color_bg, 1), p_re_in, tension, FillMode.Winding);

            pb.Image = (Image)bpm;
        }

        private Point[] set_Points(int x, int y, int w)
        {
            Point[] p_out = new Point[]
            {
                new Point(x,y),
                new Point(x+w,y),
                new Point(x+w,y+w),
                new Point(x,y+w)
            };
            return p_out;
        }

        public Region FillCircle(Rectangle re_out, Rectangle re_in, int x, int y, int r_out)
        {
            GraphicsPath myPath = new GraphicsPath();//建立GraphicsPath()类对象
            Region myRegion = new Region(re_out);//建立表示第1个矩形的区域
            Rectangle re_wai = new Rectangle(x + r_out / 2, y + r_out / 2, r_out / 2, r_out / 2);
            myPath.AddRectangle(re_wai);
            myRegion.Xor(myPath);//得到两个区域的交集
            myPath.AddEllipse(re_out);
            myPath.AddEllipse(re_in);
            myRegion.Intersect(myPath);//得到两个区域的交集
            return myRegion;
        }

        public GraphicsPath FillPath(Rectangle re_out, Rectangle re_in, Rectangle re_up, Rectangle re_down, int C_Angle, int LC_startAngle, bool isflase)
        {
            GraphicsPath myPath_Dout = new GraphicsPath();//建立GraphicsPath()类对象
            GraphicsPath myPath_Dup = new GraphicsPath();//建立GraphicsPath()类对象
            GraphicsPath myPath_Ddown = new GraphicsPath();//建立GraphicsPath()类对象
            GraphicsPath myPath_Din = new GraphicsPath();//建立GraphicsPath()类对象

            myPath_Dup.AddArc(re_up, LC_startAngle, 185);//追加一椭圆
            myPath_Dout.AddArc(re_out, -270, C_Angle);//追加一椭圆
            myPath_Ddown.AddArc(re_down, -90, 185);//追加一椭圆
            myPath_Din.AddArc(re_in, -270, C_Angle);//追加一椭圆
            myPath_Din.Reverse();
            myPath_Dout.AddPath(myPath_Dup, true);
            myPath_Dout.AddPath(myPath_Din, true);
            myPath_Dout.AddPath(myPath_Ddown, true);

            return myPath_Dout;
        }
        public int set_litterCircleStartAngle(int C_Angle)
        {
            int litterCircleStartAngle = 90 + C_Angle;
            return litterCircleStartAngle;
        }
        public Rectangle set_Rectangle(int x, int y, int w)
        {
            Rectangle Rectangle = new Rectangle(x, y, w, w);
            return Rectangle;
        }
        public Rectangle set_upRectangle(Rectangle re_out, int w, int C_Angle)
        {
            GraphicsPath myPath_Dout = new GraphicsPath();
            myPath_Dout.AddArc(re_out, 90, C_Angle);//追加一椭圆
            float fx = myPath_Dout.GetLastPoint().X;
            float fy = myPath_Dout.GetLastPoint().Y;
            int rx = Convert.ToInt32(fx + w / 2 * Math.Sin(Math.PI * (180 - C_Angle) / 180) - w / 2);
            int ry = Convert.ToInt32(fy - w / 2 * Math.Cos(Math.PI * C_Angle / 180) - w / 2);

            GraphicsPath myPath_Dup = new GraphicsPath();
            Rectangle upRectangle = new Rectangle(rx, ry, w, w);
            return upRectangle;
        }
        public LinearGradientBrush set_brush(Rectangle re_out, Color FillUpColor, Color FillDownColor)
        {
            LinearGradientBrush brush = new LinearGradientBrush(re_out, FillUpColor, FillDownColor, LinearGradientMode.Vertical);
            return brush;
        }
        public Region FillLitterCircle(Rectangle re)
        {
            GraphicsPath myPath = new GraphicsPath();//建立GraphicsPath()类对象
            Region myRegion = new Region(re);//建立表示第1个矩形的区域
            myPath.AddEllipse(re);
            myRegion.Intersect(myPath);//得到两个区域的交集
            return myRegion;
        }
    }
}
