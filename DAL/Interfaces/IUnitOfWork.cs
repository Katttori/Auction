using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Lot> Lots { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
