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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");

        public OrdersPage()
        {
            InitializeComponent();
            OrdersDataGrid_SelectionChanged();

        }

        private void OrdersDataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter order = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            order.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void Buttontoshipment(object sender, RoutedEventArgs e) //после оформления заявки и отправки в доставку - её нет - сделать
        {
                if (OrdersGrid.SelectedItems.Count > 0)
                {

                 DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                 string ID_Orders = drv.Row[0].ToString();
                 SqlConnection con = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
                 con.Open();
                 SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[shipment](id_shipment, consignee, date_of_shipments) SELECT id_order, SAP_product_code, date_of_adoption FROM orders WHERE id_order=@id", sqlConnection);
                 cmd.Parameters.AddWithValue("@id", ID_Orders);
                 cmd.ExecuteNonQuery();
                 OrdersDataGrid_SelectionChanged();
                 MessageBox.Show("Заявка успешно отправлена в отгрузку!", "Severstal Infocom");
            }
            //catch (Exception ex)
            //{
            //        MessageBox.Show("Данная заявка уже была отправлена в отгрузку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //finally
            //{

            //}
        }
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}