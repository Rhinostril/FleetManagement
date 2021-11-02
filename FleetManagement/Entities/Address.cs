using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities {
    public class Address {
        public int AddressID { get; private set; }
        public string Street { get; private set; }
        public string HouseNr { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }

        public Address() {

        }
        public Address(string street, string housenr, string postalcode, string city, string country) {
            try {
                SetStreet(street);
                SetHouseNr(housenr);
                SetPostalCode(postalcode);
                SetCity(city);
                SetCountry(country);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Address(int addressid, string street, string housenr, string postalcode, string city, string country):this(street, housenr, postalcode, city, country) {
            try {
                SetAddressID(addressid);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void SetAddressID(int ID) {
            if(ID > 0) {
                AddressID = ID;
            }else {
                throw new AddressException("Address - SetAddressID: Id cannot be 0 or negative");
            }
            
        }
        public void SetStreet(string street) {
            if(!String.IsNullOrEmpty(street)) {
                Street = street;
            }
            else {
                throw new AddressException("Address - SetStreet: street cannot be empty or missing");
            }

        }
        public void SetHouseNr(string houseNr) {
           
            // Does it have to start with a NR? in other words : houseNr validation?

            if(!String.IsNullOrEmpty(houseNr)) {
                HouseNr = houseNr;
            }
            else {
                throw new AddressException("Address - SetHouseNr: housenumber cannot be empty or missing");
            }

        }
        public void SetPostalCode(string postalcode) {
            if(!String.IsNullOrEmpty(postalcode)) {
                if(postalcode.Length == 4)
                {
                    PostalCode = postalcode;
                }
                else
                {
                    throw new AddressException("Address - SetPostalCode: postalcode must be 4 characters long");
                }
            }
            else {
                throw new AddressException("Address - SetPostalCode: postalcode cannot be empty or missing");
            }
        }
        public void SetCity(string city) {

            if(!String.IsNullOrEmpty(city)) {
                City = city;
            }
            else {
                throw new AddressException("Address - SetCity: city cannot be empty or missing");
            }
           
        }
        public void SetCountry(string country) {
           
            if(!String.IsNullOrEmpty(country)) {
                Country = country;
            }
            else {
                throw new AddressException("Address - SetCountry: city cannot be empty or missing");
            }
        }

    }
}
