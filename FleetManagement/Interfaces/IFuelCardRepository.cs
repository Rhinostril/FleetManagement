using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Interfaces
{
    public interface IFuelCardRepository
    {
        FuelCard GetFuelCard(int fuelCardId);
        IReadOnlyList<FuelCard> GetAllFuelCards();
        IReadOnlyList<FuelCard> GetTop50FuelCards();
        IReadOnlyList<FuelCard> GetFuelCardsByAmount(int amount);
        IReadOnlyList<FuelCard> SearchFuelCards(string cardNr, DateTime? validityDate);
        FuelCard SearchFuelCard(int? fuelCardId, string cardNr, FuelType fuelType);
        bool FuelCardExists(FuelCard fuelCard);
        bool FuelCardHasDriver(FuelCard fuelCard);
        void AddFuelCard(FuelCard fuelCard);
        void UpdateFuelCard(FuelCard fuelCard);
        void DeleteFuelCardTransaction(FuelCard fuelCard);

    }
}
