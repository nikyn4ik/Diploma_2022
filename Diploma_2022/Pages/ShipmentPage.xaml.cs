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
            var window = new Add.AddCalendarDateShipment();
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
                string cmd = "SELECT * FROM [dbo].[shipment] WHERE id_order like '" + pole.Text + "%'";
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

        private void go_to_dostav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              if (ShipmentGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                string shipmentId = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[delivery] (id_order) VALUES((SELECT id_order FROM package WHERE id_order = @id))", sqlConnection);
                cmd.Parameters.AddWithValue("@id", shipmentId);
                cmd.ExecuteNonQuery();
                Shipment_DataGrid_SelectionChanged();
                MessageBox.Show("Заказ успешно отправлен в доставку!", "Severstal Infocom");
                sqlConnection.Close();
        }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Данный заказ уже быа отправлен в доставку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
}

        private void outButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Add.AddShipment taskWindow = new Add.AddShipment();
            taskWindow.Show();
        }
    }
}
