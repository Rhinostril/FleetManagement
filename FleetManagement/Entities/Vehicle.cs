using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Entities
{
    public class Vehicle
    {

        public int VehicleId { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string ChassisNumber { get; private set; }
        public string LicensePlate { get; private set; }
        public string FuelType { get; private set; }
        public string VehicleType { get; private set; }
        public string Color { get; private set; }
        public int Doors { get; private set; }
        public Driver Driver { get; private set; }



    }
}
