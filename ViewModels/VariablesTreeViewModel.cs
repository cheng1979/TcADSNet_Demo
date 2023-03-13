using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcADSNet_Demo.Model;
using TcADSNet_Demo.Views;
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

        private ObservableCollection<Symbol> _selectedSymbols;
        public ObservableCollection<Symbol> SelectedSymbols
        {
            get { return _selectedSymbols; }
            set { SetProperty(ref _selectedSymbols, value); }
        }



        public VariablesTreeViewModel()
        {
            AdsConn.evAdsConnected      += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected   += AdsConn_evAdsDisconnected;
            VariablesTree.evAddSymbolToSelectedSymbols += VariablesTree_evAddSymbolToSelectedSymbols;

            SelectedSymbols = new ObservableCollection<Symbol>();
            GetPlcSymbols();
        }

        #region Event Listeners
        private void VariablesTree_evAddSymbolToSelectedSymbols(object sender, Symbol e)
        {
            AddToSelectedSymbols(e);
        }
                
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

        public void AddToSelectedSymbols(Symbol symbol)
        {
            SelectedSymbols.Add(symbol);
        }

        public bool RemoveFromSelectedSymbols(Symbol symbol)
        {
            int index = 0;
            bool itemFound = false;
            
            for (int i = 0; i < SelectedSymbols.Count; i++)
            {
                if (SelectedSymbols[i].InstancePath.Equals(symbol.InstancePath))
                {
                    itemFound = true;
                    index = i;
                    i = SelectedSymbols.Count; ///Force stop loop
                }
            }
            ///Remove element if found in collection
            if (itemFound) SelectedSymbols.RemoveAt(index);

            return itemFound;
        }


        }///Class
    }
