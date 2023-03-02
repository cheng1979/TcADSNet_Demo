﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;
using System.IO;

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


        public AdsConn()
        {
            try
            {
                
                Client = new TcAdsClient();
                //Tc amsNet = new AmsNetId("5.59.242.176.1.1");
                Client.Connect("5.59.242.176.1.1", 851);
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
            catch (AdsErrorException ex)
            {
                MessageBox.Show("AdsConnection Exception Error :\n " + ex.Message);
                Client.Dispose();
            }
        }

    }///Class
}