using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Diploma_2022.Add;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для Documents.xaml
    /// </summary>
    public partial class Documents : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public Documents()
        {
            InitializeComponent();
            DocGrid_SelectionChanged();
        }

        private void DocGrid_SelectionChanged()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery], [dbo].[orders]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter doc = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            doc.Fill(dt);
            DocGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

        }

        private void outButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery], [dbo].[orders]";
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
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[orders] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter dc = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("delivery");
                dc.Fill(dt);
                DocGrid.ItemsSource = dt.DefaultView;
                dc.Update(dt);
                cmds.Close();
            }
        }
    }
}
