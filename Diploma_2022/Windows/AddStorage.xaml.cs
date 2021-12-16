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

namespace Diploma_2022.Windows
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
            SqlDataAdapter adpt;
            DataTable dt;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            //try
            {
                sqlConnection.Open();
                String query = "INSERT INTO storage values(@name_storage,@address,@phone_storage,@date_of_entrance,@SAP_product_code,@remainder); ";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@name_storage", namesclad.Text);
                createCommand.Parameters.AddWithValue("@address", addresssklad.Text);
                createCommand.Parameters.AddWithValue("@phone_storage", phonesklad.Text);
                createCommand.Parameters.AddWithValue("@date_of_entrance", datasklad.Text);
                createCommand.Parameters.AddWithValue("@SAP_product_code", kodsap.Text);
                createCommand.Parameters.AddWithValue("@remainder", ostatok.Text);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                //  SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);

                //  DataTable dt = new DataTable("storage");
                //  dataAdp.Fill(dt);
                // StorageGrid.ItemsSource = dt.DefaultView;
                //   dataAdp.Update(dt);
                sqlConnection.Close();
                showdata();

            }
        }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}


            public void showdata()
            {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[storage]", sqlConnection);
               DataTable dt = new DataTable();
                adpt.Fill(dt);
                StorageGrid.DataContext = dt;
                StorageGrid.ItemsSource = dt.DefaultView;
        }
        }
    }
