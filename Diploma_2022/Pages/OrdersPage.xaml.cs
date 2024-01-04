using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Diploma_2022.Pages
{
    public partial class OrdersPage : Window
    {
        private readonly SqlConnection sqlConnection;
        private int id_order;
        public OrdersPage()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            OrdersDataGrid_SelectionChanged();
        }

        private void OrdersDataGrid_SelectionChanged()
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "SELECT * FROM [dbo].[orders]";
                cmd.Connection = sqlConnection;

                SqlDataAdapter orders = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("diploma_db");
                orders.Fill(dt);
                OrdersGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void Buttontoshipment(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersGrid.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)OrdersGrid.SelectedItem;
                    id_order = Convert.ToInt32(drv.Row["id_order"]);

                    sqlConnection.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [dbo].[shipment] WHERE id_order = @id", sqlConnection);
                    checkCmd.Parameters.AddWithValue("@id", id_order);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        DateTime currentDate = DateTime.Now;

                        SqlCommand updateOrdersCmd = new SqlCommand("UPDATE [dbo].[orders] SET date_of_adoption = @currentDate WHERE id_order = @id", sqlConnection);
                        updateOrdersCmd.Parameters.AddWithValue("@currentDate", currentDate);
                        updateOrdersCmd.Parameters.AddWithValue("@id", id_order);
                        updateOrdersCmd.ExecuteNonQuery();

                        SqlCommand insertShipmentCmd = new SqlCommand("INSERT INTO [dbo].[shipment] (id_order, consignee) " +
                            "SELECT id_order, name_consignee  FROM orders WHERE id_order=@id", sqlConnection);
                        insertShipmentCmd.Parameters.AddWithValue("@id", id_order);
                        insertShipmentCmd.ExecuteNonQuery();

                        OrdersDataGrid_SelectionChanged();
                        MessageBox.Show("Заявка успешно отправлена в отгрузку!", "Severstal Infocom");
                    }
                    else
                    {
                        MessageBox.Show("Данная заявка уже отправлена в отгрузку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказ.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}