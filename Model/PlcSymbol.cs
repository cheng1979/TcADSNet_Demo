using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class PlcSymbol
    {
        public String Name{ get; set; }
        public String InstancePath{ get; set; }
        public String DataTypeName{ get; set; }

        #region Constructor
        public PlcSymbol()
        {   
        }

        public PlcSymbol(String instanceName, String instancePath, String dataTypeName)
        {
            this.Name = instanceName;
            this.InstancePath = instancePath;
            this.DataTypeName = dataTypeName;
        }
        #endregion

       


    }///Class
}
