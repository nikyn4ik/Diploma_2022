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
    /// Логика взаимодействия для AddShipment.xaml
    /// </summary>
    public partial class AddShipment : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        public AddShipment()
        {
            InitializeComponent();
            checkorder();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                sqlConnection.Open();
                string query = "UPDATE [dbo].[shipment] SET shipment_total_amount_tons=@shipment_total_amount_tons, number_of_shipments_per_month_tons=@number_of_shipments_per_month_tons, date_of_shipments=@date_of_shipments WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", id_ord.Text);
                createCommand.Parameters.AddWithValue("@shipment_total_amount_tons", shipment_total_amount_tons.Text);
                createCommand.Parameters.AddWithValue("@number_of_shipments_per_month_tons", number_of_shipments_per_month_tons.Text);
                createCommand.Parameters.AddWithValue("@date_of_shipments", Convert.ToDateTime(date_of_shipments.Text));
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
                this.Close();
            }
        }

        private void date_of_shipments_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_of_shipments = (DateTime)this.DatePicker.SelectedDate;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkorder();
        }

        private void checkorder()
        {
            SqlCommand cmd = new SqlCommand("SELECT id_order FROM [dbo]. shipment", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                id_ord.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }
    }
}
