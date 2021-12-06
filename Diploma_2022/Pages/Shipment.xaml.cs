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
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;


namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для Shipment.xaml
    /// </summary>
    public partial class Shipment : Window
    {
        //string connectionString;
        //SqlDataAdapter shipment;
        //DataTable shipments;

        public Shipment()
        {
            InitializeComponent();
            Shipment_DataGrid_SelectionChanged();
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void Shipment_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand createCommand = new SqlCommand();
            createCommand.CommandText = "SELECT * FROM [dbo].[storage]";
            createCommand.Connection = sqlConnection;

            SqlDataAdapter Shipment = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("diploma_db"); 
            Shipment.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView; 
        }
    }
}
