using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TcADSNet_Demo.Model;
//using TwinCAT.Ads;

namespace TcADSNet_Demo.ViewModels
{
    public class IOViewModel : BindableBase
    {
        #region Variables
        private int _plcIsAlivePulse;
        public int PlcIsAlivePulse
        {
            get { return _plcIsAlivePulse; }
            set { SetProperty(ref _plcIsAlivePulse, value); }
        }

        private int _hvar;
        public int Hvar
        {
            get { return _hvar; }
            set { SetProperty(ref _hvar, value); }
        }

        private static AdsConn _conn;
        public static AdsConn Conn
        {
            get {
                //if (_conn == null) _conn = new AdsConn();
                return _conn; 
            }
            set { _conn = value; }
        }
        #endregion

        public IOViewModel()
        {
            IOViewModel.Conn = new AdsConn();
            
        }



    }/// Class
}
