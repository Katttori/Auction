using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuctionContext db;
        private GenericRepository<Lot> lotRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Category> categoryRepository;
        private UserRepository userRepository;

        public UnitOfWork(string connectionString)
        {
            db = new AuctionContext(connectionString);
        }

        public IGenericRepository<Lot> Lots
        {
            get
            {
                if (lotRepository == null)
                    lotRepository = new GenericRepository<Lot>(db);
                return lotRepository;
            }
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new GenericRepository<Product>(db);
                return productRepository;
            }
        }

        public IGenericRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new GenericRepository<Category>(db);
                return categoryRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
