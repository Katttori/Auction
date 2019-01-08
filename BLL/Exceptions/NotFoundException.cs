using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class NotFoundException : ArgumentNullException
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException() : base() { }
    }
}
