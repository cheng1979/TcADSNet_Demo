using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcADSNet_Demo.Model;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using TwinCAT.TypeSystem.Generic;

namespace TcADSNet_Demo.ViewModels
{
    public class VariablesTreeViewModel : BindableBase
    {
        //private PlcMembersCollection _membersCollection;
        //public PlcMembersCollection MembersCollection
        //{
        //    get { return _membersCollection; }
        //    set { SetProperty(ref _membersCollection, value); }
        //}

        //private ReadOnlyCollection<PlcSymbols> _symbolsCollection;
        //public ReadOnlyCollection<PlcSymbols> SymbolsCollection
        //{
        //    get { return _symbolsCollection; }
        //    set { SetProperty(ref _symbolsCollection, value); }
        //}

        private ReadOnlySymbolCollection _twincatSymbols;
        public ReadOnlySymbolCollection TwincatSymbols
        {
            get { return _twincatSymbols; }
            set { SetProperty(ref _twincatSymbols, value); }
        }




        public VariablesTreeViewModel()
        {
            AdsConn.evAdsConnected      += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected   += AdsConn_evAdsDisconnected;

            GetPlcSymbols();
        }

        #region Event Listener
        private void AdsConn_evAdsDisconnected(object sender, EventArgs e)
        {
            ///Update _membersCollection when ADS Disconnected
            GetPlcSymbols();
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            ///Update _membersCollection when ADS Connection Acquired
            GetPlcSymbols();
        }
        #endregion

        public void GetPlcSymbols()
        {
            #region Old Approuch
            //if (AdsConn.Instance.Client.IsConnected)
            //{
            //    ISymbolLoader symbolsColl = AdsConn.Instance.GetSymbol(); /// get plc symbols
            //    List<PlcMember> memberList = new List<PlcMember>();
            //    ///Prepare memberList
            //    foreach (IVirtualStructInstance item in symbolsColl.Symbols)
            //    {
            //        ///Get member name and symbols collection (MemberInstances)
            //        memberList.Add(new PlcMember(item.InstanceName, item.MemberInstances));
            //    }
            //    ///Assign member collection
            //    MembersCollection = new PlcMembersCollection(memberList);
            //}
            //else
            //{
            //    ///ADS Client not connected
            //    List<PlcMember> emptyList = new List<PlcMember>();
            //    //emptyList.Add(new PlcMember("Empty")); ///Set an empty collection with member name as Empty
            //    ///Assign member collection
            //    MembersCollection = new PlcMembersCollection(emptyList);
            //}
            #endregion

            if (AdsConn.Instance.Client.IsConnected)
            {
                /// set TwincatSymbols
                TwincatSymbols = AdsConn.Instance.GetSymbol().Symbols;
            }
            else
            {
                ///ADS Client not connected
                TwincatSymbols = null;
            }
        }

        #region Recursive Method - Commented
        //public void GetPlcSymbols_Recursively()
        //{
        //    List<PlcSymbols> symbolsList = new List<PlcSymbols>();
        //    if (AdsConn.Instance.Client.IsConnected)
        //    {
        //        ISymbolLoader symbolsColl = AdsConn.Instance.GetSymbol(); /// get plc symbols
        //        TwincatSymbols = symbolsColl.Symbols;                     /// set TwincatSymbols

        //        foreach (ISymbol item in symbolsColl.Symbols)
        //        {
        //            symbolsList.Add(CreateTreeNode(item));
        //        }
        //    }

        //    ///Assign member collection
        //    SymbolsCollection = new ReadOnlyCollection<PlcSymbols>(symbolsList);
        //}
        
        //private PlcSymbols CreateTreeNode(ISymbol symbol)
        //{
        //    PlcSymbols _symbol = new PlcSymbols(symbol);
        //    _symbol.SubSymbols = new List<PlcSymbols>();
        //    foreach (ISymbol subSymbol in symbol.SubSymbols)
        //    {
        //        _symbol.SubSymbols.Add(CreateTreeNode(subSymbol));
        //    }
            
        //    return _symbol;
        //}
        #endregion

        }///Class
    }
