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
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True"); //соединение с бд
        public string FIO;
        DataTable dt = new DataTable("diploma_db");
        ObservableCollection<Orders> orders = new ObservableCollection<Orders>();
        int thickness_mm;
        int width_mm;
        int length_mm;
        int IdOrder;
        SqlDataReader db;

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
            SqlCommand cmd1 = new SqlCommand("SELECT thickness_mm, width_mm, length_mm FROM [dbo].[orders] where id_order = @id", sqlConnection);
            sqlConnection.Open();
            object item = OrdersGrid.SelectedItem;
            string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            cmd1.Parameters.AddWithValue("@id", ID.ToString());
            db = cmd1.ExecuteReader();
            while (db.Read())
            {
                thickness_mm = (int)db.GetValue(0);
                width_mm = (int)db.GetValue(1);
                length_mm = (int)db.GetValue(2);
            }
            db.Close();
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[qua_certificate], [dbo].[cert_directory] WHERE [dbo].[qua_certificate].id_qua_certificate = [dbo].[cert_directory].id_qua_certificate", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            bool check = false;
            while (db.Read())
            {
                int min = Convert.ToInt32(db.GetValue(7).ToString());
                int max = Convert.ToInt32(db.GetValue(8).ToString());

                if (thickness_mm > min && thickness_mm < max && width_mm > min && width_mm < max && length_mm > min && length_mm < max)
                {
                    yes();
                    check = true;
                    break;
                    db.Close();
                }
            }
            if (check == false)
            {
                no();
            }
            sqlConnection.Close();
        }

        private void no()
        {
            SqlCommand cmd12 = new SqlCommand ("UPDATE [dbo].[orders] SET access_standart = 'Нет' WHERE  id_order=@id", sqlConnection);
            object item = OrdersGrid.SelectedItem;
            string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            cmd12.Parameters.AddWithValue("@id", ID.ToString());
            MessageBox.Show("Продукт НЕ проходит аттестацию!", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            db.Close();
            cmd12.ExecuteNonQuery();
            update();
        }
        private void yes()
        {
            SqlCommand cmd13 = new SqlCommand ("UPDATE [dbo].[orders] SET access_standart = 'Да' WHERE  id_order=@id", sqlConnection);
            object item = OrdersGrid.SelectedItem;
            string ID = (OrdersGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            cmd13.Parameters.AddWithValue("@id", ID.ToString());
            MessageBox.Show("Продукт проходит аттестацию!", "Severstal Infocom", MessageBoxButton.OK);
            var window = new AddAttestationToOrder(Convert.ToInt32(ID));
            window.ShowDialog();
            db.Close();
            cmd13.ExecuteNonQuery();
            update();
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrdersGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            SqlConnection cmds = new SqlConnection(ConnectionString);
            string cmd = "SELECT * FROM [dbo].[orders] WHERE name_product like '" + pole.Text + "%'";
            cmds.Open();
            SqlCommand sqlcom = new SqlCommand(cmd, cmds);
            SqlDataAdapter order = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable("orders");
            order.Fill(dt);
            OrdersGrid.ItemsSource = dt.DefaultView;
            order.Update(dt);
            cmds.Close();
        }
    }
    }