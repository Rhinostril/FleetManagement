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
        public List<FuelType> FuelTypes { get; private set; } = new List<FuelType>();
        public string VehicleType { get; private set; }
        public string Color { get; private set; }
        public int Doors { get; private set; }
        public Driver Driver { get; private set; }

        public Vehicle(string brand, string model, string chassisNumber, string licensePlate, List<FuelType> fuelTypes, string vehicleType, string color, int doors) {
            try {
                SetBrand(brand);
                SetModel(model);
                SetChassisNumber(chassisNumber);
                SetLicensePlate(licensePlate);
                SetFuelTypes(fuelTypes);
                SetVehicleType(vehicleType);
                SetVehicleColor(color);
                SetVehicleDoors(doors);
            }catch(Exception ex) { 
            
            }
            
        }

        public Vehicle(int vehicleId, string brand, string model, string chassisNumber, string licensePlate, List<FuelType> fuelTypes, string vehicleType, string color, int doors) : this(brand, model, chassisNumber, licensePlate, fuelTypes, vehicleType, color, doors) {
            try {
                SetVehicleId(vehicleId);
            } catch (Exception ex) {

            }
        }

        public Vehicle(int vehicleId, string brand, string model, string chassisNumber, string licensePlate, List<FuelType> fuelTypes, string vehicleType, string color, int doors, Driver driver) : this(vehicleId, brand, model, chassisNumber, licensePlate, fuelTypes, vehicleType, color, doors) {
            try {
                SetDriver(driver);
            } catch (Exception ex) {

            }
        }

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

        public void SetFuelTypes(List<FuelType> fuelTypes)
        {
            if(fuelTypes == null) {
                throw new VehicleException("fueltype list can't be missing!");
            }              
            else if (!fuelTypes.Any()) {
                throw new VehicleException("fueltype list can't be empty!");
            } else {
                this.FuelTypes.Clear();
                foreach (FuelType type in fuelTypes) {
                    if (!String.IsNullOrEmpty(type.FuelName)) {
                        this.FuelTypes.Add(type);
                    } else {
                        throw new VehicleException("fueltype fuelname can't be empty!");
                    }
                }
            }
        }
        public void SetVehicleType(string vehicleType)
        {
            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                VehicleType = vehicleType;
            }
            else
            {
                throw new VehicleException("Vehicle type can't be empty!");
            }
        }
        public void SetVehicleColor(string color)
        {
            if (!string.IsNullOrWhiteSpace(color))
            {
                Color = color;
            }
            else
            {
                throw new VehicleException("Vehicle color can't be empty!");
            }
        }

        public void SetVehicleDoors(int doors)
        {
            if(doors > 0 && doors <= 7)
            {
                Doors = doors;
            }
            else
            {
                throw new VehicleException("Vehicle has to have more than 0 and less or equal than 7 doors!");
            }
        }

        public void SetDriver(Driver newDriver) //feedback leerkracht
        {
            if (newDriver != null)
            {
                if(this.Driver == null) {
                    if(!newDriver.HasVehicle(this)) //heeft de nieuwe driver dit al als vehicle ?
                   {
                        newDriver.RemoveVehicle();
                        this.Driver = newDriver;
                        newDriver.SetVehicle(this);
                    }
                    this.Driver = newDriver;

                } 
                else if (this.Driver != newDriver) //is huidige driver niet gelijk aan nieuwe driver ?
                {
                    if (this.Driver.HasVehicle(this))
                    {
                        this.Driver.RemoveVehicle();
                        //verwijder de huidige driver zijn vehicle
                    }
                    if (!newDriver.HasVehicle(this)) //heeft de nieuwe driver dit al als vehicle ?
                    {
                        newDriver.RemoveVehicle();
                        this.Driver = newDriver;
                        newDriver.SetVehicle(this);
                    }
                    
                }
                else if(this.Driver == newDriver) // is huidige driver wel gelijk aan nieuwe driver? -> exception
                {
                    if(!this.Driver.HasVehicle(this)) {
                        this.Driver.RemoveVehicle();
                        this.Driver.SetVehicle(this);
                    }
                    
                }
            }
            else
            {
                throw new VehicleException("Vehicle - SetDriver - Driver is null");
            }

        }

        public bool HasDriver(Driver driver)
        {
            if (Driver != null)
            {
                if (Driver == driver)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void RemoveDriver()
        {
            Driver = null;
        }

        public override string ToString()
        {
            return $"{Brand}, {Model}, {ChassisNumber}, {LicensePlate}, {VehicleType}, {Color}, {Doors}";
        }




    }
}
