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

        private static AdsConn _instance;
        public static AdsConn Instance
        {
            get {
                if (_instance == null) _instance = new AdsConn();
                return _instance; 
            }
            private set { _instance = value; }
        }



        public AdsConn()
        {
            Client = new TcAdsClient();
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
                    MessageBox.Show("ADS Connection Status = " + Client.IsConnected.ToString());
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
                Client.Disconnect();
                IsConnected = false;
                MessageBox.Show("Client Disconnected.");
            }
        }

        public void GetSymbol()
        {
            SymbolLoaderSettings symbSettings_flat = new SymbolLoaderSettings(TwinCAT.SymbolsLoadMode.Flat, ValueAccessMode.IndexGroupOffsetPreferred);
            SymbolLoaderSettings symbSettings_tree = new SymbolLoaderSettings(TwinCAT.SymbolsLoadMode.VirtualTree, ValueAccessMode.IndexGroupOffsetPreferred);

            ISymbolLoader symbLoader = SymbolLoaderFactory.Create(Client, symbSettings_tree);

            Console.WriteLine("Start Interate {0} Symbols:\n", symbLoader.Symbols.Count);
            foreach (ISymbol item in symbLoader.Symbols)
            {
                Console.WriteLine(item.InstancePath);
            }
        }

    }///Class
}
