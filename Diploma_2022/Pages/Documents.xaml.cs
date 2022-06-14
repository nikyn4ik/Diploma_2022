using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Diploma_2022.Models;
using Diploma_2022.Add;
using System;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для Documents.xaml
    /// </summary>
    public partial class Documents : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");
        string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
        public Documents()
        {
            InitializeComponent();
            DocGrid_SelectionChanged();
        }

        private void DocGrid_SelectionChanged()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT delivery.id_order, orders.name_product, delivery.date_of_delivery FROM [dbo].[delivery] LEFT JOIN orders ON orders.id_order=delivery.id_order";
            cmd.Connection = sqlConnection;
            SqlDataAdapter pack = new SqlDataAdapter(cmd);
            pack.Fill(dt);
            DocGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void outButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = DocGrid.SelectedIndex;
            if (selectedIndex != -1)
                Explorer();
            else 
                MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
        private void Explorer()
        {
            Process.Start("explorer.exe", @"PDF");
        }
        
        private void UpdButton(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT delivery.id_order, orders.name_product, delivery.date_of_delivery FROM [dbo].[delivery] LEFT JOIN orders ON orders.id_order=delivery.id_order";
            cmd.Connection = sqlConnection;
            SqlDataAdapter doc = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            doc.Fill(dt);
            DocGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            DocGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string cmd = "SELECT * FROM [dbo].[orders] WHERE id_order like '" + pole.Text + "%'";
            sqlConnection.Open();
            SqlCommand sqlcom = new SqlCommand(cmd, sqlConnection);
            SqlDataAdapter dc = new SqlDataAdapter(sqlcom);
            dc.Fill(dt);
            DocGrid.ItemsSource = dt.DefaultView;
            dc.Update(dt);
            sqlConnection.Close();
        }
    }
}
