using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Hashtags
{
    public class HashtagService : IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        public HashtagService(IHashtagRepository hashtagRepository)
        {
            _hashtagRepository = hashtagRepository;
        }
        public void Delete(int id)
        {
            _hashtagRepository.Delete(id);
            _hashtagRepository.Save();
        }

        public IEnumerable<Hashtag> GetAll()
        {
            return _hashtagRepository.GetAll();
        }

        public Hashtag? GetById(int id)
        {
            return _hashtagRepository.GetById(id);
        }

        public Hashtag GetOrCreate(string name)
        {
            var normalizedName = Normalize(name);
            var existingHashtag = _hashtagRepository.GetByName(normalizedName);

            if (existingHashtag != null)
            {
                return existingHashtag;
            }

            var hashtag = new Hashtag
            {
                Name = normalizedName
            };

            _hashtagRepository.Insert(hashtag);
            _hashtagRepository.Save();

            return hashtag;
        }
        public IEnumerable<Hashtag> GetOrCreateMany(IEnumerable<string> names)
        {
            var result = new List<Hashtag>();

            foreach (var name in names)
            {
                var normalizedName = Normalize(name);

                if (string.IsNullOrWhiteSpace(normalizedName)) { 
                    continue;
                }

                var hashtag = GetOrCreate(normalizedName);
                result.Add(hashtag);
            }

            return result;
        }
        private string Normalize(string name)
        {
            return name.Trim().TrimStart('#').ToLower();
        }
    }
}
