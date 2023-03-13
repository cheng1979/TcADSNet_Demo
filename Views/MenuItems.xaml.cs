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

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for MenuItems.xaml
    /// </summary>
    public partial class MenuItems : UserControl
    {
        public MenuItems()
        {
            InitializeComponent();
        }

        private void BtnItemVariablesView_Click(object sender, RoutedEventArgs e)
        {
            /// Activate Variables View
            //var view = MyRegions.MainRegion.GetView(RegionsName.Variables);
            //MyRegions.MainRegion.Activate(view);
        }

        private void BtnItemIODevices_Click(object sender, RoutedEventArgs e)
        {
            /// Activate IODevices View
            var view = MyRegions.MainRegion.GetView(RegionsName.IoDevices);
            MyRegions.MainRegion.Activate(view);
        }
    }
}
