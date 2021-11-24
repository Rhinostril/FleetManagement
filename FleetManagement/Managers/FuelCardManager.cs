﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.Entities;

namespace FleetManagement.Business.Managers
{
    public class FuelCardManager
    {

        private IFuelCardRepository repo;

        public FuelCardManager(IFuelCardRepository repo)
        {
            this.repo = repo;
        }

        public IReadOnlyList<FuelCard> SearchFuelCards(string cardNr, DateTime? valDate)
        {
            try
            {
                return repo.SearchFuelCards(cardNr, valDate);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public FuelCard getFuelCardByID(int fuelcardId) {
            try {
                FuelCard fuelcard = repo.GetFuelCard(fuelcardId);
                return fuelcard;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public IReadOnlyList<FuelCard> GetLatestFuelcards() {
            try {
                return repo.GetTop50FuelCards();
                

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public void AddFuelCard(FuelCard fuelCard)
        {
            try
            {
                if (!repo.FuelCardExists(fuelCard))
                {
                    repo.AddFuelCard(fuelCard);
                }
                else
                {
                    throw new FuelCardManagerException("FuelCardManager - AddFuelCard - FuelCard already added");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateFuelCard(FuelCard fuelCard)
        {
            try
            {
                // Bestaat fuelCard met zelfde properties al?


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteFuelCard(FuelCard fuelCard)
        {
            try
            {
                if (repo.FuelCardExists(fuelCard))
                {
                    repo.DeleteFuelCard(fuelCard);
                }
                else
                {
                    throw new FuelCardManagerException("FuelCardManager - DeleteFuelCard - FuelCard already deleted");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
