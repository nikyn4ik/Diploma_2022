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

namespace Diploma_2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_ClickOut(object sender, RoutedEventArgs e)
        {
            var lg = new Login();
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit ?"
               // +  "\"Account\"?" - create reference to login.Text
                , "Sevestal Infocom", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.No:
                    MessageBox.Show("Welcome again!", "Sevestal Infocom");
                    break;
                case MessageBoxResult.Yes:
                    MessageBox.Show("Goodbye!", "Sevestal Infocom");
                    this.Close();
                    lg.Show();
                    break;
                case MessageBoxResult.Cancel:
                    break;

            }
        }

        private void Button_Order(object sender, RoutedEventArgs e)
        {
            Pages.Orders ord = new Pages.Orders(); //<Button Content="Orders" RenderTransformOrigin="0,0.5" Click="Button_Order" Height="33" FontFamily="Times New Roman" FontSize="16"
            ord.Show();
            this.Close();
        }

        private void Button_Shipment(object sender, RoutedEventArgs e)
        {
            Pages.Shipment ord = new Pages.Shipment();
            ord.Show();
            this.Close();
        }

        private void Button_Storage(object sender, RoutedEventArgs e)
        {
            Pages.Storage ord = new Pages.Storage(); //
            ord.Show();
            this.Close();
        }

        private void Button_Certificatet(object sender, RoutedEventArgs e)
        {

        }
    }
}
