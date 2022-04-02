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
            cmd.CommandText = "SELECT * FROM [dbo].[qua_certificate]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter certificates = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            certificates.Fill(dt);
            CertificatesGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            CertificatesGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE id_qua_certificate like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter certificat = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("qua_certificate");
                certificat.Fill(dt);
                CertificatesGrid.ItemsSource = dt.DefaultView;
                certificat.Update(dt);
                cmds.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            SqlConnection cmds = new SqlConnection(ConnectionString);
            string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE id_qua_certificate like '" + pole.Text + "%'";
            cmds.Open();
            SqlCommand sqlcom = new SqlCommand(cmd, cmds);
            SqlDataAdapter certificat = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable("qua_certificate");
            certificat.Fill(dt);
            CertificatesGrid.ItemsSource = dt.DefaultView;
            certificat.Update(dt);
            cmds.Close();
        }
    }
}
