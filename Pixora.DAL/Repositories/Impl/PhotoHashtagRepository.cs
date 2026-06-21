using Microsoft.EntityFrameworkCore;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Impl
{
    public class PhotoHashtagRepository : IPhotoHashtagRepository
    {
        private readonly PixoraContext _context;
        public PhotoHashtagRepository(PixoraContext context)
        {
            _context = context;
        }

        public void Delete(PhotoHashtag entity)
        {
            _context.PhotoHashtags.Remove(entity);
        }

        public void DeleteByPhotoId(int photoId)
        {
            var items = _context.PhotoHashtags.Where(ph => ph.PhotoId == photoId);
            _context.PhotoHashtags.RemoveRange(items);
        }

        public IEnumerable<PhotoHashtag> GetByHashtagId(int hashtagId)
        {
            return _context.PhotoHashtags
                .Include(ph => ph.Photo)
                .Where(ph => ph.HashtagId == hashtagId)
                .ToList();
        }

        public IEnumerable<PhotoHashtag> GetByPhotoId(int photoId)
        {
            return _context.PhotoHashtags
                .Include(ph => ph.Hashtag)
                .Where(ph => ph.PhotoId == photoId)
                .ToList();
        }

        public void Insert(PhotoHashtag entity)
        {
            _context.PhotoHashtags.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
