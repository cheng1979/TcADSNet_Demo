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
        private static string filePath_symbolsPollConfig = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_symbolsPollConfig);
        private static string filePath_symbolsPollConfig_BKP = Path.Combine(Environment.CurrentDirectory, @"Config\", ConfigFile.fileName_symbolsPollConfig_BKP);

        public static Boolean SaveFile_SymbolsPool(SymbolsCollection mySymbols)
        {

            Boolean isSaved = false;
            try
            {
                String filePath = ConfigFile.filePath_symbolsPollConfig;
                JsonSerializeReturn json = new JsonSerializeReturn();
                if (File.Exists(filePath))
                {
                    json = ConvertSymbolsToJson(mySymbols);
                    File.WriteAllText(filePath, json.Serialized);
                }
                else
                {
                    StreamWriter sw = File.CreateText(filePath);
                    sw.Write(ConvertSymbolsToJson(mySymbols));
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
            String filePath = ConfigFile.filePath_symbolsPollConfig;
            SymbolsCollection syms = new SymbolsCollection();

            if (File.Exists(filePath))
            {
                syms.Collection = ConvertJsonToSymbols(File.ReadAllText(filePath));
            }

            return syms;
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
                Console.WriteLine("\nJSON Serialization Exeption\n" + e.Message);
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

    }///Class

}
