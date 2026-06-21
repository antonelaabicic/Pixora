namespace Pixora.DAL.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public string ImagePath { get; set; } = null!;

        public long FileSizeBytes { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PhotoHashtag> PhotoHashtags { get; set; } = new List<PhotoHashtag>();
    }
}