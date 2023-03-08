using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class PlcSymbolsDTO
    {
        public String Name{ get; set; }
        readonly ReadOnlyCollection<PlcMemberDTO> Members;

        #region Constructor
        public PlcSymbolsDTO()
        {   
        }

        public PlcSymbolsDTO(ReadOnlySymbolCollection twincatSymbols)
        {

        }
        #endregion

        public void SetMenbersCollection(ReadOnlySymbolCollection symbols)
        {
            Members = new ReadOnlyCollection<PlcMemberDTO>(
                (from symb in symbols
                 select new PlcMemberDTO(symbols)
                ).ToList()
                );
        }


    }///Class
}
