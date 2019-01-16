using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWorkIdentity
    {
        private AuctionContext db;

        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        public IdentityUnitOfWork(string connectionString)
        {
            db = new AuctionContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}

