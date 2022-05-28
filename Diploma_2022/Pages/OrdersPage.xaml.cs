using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.ObjectModel;
using Diploma_2022.Models;
using Diploma_2022.Add;
using System.Collections.Generic;
using System.Linq;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public string FIO;
        DataTable dt = new DataTable("diploma_db");
        ObservableCollection<Orders> orders = new ObservableCollection<Orders>();

        public OrdersPage(string fIO_work)
        {
            InitializeComponent();
            OrdersDataGrid_SelectionChanged();
            orders = new ObservableCollection<Orders>();
            FIO = fIO_work;
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter ord = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            ord.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            object item = OrdersGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                DataRowView drv = (DataRowView)OrdersGrid.SelectedItem; // проверка на нахождение заказа в  браке
                string ID_Orders = drv.Row[0].ToString();
                var selectbrak = "SELECT COUNT(*) FROM [dbo].[defect_product] WHERE id_order=@id";
                SqlCommand sqlCommand1 = new SqlCommand(selectbrak, sqlConnection);
                sqlCommand1.Parameters.AddWithValue("@id", ID_Orders);
                int count1 = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                sqlConnection.Close();

                sqlConnection.Open(); // проверка на повторную отправку в упаковку
                var select = "SELECT COUNT(*) FROM [dbo].[package] WHERE id_order=@id";
                SqlCommand sqlCommand = new SqlCommand(select, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", ID_Orders);
                int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                
                if (count1 == 1) // проверка на нахождение заказа в  браке
                {
                    MessageBox.Show("Данный заказ находится в браке или не пройдена аттестация!", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                    update();
                }
                else if (count == 0) // проверка на повторную отправку в упаковку
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[package] (id_order) ((SELECT id_order FROM orders WHERE id_order=@id))", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", ID_Orders);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Заказ успешно отправлен в упаковку!", "Severstal Infocom");
                    sqlConnection.Close();
                    update();
                }
                else
                {
                 MessageBox.Show("Данный заказ уже был отправлен в упаковку!", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                 update();
                }
             }
         }

        private void brakButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            object item = OrdersGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new ReasonForSendingDefectProduct(Convert.ToInt32(ID),(FIO));
                DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                string ID_Orders = drv.Row[0].ToString();
                var select = "SELECT COUNT(*) FROM [dbo].[defect_product] WHERE id_order=@id";
                SqlCommand sqlCommand = new SqlCommand(select, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", ID_Orders);
                int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                if (count == 0)
                {
                    window.ShowDialog();
                    Show();
                }
                else
                {
                    MessageBox.Show("Данный заказ уже находится в браке", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        protected void update()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter ord = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            ord.Fill(dt);
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
                var window = new AddOrder(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
        }

        private void AddButton_attestation(object sender, RoutedEventArgs e)
        {
            object items = OrdersGrid.SelectedItem;
            if (items == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                object item = OrdersGrid.SelectedItem;
                string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new AddAttestationToOrder(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
        }
        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[orders] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter order = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("package");
                order.Fill(dt);
                OrdersGrid.ItemsSource = dt.DefaultView;
                order.Update(dt);
                cmds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не найдено в системе.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    }