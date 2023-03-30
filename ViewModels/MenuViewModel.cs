using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcADSNet_Demo.Model;

namespace TcADSNet_Demo.ViewModels
{
    class MenuViewModel : BindableBase
    {
        #region Variables
        private static bool _firstRun = false;

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

        private bool _btnConnectIsEnabled;
        public bool BtnConnectIsEnabled
        {
            get { return _btnConnectIsEnabled; }
            set { SetProperty(ref _btnConnectIsEnabled, value); }
        }

        private bool _btnDisconnectIsEnabled;
        public bool BtnDisconnectIsEnabled
        {
            get { return _btnDisconnectIsEnabled; }
            set { SetProperty(ref _btnDisconnectIsEnabled, value); }
        }

        private ObservableCollection<AmsNetId> _amsRoutes;
        public ObservableCollection<AmsNetId> AmsRoutes
        {
            get
            {
                if (_amsRoutes == null) _amsRoutes = new ObservableCollection<AmsNetId>();
                return _amsRoutes;
            }
            set { SetProperty(ref _amsRoutes, value); }
        }

        #endregion


        public MenuViewModel()
        {
            AdsConn.Instance.Client.ConnectionStateChanged += Client_ConnectionStateChanged;

            AdsConn.evAdsConnected      += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected   += AdsConn_evAdsDisconnected;
            Publisher.evPublisher       += Publisher_evPublisher;
            Publisher.evOnConnectSubTasksCompleted  += Publisher_evOnConnectSubTasksCompleted;
            Publisher.evEnableConnectButton         += Publisher_evEnableConnectButton;

            ButtonConnectColor = "Transparent";
            StatusMessage = "Loading App...";
            BtnDisconnectIsEnabled = false;
            GetUsedAmsRoutes();
        }


        #region Event Listeners
        private void Publisher_evEnableConnectButton(object sender, EventArgs e)
        {
            BtnConnectIsEnabled     = true;
            BtnDisconnectIsEnabled  = false;
        }
                
        private void Publisher_evOnConnectSubTasksCompleted(object sender, bool e)
        {
            /// All On Connected SubTasks are completed --> enable Disconnect button
            BtnDisconnectIsEnabled  = true;
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
            _firstRun = false;
            BtnConnectIsEnabled     = true;
            BtnDisconnectIsEnabled  = false;
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            if (!_firstRun)
            {
                ButtonConnectColor = "#11f705"; ///Light Green
                                                /// Save Connected AMS NetID if it's new
                String route = AdsConn.Instance.NetId;
                bool isKnownRoute = false;
                for (int i = 0; i < AmsRoutes.Count; i++)
                {
                    if (AmsRoutes[i].NetId.Equals(route))
                    {
                        isKnownRoute = true;
                        i = AmsRoutes.Count;
                    }
                }
                if (!isKnownRoute)
                {
                    AmsRoutes.Add(new AmsNetId(route));
                    ConfigFile.SaveFile_AmsRoutes(AmsRoutes);
                }

                /// Publish that on connected subtask is completed
                Publisher.OnConnectSubTaskDone();
                BtnConnectIsEnabled     = false;
                BtnDisconnectIsEnabled  = true;
                _firstRun = true;
            }
        }

        #endregion

        #region Methods
        public void GetUsedAmsRoutes()
        {
            ObservableCollection<AmsNetId> collection = ConfigFile.ReadFile_RoutesConfig();
            if(collection != null)
            {
                AmsRoutes = collection;
            }
        }
        #endregion

    }///Class
}
