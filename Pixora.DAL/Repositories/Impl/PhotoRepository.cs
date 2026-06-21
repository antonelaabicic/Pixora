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
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PixoraContext _context;
        public PhotoRepository(PixoraContext context)
        {
            _context = context;
        }

        public Photo Delete(int id)
        {
            var photo = GetById(id);

            if (photo == null)
            {
                throw new ArgumentException($"Photo with id {id} not found.");
            }

            _context.Photos.Remove(photo);

            return photo;
        }

        public IEnumerable<Photo> GetAll()
        {
            return _context.Photos
                .Include(p => p.Author)
                .Include(p => p.PhotoHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .OrderByDescending(p => p.UploadedAt)
                .ToList();
        }

        public IEnumerable<Photo> GetByAuthorId(string authorId)
        {
            return _context.Photos
                .Include(p => p.Author)
                .Include(p => p.PhotoHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.UploadedAt)
                .ToList();
        }

        public Photo? GetById(int id)
        {
            return _context.Photos
                .Include(p => p.Author)
                .Include(p => p.PhotoHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Photo> GetLatest(int count)
        {
            return _context.Photos
                .Include(p => p.Author)
                .Include(p => p.PhotoHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .OrderByDescending(p => p.UploadedAt)
                .Take(count)
                .ToList();
        }

        public void Insert(Photo entity)
        {
            _context.Photos.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Photo> Search(string? hashtag, long? minSizeBytes, long? maxSizeBytes, DateTime? uploadedFrom, DateTime? uploadedTo, string? authorId)
        {
            var query = _context.Photos
                .Include(p => p.Author)
                .Include(p => p.PhotoHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .AsQueryable();  

            if (!string.IsNullOrWhiteSpace(hashtag))
            {
                query = query.Where(p => p.PhotoHashtags.Any(ph => ph.Hashtag.Name.ToLower() == hashtag.ToLower()));
            }

            if (minSizeBytes.HasValue)
            {
                query = query.Where(p => p.FileSizeBytes >= minSizeBytes.Value);
            }

            if (maxSizeBytes.HasValue)
            {
                query = query.Where(p => p.FileSizeBytes <= maxSizeBytes.Value);
            }

            if (uploadedFrom.HasValue)
            {
                query = query.Where(p => p.UploadedAt >= uploadedFrom.Value);
            }

            if (uploadedTo.HasValue)
            {
                query = query.Where(p => p.UploadedAt <= uploadedTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(authorId))
            {
                query = query.Where(p => p.AuthorId == authorId);
            }

            return query.ToList();
        }

        public void Update(Photo entity)
        {
            _context.Photos.Update(entity);
        }
    }
}
