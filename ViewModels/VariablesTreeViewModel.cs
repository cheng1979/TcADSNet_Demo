using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private SymbolsCollection _symbolsPoll;
        public SymbolsCollection SymbolsPoll
        {
            get
            {
                if (_symbolsPoll == null) _symbolsPoll = new SymbolsCollection();
                return _symbolsPoll;
            }
            set
            {
                SetProperty(ref _symbolsPoll, value);
                StaticSymbolsPoll = _symbolsPoll;
            }
        }

        public static SymbolsCollection StaticSymbolsPoll { get; private set; }


        public VariablesTreeViewModel()
        {
            AdsConn.evAdsConnected      += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected   += AdsConn_evAdsDisconnected;
            VariablesTree.evAddSymbolToSelectedSymbols += VariablesTree_evAddSymbolToSelectedSymbols;
            VariablesTree.evSaveSelectedSymbolsList += VariablesTree_evSaveSelectedSymbolsList;
            VariablesTree.evDeleteSymbolFromSelectedSymbols += VariablesTree_evDeleteSymbolFromSelectedSymbols;

            SelectedSymbols = new ObservableCollection<Symbol>();
            GetPlcSymbols();
            //UpdadeSymbolsPollFromFile_OnLoad();
            UpdateSelectedSymbolsFromFile_OnLoad();
        }


        #region Event Listeners
        private void VariablesTree_evDeleteSymbolFromSelectedSymbols(object sender, Symbol e)
        {
            RemoveSymbolFromSelectedSymbolsCollection(e);

        }

        private void VariablesTree_evSaveSelectedSymbolsList(object sender, EventArgs e)
        {
            try
            {
                bool fileSaved = false;
                SymbolsCollection symbolsCollection_beforeSave = new SymbolsCollection();
                if (SelectedSymbols != null)
                {
                    ///Convert Twincat Symbol to MySymbol type to allow JsonConvert without "Self Referencing Loop" issue
                    symbolsCollection_beforeSave.ConvertTwincatSymbolToMySymbol_Collection(SelectedSymbols);
                    fileSaved = ConfigFile.SaveFile_SymbolsPool(symbolsCollection_beforeSave);

                }
                if (fileSaved)
                {
                    /// Update Symbols Poll if save file successfully
                    SymbolsPoll = symbolsCollection_beforeSave;
                    //MessageBox.Show("Symbols Selection Saved", "SAVE SYMBOLS SELECTION");
                    Publisher.Publish("Symbols Selection Saved");
                }
                else
                {
                    MessageBox.Show("Error on Saving Symbol Selection!", "SAVE ERROR");
                    Publisher.Publish("Error on Saving Symbol Selection!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nExecption Error:\n" + ex.Message);
            }

        }
        
        private void VariablesTree_evAddSymbolToSelectedSymbols(object sender, Symbol e)
        {
            AddToSelectedSymbols(e);
        }
        
        private void AdsConn_evAdsDisconnected(object sender, EventArgs e)
        {
            ///Update TwncatSymbols when ADS Disconnected
            GetPlcSymbols();
            UpdateSelectedSymbolsFromFile();
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            ///Update _membersCollection when ADS Connection Acquired
            GetPlcSymbols();
            UpdadeSymbolsPollFromFile();
            UpdateSelectedSymbolsFromFile();

            /// Publish that on connected subtask is completed
            Publisher.OnConnectSubTaskDone();
        }
        #endregion///Event Listeners


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
            /// Add Symbol that isn't already in the collection and if Symbol doesn't have SubSymbols
            bool noSubSymbol = true;
            bool alreadyInCollection = false;
            /// Verify if symbol has subsymbols
            if (symbol.SubSymbolCount > 0) noSubSymbol = false;
            /// Verify if symbol is already in collection
            for (int i = 0; i < SelectedSymbols.Count; i++)
            {
                if (SelectedSymbols[i].InstancePath.Equals(symbol.InstancePath))
                {
                    alreadyInCollection = true;
                    i = SelectedSymbols.Count; ///End the loop
                }
            }
            
            /// Add to collection if meets requirements
            if(noSubSymbol && !alreadyInCollection)
            {
                SelectedSymbols.Add(symbol);
            }
            
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

        private void UpdadeSymbolsPollFromFile_OnLoad()
        {
            SymbolsPoll = ConfigFile.ReadFile_SymbolsPool();
        }

        private void UpdateSelectedSymbolsFromFile_OnLoad()
        {
            #region Debug
            //long initialTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            #endregion

            if (AdsConn.Instance.Client.IsConnected)
            {
                ObservableCollection<Symbol> newcol = new ObservableCollection<Symbol>();
                foreach (Symbol symb in TwincatSymbols)
                {
                    foreach (MySymbol item in SymbolsPoll.Collection)
                    {
                        FindRecursively(symb, item, ref newcol);
                    }
                }
                SelectedSymbols = newcol;
            }

            #region Debug
            //long finalTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //Console.WriteLine("\nUpdate Selected Symbols Elapsed Time\n" + (finalTime - initialTime)+" ms\n");
            #endregion
        }

        private void UpdadeSymbolsPollFromFile()
        {
            SymbolsPoll = ConfigFile.ReadFile_SymbolsPool();
        }

        private void UpdateSelectedSymbolsFromFile()
        {
            ObservableCollection<Symbol> newcol = new ObservableCollection<Symbol>();
            if (TwincatSymbols != null)
            {
                foreach (Symbol symb in TwincatSymbols)
                {
                    foreach (MySymbol item in SymbolsPoll.Collection)
                    {
                        FindRecursively(symb, item, ref newcol);
                    }
                }
            }
            SelectedSymbols = newcol;
            
        }

        private void FindRecursively(Symbol symbols, MySymbol refSymbol, ref ObservableCollection<Symbol> col)
        {
            //bool isFound = false;
            if (symbols.InstancePath.Equals(refSymbol.Path))
            {
                col.Add(symbols);
                //isFound = true;
            }
            if(symbols.SubSymbolCount > 0)
            {
                foreach (Symbol item in symbols.SubSymbols)
                {
                    FindRecursively(item, refSymbol, ref col);
                }

                #region Using Stop Loop Method - Commented
                /// The method below was slower then the method above in the test with small subsymbols collections
                
                //for (int i = 0; i < symbols.SubSymbols.Count; i++)
                //{
                //    FindRecursively((Symbol)symbols.SubSymbols[i], refSymbol, ref col);
                //    if (isFound) i = symbols.SubSymbols.Count;                          /// Stop the loop
                //}
                #endregion
            }
        }


        private void RemoveSymbolFromSelectedSymbolsCollection(Symbol symbol)
        {
            if(SelectedSymbols != null)
            {
                SelectedSymbols.Remove(symbol);
            }
        }


        }///Class
}
