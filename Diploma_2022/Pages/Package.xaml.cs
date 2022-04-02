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
    /// Логика взаимодействия для Package.xaml
    /// </summary>
    public partial class Package : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public Package()
        {
            InitializeComponent();
            Package_DataGrid_SelectionChanged();
        }
        private void Package_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[package]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter pack = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            pack.Fill(dt);
            PackageGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void Buttontoshipment(object sender, RoutedEventArgs e)
        {
                try
                {
                    if (PackageGrid.SelectedItems.Count > 0)
                    {
                        sqlConnection.Open();
                        DataRowView drv = (DataRowView)PackageGrid.SelectedItem;
                        string ID_Orders = drv.Row[0].ToString();
                        SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[shipment] (id_order) ((SELECT id_order FROM package WHERE id_order=@id))", sqlConnection);
                        cmd.Parameters.AddWithValue("@id", ID_Orders);
                        cmd.ExecuteNonQuery();
                        Package_DataGrid_SelectionChanged();
                        MessageBox.Show("Заказ успешно отправлен в отгрузку!", "Severstal Infocom");
                        sqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Данный заказ уже быа отправлен в отгрузку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        private void outButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PackageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Add.AddPackage taskWindow = new Add.AddPackage();
            taskWindow.Show();
        }
    }
    }

