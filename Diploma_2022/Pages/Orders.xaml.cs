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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        string connectionString;
       SqlDataAdapter orders;
         DataTable OrdersG;


        public Orders()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
            sqlConnection.Open();
            string cmd = "SELECT * FROM orders"; // orders
            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter Orders = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("orders"); // name table не хватает usingов "orders"
            Orders.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView; // out
            sqlConnection.Close();

        } 

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }



        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(orders);
            orders.Update(OrdersG);
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(orders);
            orders.Update(OrdersG);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItems != null)
            {
                for (int i = 0; i < OrdersGrid.SelectedItems.Count; i++) //FK_orders_type_product
                {
                    DataRowView datarowView = OrdersGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
        }
    }
}
