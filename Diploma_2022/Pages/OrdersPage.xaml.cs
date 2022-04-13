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
        DataTable dt = new DataTable("diploma_db");

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
            order.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
        }

        private void Buttontopack(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersGrid.SelectedItems.Count > 0)
                {
                    sqlConnection.Open();
                    DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                    string ID_Orders = drv.Row[0].ToString();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[package] (id_order) ((SELECT id_order FROM orders WHERE id_order=@id))", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", ID_Orders);
                    cmd.ExecuteNonQuery();
                    OrdersDataGrid_SelectionChanged();
                    MessageBox.Show("Заказ успешно отправлен в упаковку!", "Severstal Infocom");
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Данный заказ уже был отправлен в упаковку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void brakButton_Click(object sender, RoutedEventArgs e)
        {
                var window = new OrdersPage();
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отправить заказ в брак?", "Sevestal Infocom", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        MessageBox.Show("Заказ НЕ был отправлен в брак", "Severstal Infocom");
                        break;
                    case MessageBoxResult.Yes:
                        MessageBox.Show("Заказ отправлен в брак", "Severstal Infocom");
                        this.Hide();
                        DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                        string opder = drv.Row[0].ToString();
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM orders WHERE id_order=@id", sqlConnection);
                        cmd.Parameters.AddWithValue("@id", opder);
                        cmd.ExecuteNonQuery();
                        OrdersDataGrid_SelectionChanged();
                        window.Show();
                        break;
                }
            }

        protected void update()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter order = new SqlDataAdapter(cmd);
            order.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new Add.AddOrder();
            window.ShowDialog();
            Show();
        }

    }
}