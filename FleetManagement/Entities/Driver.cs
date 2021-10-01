using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Entities
{
    public class Driver {

        public int DriverID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Address Address { get; private set; }
        public string SecurityNumber { get; private set; }
        public List<string> DriversLicenceType { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public FuelCard FuelCard { get; private set; }

        public Driver(string firstname, string lastname, DateTime dateofBirth, string securitynumber, List<string> licensetypes,) {
            DriversLicenceType = new List<string>();

        }
        private void setFirstName(string firstname) {
            FirstName = firstname;
        }
        private void setLastName(string lastname) {
            LastName = lastname;
        }
        private void setDateOfBirth(DateTime dateofbirth) {
            //possible check if Date is not past 150 years or so --> ask client
            DateOfBirth = dateofbirth;
        }
        public void setSecurityNumber(string securtiynumber) {
            //this is public because this parameter can concievably still change after it is set
            if(validateSecurityNumber(securtiynumber)) {
                SecurityNumber = securtiynumber;
            }
            else {
                //TODO: throw validation error message here
            }

        }
        private bool validateSecurityNumber(string securitynumber) {
            //this method validates the inputted SN strictly, it is invalid until proven otherwise
            bool validationsucceeded = false;
            //TODO validation

            return validationsucceeded;
        }

        private void setDriversLicensetypes(List<string> licensetypes) {
            DriversLicenceType.Clear();
            DriversLicenceType.AddRange(licensetypes);
        }
        public void addTypeToDriversLicense(List<string> licenseTypesToAdd) {
            foreach(string type in licenseTypesToAdd) {
                if(!DriversLicenceType.Contains(type)) {
                    //the list does not contain this type yet and it can be added
                    DriversLicenceType.Add(type);
                }//else it should be ignored
            }
        }

        public void setAdress(Address address) {
            Address = address;
        }
        public void removeAdress(Address adress) {
            if(Address == adress) {
                Address = null;
            }
            else {
                //TODO throw error could not find this specific adress to remove
            }
        }
        public void setDriverID(int driverid) {
            DriverID = driverid;
        }
        public void setVehicle(Vehicle vehicle) {
            Vehicle = vehicle;
        }

        public bool TryRemoveVehicle(Vehicle vehicle) {
            if(Vehicle == vehicle) {
                Vehicle = null;
                return true;
            }else {
                //TODO throw error vehicle cannot be removed because the driver does not drive this exact vehicle
                return false;
            }
        }
        public void setFuelCard(FuelCard fuelcard) {
            FuelCard = fuelcard;
        }
        public void removeAnyFuelCard() {
            FuelCard = null;
        }
        public bool tryRemoveSpecificFuelCard(FuelCard fuelcard) {
            if(FuelCard != null && FuelCard == fuelcard) {
                FuelCard = null;
                return true;
            }else {
                //TODO Throw error cannot find this specific fuelcard to remove
                return false;
            }
        }


    }
}
