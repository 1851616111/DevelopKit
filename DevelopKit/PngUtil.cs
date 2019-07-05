using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DevelopKit
{
    public static class PngUtil
    {
        public static void WriteImage(Image image, Rectangle textAero, string text, Font font, Color color)
        {
            Graphics g = Graphics.FromImage(image);
            SolidBrush drawBrush = new SolidBrush(color);
            g.DrawString(text, font, drawBrush, textAero);
        }

        public struct FavoriteImage
        {
            private string _imagePath;
            private int _x;
            private int _y;

            public int x
            {
                get
                {
                    return _x;
                }
                set
                {
                    _x = value;
                }
            }

            public int y
            {
                get
                {
                    return _y;
                }
                set
                {
                    _y = value;
                }
            }

            public string imagePath
            {
                get
                {
                    return _imagePath;
                }
                set
                {
                    _imagePath = value;
                }
            }
        }

        public struct FavoriteImageObj
        {
            private Image _imageObj;
            private int _x;
            private int _y;

            public int x
            {
                get
                {
                    return _x;
                }
                set
                {
                    _x = value;
                }
            }

            public int y
            {
                get
                {
                    return _y;
                }
                set
                {
                    _y = value;
                }
            }

            public Image image
            {
                get
                {
                    return _imageObj;
                }
                set
                {
                    _imageObj = value;
                }
            }
        }


        /// <summary>
        /// 生成水印
        /// </summary>
        /// <param name="Main">主图片路径,eg:body</param>
        /// <param name="Child">要叠加的图片路径</param>
        /// <param name="x">要叠加的图片位置的X坐标</param>
        /// <param name="y">要叠加的图片位置的Y坐标</param>
        /// <param name="isSave"></param>
        /// <returns>生成图片的路径</returns>        
        public static string MergeImages(string savePath, string body_path, FavoriteImage[] favorite)
        {
            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(body_path);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //设置此 Bitmap 的分辨率。 
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;//清除锯齿的呈现
            //haix
            for (int i = 0; i < favorite.Length; i++)
            {
                //Draws the photo Image object at original size to the graphics object.
                grPhoto.DrawImage(
                    imgPhoto,                               // Photo Image object
                    new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                    0,                                      // x-coordinate of the portion of the source image to draw. 
                    0,                                      // y-coordinate of the portion of the source image to draw. 
                    phWidth,                                // Width of the portion of the source image to draw. 
                    phHeight,                               // Height of the portion of the source image to draw. 
                    GraphicsUnit.Pixel);                    // Units of measure 


                //------------------------------------------------------------
                //Step #2 - Insert Property image,For example:hair,skirt,shoes etc.
                //------------------------------------------------------------
                //create a image object containing the watermark
                Image imgWatermark = new Bitmap(favorite[i].imagePath);
                int wmWidth = imgWatermark.Width;
                int wmHeight = imgWatermark.Height;


                //Create a Bitmap based on the previously modified photograph Bitmap
                Bitmap bmWatermark = new Bitmap(bmPhoto);
                bmWatermark.MakeTransparent(); //使默认的透明颜色对此 Bitmap 透明。

                //bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
                //Load this Bitmap into a new Graphic Object
                Graphics grWatermark = Graphics.FromImage(bmWatermark);


                int xPosOfWm = favorite[i].x;
                int yPosOfWm = favorite[i].y;

                //叠加
                grWatermark.DrawImage(imgWatermark, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                null);   //ImageAttributes Object


                //Replace the original photgraphs bitmap with the new Bitmap
                imgPhoto = bmWatermark;

                //grWatermark.Dispose();
                //imgWatermark.Dispose();
                //grPhoto.Dispose();                
                //bmWatermark.Dispose();
            }
            //haix

            string nowTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            nowTime += DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

            string saveImagePath = savePath + "\\FA" + nowTime + ".png";

            //save new image to file system.
            imgPhoto.Save(saveImagePath, ImageFormat.Png);
            imgPhoto.Dispose();


            return saveImagePath;
        }


        /// <summary>
        /// 生成水印
        /// </summary>

        /// <param name="Child">要叠加的图片路径</param>
        /// <param name="x">要叠加的图片位置的X坐标</param>
        /// <param name="y">要叠加的图片位置的Y坐标</param>
        /// <param name="isSave"></param>
        /// <returns>生成图片的路径</returns>        
        public static Image MergeImages2(Image imgPhoto, params FavoriteImageObj[] favorite)
        {
            //create a image object containing the photograph to watermark
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //设置此 Bitmap 的分辨率。 
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;//清除锯齿的呈现
            //haix
            for (int i = 0; i < favorite.Length; i++)
            {
                //Draws the photo Image object at original size to the graphics object.
                grPhoto.DrawImage(
                    imgPhoto,                               // Photo Image object
                    new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                    0,                                      // x-coordinate of the portion of the source image to draw. 
                    0,                                      // y-coordinate of the portion of the source image to draw. 
                    phWidth,                                // Width of the portion of the source image to draw. 
                    phHeight,                               // Height of the portion of the source image to draw. 
                    GraphicsUnit.Pixel);                    // Units of measure 


                //------------------------------------------------------------
                //Step #2 - Insert Property image,For example:hair,skirt,shoes etc.
                //------------------------------------------------------------
                //create a image object containing the watermark
                Image imgWatermark = new Bitmap(favorite[i].image);
                int wmWidth = imgWatermark.Width;
                int wmHeight = imgWatermark.Height;


                //Create a Bitmap based on the previously modified photograph Bitmap
                Bitmap bmWatermark = new Bitmap(bmPhoto);
                bmWatermark.MakeTransparent(); //使默认的透明颜色对此 Bitmap 透明。

                //bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
                //Load this Bitmap into a new Graphic Object
                Graphics grWatermark = Graphics.FromImage(bmWatermark);


                int xPosOfWm = favorite[i].x;
                int yPosOfWm = favorite[i].y;

                //叠加
                grWatermark.DrawImage(imgWatermark, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                null);   //ImageAttributes Object


                //Replace the original photgraphs bitmap with the new Bitmap
                imgPhoto = bmWatermark;

                //grWatermark.Dispose();
                //imgWatermark.Dispose();
                //grPhoto.Dispose();                
                //bmWatermark.Dispose();
            }

            return imgPhoto;
        }

        public static Bitmap FilPic(Bitmap mybm, Color targetColor)
        {
            int width = ((Image)mybm).Width;
            int height = ((Image)mybm).Height;
            Bitmap bm = new Bitmap(width, height);//初始化一个记录滤色效果的图片对象
            int x, y;


            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {

                    Color pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值

                    if (pixel.Equals(Color.FromArgb(0, 0, 0, 0)))
                    {
                        continue;
                    }

                    if (!pixel.Equals(Color.FromArgb(0, 255, 255, 255)))
                    {

                        Color pixelTargetColor = Color.FromArgb(pixel.A, targetColor.R, targetColor.G, targetColor.B); //采用原图的阿尔法通道+用户选中的色值
                        bm.SetPixel(x, y, pixelTargetColor);//绘图
                    }
                }
            }
            return bm;
        }

        public static Bitmap ChangeWhiteColor(Bitmap mybm, Color targetColor)
        {
            Bitmap bm = (Bitmap)mybm.Clone();//初始化一个记录滤色效果的图片对象
            int x, y;


            for (x = 0; x < bm.Width; x++)
            {
                for (y = 0; y < bm.Height; y++)
                {

                    Color pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值

                    if (pixel.Equals(Color.FromArgb(0, 0, 0, 0)))
                    {
                        continue;
                    }

                    if (pixel.A == 255 && pixel.R != 0 && pixel.R == pixel.G && pixel.G == pixel.B)
                    {
                        if (pixel.R == 255)
                        {
                            bm.SetPixel(x, y, Color.FromArgb(255, targetColor.R, targetColor.G, targetColor.B));
                        }
                        else
                        {
                            double downRate = pixel.R / 255.0d;
                            bm.SetPixel(x, y, Color.FromArgb(255, (int)(downRate * targetColor.R), (int)(downRate * targetColor.G), (int)(downRate * targetColor.B)));
                        }
                    }
                }
            }
            return bm;
        }


        //相位值 从-180到+180共360个小阶段组成一个环状
        //每次只调整一个通道， 调整幅度
        //其对应RGB的调整关系如下
        //1. G降低  ---> 2. R升高  ---> 3. B降低  ---> 4. G升高 ---> 5. R降低 ---> 6. B升高
        // 第一阶段 G原始值：148      R原始值：0
        // 第一极端 结束时:  G的新值调整至R的原始值(0)   R开始升高

        //0 	    0，     148,     157
        //+56 	    0，     1，      157
        //+57 	    1，     0，      157             ---1    G降低(G=R）  ---> R升高
        //+116      155，   0，      157		     ---2    R升高(R=B)   ---> B降低 		
        //+117      157，   0，      155
        //+176	    157，   0，      1			     ---3    B降低(B=G)  ---> G升高			
        //+177	    157，   1，      0
        //-124	    157,    155,     0 		         ---4    G升高(G=R) ---> R降低	
        //-123	    155,    157,     0 
        //-64       1,      157,     0		         ---5    R降低(R=B) ---> B升高	
        //-63       0,      157,     1			    	
        //-2	    0,      152,     157		     ---6    B升高(B=G) ---> G降低		
        //-1	    0,      150,     157		      	

        // stage1 = G-R, 
        // stage2 = 

        public static Bitmap RelativeChangeColor(Bitmap mybm, int target)
        {

            int width = ((Image)mybm).Width;
            int height = ((Image)mybm).Height;
            Bitmap bm = new Bitmap(width, height);//初始化一个记录滤色效果的图片对象
            int x, y;


            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {

                    Color pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值

                    //alpha 通道不为0， 其他通道全为0时，图像为类似于背景的灰色图像，此时我们不做任何改变
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        bm.SetPixel(x, y, Color.FromArgb(pixel.A, 0, 0, 0));
                        continue;
                    }

                    if (!pixel.Equals(Color.FromArgb(0, 255, 255, 255)))
                    {
                        int maxSpan = MathUtil.MaxSpan(pixel.R, pixel.G, pixel.B);
                        double unit = maxSpan / 60.0d;

                        int max = MathUtil.Max(pixel.R, pixel.G, pixel.B);
                        int min = MathUtil.Min(pixel.R, pixel.G, pixel.B);

                        Color newColor = ChangePixelByStage(1, unit, pixel.A, pixel.R, pixel.G, pixel.B, max, min, target);

                        bm.SetPixel(x, y, newColor);//绘图
                    }
                }
            }

            return bm;
        }

        //B=Max     R=Min     G下降
        //B=Max     G=Min     R升高  
        //R=Max     G=Min     B下降
        //R=Max     B=Min     G上升
        //G=Max 	B=Min     R下降
        //G=Max     R=Min     B上升
        private static Color ChangePixelByStage(int stage, double Unit, int a, int r, int g, int b, int max, int min, int change)
        {
            if (change <= 0)
            {
                return Color.FromArgb(a, r, g, b);
            }

            if (stage == 1)
            {
                int canChange = (g - min);
                if (change * Unit < canChange)
                {
                    g -= (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    if (change > 240)
                    {
                        return ChangePixelByStage(4, Unit, a, g, b, r, max, min, change - 240);
                    }
                    else if (change > 120)
                    {
                        return ChangePixelByStage(2, Unit, a, b, r, g, max, min, change - 120);
                    }

                    change -= (int)Math.Round((g - min) / Unit);
                    g = min;
                    return ChangePixelByStage(2, Unit, a, r, g, b, max, min, change);
                }
            }
            else if (stage == 2)
            {


                int canChange = (max - r);
                if (change * Unit < canChange)
                {
                    r += (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    change -= (int)Math.Round((max - r) / Unit);
                    r = max;
                    return ChangePixelByStage(3, Unit, a, r, g, b, max, min, change);
                }
            }
            else if (stage == 3)
            {
                int canChange = (b - min);
                if (change * Unit < canChange)
                {
                    b -= (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    change -= (int)Math.Round((b - min) / Unit);
                    b = min;
                    return ChangePixelByStage(4, Unit, a, r, g, b, max, min, change);
                }
            }
            else if (stage == 4)
            {
                int canChange = (max - g);
                if (change * Unit < canChange)
                {
                    g += (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    change -= (int)Math.Round((max - g) / Unit);
                    g = max;
                    return ChangePixelByStage(5, Unit, a, r, g, b, max, min, change);
                }
            }
            else if (stage == 5)
            {
                int canChange = (r - min);
                if (change * Unit < canChange)
                {
                    r -= (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    change -= (int)Math.Round((r - min) / Unit);
                    r = min;
                    return ChangePixelByStage(6, Unit, a, r, g, b, max, min, change);
                }
            }
            else if (stage == 6)
            {
                int canChange = (max - b);
                if (change * Unit < canChange)
                {
                    b += (int)Math.Round(change * Unit);
                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    change -= (int)Math.Round((max - b) / Unit);
                    b = max;
                    return ChangePixelByStage(1, Unit, a, r, g, b, max, min, change);
                }
            }
            else
            {
                return Color.Transparent;
            }
        }
    }
}
