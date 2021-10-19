using System;

namespace FleetManagement.Business.Managers
{
    public class VehicleManagerException : Exception
    {
        public VehicleManagerException()
        {
        }

        public VehicleManagerException(string message) : base(message)
        {
        }

        public VehicleManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}