using System;

namespace FleetManagement.Business.Managers
{
    public class FuelCardManagerException : Exception
    {
        public FuelCardManagerException()
        {
        }

        public FuelCardManagerException(string message) : base(message)
        {
        }

        public FuelCardManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}