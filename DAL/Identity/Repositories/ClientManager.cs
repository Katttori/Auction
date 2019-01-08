using DAL.Entities;
using DAL.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity.Repositories
{
    class ClientManager : IClientManager
    {
        private readonly AuctionContext database;
        public ClientManager(AuctionContext db)
        {
            database = db;
        }

        public void Create(User user)
        {
            database.UsersProfiles.Add(user);
            database.SaveChanges();
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
