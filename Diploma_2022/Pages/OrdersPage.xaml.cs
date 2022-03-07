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
        string connectionString;
        //SqlDataAdapter orders;
        //  DataTable OrdersG;
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

            SqlDataAdapter orders = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            orders.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

        } 

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Buttontoshipment(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                string orders = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT shipment ON  INSERT INTO shipment(id_shipment, consignee, date_of_shipments) SELECT id_order, SAP_product_code, date_of_delivery FROM orders WHERE id_order=@id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", orders);
                cmd.ExecuteNonQuery();
                OrdersDataGrid_SelectionChanged();
                MessageBox.Show("Заявка успешно отправлена в отгрузку!", "Severstal Infocom");
            }

        }
    }
}
