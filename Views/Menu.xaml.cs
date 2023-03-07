using Prism.Ioc;
using Prism.Regions;
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
using System.Windows.Shapes;
using TcADSNet_Demo.Model;

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private IContainerExtension _container;
        private IRegionManager _regionManager;
        public static EventHandler evStartClientRead;

        public Menu(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();

            _container = container;
            _regionManager = regionManager;

            this.Loaded += Menu_Loaded;
            this.Closing += Menu_Closing;

            ///// Initial region view
            //var view = MyRegions.MainRegion.GetView("MenuItems");
            //MyRegions.MainRegion.Activate(view);
        }

        
        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            MyRegions.MainRegion = _regionManager.Regions["BodyContent"];
            /// first region added will be the one shown on App loaded.
            MyRegions.MainRegion.Add(_container.Resolve<MenuItems>(),   "MenuItems");
            MyRegions.MainRegion.Add(_container.Resolve<MoveDemo>(),    "MoveDemo");
            MyRegions.MainRegion.Add(_container.Resolve<IO>(),          "IO");

            /// Pre fill TextBox Values
            TxtBox_NetId.Text = "5.59.242.176.1.1";
            TxtBox_Port.Text  = "851";
        }

        private void Menu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /// implement ADS connection dispose before closing the wpf form.
        }


        private void BtnMenuItems_Click(object sender, RoutedEventArgs e)
        {
            /// Change region view to MenuItems
            var view = MyRegions.MainRegion.GetView("MenuItems");
            MyRegions.MainRegion.Activate(view);
        }

        private void BtnMoveDemo_Click(object sender, RoutedEventArgs e)
        {
            /// Change region view to MoveDemo
            var view = MyRegions.MainRegion.GetView("MoveDemo");
            MyRegions.MainRegion.Activate(view);
        }

        private void BtnAdsConnect_Click(object sender, RoutedEventArgs e)
        {
            AdsConn adsConn = AdsConn.Instance;
            adsConn.NetId   = TxtBox_NetId.Text;
            adsConn.AdsPort = TxtBox_Port.Text;
            adsConn.Connect();
            /// Rise event to read client variables
            evStartClientRead?.Invoke(this, EventArgs.Empty);
        }

        private void BtnAdsDesconnect_Click(object sender, RoutedEventArgs e)
        {
            if (AdsConn.Instance.IsConnected) AdsConn.Instance.Disconnect();

        }
    }
}
