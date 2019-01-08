using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity.Interfaces
{
    public interface IClientManager :IDisposable
    {
        void Create(User user);
    }
}
