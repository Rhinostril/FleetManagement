﻿using System;
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

        public void AddFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }

        public void DeleteFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }

        public bool FuelCardExists(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<FuelCard> GetAllFuelCards()
        {
            throw new NotImplementedException();
        }

        public FuelCard GetFuelCard()
        {
            throw new NotImplementedException();
        }

        public FuelCard SearchFuelCard(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<FuelCard> SearchFuelCards(int? fuelCardId, string cardNr, FuelType fuelType)
        {
            throw new NotImplementedException();
        }

        public void UpdateFuelCard(FuelCard fuelCard)
        {
            throw new NotImplementedException();
        }

    }
}