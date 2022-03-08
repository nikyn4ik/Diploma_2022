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
    /// Логика взаимодействия для ShipmentPage.xaml
    /// </summary>
    public partial class ShipmentPage : Window
    {
        //string connectionString;
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");

        public ShipmentPage()
        {
            InitializeComponent();
            Shipment_DataGrid_SelectionChanged();

        }

        private void Shipment_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[shipment], [dbo].[qua_certificate] ";
            cmd.Connection = sqlConnection;

            SqlDataAdapter Shipment = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            Shipment.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[shipment]", sqlConnection);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ShipmentGrid.DataContext = dt;
            ShipmentGrid.ItemsSource = dt.DefaultView;
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShipmentGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[shipment] WHERE id_shipment like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter shipments = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("shipment");
                shipments.Fill(dt);
                ShipmentGrid.ItemsSource = dt.DefaultView;
                shipments.Update(dt);
                cmds.Close();
            }
            }
        private void clientButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void brakButton_Click(object sender, RoutedEventArgs e) //!!!!!!!!!!!
        {
            try
            {
                if (ShipmentGrid.SelectedItems.Count > 0)
                {
                    sqlConnection.Open();
                    DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                    string shipment = drv.Row[0].ToString();
                    SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", shipment);
                    cmd.ExecuteNonQuery();
                    Shipment_DataGrid_SelectionChanged();
                }
            }
            catch (Exception ex)
            {
                var window = new ShipmentPage();
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить заявку?"
     , "Sevestal Infocom", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        MessageBox.Show("Заявка была НЕ отменена", "Severstal Infocom");
                        break;
                    case MessageBoxResult.Yes:
                        MessageBox.Show("Заявка отменена", "Severstal Infocom");
                        this.Hide();
                        window.Show();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
        }

        private void cert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void go_to_dostav_Click(object sender, RoutedEventArgs e)
        {
            if (ShipmentGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                string shipment = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT delivery ON  INSERT INTO delivery (id_delivery, consignee, date_of_delivery) SELECT id_shipment, consignee, date_of_shipment FROM shipment WHERE id_shipment=@id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", shipment);
                cmd.ExecuteNonQuery();
                Shipment_DataGrid_SelectionChanged();
                MessageBox.Show("Заявка из отгрузки успешно отправлена в доставку!", "Severstal Infocom");
                Hide();
                var window = new Windows.Delivery();
                window.ShowDialog();
                Show();
            }
        }

        private void ShipmentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
