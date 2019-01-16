using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuctionContext database;

        public UserRepository(AuctionContext db)
        {
            database = db;
        }

        public IEnumerable<User> GetAll()
        {
            return database.UsersProfiles;
        }

        public User Get(string id)
        {
            return database.UsersProfiles.Find(id);
        }

        public void Create(User item)
        {
            database.UsersProfiles.Add(item);
            database.SaveChanges();
        }

        public void Update(User item)
        {
            database.Entry(item).State = EntityState.Modified;
            database.SaveChanges();
        }

        public void Delete(string id)
        {
            User user = database.UsersProfiles.Find(id);
            if (user != null)
                database.UsersProfiles.Remove(user);
            database.SaveChanges();
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return database.UsersProfiles.Where(predicate).ToList();
        }
    }
}
