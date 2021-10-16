using System;
using System.Runtime.Serialization;

namespace FleetManagement.Business.Exceptions
{

    public class FuelTypeException : Exception
    {
        public FuelTypeException()
        {
        }

        public FuelTypeException(string message) : base(message)
        {
        }

        public FuelTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FuelTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}