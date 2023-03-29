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
        
        private String _statusMessage;
        public String StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        private bool _btnDisconnectIsEnabled;
        public bool BtnDisconnectIsEnabled
        {
            get { return _btnDisconnectIsEnabled; }
            set { SetProperty(ref _btnDisconnectIsEnabled, value); }
        }



        public MenuViewModel()
        {
            AdsConn.Instance.Client.ConnectionStateChanged += Client_ConnectionStateChanged;

            AdsConn.evAdsConnected += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected += AdsConn_evAdsDisconnected;
            Publisher.evPublisher += Publisher_evPublisher;
            Publisher.evOnConnectSubTasksCompleted += Publisher_evOnConnectSubTasksCompleted;

            ButtonConnectColor = "Transparent";
            StatusMessage = "Loading App...";
            BtnDisconnectIsEnabled = false;

        }
        
        
        #region Event Listeners
        private void Publisher_evOnConnectSubTasksCompleted(object sender, bool e)
        {
            /// All On Connected SubTasks are completed --> enable Disconnect button
            BtnDisconnectIsEnabled = true;
        }
        
        
        private void Publisher_evPublisher(object sender, string msg)
        {
            if (msg != null)
            {
                DateTime dt = DateTime.Now;
                //StatusMessage = msg + " | " + 
                //                dt.Hour.ToString()  +":"+ 
                //                dt.Minute.ToString()+":"+
                //                dt.Second.ToString()+"."+
                //                dt.Millisecond.ToString();

                StatusMessage = dt.Hour.ToString() + ":" +
                                dt.Minute.ToString() + ":" +
                                dt.Second.ToString() + "." +
                                dt.Millisecond.ToString() + "  |  " +
                                msg;
            }
        }

        private void VariablesViewModel_evReadValueException(object sender, EventArgs e)
        {
            //AdsConn.Instance.Client.Disconnect();
            
        }

        private void Client_ConnectionStateChanged(object sender, TwinCAT.ConnectionStateChangedEventArgs e)
        {
            //Console.WriteLine(e);
            //Console.WriteLine("NewState: {0}, OldState: {1}, Reason: {2}, Exception: {3}",e.NewState, e.OldState, e.Reason, e.Exception);
            Publisher.Publish("NewState: "+e.NewState+", OldState: "+e.OldState+", Reason: "+e.Reason+", Exception: "+e.Exception+".");
        }

        private void AdsConn_evAdsDisconnected(object sender, EventArgs e)
        {
            ButtonConnectColor = "Transparent";
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            ButtonConnectColor = "#11f705"; ///Light Green

            /// Publish that on connected subtask is completed
            Publisher.OnConnectSubTaskDone();
        }

        #endregion

    }///Class
}
