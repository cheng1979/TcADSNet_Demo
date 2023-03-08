using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public class PlcMemberDTO
    {
        public String DataTypeName { get; set; }
        public String InstanceName { get; set; }
        public String InstancePath { get; set; }

        public PlcMemberDTO() { }

        public PlcMemberDTO(String dataTypeName, String name, String path)
        {
            DataTypeName = dataTypeName;
            InstanceName = name;
            InstancePath = path;
        }
    }
}
