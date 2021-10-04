using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Exceptions
{
    public class FuelCardException : Exception
    {
        public FuelCardException(string message) : base(message)
        {

        }

    }
}
