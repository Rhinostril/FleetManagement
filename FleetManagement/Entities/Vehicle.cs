using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

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

        public void SetVehicleId(int id)
        {
            if (id > 0)
            {
                VehicleId = id;
            }
            else
            {
                throw new VehicleException("Vehicle id must be greater than 0!");
            }
        }

        public void SetBrand(string brand)
        {
            if (!string.IsNullOrWhiteSpace(brand))
            {
                Brand = brand;
            }
            else
            {
                throw new VehicleException("Vehicle brand can't be empty!");
            }
        }

        public void SetModel(string model)
        {
            if (!string.IsNullOrWhiteSpace(model))
            {
                Model = model;
            }
            else
            {
                throw new VehicleException("Vehicle brand can't be empty!");
            }
        }

        public void SetChassisNumber(string number)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                if(number.Length == 3 + 6 + 8)
                {
                    ChassisNumber = number;
                }
                else
                {
                    throw new VehicleException("Vehicle chassis number must be 17 characters long!");
                }

            }
            else
            {
                throw new VehicleException("Vehicle chassis number can't be empty!");
            }
        }

        public void SetLicensePlate(string plate)
        {
            if (!string.IsNullOrWhiteSpace(plate))
            {
                LicensePlate = plate;
            }
            else
            {
                throw new VehicleException("Vehicle license plate can't be empty!");
            }
        }

        public void SetFuelType(string fuel)
        {
            if (!string.IsNullOrWhiteSpace(fuel))
            {
                FuelType = fuel;
            }
            else
            {
                throw new VehicleException("Vehicle license plate can't be empty!");
            }
        }





    }
}
