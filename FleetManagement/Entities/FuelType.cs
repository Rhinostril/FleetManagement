using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities
{
    public class FuelType
    {
        public string FuelName { get; private set; }

        public FuelType(string fuelName)
        {
            SetFuelName(fuelName);
        }

        public void SetFuelName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                FuelName = name;
            }
            else
            {
                throw new FuelTypeException("FuelTypeName cannot be empty!");
            }
        }
 
    }
}
