﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads.TypeSystem;
using Newtonsoft.Json;

namespace TcADSNet_Demo.Model
{
    public class SymbolsCollection
    {
        [JsonIgnore]
        public ObservableCollection<MySymbol> Collection { get; set; }

        public SymbolsCollection()
        {
            Collection = new ObservableCollection<MySymbol>();
        }

        public SymbolsCollection(ObservableCollection<MySymbol> mySymbols)
        {
            Collection = mySymbols;
        }

        public SymbolsCollection(ObservableCollection<Symbol> symbols)
        {
            ConvertTwincatSymbolToMySymbol_Collection(symbols);
        }


        public void SetCollection(ObservableCollection<MySymbol> mySymbols)
        {
            Collection = mySymbols;
        }

        public void ConvertTwincatSymbolToMySymbol_Collection(ObservableCollection<Symbol> symbols)
        {
            ///symbols collection refers to only one symbol without subsymbols, although the symbol type has the property subsymbol and other self references properties.
            Collection = new ObservableCollection<MySymbol>();
            foreach (Symbol item in symbols)
            {
                Collection.Add(new MySymbol(item));
            }
        }

    }///Class
}
