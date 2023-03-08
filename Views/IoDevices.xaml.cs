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
using Prism.Ioc;
using Prism.Regions;

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for IODevices.xaml
    /// </summary>
    public partial class IoDevices : UserControl
    {
        public IoDevices()
        {
            InitializeComponent();

            this.Loaded += IODevices_Loaded;
        }

        private void IODevices_Loaded(object sender, RoutedEventArgs e)
        {
            //IRegionManager _regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            /// load sub-content regions : IoDevicesRegion
            IRegionManager regionManager = Menu._regionManager; ///Get region manager form Menu Class
            IContainerExtension container = Menu._container;    ///Get container extension form Menu Class
            /// Region IoDevicesContent is add in XAML (IODevices.xaml)
            MyRegions.IoDevicesRegion = regionManager.Regions[RegionsName.IoDevicesContent];
            var hasView = MyRegions.IoDevicesRegion.GetView(RegionsName.VariablesTree);
            if (hasView == null) MyRegions.IoDevicesRegion.Add(container.Resolve<VariablesTree>(), RegionsName.VariablesTree);
            
        }
    }///Class
}
