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
using System.Collections.ObjectModel;
using Diploma_2022.Models;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");
        ObservableCollection<Orders> orders = new ObservableCollection<Orders>();

        public OrdersPage()
        {
            InitializeComponent();
            OrdersDataGrid_SelectionChanged();
            orders = new ObservableCollection<Orders>();
        }

        private void OrdersDataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            DataTable tap = new DataTable();
            cmd.CommandText = "SELECT * FROM [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter order = new SqlDataAdapter(cmd);
            new SqlDataAdapter(cmd.CommandText, sqlConnection).Fill(tap);
            order.Fill(dt);
            List<int> result = new List<int>();
            result = tap.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("id_order")).ToList();
            orders = new ObservableCollection<Orders>();
            
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
            object item = OrdersGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new Add.ReasonForSendingDefectProduct(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
            }
        }

        protected void update()
        {
            OrdersGrid.Items.Clear();

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
            object item = OrdersGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new Add.AddOrder(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
            }
                
        }

        private void AddButton_cert(object sender, RoutedEventArgs e)
        {
            object items = OrdersGrid.SelectedItem;
            if (items == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
            object item = OrdersGrid.SelectedItem;
            string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            var window = new Add.AddCertifToOrder(Convert.ToInt32(ID));
            window.ShowDialog();
            Show();
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[orders] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter order = new SqlDataAdapter(sqlcom);
                order.Fill(dt);
                OrdersGrid.ItemsSource = dt.DefaultView;
                order.Update(dt);
                cmds.Close();
            }
        }
    }
}