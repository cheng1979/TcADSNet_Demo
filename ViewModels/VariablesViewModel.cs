﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;
using TcADSNet_Demo.Model;
using TcADSNet_Demo.Views;
using TwinCAT.Ads.SumCommand;
using TwinCAT.TypeSystem;
//using TwinCAT.Ads;

namespace TcADSNet_Demo.ViewModels
{
    public class VariablesViewModel : BindableBase
    {
        //public delegate void delUpdateDataGridSource(Object[] obj, SymbolsInfo sym, SymbolsCollection sbCol);

        #region Variables
        private bool _endThreadReadAny { get; set; }
        private bool _endThreadReadBlockAny { get; set; }


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

        private ObservableCollection<MySymbol> _readSymbolsCollection;
        public ObservableCollection<MySymbol> ReadSymbolsCollection
        {
            get 
            {
                if (_readSymbolsCollection == null) _readSymbolsCollection = new ObservableCollection<MySymbol>();
                return _readSymbolsCollection;
            }
            set { SetProperty(ref _readSymbolsCollection, value); }
        }



        #endregion

        public VariablesViewModel()
        {
            Variables.evAdsDebugClicked  += IO_evAdsDebugClicked;
            Menu.evStartClientRead       += Menu_evStartClientRead;
            AdsConn.evAdsIsDisconnecting += AdsConn_evAdsIsDisconnecting;


        }


        #region Events Listener
        private void AdsConn_evAdsIsDisconnecting(object sender, EventArgs e)
        {
            ///ADS is about to disconnect
            ///Sign out from Ads Connection usage control list and end the Thread
            //AdsConn.Instance.SignOutFromConnectionAssociation("Thread_ContinuousReadAny");
            _endThreadReadAny = true;
            _endThreadReadBlockAny = true;
        }

        private void IO_evAdsDebugClicked(object sender, EventArgs e)
        {
            ReadValueOnce();

            //Console.WriteLine("Connection Association Count = " + AdsConn.Instance.ConnectionAssociation.Count);
        }

        private void Menu_evStartClientRead(object sender, EventArgs e)
        {
            #region Debug
            //Thread readThr = new Thread(ContinuousReadAny);
            //Thread.Sleep(1000);
            //Console.WriteLine("Start Read Client Variables Thread");
            //readThr.Start();
            #endregion

            /// *****NEED DELEGATE*****
            /// 
            Thread readBlockThr = new Thread(ContinuousReadBlockAny);
            Thread.Sleep(1000);
            Console.WriteLine("Start Read Block Client Variables Thread");
            readBlockThr.Start();
        }
        #endregion

        public void ReadValueOnce()
        {
            if (AdsConn.Instance.IsConnected)
            {
                uint[] handles = new uint[] { };
                try
                {
                    /// Get Instance Path List and Instance Type List
                    SymbolsInfo symbolsInfo = GetInstancePathListFromSymbolsPoll();
                    String[] instancePathList = symbolsInfo.InstancePathArray;
                    /// Create Sum Handles
                    SumCreateHandles sumHandles = new SumCreateHandles(AdsConn.Instance.Client, instancePathList);
                    /// Handles and Value Types
                    handles = sumHandles.CreateHandles();
                    Type[] valueTypes = symbolsInfo.TypesArray;
                    /// Read Command
                    SumHandleRead readCommand = new SumHandleRead(AdsConn.Instance.Client, handles, valueTypes);
                    /// Read Values
                    Object[] readValues = readCommand.Read();

                    Console.WriteLine();
                    for (int i = 0; i < instancePathList.Length; i++)
                    {
                        Console.WriteLine("Symbol: {0} (Value: {1}, Type: {2} | {3})", instancePathList[i], readValues[i].ToString(), valueTypes[i].Name, readValues[i].GetType().Name);
                    }
                }
                catch (TwinCAT.Ads.AdsErrorException ex)
                {
                    Console.WriteLine("ADS Error: " + ex.Message);
                    MessageBox.Show("ADS Read Error!\n"+ex.Message);
                }
                finally
                {
                    /// Delete handles
                    SumReleaseHandles releaseCommand = new SumReleaseHandles(AdsConn.Instance.Client, handles);
                    releaseCommand.ReleaseHandles();
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
                    Thread.Sleep(500); /// set poll time to 500ms
                }
            }
            catch (TwinCAT.Ads.AdsErrorException ex)
            {
                Console.WriteLine("ADS Error: " + ex.Message);
                MessageBox.Show("ADS Read Error!\n" + ex.Message);
            }
            finally
            {
                ///Sign out from Ads Connection usage control list
                AdsConn.Instance.SignOutFromConnectionAssociation("Thread_ContinuousReadAny");
            }
        }

