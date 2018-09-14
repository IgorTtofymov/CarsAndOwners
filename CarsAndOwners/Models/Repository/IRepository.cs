using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndOwners.Models.Repository
{
    public interface IRepository<T,K> : IDisposable where T:class where K:class
    {
        IEnumerable<T> GetInstances();
        T GetInstance(int? id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
        IEnumerable<K> GetConnectedInstances();
    }
}
