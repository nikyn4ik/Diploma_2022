using Diploma_2022.Windows;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Diploma_2022.Pages
{
    public partial class ShipmentPage : Window
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;
        private DataTable shipmentDataTable;
        public ShipmentPage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            showdata();
        }
        public void showdata()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlDataAdapter adpt = new SqlDataAdapter(@"
            SELECT TOP (1000)
            s.[id_shipment],
            o.[id_order] as [id_order], 
            o.[name_product] as [name_product],
            s.[date_of_shipments],
            s.[shipment_total_amount_tons],
            deliv.[early_delivery],
            cd.[standard_per_mark]
            FROM [diploma_db].[dbo].[shipment] s
            LEFT JOIN [diploma_db].[dbo].[storage] stor ON s.[id_storage] = stor.[id_storage]
            LEFT JOIN [diploma_db].[dbo].[delivery] deliv ON s.[id_order] = deliv.[id_order]
            LEFT JOIN [diploma_db].[dbo].[orders] o ON s.[id_order] = o.[id_order]
            LEFT JOIN [diploma_db].[dbo].[transport] t ON s.[id_transport] = t.[id_transport]
            LEFT JOIN [diploma_db].[dbo].[cert_directory] cd ON o.[id_qua_certificate] = cd.[id_qua_certificate]",
                    sqlConnection);

                shipmentDataTable = new DataTable();
                adpt.Fill(shipmentDataTable);
                ShipmentGrid.DataContext = shipmentDataTable;
                ShipmentGrid.ItemsSource = shipmentDataTable.DefaultView;
            }
            }
            finally
            {
                sqlConnection.Close();
        }
        }
        private void Shipment_DataGrid_SelectionChanged()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlDataAdapter Shipment = new SqlDataAdapter(@"
SELECT TOP (1000)
s.[id_shipment],
o.[id_order] as [id_order], 
o.[name_product] as [name_product],
s.[date_of_shipments],
s.[shipment_total_amount_tons],
deliv.[early_delivery],
cd.[standard_per_mark]
FROM [diploma_db].[dbo].[shipment] s
LEFT JOIN [diploma_db].[dbo].[storage] stor ON s.[id_storage] = stor.[id_storage]
LEFT JOIN [diploma_db].[dbo].[delivery] deliv ON s.[id_order] = deliv.[id_order]
LEFT JOIN [diploma_db].[dbo].[orders] o ON s.[id_order] = o.[id_order]
LEFT JOIN [diploma_db].[dbo].[transport] t ON s.[id_transport] = t.[id_transport]
LEFT JOIN [diploma_db].[dbo].[cert_directory] cd ON o.[id_qua_certificate] = cd.[id_qua_certificate]",
sqlConnection);
                    DataTable dt = new DataTable("diploma_db");
                    Shipment.Fill(dt);

                    ShipmentGrid.ItemsSource = null;
                    ShipmentGrid.Items.Clear();

                    ShipmentGrid.ItemsSource = dt.DefaultView;
                    
                    using (SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(Shipment))
                    {
                        Shipment.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
                        Shipment.Update(dt);
                    }
                }
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShipmentGrid.Items.Refresh();
        }

        private void ShipmentGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column != null &&
                (e.Column.Header.ToString() != "shipment_total_amount_tons" &&
                 e.Column.Header.ToString() != "early_shipment"))
            {
                e.Cancel = true;
            }
        }
        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection cmds = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    string cmdText = "SELECT * FROM [dbo].[shipment] WHERE id_shipment like '" + pole.Text + "%'";
                    cmds.Open();
                    using (SqlCommand sqlcom = new SqlCommand(cmdText, cmds))
                    {
                        SqlDataAdapter shipments = new SqlDataAdapter(sqlcom);
                        DataTable dt = new DataTable("shipment");
                        shipments.Fill(dt);
                        ShipmentGrid.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void brakButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить заявку?", "Sevestal Infocom", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        MessageBox.Show("Заявка НЕ была отменена", "Severstal Infocom");
                        break;
                    case MessageBoxResult.Yes:
                        DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                        if (drv != null)
                        {
                            string shipment = drv.Row[0].ToString();
                            sqlConnection.Open();
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM shipment WHERE id_shipment=@id", sqlConnection))
                            {
                                cmd.Parameters.AddWithValue("@id", shipment);
                                cmd.ExecuteNonQuery();
                            }
                            Shipment_DataGrid_SelectionChanged();
                            MessageBox.Show("Заявка отменена", "Severstal Infocom");
                        }
                        else
                        {
                            MessageBox.Show("Выберите заказ.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        private void cert_Click(object sender, RoutedEventArgs e)
        {
            if (ShipmentGrid.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;

                    string orderIdString = drv.Row["id_order"].ToString();

                    if (int.TryParse(orderIdString, out int selectedOrderId))
                    {
                    AddCertificates addCertificatesWindow = new AddCertificates(selectedOrderId);

                    addCertificatesWindow.Closed += (s, args) => { RefreshShipmentData(); };
                    addCertificatesWindow.Show();
                }
                else
                {
                    MessageBox.Show("Выберите заказ из отгрузки.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public void RefreshShipmentData()
        {
            showdata();
        }
        private void go_to_dostav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShipmentGrid.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;

                    if (drv != null && drv.Row != null)
                    {
                        if (!string.IsNullOrEmpty(drv.Row["standard_per_mark"]?.ToString()))
                        {
                            string shipment = drv.Row[0].ToString();
                            sqlConnection.Open();
                            using (SqlCommand cmd = new SqlCommand(@"
SET IDENTITY_INSERT delivery ON;
INSERT INTO delivery (id_delivery, consignee)
SELECT s.id_shipment, c.name_consignee
FROM shipment s
JOIN consignee c ON s.id_order = o.id_order
WHERE s.id_shipment = @id;
SET IDENTITY_INSERT delivery OFF;", sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@id", shipment);
                            cmd.ExecuteNonQuery();
                        }
                            MessageBox.Show("Заявка из отгрузки успешно отправлена в доставку!", "Severstal Infocom");

                            Windows.AddDelivery taskWindow = new Windows.AddDelivery();
                            taskWindow.Show();
                            Shipment_DataGrid_SelectionChanged();
                        }
                        else
                        {
                            MessageBox.Show("Невозможно отправить заказ в доставку, так как у заказа отсутствует сертификат.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку с заказом из отгрузки.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите строку с заказом из отгрузки.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
