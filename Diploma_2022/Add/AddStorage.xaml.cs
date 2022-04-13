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

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddStorage.xaml
    /// </summary>
    public partial class AddStorage : Window
    {
        public AddStorage()
        {
            InitializeComponent();
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            //try
            {
                sqlConnection.Open();
                String query = "INSERT INTO [dbo]. storage values(@name_storage, @address, @phone_storage, @remainder, @date_add_storage);";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@name_storage", namesclad.Text);
                createCommand.Parameters.AddWithValue("@address", addresssklad.Text);
                createCommand.Parameters.AddWithValue("@phone_storage", phonesklad.Text);
                createCommand.Parameters.AddWithValue("@remainder", ostatok.Text);
                createCommand.Parameters.AddWithValue("@date_add_storage", namesclad.Text);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
                showdata();

            }
        }

            public void showdata()
            {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[storage]", sqlConnection);
               DataTable dt = new DataTable();
                adpt.Fill(dt);
                StorageGrid.DataContext = dt;
                StorageGrid.ItemsSource = dt.DefaultView;
        }

        private void dataaddsklad_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime dataaddsklad = (DateTime)this.DatePicker.SelectedDate;
        }
    }
    }
