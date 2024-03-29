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

namespace TcADSNet_Demo.Views
{
    /// <summary>
    /// Interaction logic for IO.xaml
    /// </summary>
    public partial class Variables : UserControl
    {
        public static EventHandler evAdsDebugClicked;
        public static EventHandler evAdsWriteToPlc;

        public Variables()
        {
            InitializeComponent();
        }

        private void BtnDebug_Click(object sender, RoutedEventArgs e)
        {
            evAdsDebugClicked?.Invoke(this, EventArgs.Empty);
        }

        private void BtnWriteToPLC_Click(object sender, RoutedEventArgs e)
        {
            evAdsWriteToPlc?.Invoke(this, EventArgs.Empty);
        }

    }///Class
}
