using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TwinCAT.Ads.TypeSystem;
using System.Windows;
using TwinCAT.TypeSystem;
using System.Collections.ObjectModel;

namespace TcADSNet_Demo.Model
{
    public static class ConfigFile
    {
        private static string fileName_symbolsPollConfig = "configSymbolsPoll.txt";
        private static string fileName_symbolsPollConfig_BKP = "configSymbolsPollBKP.txt";
        private static string fileName_routesConfig = "configRoutes.txt";
        private static string filePath_symbolsPollConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_symbolsPollConfig);
        private static string filePath_symbolsPollConfig_BKP = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_symbolsPollConfig_BKP);
        private static string filePath_routesConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_routesConfig);

        public static Boolean SaveFile_SymbolsPool(SymbolsCollection mySymbols)
        {

            Boolean isSaved = false;
            try
            {
                String dynamicFilePath = AdsConn.Instance.NetId + "_" + ConfigFile.fileName_symbolsPollConfig; /// Concatenate AMS NetID with file name to generate unique path name
                ConfigFile.filePath_symbolsPollConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", dynamicFilePath);
                String filePath = ConfigFile.filePath_symbolsPollConfig;
                JsonSerializeReturn json = new JsonSerializeReturn();
                json = ConvertSymbolsToJson(mySymbols);
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, json.Serialized);
                }
                else
                {
                    StreamWriter sw = File.CreateText(filePath);
                    sw.Write(json.Serialized);
                    sw.Close();
                }

                if(json.Converted) isSaved = true;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Save Symbols Pool Config. ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSaved;
        }

        public static SymbolsCollection ReadFile_SymbolsPool()
        {
            String dynamicFilePath = AdsConn.Instance.NetId + "_" + ConfigFile.fileName_symbolsPollConfig; /// Concatenate AMS NetID with file name to get especific file path for the ADS Client
            ConfigFile.filePath_symbolsPollConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", dynamicFilePath);
            String filePath = ConfigFile.filePath_symbolsPollConfig;
            SymbolsCollection syms = new SymbolsCollection();

            if (File.Exists(filePath))
            {
                syms.Collection = ConvertJsonToSymbols(File.ReadAllText(filePath));
            }

            return syms;
        }

        public static Boolean SaveFile_AmsRoutes(ObservableCollection<AmsNetId> routes)
        {

            Boolean isSaved = false;
            try
            {
                ConfigFile.filePath_routesConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_routesConfig);
                String filePath = ConfigFile.filePath_routesConfig;
                JsonSerializeReturn json = new JsonSerializeReturn();
                json = ConvertAmsRoutesToJson(routes);
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, json.Serialized);
                }
                else
                {
                    StreamWriter sw = File.CreateText(filePath);
                    sw.Write(json.Serialized);
                    sw.Close();
                }

                if (json.Converted) isSaved = true;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Save Routes Config. ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSaved;
        }

        public static ObservableCollection<AmsNetId> ReadFile_RoutesConfig()
        {
            ConfigFile.filePath_routesConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_routesConfig);
            String filePath = ConfigFile.filePath_routesConfig;
            ObservableCollection<AmsNetId> routes = new ObservableCollection<AmsNetId>();

            if (File.Exists(filePath))
            {
                routes = ConvertJsonToAmsRoutes(File.ReadAllText(filePath));
            }

            return routes;
        }


        private static JsonSerializeReturn ConvertSymbolsToJson(SymbolsCollection mySymbols)
        {
            JsonSerializeReturn ret = new JsonSerializeReturn();
            try
            {
                ret.Serialized = JsonConvert.SerializeObject(mySymbols.Collection);

                ret.Converted = true;
            }
            catch (Exception e)
            {
                //Console.WriteLine("\nJSON Serialization Exeption\n" + e.Message);
                Publisher.Publish("JSON Serialization Exeption: " + e.Message);
                ret.Converted = false;
            }

            return ret;
        }

        private static ObservableCollection<MySymbol> ConvertJsonToSymbols(String jsonObj)
        {
            ObservableCollection<MySymbol> collection;
            
            collection = JsonConvert.DeserializeObject<ObservableCollection<MySymbol>>(jsonObj);
                        
            return collection;
        }

        private static JsonSerializeReturn ConvertAmsRoutesToJson(ObservableCollection<AmsNetId> routes)
        {
            JsonSerializeReturn ret = new JsonSerializeReturn();
            try
            {
                ret.Serialized = JsonConvert.SerializeObject(routes);

                ret.Converted = true;
            }
            catch (Exception e)
            {
                //Console.WriteLine("\nJSON Serialization Exeption\n" + e.Message);
                Publisher.Publish("JSON Serialization Exeption: " + e.Message);
                ret.Converted = false;
            }

            return ret;
        }

        private static ObservableCollection<AmsNetId> ConvertJsonToAmsRoutes(String jsonObj)
        {
            ObservableCollection<AmsNetId> collection;

            collection = JsonConvert.DeserializeObject<ObservableCollection<AmsNetId>>(jsonObj);

            return collection;
        }

    }///Class

}
