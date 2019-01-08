using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class RegistrationDetails
    {
        public RegistrationDetails(bool successful, string message, string property)
        {
            IsSuccessful = successful;
            Message = message;
            Property = property;
        }
        public bool IsSuccessful { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}
