using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities
{
    public class FuelCard
    {

        public int FuelCardId { get; private set; } // cardNumber als id gebruiken ?
        public string CardNumber { get; private set; }
        public DateTime ValidityDate { get; set; }
        public string Pin { get; private set; }
        public FuelType FuelType { get; private set; }
        public Driver Driver { get; private set; }


        public FuelCard(int fuelCardId, string cardNumber, DateTime validityDate, string pin, FuelType fuelType)
        {
            SetFuelCardId(fuelCardId);
            SetCardNumber(cardNumber);
            ValidityDate = validityDate;
            SetPin(pin);
            SetFuelType(fuelType);
        }

        public FuelCard(int fuelCardId, string cardNumber, DateTime validityDate, string pin, FuelType fuelType, Driver driver)
        {
            SetFuelCardId(fuelCardId);
            SetCardNumber(cardNumber);
            ValidityDate = validityDate;
            SetPin(pin);
            SetFuelType(fuelType);
            SetDriver(driver);
        }

        public void SetFuelCardId(int id)
        {
            if(id > 0)
            {
                FuelCardId = id;
            }
            else
            {
                throw new FuelCardException("FuelCardId has to be greater than 0!");
            }
        }
        public void SetCardNumber(string cardNumber)
        {
            if (!string.IsNullOrWhiteSpace(cardNumber))
            {
                CardNumber = cardNumber;
            }
            else
            {
                throw new FuelCardException("Cardnumber can't be empty!");
            }
        }
        public void SetPin(string pin)
        {
            if (!string.IsNullOrWhiteSpace(pin))
            {
                if(pin.Length == 4)
                {
                    Pin = pin;
                }
                else
                {
                    throw new FuelCardException("Pin has to be 4 characters long!");
                }
            }
            else
            {
                throw new FuelCardException("Pin can't be empty!");
            }
        }

        public void SetFuelType(FuelType fuelType)
        {
            if(fuelType != null)
            {
                FuelType = fuelType;
            }
            else
            {
                throw new FuelCardException("FuelType cannot be null!");
            }
        }

        public void SetDriver(Driver driver)
        {
            if (driver == null) throw new FuelCardException("FuelCard - SetDriver - invalid driver");
            if (driver == Driver) throw new FuelCardException("FuelCard - SetDriver - not new");
            if(Driver != null)
            {
                if (driver.HasFuelCard(this))
                {
                    Driver.RemoveFuelCard();
                }
                if (!driver.HasFuelCard(this))
                {
                    Driver.SetFuelCard(this);
                }
                
                Driver = driver;
            }
        }

        public bool HasDriver(Driver driver)
        {
            if(Driver != null)
            {
                if(Driver == driver)
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


    }
}
