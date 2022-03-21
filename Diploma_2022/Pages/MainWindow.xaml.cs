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
using System.Data.SqlClient;

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

        private void Button_Shipment(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Pages.ShipmentPage();
            window.ShowDialog();
            Show();
        }

        private void Button_Storage(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Pages.StoragePage();
            window.ShowDialog();
            Show();
        }

        private void Button_Certificates(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Pages.Certificates();
            window.ShowDialog();
            Show();
        }

        private void Button_order(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Pages.OrdersPage();
            window.ShowDialog();
            Show();
        }

        private void Button_RabMestoMastera(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Windows.AddDelivery();
            window.ShowDialog();
            Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var lg = new Login();
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?"
                , "Sevestal Infocom", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.No:
                    Hide();
                    var window = new MainWindow();
                    MessageBox.Show("Вы в Системе!", "Severstal Infocom");
                    window.ShowDialog();
                    break;

                case MessageBoxResult.Yes:
                    MessageBox.Show("До свидания!", "Severstal Infocom");
                    this.Hide();
                    lg.Show();
                    break;
            }
            }
    }
}
