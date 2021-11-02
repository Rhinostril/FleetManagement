using FleetManagement.Business.Exceptions;
using FleetManagement.Business.Tools;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace FleetManagement.Business.Entities
{
    public class Driver {

        public int DriverID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Address Address { get; private set; }
        public string SecurityNumber { get; private set; }
        public List<string> DriversLicenceType { get; private set; } = new List<string>();
        public Vehicle Vehicle { get; private set; }
        public FuelCard FuelCard { get; private set; }

        public Driver(string firstName, string lastName, DateTime dateOfBirth, string securityNumber) {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetDateOfBirth(dateOfBirth);
            SetSecurityNumber(securityNumber);
        }
        public Driver(string firstname, string lastname, DateTime dateofBirth, string securitynumber, List<string> licensetypes) : this (firstname, lastname, dateofBirth, securitynumber) {

            try {
            SetDriversLicensetypes(licensetypes);
              
            }catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public Driver(string firstname, string lastname, DateTime dateofBirth, string securitynumber, List<string> licensetypes, Vehicle vehicle) :this(firstname, lastname, dateofBirth, securitynumber, licensetypes) {

            try {
                SetVehicle(vehicle);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public Driver(string firstname, string lastname, DateTime dateofBirth, string securitynumber, List<string> licensetypes, Vehicle vehicle,FuelCard fuelcard) : this(firstname, lastname, dateofBirth, securitynumber, licensetypes,vehicle) {

            try {
                SetFuelCard(fuelcard);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public Driver(string firstname, string lastname, DateTime dateofBirth, Address address,string securitynumber, List<string> licensetypes, Vehicle vehicle, FuelCard fuelcard) : this(firstname, lastname, dateofBirth, securitynumber, licensetypes, vehicle, fuelcard) {

            try {
                SetAddress(address);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        private void SetFirstName(string firstname) {
            
            if(String.IsNullOrWhiteSpace(firstname)) {
                throw new DriverException("Driver -Setfirstname: Cannot set an empty first name");
            }
            else {
                FirstName = firstname;
            }
           
        }
        private void SetLastName(string lastname) {
            if(String.IsNullOrWhiteSpace(lastname)) {
                throw new DriverException("Driver - SetLastName: Cannot set an empty last name");
            }
            else {
                LastName = lastname;
            }
            
        }
        private void SetDateOfBirth(DateTime dateofbirth) {
            TimeSpan offset = DateTime.Now - dateofbirth;
            if(offset.TotalDays <= 0) {
                throw new DriverException("Driver - SetDateOfBirth: Date of birth cannot be in the future");
            }else if(offset.TotalDays >= 36524) {
                throw new DriverException("Driver - SetDateOfBirth: Date of birth cannot be more than 100 years in the past");
            }
            else {
                DateOfBirth = dateofbirth;
            }
         
        }
        public void SetSecurityNumber(string securtiynumber) {
            //this is public because this parameter can concievably still change after it is set
            bool validationSucceeded = SocialSecurityNumberChecker.checknormalSecurityNumber(securtiynumber);
            if(validationSucceeded) {
                SecurityNumber = securtiynumber;
            }
            else {
                throw new SecurityNumberException("Driver - SetSecurityNumber: The given social security number is invalid");

                //ASK client: driverexception or other
            }
        }

        private void SetDriversLicensetypes(List<string> licensetypes) {
            try {
                foreach (string Ltype in licensetypes) {

                    if (String.IsNullOrEmpty(Ltype)) {
                        throw new DriverException("Driver - SetDriversLicensetypes: licensetype cannot be null or empty");
                    }
                }
            }catch(Exception ex) {
                return;
            }
            DriversLicenceType.Clear();
            DriversLicenceType.AddRange(licensetypes);
        }
        public void AddTypeToDriversLicense(List<string> licenseTypesToAdd) {
            foreach(string type in licenseTypesToAdd) {
                if(!DriversLicenceType.Contains(type)) {
                    //the list does not contain this type yet and it can be added
                    DriversLicenceType.Add(type);
                }//else it should be ignored
            }
        }
        
        public void SetAddress(Address address) {
            if(address != null)
            {
                Address = address;
            }
            else { 
                throw new DriverException("Driver - SetAddress: Address cannot be null or empty");
            }
        }

        public void RemoveAddress(Address address) {
            if(Address == address) {
                Address = null;
            }
            else {
                throw new DriverException("Driver - RemoveAddress: Can't remove the targeted address because it is not the same as the given address");
            }
        }
        public void SetDriverID(int driverId) {
            if(driverId > 0)
            {
                DriverID = driverId;
            }
            else
            {
                throw new DriverException("Driver - SetDriverID:  id needs to be greater than 0");
            }
        }
        public void SetVehicle(Vehicle newvehicle) {

            if(newvehicle != null) {
                if(this.Vehicle == null) {
                    if(!newvehicle.HasDriver(this)) {
                        newvehicle.SetDriver(this);
                    }
                }
                else if(this.Vehicle != newvehicle) {
                    //case where the new vehicle isnt the same as the current one
                    if(this.Vehicle.HasDriver(this)) {
                        this.Vehicle.RemoveDriver(); //if the previous vehicle still has this driver, remove it
                    }
                    if(!newvehicle.HasDriver(this)) {
                        newvehicle.RemoveDriver();
                        newvehicle.SetDriver(this);
                    }
                }
                Vehicle = newvehicle;
                if(!newvehicle.HasDriver(this)) {
                    newvehicle.RemoveDriver();
                    newvehicle.SetDriver(this);
                }
            }
            else {
                throw new DriverException("Driver - Setvehicle: new vehicle is null");
            }     
        }
        public void RemoveVehicle() {
            Vehicle = null;
        }
        public bool HasVehicle(Vehicle vehicle) { //maybe ambiguous language: hasvehicle could be interpreted as "Does the driver have any vehicle?"
            if(Vehicle != null) {
                if(this.Vehicle == vehicle) {
                    return true;
                }
                else { 
                    return false;
                }
            }
            else {
                return false;
            }
        }
      
        public void SetFuelCard(FuelCard newfuelCard) {
            if(newfuelCard != null) {
                if(this.FuelCard == null) {
                    //no previous fuelcard exitst
                    if(!newfuelCard.HasDriver(this)) {
                        newfuelCard.SetDriver(this);
                    }
                }else if(this.FuelCard != newfuelCard) {
                    //previous fuelcard exitst and is not the new fuelcard
                    if(!newfuelCard.HasDriver(this)) {
                        newfuelCard.RemoveDriver();
                        newfuelCard.SetDriver(this);
                    }
                }
                this.FuelCard = newfuelCard;
                if(!FuelCard.HasDriver(this)) {
                    FuelCard.RemoveDriver();
                    FuelCard.SetDriver(this);
                }
            }
            else {
                throw new DriverException("Driver - Setvehicle: Fuelcard cannot be empty or null");
            }
        }
        public void RemoveFuelCard() {
            FuelCard = null;
        }
        public bool HasFuelCard(FuelCard fuelcard) {
            if(FuelCard != null) {
                if(this.FuelCard == fuelcard) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

    }
}
