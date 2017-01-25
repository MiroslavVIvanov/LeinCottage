namespace LeinCottage.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public string OriginalName { get; set; }

        public string Name { get; set; }

        public string RelativePath { get; set; }

        public string ThumbnailName { get; set; }

        public string ThumbnailRelativePath { get; set; }

        public bool IsVisible { get; set; }
    }
}
