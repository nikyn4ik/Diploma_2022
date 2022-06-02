using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Diploma_2022.Models;
using Diploma_2022.Add;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Interaction logic for CertDirectoryPage.xaml
    /// </summary>
    public partial class CertDirectoryPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public CertDirectoryPage()
        {
            InitializeComponent();
            DirectoryGrid_SelectionChanged();
        }

        private void DirectoryGrid_SelectionChanged()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[cert_directory]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter directory = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            directory.Fill(dt);
            DirectoryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void addButton(object sender, RoutedEventArgs e)
        {
            var window = new AddDirectory();
            window.ShowDialog();
            Show();
            update();
        }

        protected void update()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[cert_directory]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter directory = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            directory.Fill(dt);
            DirectoryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void editButton(object sender, RoutedEventArgs e)
        {
            object item = DirectoryGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите нужную строчку", "Severstal Infocom");
            else
            {
                string ID = (DirectoryGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new EditDirectory(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            DirectoryGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[cert_directory] WHERE id_qua_certificate like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter directory = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("cert_directory");
                directory.Fill(dt);
                DirectoryGrid.ItemsSource = dt.DefaultView;
                directory.Update(dt);
                cmds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не найдено в системе.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
