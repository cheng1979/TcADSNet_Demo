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

        private void BtnItemIOView_Click(object sender, RoutedEventArgs e)
        {
            /// Activate IO View
            var view = MyRegions.MainRegion.GetView("IO");
            MyRegions.MainRegion.Activate(view);
        }
    }
}
