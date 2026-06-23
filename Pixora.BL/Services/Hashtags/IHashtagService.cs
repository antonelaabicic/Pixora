using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Hashtags
{
    public interface IHashtagService
    {
        IEnumerable<Hashtag> GetAll();
        Hashtag? GetById(int id);
        Hashtag GetOrCreate(string name);
        IEnumerable<Hashtag> GetOrCreateMany(IEnumerable<string> names);
        void Delete(int id);
    }
}
