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
            cmd.CommandText = "SELECT * FROM [dbo].[shipment]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter shipment = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            shipment.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void dateships(object sender, RoutedEventArgs e)/* (!!!)*/
        {
            var window = new Windows.AddCalendarDateShipment();
            window.ShowDialog();
            Show();
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShipmentGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void brakButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ShipmentPage();
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить заявку?", "Sevestal Infocom", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.No:
                    MessageBox.Show("Заявка НЕ была отменена", "Severstal Infocom");
                    break;
                case MessageBoxResult.Yes:
                    MessageBox.Show("Заявка отменена", "Severstal Infocom");
                    this.Hide();
                    DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem; //if (ShipmentGrid.SelectedItems.Count > 0)
                        string shipment = drv.Row[0].ToString();
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM shipment WHERE id_shipment=@id", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", shipment);
                    cmd.ExecuteNonQuery();
                    Shipment_DataGrid_SelectionChanged();
                    window.Show();
                    break;
}
        }

        private void cert_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Certificates();
            window.ShowDialog();
            Show();
        }

        private void go_to_dostav_Click(object sender, RoutedEventArgs e)
        {
            if (ShipmentGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                string shipmentId = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[delivery](consignee, product_standard, name_storage, id_order) " +
                "VALUES((SELECT consignee FROM shipment WHERE id_shipment = @id), 's', 's', (SELECT consignee FROM shipment WHERE id_shipment = @id))", sqlConnection);
                cmd.Parameters.AddWithValue("@id", shipmentId);
                cmd.ExecuteNonQuery();
                Shipment_DataGrid_SelectionChanged();
                MessageBox.Show("Заявка из отгрузки успешно отправлена в доставку!", "Severstal Infocom");
                var window = new Windows.AddDelivery();
                window.ShowDialog();
                Show();
            }
        }
    }
}
