namespace LeinCottage.Web.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;

    public static class ImageProcessor
    {
        public static void SavePhoto(Stream fileStream, string path)
        {
            using (var originalImage = Bitmap.FromStream(fileStream, true, true))
            {
                originalImage.Save(path);
            }
        }

        public static void SavePhotoThumbnail(Stream fileStream, string path)
        {
            using (var originalImage = Bitmap.FromStream(fileStream, true, true))
            {
                float width = 200;
                float height = 200;
                var brush = new SolidBrush(Color.White);

                float scale = Math.Min(width / originalImage.Width, height / originalImage.Height);
                var bmp = new Bitmap((int)width, (int)height);
                var graph = Graphics.FromImage(bmp);

                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;

                var scaleWidth = (int)(originalImage.Width * scale);
                var scaleHeight = (int)(originalImage.Height * scale);

                graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
                graph.DrawImage(
                    originalImage,
                    new Rectangle(
                        ((int)width - scaleWidth) / 2,
                        ((int)height - scaleHeight) / 2,
                        scaleWidth,
                        scaleHeight));

                bmp.Save(path);
            }
        }

        public static void PhysicallyDeletePhoto(string path)
        {
            File.Delete(path);
        }
    }
}