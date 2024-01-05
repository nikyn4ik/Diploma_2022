using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Diploma_2022.Windows;

namespace Diploma_2022.Pages
{
    public partial class StoragePage : Window
    {
        SqlConnection sqlConnection;
        private DataTable storageDataTable;

        public StoragePage()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            Storage_DataGrid_SelectionChanged();
            showdata();
        }

        private void Storage_DataGrid_SelectionChanged()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [dbo].[storage]";
                cmd.Connection = sqlConnection;

                SqlDataAdapter storage = new SqlDataAdapter(cmd);
                storageDataTable = new DataTable("diploma_db");
                storage.Fill(storageDataTable);
                StorageGrid.ItemsSource = storageDataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showdata()
        {
            try
            {
                sqlConnection.Open();
                SqlDataAdapter adpt = new SqlDataAdapter(@"
SELECT TOP (1000) [id_storage]
,[name_storage]
,[address]
,[phone_storage]
,[date_add_storage]
,[FIO_responsible_person]
FROM [diploma_db].[dbo].[storage]", sqlConnection);

                storageDataTable = new DataTable();
                adpt.Fill(storageDataTable);
                StorageGrid.DataContext = storageDataTable;
                StorageGrid.ItemsSource = storageDataTable.DefaultView;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            AddStorage taskWindow = new AddStorage();
            taskWindow.Closed += (s, args) => { RefreshStorageData(); };
            taskWindow.Show();
        }

        public void RefreshStorageData()
        {
            showdata();
        }

        private void deleteButton(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данный склад из базы?", "Sevestal Infocom", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes && StorageGrid.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)StorageGrid.SelectedItem;
                    string storage = drv.Row[0].ToString();

                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", storage);
                    cmd.ExecuteNonQuery();
                    Storage_DataGrid_SelectionChanged();
                    MessageBox.Show("Удален!", "Severstal Infocom");
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

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            RefreshStorageData();
        }
            private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection cmds = new SqlConnection(connectionString))
                {
                    string cmdText = "SELECT * FROM [dbo].[storage] WHERE id_storage like '" + pole.Text + "%'";
                    cmds.Open();
                    SqlCommand sqlcom = new SqlCommand(cmdText, cmds);
                    SqlDataAdapter storages = new SqlDataAdapter(sqlcom);
                    storageDataTable = new DataTable("storage");
                    storages.Fill(storageDataTable);
                    StorageGrid.ItemsSource = storageDataTable.DefaultView;
                    storages.Update(storageDataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                StorageGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StorageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
