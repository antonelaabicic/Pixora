using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Impl
{
    public class HashtagRepository : IHashtagRepository
    {
        private readonly PixoraContext _context;
        public HashtagRepository(PixoraContext context)
        {
            _context = context;
        }

        public Hashtag Delete(int id)
        {
            var hashtag = GetById(id) ?? throw new ArgumentException($"Hashtag with id {id} not found.");
            _context.Hashtags.Remove(hashtag);

            return hashtag;
        }

        public IEnumerable<Hashtag> GetAll()
        {
            return _context.Hashtags.OrderBy(h => h.Name).ToList();
        }

        public Hashtag? GetById(int id)
        {
            return _context.Hashtags.Find(id);
        }

        public Hashtag? GetByName(string name)
        {
            return _context.Hashtags.FirstOrDefault(h => h.Name.ToLower() == name.ToLower());
        }

        public IEnumerable<Hashtag> GetByNames(IEnumerable<string> names)
        {
            var normalized = names.Select(n => n.ToLower()).ToList();

            return _context.Hashtags
                .Where(h => normalized.Contains(h.Name.ToLower()))
                .ToList();
        }

        public void Insert(Hashtag entity)
        {
            _context.Hashtags.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Hashtag entity)
        {
            _context.Hashtags.Update(entity);
        }
    }
}
