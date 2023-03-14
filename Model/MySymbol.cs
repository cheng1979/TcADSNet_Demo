using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class MySymbol
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _path;
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private String _type;
        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public MySymbol(Symbol symbol)
        {
            Name = symbol.InstanceName;
            Path = symbol.InstancePath;
            Type = symbol.DataType.Name;
        }



    }///Class
}
