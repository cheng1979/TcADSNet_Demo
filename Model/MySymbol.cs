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

        private Object _value;
        public Object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public MySymbol()
        {

        }
        public MySymbol(Symbol symbol)
        {
            if (symbol != null)
            {
                Name = symbol.InstanceName;
                Path = symbol.InstancePath;
                Type = symbol.DataType.Name;
            }
        }



    }///Class
}