        private void ContinuousReadBlockAny()
        {
            uint[] handles = new uint[] { };
            try
            {
                _endThreadReadBlockAny = false;                                                  ///Loop Control
                AdsConn.Instance.SignInToConnectionAssociation("Thread_ContinuousReadBlockAny"); ///Sign into Ads Connection usage control list
                ReadSymbolsCollection = new ObservableCollection<MySymbol>();                                 ///Clear or create instance of ReadSymbolsCollection
                /// Loop Poll
                while (AdsConn.Instance.Client.IsConnected && !_endThreadReadBlockAny)
                {
                    /// Get Instance Path List and Instance Type List
                    SymbolsInfo symbolsInfo = GetInstancePathListFromSymbolsPoll();
                    String[] instancePathList = symbolsInfo.InstancePathArray;
                    /// Create Sum Handles
                    SumCreateHandles sumHandles = new SumCreateHandles(AdsConn.Instance.Client, instancePathList);
                    /// Handles and Value Types
                    handles = sumHandles.CreateHandles();
                    Type[] valueTypes = symbolsInfo.TypesArray;
                    /// Read Command
                    SumHandleRead readCommand = new SumHandleRead(AdsConn.Instance.Client, handles, valueTypes);
                    /// Read Values
                    Object[] readValues = readCommand.Read();

                    /// Show read values in UI using delegate method
                    /// Use Dispatcher to allow other Thread to manipulate Collection created in UI Thread
                    App.Current.Dispatcher.Invoke((Action)delegate 
                    {
                        SetReadSymbolsCollection(readValues, symbolsInfo, ReadSymbolsCollection);
                    });

                    /// Set Poll Time
                    Thread.Sleep(500);
                }
            }
            catch (TwinCAT.Ads.AdsErrorException ex)
            {
                Console.WriteLine("ADS Error: " + ex.Message);
                MessageBox.Show("ADS Read Error!\n" + ex.Message);
            }
            finally
            {
                /// Delete handles
                SumReleaseHandles releaseCommand = new SumReleaseHandles(AdsConn.Instance.Client, handles);
                releaseCommand.ReleaseHandles();
                ///Sign out from Ads Connection usage control list
                AdsConn.Instance.SignOutFromConnectionAssociation("Thread_ContinuousReadBlockAny");
            }
        }

        private SymbolsInfo GetInstancePathListFromSymbolsPoll()
        {
            SymbolsCollection symbolsPoll = VariablesTreeViewModel.StaticSymbolsPoll;
            SymbolsInfo symbolsInfo = new SymbolsInfo();
            ObservableCollection<MySymbol> collection;
            if (symbolsPoll != null)
            {
                collection = symbolsPoll.Collection;
                symbolsInfo.InstancePathArray = new string[collection.Count];
                symbolsInfo.TypesArray        = new Type[collection.Count];
                for (int i = 0; i < collection.Count; i++)
                {
                    symbolsInfo.InstancePathArray[i] = collection[i].Path;
                    /// Set TypesArray
                    /// Not contemplating Array type
                    if (collection[i].Type.IndexOf("string", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        /// Is String Type
                        symbolsInfo.TypesArray[i] = typeof(String);
                    }
                    else
                    {
                        switch (collection[i].Type)
                        {
                            case "BOOL":
                                symbolsInfo.TypesArray[i] = typeof(bool);
                                break;
                            case "BYTE":
                                symbolsInfo.TypesArray[i] = typeof(byte);
                                break;
                            case "WORD":
                                symbolsInfo.TypesArray[i] = typeof(ushort);
                                break;
                            case "DWORD":
                                symbolsInfo.TypesArray[i] = typeof(uint);
                                break;
                            case "SINT":
                                symbolsInfo.TypesArray[i] = typeof(sbyte);
                                break;
                            case "INT":
                                symbolsInfo.TypesArray[i] = typeof(short);
                                break;
                            case "DINT":
                                symbolsInfo.TypesArray[i] = typeof(int);
                                break;
                            case "LINT":
                                symbolsInfo.TypesArray[i] = typeof(long);
                                break;
                            case "UINT":
                                symbolsInfo.TypesArray[i] = typeof(ushort);
                                break;
                            case "UDINT":
                                symbolsInfo.TypesArray[i] = typeof(uint);
                                break;
                            case "ULINT":
                                symbolsInfo.TypesArray[i] = typeof(ulong);
                                break;
                            case "REAL":
                                symbolsInfo.TypesArray[i] = typeof(float);
                                break;
                            case "LREAL":
                                symbolsInfo.TypesArray[i] = typeof(double);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return symbolsInfo;
        }

        
        public static void SetReadSymbolsCollection(Object[] objectsRead, SymbolsInfo sbInfo, ObservableCollection<MySymbol> sbCol)
        {
            sbCol.Clear(); ///Clear collection
            for (int i = 0; i < objectsRead.Length; i++)
            {
                MySymbol mySymbol = new MySymbol();
                mySymbol.Name   = sbInfo.InstancePathArray[i].Substring(sbInfo.InstancePathArray[i].IndexOf('.')+1);
                mySymbol.Path   = sbInfo.InstancePathArray[i];
                mySymbol.Type   = sbInfo.TypesArray[i].Name;
                mySymbol.Value  = objectsRead[i];
                sbCol.Add(mySymbol);
                //Console.WriteLine("Symbol Name: {0}, Path: {1}, Type: {2}, Value: {3}",
                //                  mySymbol.Name, mySymbol.Path, mySymbol.Type, mySymbol.Value.ToString());
            }
            
        }

    }/// Class
}
