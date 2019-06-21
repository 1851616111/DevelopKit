using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class PngUtil
    {
        public static void writeImage(Image image, Rectangle textAero, string text, Font font, Color color )
        {
                Graphics g = Graphics.FromImage(image);
                SolidBrush drawBrush = new SolidBrush(color);
                g.DrawString(text, font, drawBrush, textAero);
        }

        public struct favoriteImage
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

        public struct favoriteImageObj
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
        public static string MergeImages(string savePath, string body_path, favoriteImage[] favorite)
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
        public static Image MergeImages2(Image imgPhoto, params favoriteImageObj[] favorite)
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
    }
}
