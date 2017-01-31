namespace LeinCottage.Web.Exceptions
{
    using System;

    public class FileNotPhotoException: ApplicationException
    {
        public FileNotPhotoException()
            : base("The file is not an image!")
        {
        }
    }
}