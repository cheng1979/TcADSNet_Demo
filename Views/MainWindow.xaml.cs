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
using TcADSNet_Demo.ViewModels;
using System.Threading;

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool cmdStart = false;

        public MainWindow()
        {
            InitializeComponent();
            //objPos = new MainWindowViewModel();
        }

        public void Moving(int step)
        {
            MainWindowViewModel.Instance.SetPositionByNStep(step);
        }

       

        private void Btn_Veocity_Click(object sender, RoutedEventArgs e)
        {
            String text = TxBox_Velocity.Text;
            if (!text.Equals("") && text != null)
            {
                //Moving(int.Parse(text));
                MainWindowViewModel.Instance.Velocity = int.Parse(text);
            }
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            Thread thOscilation;
            if (!cmdStart)
            {
                cmdStart = true;
                MainWindowViewModel.Start = true;
                thOscilation = new Thread(new ThreadStart(MainWindowViewModel.Oscilation));
                thOscilation.Start();
            }
            else
            {
                cmdStart = false;
                MainWindowViewModel.Start = false;
            }

        }
    }
}
