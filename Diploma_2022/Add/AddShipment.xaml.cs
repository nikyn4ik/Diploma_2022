using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        int IdOrder;
        public AddShipment(int idOrder)
        {
            InitializeComponent();
            storageSelect();
            transportSelect();
            IdOrder = idOrder;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (shipment_total_amount_tons.Text != "" && storage.Text != "" && date_of_shipments.Text != "" && transport.Text != "")
            {
                query = "UPDATE [dbo].[shipment] SET shipment_total_amount_tons=@shipment_total_amount_tons, id_storage=@id_storage, id_transport=@id_transport, date_of_shipments=@date_of_shipments WHERE id_order=@id";//
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@shipment_total_amount_tons", shipment_total_amount_tons.Text);
                createCommand.Parameters.AddWithValue("@id_storage", storage.Text);
                createCommand.Parameters.AddWithValue("@id_transport", transport.Text);
                createCommand.Parameters.AddWithValue("@date_of_shipments", Convert.ToDateTime(date_of_shipments.Text));
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
            storageSelect();
        }

        private void storageSelect()
        {
            SqlCommand cmd = new SqlCommand("SELECT name_storage, address FROM [dbo].[storage]", sqlConnection);
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            //cmd.CommandType = CommandType.Text;
            //db = cmd.ExecuteReader();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                storage.Items.Add(ds.Tables[0].Rows[i][0] + " | " + ds.Tables[0].Rows[i][1]);
            }
            sqlConnection.Close();
        }

        private void transportSelect()
        {
            SqlCommand cmd = new SqlCommand("SELECT name_transport, railway_number_transport FROM [dbo].[transport]", sqlConnection);
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                transport.Items.Add(ds.Tables[0].Rows[i][0] + " | " + ds.Tables[0].Rows[i][1]);
            }
            sqlConnection.Close();
        }

        private void transport_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            transportSelect();
        }
    }
}
