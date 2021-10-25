using System;
using System.Collections.Generic;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using System.Data.SqlClient;

namespace FleetManagement.Data
{
    public class FuelCardRepository : IFuelCardRepository
    {

        private string connectionString = $"Data Source=fleetmanagserver.database.windows.net;Persist Security Info=True;User ID=fleetadmin;Password=***********";

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


        IReadOnlyList<FuelCard> GetAllFuelCards()
        {
            throw new NotImplementedException();
        }
        IReadOnlyList<FuelCard> SearchFuelCards(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            throw new NotImplementedException();
        }
        FuelCard GetFuelCard()
        {
            throw new NotImplementedException();
        }
        FuelCard SearchFuelCard(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            throw new NotImplementedException();
        }
        bool FuelCardExists(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }
        void AddFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }
        void UpdateFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }
        void DeleteFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }


    }
}
