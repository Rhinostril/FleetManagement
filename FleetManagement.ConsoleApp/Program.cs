using System;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;

namespace FleetManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testing
            FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());

            Console.WriteLine("Add Fuelcard: ");
            FuelCard fuelCard = new FuelCard();




        }

    }


}



