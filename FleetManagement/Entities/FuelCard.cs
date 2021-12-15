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
        public List<FuelType> FuelTypes { get; private set; } = new List<FuelType>();
        public Driver Driver { get; private set; }
        public bool IsEnabled { get; set; }

        public FuelCard(string cardNumber, DateTime validityDate, string pin, List<FuelType> fuelTypes, bool isEnabled)
        {
            try
            {
                SetCardNumber(cardNumber);
                ValidityDate = validityDate;
                SetPin(pin);
                SetFuelTypes(fuelTypes);
                IsEnabled = isEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FuelCard(int fuelCardId, string cardNr, DateTime valDate, string pin, Driver driver, bool isEnabled)
        {
            SetFuelCardId(fuelCardId);
            SetCardNumber(cardNr);
            ValidityDate = valDate;
            SetPin(pin);
            Driver = driver;
            IsEnabled = isEnabled;
        }

        public FuelCard(string cardNumber, DateTime validityDate, string pin,  bool isEnabled)
        {
            try
            {
                SetCardNumber(cardNumber);
                ValidityDate = validityDate;
                SetPin(pin);
                IsEnabled = isEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FuelCard(int fuelCardId, string cardNumber, DateTime validityDate, string pin, List<FuelType> fuelTypes, bool isEnabled):this(cardNumber,validityDate,pin,fuelTypes,isEnabled)
        {
            try { 
            SetFuelCardId(fuelCardId);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public FuelCard(int fuelCardId, string cardNumber, DateTime validityDate, string pin, List<FuelType> fuelTypes, Driver driver, bool isEnabled) : this(fuelCardId,cardNumber, validityDate, pin, fuelTypes, isEnabled) {
            try {
            SetDriver(driver);
            
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public FuelCard(int fuelCardId, string cardNumber, DateTime validityDate, string pin, bool isEnabled)
        {
            SetFuelCardId(fuelCardId);
            SetCardNumber(cardNumber);
            ValidityDate = validityDate;
            SetPin(pin);
            IsEnabled = isEnabled;
        }

        public void SetFuelCardId(int id)
        {
            if(id > 0)
            {
                FuelCardId = id;
            }
            else
            {
                throw new FuelCardException("FuelCard - SetFuelCardId: FuelCardId has to be greater than 0!");
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
                throw new FuelCardException("FuelCard - SetCardNumber: Cardnumber can't be empty!");
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
                    throw new FuelCardException("FuelCard - SetPin: Pin has to be 4 characters long!");
                }
            }
            else
            {
                throw new FuelCardException("FuelCard - SetPin: Pin can't be empty!");
            }
        }

        public void SetFuelTypes(List<FuelType> fuelTypes)
        {
            if (fuelTypes == null) {
                throw new FuelCardException("fueltype list can't be missing!");
            } else if (!fuelTypes.Any()) {
                throw new FuelCardException("FuelCard - SetFuelTypes: Fueltypes must contain at least 1 fueltype!");
            } else {
                this.FuelTypes.Clear();
                foreach (FuelType type in fuelTypes) {
                    if (!String.IsNullOrEmpty(type.FuelName)) {
                        this.FuelTypes.Add(type);
                    } else {
                        throw new VehicleException("FuelCard - SetFuelTypes: fuelname can't be empty!");
                    }
                }
            }
        }

        public void SetDriver(Driver newDriver) //feedback leerkracht
        {
            if (newDriver == null) throw new FuelCardException("FuelCard - SetDriver - invalid driver");
            if (newDriver == Driver) throw new FuelCardException("FuelCard - SetDriver - not new");
            if(Driver != null)
            {
                if (newDriver.HasFuelCard(this))
                {
                    Driver.RemoveFuelCard();
                }
                if (!newDriver.HasFuelCard(this))
                {
                    Driver.SetFuelCard(this);
                }
                
                Driver = newDriver;
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

        public override string ToString()
        {
            return $"ID:{FuelCardId}, #{CardNumber}, {ValidityDate}, [{Pin}], {IsEnabled}, {Driver}";
        }

    }
}
