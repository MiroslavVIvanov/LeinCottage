namespace LeinCottage.Web.Common
{
    using System;

    public static class PhotoNameProvider
    {
        public static string GetName(string originalName)
        {
            string baseName = GetBaseName(originalName);
            string extension = GetExtension(originalName);

            return string.Format("{0}{1}", baseName, extension);
        }

        public static string GetThumbnailName(string originalName)
        {
            string baseName = GetBaseName(originalName);
            string thumb = "thumbnail";
            string extension = GetExtension(originalName);

            return string.Format("{0}-{1}{2}", baseName, thumb, extension);
        }

        private static string GetBaseName(string originalName)
        {
            var now = DateTime.Now;

            return string.Format(
                "{0}-{1}-{2}-{3}-{4}-{5}-photo",
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second);
        }

        private static string GetExtension(string originalName)
        {
            return originalName.Substring(originalName.LastIndexOf('.'));
        }
    }
}