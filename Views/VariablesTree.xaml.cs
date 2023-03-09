using System;
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

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for VariablesTree.xaml
    /// </summary>
    public partial class VariablesTree : UserControl
    {
        public VariablesTree()
        {
            InitializeComponent();
            
        }

        private void TreeViewVariables_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e != null && e.NewValue!= null)
            {
                if (e.NewValue is PlcSymbol)
                {
                    Console.WriteLine("Selected Symbol : " + ((PlcSymbol)e.NewValue).InstancePath);
                }
                if (e.NewValue is PlcMember)
                {
                    Console.WriteLine("Selected Member : " + ((PlcMember)e.NewValue).Name);
                }
            }
        }

    }
}
