using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;
using System.IO;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using TwinCAT.Ads.ValueAccess;

namespace TcADSNet_Demo.Model
{
    public class AdsConn
    {
        public static EventHandler evAdsConnected;
        public static EventHandler evAdsDisconnected;
        public static EventHandler evAdsIsDisconnecting;


        #region Variables
        private TcAdsClient _client;
        public TcAdsClient Client
        {
            get { return _client; }
            set { _client = value; }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

        private String _netId;
        public String NetId
        {
            get { return _netId; }
            set { _netId = value; }
        }

        private String _adsPort;
        public String AdsPort
        {
            get { return _adsPort; }
            set { _adsPort = value; }
        }


        private List<String> _connectionAssociation;

        public List<String> ConnectionAssociation
        {
            get { return _connectionAssociation; }
            set { _connectionAssociation = value; }
        }



        private static AdsConn _instance;
        public static AdsConn Instance
        {
            get {
                if (_instance == null) _instance = new AdsConn();
                return _instance; 
            }
            private set { _instance = value; }
        }

        
        #endregion


        public AdsConn()
        {
            Client = new TcAdsClient();
            ConnectionAssociation = new List<String>();
        }
        public AdsConn(String netid, String port)
        {
            this._netId = netid;
            this._adsPort = port;
        }

        public void Connect()
        {
            try
            {
                int port = -1;
                if (int.TryParse(AdsPort, out port))
                {
                    AmsAddress amsAddr = new AmsAddress(NetId, port);
                    //Client.Connect("5.59.242.176.1.1", 851); /// CP-WinCE
                    //Client.Connect("41.224.193.92.1.1", 851); /// PC-BSD
                    Client.Connect(amsAddr.NetId, amsAddr.Port);

                    Thread.Sleep(50);
                    if (Client.IsConnected)
                    {
                        _isConnected = true;
                    }
                    else
                    {
                        _isConnected = false;
                    }
                    ///Rise event when connection acquired
                    evAdsConnected?.Invoke(this, EventArgs.Empty);

                    //MessageBox.Show("ADS Connection Status = " + Client.IsConnected.ToString());
                }
                else
                {
                    MessageBox.Show("Incorrect Port. Type an integer value.");
                }
            }
            catch (AdsErrorException ex)
            {
                MessageBox.Show("AdsConnection Exception Error :\n " + ex.Message);
                Client.Dispose();
            }
        }

        public void Disconnect()
        {
            if (Client.IsConnected)
            {
                ///Rise event IsDisconnecting to tell tasks to stop using Client's Connection
                evAdsIsDisconnecting?.Invoke(this, EventArgs.Empty);
                ///wait tasks to free connection usage
                while(ConnectionAssociation.Count > 0)
                {
                    Thread.Sleep(10);
                }
                Client.Disconnect();
                IsConnected = false;
                ///Rise event
                evAdsDisconnected?.Invoke(this, EventArgs.Empty);
                
                //MessageBox.Show("Client Disconnected.");
            }
        }

        public ISymbolLoader GetSymbol()
        {
            //SymbolLoaderSettings symbSettings_flat = new SymbolLoaderSettings(TwinCAT.SymbolsLoadMode.Flat, ValueAccessMode.IndexGroupOffsetPreferred);
            SymbolLoaderSettings symbSettings_tree = new SymbolLoaderSettings(TwinCAT.SymbolsLoadMode.VirtualTree, ValueAccessMode.IndexGroupOffsetPreferred);

            ISymbolLoader symbLoader = SymbolLoaderFactory.Create(Client, symbSettings_tree);

            #region For Debug
            //Console.WriteLine("Start Interate {0} Symbols:\n", symbLoader.Symbols.Count);
            //foreach (ISymbol item in symbLoader.Symbols)
            //{
            //    Console.WriteLine(item.InstancePath);
            //}
            #endregion

            return symbLoader;
        }

        public void SignInToConnectionAssociation(String name)
        {
            ConnectionAssociation.Add(name);
        }

        public void SignOutFromConnectionAssociation(String name)
        {
            if (!ConnectionAssociation.Remove(name))
            {
                ///Message if Remove from list fails. Item not found.
                var response = MessageBox.Show("Fail to Sign Out "+name+" From ConnectionAssociation List!\nForce to Sign Out?","Ads Connection",
                                MessageBoxButtons.YesNo);
                ///Force Sign Out by creating new empty list
                if (response == DialogResult.Yes)
                {
                    ConnectionAssociation = new List<string>();
                    MessageBox.Show("Forced Signed Out From ConnectionAssociation", "Ads Connection");
                }
            }
        }


    }///Class
}
