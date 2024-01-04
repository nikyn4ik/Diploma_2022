using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Diploma_2022.Pages
{
    public partial class Certificates : Window
    {
        private readonly SqlConnection sqlConnection;

        public Certificates()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            Certificates_DataGrid_SelectionChanged();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           //// Windows.AddCertificates taskWindow = new Windows.AddCertificates();
           // taskWindow.Show();
           // Certificates_DataGrid_SelectionChanged();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CertificatesGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)CertificatesGrid.SelectedItem;
                string certific = drv.Row[0].ToString();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM qua_certificate WHERE id_qua_certificate=@id", con);
                    cmd.Parameters.AddWithValue("@id", certific);
                    cmd.ExecuteNonQuery();
                }

                Certificates_DataGrid_SelectionChanged();
            }
        }

        private void Certificates_DataGrid_SelectionChanged()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[qua_certificate]", sqlConnection))
            {
                SqlDataAdapter certificates = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("diploma_db");
                certificates.Fill(dt);
                CertificatesGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            CertificatesGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection cmds = new SqlConnection(ConnectionString))
                {
                    string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE id_qua_certificate like '" + pole.Text + "%'";
                    cmds.Open();
                    SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                    SqlDataAdapter certificat = new SqlDataAdapter(sqlcom);
                    DataTable dt = new DataTable("qua_certificate");
                    certificat.Fill(dt);
                    CertificatesGrid.ItemsSource = dt.DefaultView;
                    certificat.Update(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection cmds = new SqlConnection(ConnectionString))
            {
                string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE id_qua_certificate like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter certificat = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("qua_certificate");
                certificat.Fill(dt);
                CertificatesGrid.ItemsSource = dt.DefaultView;
                certificat.Update(dt);
            }
        }
    }
}
