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

        public int FuelCardId { get; private set; }
        public string CardNumber { get; private set; }
        public DateTime ValidityDate { get; set; }
        public string Pin { get; private set; }
        public FuelType FuelType { get; private set; }
        public Driver Driver { get; set; }


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
        public void SetDriver(Driver driver)
        {
            if(driver != null)
            {
                if(driver.FuelCard == null)
                {
                    Driver = driver;
                    driver.setFuelCard(this);
                }
                else
                {
                    //TODO
                }
            }
            else
            {
                //TODO
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


    }
}
