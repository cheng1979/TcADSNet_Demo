using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcADSNet_Demo.Model;

namespace TcADSNet_Demo.ViewModels
{
    class MenuViewModel : BindableBase
    {
        private String _buttonConnectColor;
        public String ButtonConnectColor
        {
            get { return _buttonConnectColor; }
            set { SetProperty(ref _buttonConnectColor, value); }
        }


        public MenuViewModel()
        {
            AdsConn.Instance.Client.ConnectionStateChanged += Client_ConnectionStateChanged;
            AdsConn.evAdsConnected += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected += AdsConn_evAdsDisconnected;

            ButtonConnectColor = "Transparent";
        }

        private void Client_ConnectionStateChanged(object sender, TwinCAT.ConnectionStateChangedEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void AdsConn_evAdsDisconnected(object sender, EventArgs e)
        {
            ButtonConnectColor = "Transparent";
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            ButtonConnectColor = "#11f705"; ///Light Green
        }


    }///Class
}
