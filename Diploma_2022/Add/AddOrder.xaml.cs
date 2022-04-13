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
using System.Globalization;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;

        public AddOrder()
        {
            InitializeComponent();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                sqlConnection.Open();
                string query = "UPDATE [dbo].[orders] SET consignee=@consignee, status=@status_order  WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@consignee", consignee.Text);
                createCommand.Parameters.AddWithValue("@status_order", status.Text);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
                this.Close();
            }
        }


        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            status.Items.Add("Заказ на выполнении");
            status.Items.Add("Заказ выполнен");
        }
    }
}