using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IHashtagRepository : IRepository<Hashtag>
    {
        Hashtag? GetByName(string name);
        IEnumerable<Hashtag> GetByNames(IEnumerable<string> names);
    }
}
