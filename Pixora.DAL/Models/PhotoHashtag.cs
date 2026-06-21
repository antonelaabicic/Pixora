namespace Pixora.DAL.Models
{
    public class PhotoHashtag
    {
        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = null!;

        public int HashtagId { get; set; }
        public Hashtag Hashtag { get; set; } = null!;
    }
}