//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Data.SqlClient;
//using System.Data;
//using System.IO;
//using System.Configuration;
//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    public partial class StoragePage : Window
    {
        List<Models.Storage> list = new();
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public StoragePage()
        {
            InitializeComponent();
            Storage_DataGrid_SelectionChanged();
        }

        private void Storage_DataGrid_SelectionChanged()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[storage]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter storage = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            storage.Fill(dt);
            StorageGrid.ItemsSource = dt.DefaultView;
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            Windows.AddStorage taskWindow = new Windows.AddStorage();
            taskWindow.Show();
            Storage_DataGrid_SelectionChanged();
        }

        private void deleteButton(object sender, RoutedEventArgs e)
        {
            if (StorageGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)StorageGrid.SelectedItem;
                string storage = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", storage);
                cmd.ExecuteNonQuery();
                Storage_DataGrid_SelectionChanged();
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            StorageGrid.Items.Refresh();

        }
        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[storage] WHERE id_storage like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter storages = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("storage");
                storages.Fill(dt);
                StorageGrid.ItemsSource = dt.DefaultView;
                storages.Update(dt);
                cmds.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            StorageGrid.Items.Refresh();
        }

        private void StorageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    }
