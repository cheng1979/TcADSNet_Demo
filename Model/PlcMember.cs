using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;
using TwinCAT.Ads.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class PlcMember
    {
        /// <summary>
        /// This Class have the PLC symbols collection and the parent name
        /// ReadOnlyCollection of PlcSymbolsDTO
        /// </summary>

        public String Name { get; set; }
        public ReadOnlyCollection<PlcSymbol> Members { get; set; }

        public PlcMember(String Name) 
        {
            this.Name = Name;
            this.Members = SetEmptyCollection();
        }

        public PlcMember(String name, ReadOnlySymbolCollection memberInstances)
        {
            this.Name = name;
            this.Members = GetSymbolsInTwinCATSymbolCollection(memberInstances);
        }
        
        private ReadOnlyCollection<PlcSymbol> GetSymbolsInTwinCATSymbolCollection(ReadOnlySymbolCollection _symbols)
        {
            /// Get Symbols in TwinCAT ReadOnlySymbolCollection and assign them to a ReadOnlyCollection<T>
            ReadOnlyCollection<PlcSymbol> retunCol;
            List<PlcSymbol> lst = new List<PlcSymbol>();

            /// Generate a list with all symbols in a member collection
            foreach (Symbol syb in _symbols)
            {
                lst.Add(new PlcSymbol(syb.InstanceName, syb.InstancePath, syb.DataType.Name));
            }
            /// Assign list to ReadOnlyCollection 
            retunCol = new ReadOnlyCollection<PlcSymbol>(lst);

            return retunCol;
        }

        private ReadOnlyCollection<PlcSymbol> SetEmptyCollection()
        {
            ///Set an empty collection
            ReadOnlyCollection<PlcSymbol> retunCol;
            List<PlcSymbol> lst = new List<PlcSymbol>();
            lst.Add(new PlcSymbol("empty", "empty", "empty"));
            return retunCol = new ReadOnlyCollection<PlcSymbol>(lst);
        }

    }///Class
}
