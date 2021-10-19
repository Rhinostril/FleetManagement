using System;

namespace FleetManagement.Business.Managers
{
    public class ManagerException : Exception
    {
        public ManagerException()
        {
        }

        public ManagerException(string message) : base(message)
        {
        }

        public ManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}