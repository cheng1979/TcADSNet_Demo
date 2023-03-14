﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TcADSNet_Demo.Model;
using TwinCAT.Ads.TypeSystem;

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for VariablesTree.xaml
    /// </summary>
    public partial class VariablesTree : UserControl
    {
        public static EventHandler<Symbol> evAddSymbolToSelectedSymbols;
        public static EventHandler evSaveSelectedSymbolsList;

        private Symbol _selectedSymbol { get; set; }


        public VariablesTree()
        {
            InitializeComponent();
            
        }

        private void TreeViewSecondVariables_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Symbol selection = (Symbol)TreeViewSecondVariables.SelectedItem;
            /// Rise event to add symbol to SelectedSymbols
            evAddSymbolToSelectedSymbols?.Invoke(this, selection);
        }

        private void BtnSaveSelectedSymbols_Click(object sender, RoutedEventArgs e)
        {
            evSaveSelectedSymbolsList?.Invoke(this, EventArgs.Empty);
        }
    }
}
