using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.ManagerExceptions
{
    class AddressManagerException : Exception
    {
        public AddressManagerException()
        {
        }

        public AddressManagerException(string message) : base(message)
        {
        }

        public AddressManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
