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
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    public partial class StoragePage : Window
    {
        // string connectionString;
        // SqlDataAdapter Dstorage;
       //DataTable dt;

        public StoragePage()
        {
            InitializeComponent();
            Storage_DataGrid_SelectionChanged();
        }

        private void Storage_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[storage]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter storage = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            storage.Fill(dt);
            StorageGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
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
                SqlConnection con = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", con);
                cmd.Parameters.AddWithValue("@id", storage);
                cmd.ExecuteNonQuery();
                Storage_DataGrid_SelectionChanged();
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            StorageGrid.Items.Refresh();

        }



        private void StorageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string searchValue = search.Text;

        //    StorageGrid.SelectionMode = StorageGrid.FullRowSelect;
        //    try
        //    {
        //        foreach (StorageGrid str in DataGridView.Rows)
        //        {
        //            if (str.Cells[1].Value.ToString().Equals(searchValue))
        //            {
        //                str.Selected = true;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        MessageBox.Show(exc.Message);
        //    }
        //}


        //private void UpdateDB()
        //{
        //    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(Dstorage);
        //    Dstorage.Update(storages); //System.NullReferenceException: 'Object reference not set to an instance of an object.'
        //    /// System.NullReferenceException: 'Object reference not set to an instance of an object.'
        //}

        //private void updateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(Dstorage);
        //    Dstorage.Update(storages); //System.NullReferenceException: 'Object reference not set to an instance of an object.'
        //}

        //private void deleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (StorageGrid.SelectedItems != null)
        //    {
        //        for (int i = 0; i < StorageGrid.SelectedItems.Count; i++) //FK_orders_type_product
        //        {
        //            DataRowView datarowView = StorageGrid.SelectedItems[i] as DataRowView;
        //            if (datarowView != null)
        //            {
        //                DataRow dataRow = (DataRow)datarowView.Row;
        //                dataRow.Delete();
        //            }
        //        }
        //    }
        //    UpdateDB();
        //}
    }
}
