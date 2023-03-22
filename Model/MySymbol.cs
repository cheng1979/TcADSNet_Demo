using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads.TypeSystem;
using Prism.Mvvm;

namespace TcADSNet_Demo.Model
{
    public class MySymbol : BindableBase
    {
        private readonly Object _mySymbolLock = new Object();

        private readonly Object _nameLock = new Object();
        private String _name;
        public String Name
        {
            get { lock (_nameLock) { return _name; } }
            set { lock (_nameLock) { SetProperty(ref _name, value); } }
        }

        private readonly Object _pathLock = new Object();
        private String _path;
        public String Path
        {
            get { lock (_pathLock) { return _path; } }
            set { lock (_pathLock) { SetProperty(ref _path, value); } }
        }

        private readonly Object _typeLock = new Object();
        private String _type;
        public String Type
        {
            get { lock (_typeLock) { return _type; } }
            set { lock (_typeLock) { SetProperty(ref _type, value); } }
        }

        private readonly Object _valueLock = new Object();
        private Object _value;
        public Object Value
        {
            get { lock (_valueLock) { return _value; } }
            set { lock (_valueLock) { SetProperty(ref _value, value); } }
        }

        private readonly Object _writeValueLock = new Object();
        private String _writeValue;
        public String WriteValue
        {
            get { lock (_writeValueLock) { return _writeValue; } }
            set { lock (_writeValueLock) { SetProperty(ref _writeValue, value); } }
        }


        public MySymbol()
        {
            
        }
        public MySymbol(Symbol symbol)
        {
            if (symbol != null)
            {
                lock (_mySymbolLock)
                {
                    Name = symbol.InstanceName;
                    Path = symbol.InstancePath;
                    Type = symbol.DataType.Name;
                }
            }
        }



    }///Class
}
