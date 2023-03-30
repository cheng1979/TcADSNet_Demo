using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static IContainerExtension _container;
        public static IRegionManager _regionManager;
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


        #region Events
        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            MyRegions.MainRegion = _regionManager.Regions[RegionsName.BodyContent];
            /// first region added will be the one shown on App loaded.
            MyRegions.MainRegion.Add(_container.Resolve<MenuItems>(),   RegionsName.MenuItems);
            MyRegions.MainRegion.Add(_container.Resolve<MoveDemo>(),    RegionsName.MoveDemo);
            //MyRegions.MainRegion.Add(_container.Resolve<Variables>(),    RegionsName.Variables);
            MyRegions.MainRegion.Add(_container.Resolve<IoDevices>(),    RegionsName.IoDevices);


            ///// load sub-content regions : IoDevicesRegion
            ////_regionManager.Regions.Add(MyRegions.IoDevicesRegion);
            //MyRegions.IoDevicesRegion = _regionManager.Regions[RegionsName.IoDevicesContent];
            //MyRegions.IoDevicesRegion.Add(_container.Resolve<Variables>(), RegionsName.Variables);

            /// Pre fill TextBox Values
            TxtBox_NetId.Text = "5.59.242.176.1.1";
            TxtBox_Port.Text  = "851";
            Publisher.Publish("Ready");
            
        }
                
        private void Menu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /// implement ADS connection dispose before closing the wpf form.

            /// If has connection activate Disconnect
            if (AdsConn.Instance.Client.IsConnected)
            {
                BtnAdsDisconnect_Click(null, null);
                Thread.Sleep(3000);
            }
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
            if (AdsConn.Instance.Client.IsConnected || cbBox_NetId.Text == null || cbBox_NetId.Text.Equals("")) return;
            AdsConn adsConn = AdsConn.Instance;
            //adsConn.NetId   = TxtBox_NetId.Text;
            adsConn.NetId   = cbBox_NetId.Text;
            adsConn.AdsPort = TxtBox_Port.Text;
            adsConn.Connect();
            /// Rise event to read client variables
            evStartClientRead?.Invoke(this, EventArgs.Empty);
        }

        private void BtnAdsDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (AdsConn.Instance.IsConnected) AdsConn.Instance.Disconnect();

        }

        private void BtnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void GridFirstRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /// Drag move window
            this.DragMove();
        }

        private void StackPanelWindowsControlButtons_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /// Drag move window
            this.DragMove();
        }
        #endregion

        #region Methods
        private void cbBox_NetId_DropDownClosed(object sender, EventArgs e)
        {
            if (cbBox_NetId.SelectedItem != null)
            {
                AmsNetId item = (AmsNetId)cbBox_NetId.SelectedItem;
                cbBox_NetId.Text = item.NetId;
            }
        }

        #endregion

    }/// Class
}
