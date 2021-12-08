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



        //private void UpdateDB()
        //{
        //    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(orders);
        //    orders.Update(OrdersG);
        //}


        //private void deleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (OrdersGrid.SelectedItems != null)
        //    {
        //        for (int i = 0; i < OrdersGrid.SelectedItems.Count; i++) //FK_orders_type_product
        //        {
        //            DataRowView datarowView = OrdersGrid.SelectedItems[i] as DataRowView;
        //            if (datarowView != null)
        //            {
        //                DataRow dataRow = (DataRow)datarowView.Row;
        //                dataRow.Delete();
        //            }
        //        }
        //    }
        //    //UpdateDB();
        //}

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddOrders taskWindow = new Windows.AddOrders();
            taskWindow.Show();
            OrdersDataGrid_SelectionChanged();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
