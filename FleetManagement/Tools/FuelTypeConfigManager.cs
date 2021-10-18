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
using System.IO;
using System.Reflection;

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
        public static void AddFuelTypeToConfigFile(string keyName, string valueName) {

            //lijkt relatief onmogelijk om te doen idk

            //ResourceManager FueltypesResourceManager = FuelTypeConfig.ResourceManager;
            //string rootname = FueltypesResourceManager.BaseName;
            ////var stream = FueltypesResourceManager.GetStream(rootname);
            //var assembly = Assembly.GetExecutingAssembly();
            //var stream = assembly.GetManifestResourceStream("FleetManagement.Business.ConfigFiles.FuelTypeConfig");
            //string filename = "FuelTypeConfig.resx";
            //string path = Path.GetFullPath(".\\ConfigFiles\\FuelTypeConfig.resx");
            //string dirpath = Directory.GetCurrentDirectory();
            //string singleparent = Directory.GetParent(dirpath).FullName;
            //string twiceparent = Directory.GetParent(singleparent).FullName;
            //string triceparent = Directory.GetParent(twiceparent).FullName;
            //string lastparent = Directory.GetParent(triceparent).FullName;
            //string finalpath = Path.Combine(lastparent, "Fleetmanagement\\ConfigFiles\\FuelTypeConfig.resx");

            //Console.WriteLine();
            //ResourceReader reader = new ResourceReader(finalpath);
            ////ResourceWriter writer = new ResourceWriter(finalpath);
            ////C: \Users\Stan\source\repos\FleetManagement\FleetManagement\Properties\FuelTypeConfig.resx
            ////"FleetManagement.Business.Properties.FuelTypeConfig"
            //Console.WriteLine();
            ////writer.AddResource(keyName, valueName);
            ////writer.Generate();
            ////writer.Close();
            ////@"./ConfigFiles/FuelTypeConfig.resx"

        }
    }
}
