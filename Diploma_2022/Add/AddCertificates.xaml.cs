using System;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.IO;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Interaction logic for AddCertificates.xaml
    /// </summary>
    public partial class AddCertificates : Window
    {
        public AddCertificates()
        {
            InitializeComponent();
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                sqlConnection.Open();
                String query = "INSERT INTO [dbo].qua_certificate values(@standard_per_mark, @tolerance_standart, @product_standard); ";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@standard_per_mark", standard_per_mark.Text);
                createCommand.Parameters.AddWithValue("@tolerance_standart", tolerance_standart.Text);
                createCommand.Parameters.AddWithValue("@product_standard", product_standard.Text);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();

            }
        }

    }
}
