using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Models
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<PhotoHashtag> PhotoHashtags { get; set; } = new List<PhotoHashtag>();
    }
}
