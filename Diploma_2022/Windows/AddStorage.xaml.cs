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
        }
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                sqlConnection.Open();
                String query = "SELECT * FROM [dbo].[storage]"; ;
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.ExecuteNonQuery();
                //  MessageBox.Show("Saved");
                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("storage");
                dataAdp.Fill(dt);
                StorageGrid.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           // add(idsclad.Text, namesclad.Text, addresssklad.Text, datasklad.Text, kodsap.Text, ostatok.Text);

        }
    }
}
