using System;
using System.Runtime.Serialization;

namespace FleetManagement.Business.Exceptions
{
    [Serializable]
    internal class LicenseTypeException : Exception
    {
        public LicenseTypeException()
        {
        }

        public LicenseTypeException(string message) : base(message)
        {
        }

        public LicenseTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LicenseTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}