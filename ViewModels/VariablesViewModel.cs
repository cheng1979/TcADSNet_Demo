using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;
using TcADSNet_Demo.Model;
using TcADSNet_Demo.Views;
using TwinCAT.TypeSystem;
//using TwinCAT.Ads;

namespace TcADSNet_Demo.ViewModels
{
    public class VariablesViewModel : BindableBase
    {
        #region Variables
        private bool _endThreadReadAny { get; set; }

        private UInt32 _plcIsAlivePulse;
        public UInt32 PlcIsAlivePulse
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

        
        #endregion

        public VariablesViewModel()
        {
            Variables.evAdsDebugClicked += IO_evAdsDebugClicked;
            Menu.evStartClientRead      += Menu_evStartClientRead;
            AdsConn.evAdsIsDisconnecting += AdsConn_evAdsIsDisconnecting;


        }


        #region Events Listener
        private void AdsConn_evAdsIsDisconnecting(object sender, EventArgs e)
        {
            ///ADS is about to disconnect
            ///Sign out from Ads Connection usage control list and end the Thread
            AdsConn.Instance.SignOutFromConnectionAssociation("Thread_ContinuousReadAny");
            _endThreadReadAny = true;
        }

        private void IO_evAdsDebugClicked(object sender, EventArgs e)
        {
            //ReadValueOnce();
            
        }

        private void Menu_evStartClientRead(object sender, EventArgs e)
        {
            Thread readThr = new Thread(ContinuousReadAny);
            Thread.Sleep(1000);
            Console.WriteLine("Start Read Client Variables Thread");
            readThr.Start();
        }
        #endregion

        public void ReadValueOnce()
        {
            if (AdsConn.Instance.IsConnected)
            {
                try
                {
                    Hvar = AdsConn.Instance.Client.CreateVariableHandle("ALIVE.nClockPulse");
                    PlcIsAlivePulse = (UInt32)AdsConn.Instance.Client.ReadAny(Hvar, typeof(UInt32));
                }
                catch (TwinCAT.Ads.AdsErrorException ex)
                {
                    Console.WriteLine("ADS Error: " + ex.Message);
                    MessageBox.Show("ADS Read Error!\n"+ex.Message);
                }
                finally
                {
                    AdsConn.Instance.Client.DeleteVariableHandle(Hvar);
                }
            }
            else
            {
                MessageBox.Show("ADS Client Not Connected");
            }
        }

        private void ContinuousReadAny()
        {
            try
            {
                _endThreadReadAny = false;
                AdsConn.Instance.SignInToConnectionAssociation("Thread_ContinuousReadAny"); ///Sign into Ads Connection usage control list
                while (AdsConn.Instance.Client.IsConnected && !_endThreadReadAny)
                {
                    Hvar = AdsConn.Instance.Client.CreateVariableHandle("ALIVE.nClockPulse");
                    PlcIsAlivePulse = (UInt32)AdsConn.Instance.Client.ReadAny(Hvar, typeof(UInt32));
                    AdsConn.Instance.Client.DeleteVariableHandle(Hvar);
                    Thread.Sleep(500); /// set pool time to 500ms
                }
            }
            catch (TwinCAT.Ads.AdsErrorException ex)
            {
                Console.WriteLine("ADS Error: " + ex.Message);
                MessageBox.Show("ADS Read Error!\n" + ex.Message);
                ///Sign out from Ads Connection usage control list
                AdsConn.Instance.SignOutFromConnectionAssociation("Thread_ContinuousReadAny");
            }
            finally
            {
                
            }
        }

        


    }/// Class
}
