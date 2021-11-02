using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities
{
    public class FuelType
    {   public int FuelTypeId { get; private set; }
        public string FuelName { get; private set; }

        public FuelType(string fuelName)
        {
            try {
                SetFuelName(fuelName);
            }catch(Exception ex) {

            }

        }
        public FuelType(int fueltypeId, string fuelName) : this(fuelName) {
            try {
                SetFuelTypeID(fueltypeId);
            } catch (Exception ex) {

            }


        }

        public void SetFuelTypeID(int Id) {
            if (Id > 0) {
                this.FuelTypeId = Id;
            } else {

                throw new FuelTypeException("FuelType - SetFuelTypeID: Id cannot be less than 1!");
            }
        }
        public void SetFuelName(string name)
        {   
            if (!string.IsNullOrWhiteSpace(name))
            {
                FuelName = name;
            }
            else
            {
                throw new FuelTypeException("FuelType - SetFuelName: name cannot be empty!");
            }
        }
 
    }
}
