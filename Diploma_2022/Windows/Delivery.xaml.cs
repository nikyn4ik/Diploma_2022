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

namespace Diploma_2022.Windows
{ 
    public partial class Delivery : Window
    {
        public Delivery()
        {
            InitializeComponent();
            SqlDataAdapter adpt;
            DataTable dt;
        }
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        //private void Button_add(object sender, RoutedEventArgs e)
        //{
        //    SqlConnection sqlConnection = new SqlConnection();
        //    sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
        //    {
        //        sqlConnection.Open();
        //        String query = "INSERT INTO shipment values(@early_shipment,@consignee,@date_of_shipments,@shipment_total_amount_tons,@number_of_shipments_per_month_tons); ";
        //        SqlCommand createCommand = new SqlCommand(query, sqlConnection);
        //        createCommand.Parameters.AddWithValue("@early_shipment", early_shipment.Text);
        //        createCommand.Parameters.AddWithValue("@consignee", consignee.Text);
        //        createCommand.Parameters.AddWithValue("@date_of_shipments", date_of_shipments.Text);
        //        createCommand.Parameters.AddWithValue("@shipment_total_amount_tons", shipment_total_amount_tons.Text);
        //        createCommand.Parameters.AddWithValue("@number_of_shipments_per_month_tons", number_of_shipments_per_month_tons.Text);
        //        createCommand.ExecuteNonQuery();
        //        MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
        //        sqlConnection.Close();
        //        showdata();

        //    }
        //}
        public void showdata()
        {
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[delivery]", sqlConnection);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            DeliveryGrid.DataContext = dt;
            DeliveryGrid.ItemsSource = dt.DefaultView;
        }

        private void product_standart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT product_standart FROM confirmation", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                product_standart.Items.Add(db.GetValue(0));
            }
        }

        private void Storage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT name_storage FROM delivery", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                Storage.Items.Add(db.GetValue(0));
            }
        }

        private void DateDelivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT date_of_delivery FROM delivery", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                DateDelivery.Items.Add(db.GetValue(0));
            }
        }

        private void Done_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT done_delivery FROM confirmation", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                Done.Items.Add(db.GetValue(0));
            }
        }


    }
}