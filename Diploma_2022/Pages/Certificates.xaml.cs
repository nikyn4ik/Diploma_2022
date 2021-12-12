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
    /// Interaction logic for Certificates.xaml
    /// </summary>
    public partial class Certificates : Window
    {
        public Certificates()
        {
            InitializeComponent();
            Certificates_DataGrid_SelectionChanged();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Certificates_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[qua_certificate]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter certificates = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            certificates.Fill(dt);
            CertificatesGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
    }
}
