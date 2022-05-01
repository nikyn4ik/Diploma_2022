using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.ObjectModel;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Diploma_2022.Models;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Interaction logic for Certificates.xaml
    /// </summary>
    public partial class Certificates : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");
        ObservableCollection<Certificates> certificates = new ObservableCollection<Certificates>();
        public Certificates()
        {
            InitializeComponent();
            Certificates_DataGrid_SelectionChanged();
            certificates = new ObservableCollection<Certificates>();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Add.AddCertificates taskWindow = new Add.AddCertificates();
            taskWindow.Show();
            Certificates_DataGrid_SelectionChanged();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CertificatesGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)CertificatesGrid.SelectedItem;
                string certific = drv.Row[0].ToString();
                SqlConnection con = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM qua_certificate WHERE id_qua_certificate=@id", con);
                cmd.Parameters.AddWithValue("@id", certific);
                cmd.ExecuteNonQuery();
                Certificates_DataGrid_SelectionChanged();
            }
        }

        private void Certificates_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            DataTable tap = new DataTable();
            cmd.CommandText = "SELECT * FROM [dbo].[qua_certificate]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter order = new SqlDataAdapter(cmd);
            new SqlDataAdapter(cmd.CommandText, sqlConnection).Fill(tap);
            order.Fill(dt);
            List<int> result = new List<int>();
            result = tap.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("id_qua_certificate")).ToList();
            certificates = new ObservableCollection<Certificates>();
            CertificatesGrid.ItemsSource = dt.DefaultView;
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            CertificatesGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            //string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            //try
            //{
            //    SqlConnection cmds = new SqlConnection(ConnectionString);
            //    string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE id_qua_certificate like '" + pole.Text + "%'";
            //    cmds.Open();
            //    SqlCommand sqlcom = new SqlCommand(cmd, cmds);
            //    SqlDataAdapter certificat = new SqlDataAdapter(sqlcom);
            //    certificat.Fill(dt);
            //    CertificatesGrid.ItemsSource = dt.DefaultView;
            //    certificat.Update(dt);
            //    cmds.Close();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Не найдено в системе.", "Severstal Infocom",MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            foreach (var cert in certificates) 
            {
                var command = new SqlCommand("UPDATE qua_certificate SET id_qua_certificate=@id_qua_certificate,standard_per_mark=@standard_per_mark, product_standard=@product_standard, date_add_certificate=@date_add_certificate WHERE id_qua_certificate=@id_qua_certificate");
                command.Parameters.AddWithValue("@id_qua_certificate", cert.CertificatesGrid);
                command.Parameters.AddWithValue("@standard_per_mark", cert.CertificatesGrid);
                command.Parameters.AddWithValue("@product_standard", cert.CertificatesGrid);
                command.Parameters.AddWithValue("@date_add_certificate", cert.CertificatesGrid);
                command.ExecuteNonQuery();  
            }  
            sqlConnection.Close();
        }
    }
}
