using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T Read(string id);
        IEnumerable<T> ReadAll();
        void Update(string id, T updated);
        void Delete(string id);
    }
}
