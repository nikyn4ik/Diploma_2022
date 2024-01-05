using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using iTextSharp.text.pdf;
using Diploma_2022.Models;

namespace Diploma_2022.Windows
{
    public partial class AddStorage : Window
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;
        public AddStorage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            showdata();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    String query = "INSERT INTO [dbo].storage (name_storage, address, phone_storage, FIO_responsible_person, date_add_storage) OUTPUT INSERTED.id_storage, INSERTED.date_add_storage VALUES (@name_storage, @address, @phone_storage, @FIO_responsible_person, GETDATE());";

                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@name_storage", namesclad.Text);
                    createCommand.Parameters.AddWithValue("@address", addresssklad.Text);
                    createCommand.Parameters.AddWithValue("@phone_storage", phonesklad.Text);
                    createCommand.Parameters.AddWithValue("@FIO_responsible_person", FIO_responsible.Text);

                    using (SqlDataReader reader = createCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int storageId = reader.GetInt32(0);
                            DateTime dateAddStorage = reader.GetDateTime(1);
                        }
                    }
                    MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                    sqlConnection.Close();
                    showdata();
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
                SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[storage]", sqlConnection);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                StorageGrid.DataContext = dt;
                StorageGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
    }
