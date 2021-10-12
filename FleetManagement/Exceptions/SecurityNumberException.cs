using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Exceptions {
    public class SecurityNumberException : Exception {

        public SecurityNumberException(string message) : base(message) {

        }

    }
}
