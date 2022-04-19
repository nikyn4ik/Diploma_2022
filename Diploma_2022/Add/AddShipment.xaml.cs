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

        int IdOrder;
        public AddShipment(int idOrder)
        {
            InitializeComponent();
            storageSelect();
            IdOrder = idOrder;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (shipment_total_amount_tons.Text != "" && storage.Text != "" && date_of_shipments.Text != "")
            {
                query = "UPDATE [dbo].[shipment] SET shipment_total_amount_tons=@shipment_total_amount_tons, id_storage=@id_storage, date_of_shipments=@date_of_shipments WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@shipment_total_amount_tons", shipment_total_amount_tons.Text);
                createCommand.Parameters.AddWithValue("@storage", storage.Text);
                createCommand.Parameters.AddWithValue("@date_of_shipments", Convert.ToDateTime(date_of_shipments.Text));
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                update(createCommand);
            }
            else
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
            }
        }
        private void update(SqlCommand createCommand)
        {
            createCommand.ExecuteNonQuery();
            MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
            sqlConnection.Close();
            this.Close();
        }

        private void date_of_shipments_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_of_shipments = (DateTime)this.DatePicker.SelectedDate;
        }

        private void storage_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT id_storage FROM [dbo].[storage]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                storage.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void storageSelect()
        {
            SqlCommand cmd = new SqlCommand("SELECT id_storage FROM [dbo].[storage]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                storage.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

    }
}
