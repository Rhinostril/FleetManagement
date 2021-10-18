using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using FleetManagement.Business.Entities;
using FleetManagement.Business.ConfigFiles;

namespace FleetManagement.Business.Tools {
    public static class FuelTypeConfigManager {

        public static IReadOnlyList<FuelType> GetAllFuelTypes() {
            //this method can read all the entries in the fueltype config
            ResourceManager FueltypesResourceManager =  FuelTypeConfig.ResourceManager;
            ResourceSet set = FueltypesResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            List<FuelType> fuelTypes = new List<FuelType>();
            foreach(DictionaryEntry entry in set) {
                object value = entry.Value;
                string valueAsString = value.ToString();
                FuelType f = new FuelType(valueAsString);
                fuelTypes.Add(f);
            }
            return fuelTypes.AsReadOnly();

        }
        //public static void 
    }
}
