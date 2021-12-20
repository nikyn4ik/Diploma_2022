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
        string connectionString;
      //  SqlDataAdapter shipment;
       // DataTable shipments;

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

            SqlDataAdapter Shipment = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db"); 
            Shipment.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            if (ShipmentGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                string shipmen = drv.Row[0].ToString();
                SqlConnection con = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM shipment WHERE id_shipment=@id", con);
                cmd.Parameters.AddWithValue("@id", shipmen);
                cmd.ExecuteNonQuery();
                Shipment_DataGrid_SelectionChanged();
            }
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            Windows.AddShipment taskWindow = new Windows.AddShipment();
            taskWindow.Show();
            Shipment_DataGrid_SelectionChanged();
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            ShipmentGrid.Items.Refresh();
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
    }
}
