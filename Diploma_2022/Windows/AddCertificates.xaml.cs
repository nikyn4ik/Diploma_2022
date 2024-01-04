using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Diploma_2022.Pages;

namespace Diploma_2022.Windows
{
    public partial class AddCertificates : Window
    {
        private readonly string connectionString;
        private DataTable certificatesDataTable;
        private int id_order;
        public AddCertificates(int orderId)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            id_order = orderId;
            showdata();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string selectLastCertificateIdQuery = "SELECT TOP 1 id_qua_certificate FROM [dbo].qua_certificate ORDER BY date_add_certificate DESC;";
                    using (SqlCommand selectLastCertificateIdCommand = new SqlCommand(selectLastCertificateIdQuery, sqlConnection))
                    {
                        int newQuaCertificateId = (int)selectLastCertificateIdCommand.ExecuteScalar();

                        string updateOrderQuery = "UPDATE [dbo].orders SET id_qua_certificate = @newQuaCertificateId WHERE id_order = @order_id;";
                        using (SqlCommand updateOrderCommand = new SqlCommand(updateOrderQuery, sqlConnection))
                        {
                            updateOrderCommand.Parameters.AddWithValue("@newQuaCertificateId", newQuaCertificateId);
                            updateOrderCommand.Parameters.AddWithValue("@order_id", id_order);

                            updateOrderCommand.ExecuteNonQuery();
                        }

                        string updateShipmentQuery = "UPDATE [dbo].[shipment] SET date_of_shipments = GETDATE() WHERE id_order = @order_id;";
                        using (SqlCommand updateShipmentCommand = new SqlCommand(updateShipmentQuery, sqlConnection))
                        {
                            updateShipmentCommand.Parameters.AddWithValue("@order_id", id_order);

                            updateShipmentCommand.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Сертификат сохранен и привязан к заказу!", "Severstal Infocom", MessageBoxButton.OK);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void showdata()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[qua_certificate]", sqlConnection);
                certificatesDataTable = new DataTable();
                adpt.Fill(certificatesDataTable);
                CertificatesGrid.DataContext = certificatesDataTable;
                CertificatesGrid.ItemsSource = certificatesDataTable.DefaultView;

                standardCombo.ItemsSource = certificatesDataTable.AsEnumerable()
                    .Select(row => row.Field<string>("standard_per_mark"))
                    .Distinct()
                    .ToList();
                manufacturerCombo.IsEditable = false;
                productCombo.IsEditable = false;
            }
            }

            private void standardComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateManufacturerAndProductComboBoxes();
        }

        private void PopulateManufacturerAndProductComboBoxes()
        {
            string selectedStandard = (string)standardCombo.SelectedItem;

            if (selectedStandard != null)
            {
                DataRow[] selectedRows = certificatesDataTable.Select($"standard_per_mark = '{selectedStandard}'");

                List<string> manufacturers = selectedRows.Select(row => row.Field<string>("manufacturer")).Distinct().ToList();
                List<string> products = selectedRows.Select(row => row.Field<string>("product_standard")).Distinct().ToList();

                manufacturerCombo.ItemsSource = manufacturers;
                productCombo.ItemsSource = products;

                manufacturerCombo.IsDropDownOpen = false;
                productCombo.IsDropDownOpen = false;
                manufacturerCombo.IsEditable = false;
                productCombo.IsEditable = false;

                if (manufacturers.Count > 0)
                {
                    manufacturerCombo.SelectedIndex = 0;
                }
                if (products.Count > 0)
                {
                    productCombo.SelectedIndex = 0;
                }
            }
        }
    }
}