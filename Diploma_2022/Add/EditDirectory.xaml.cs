using System;
using System.Windows;
using System.Data.SqlClient;
using Diploma_2022.Pages;
using Diploma_2022.Models;
using System.Data;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Interaction logic for EditDirectory.xaml
    /// </summary>
    public partial class EditDirectory : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        int Id_qua_cert;
        public EditDirectory(int id_qua_cert)
        {
            InitializeComponent();
            Id_qua_cert = id_qua_cert;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (min.Text != "" && max.Text != "" && units.Text != "")
            {
                query = "UPDATE [dbo].[cert_directory] SET max=@max, min=@min, units=@units WHERE id_cert_directory=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", Id_qua_cert.ToString());
                createCommand.Parameters.Add("@min", SqlDbType.Int).Value = Convert.ToInt32(min.Text);
                createCommand.Parameters.Add("@max", SqlDbType.Int).Value = Convert.ToInt32(max.Text);
                createCommand.Parameters.AddWithValue("@units", units.Text);
                update(createCommand);
            }
            else
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
            }
        }
        private void update(SqlCommand createCommand)
        {
            createCommand.ExecuteNonQuery();
            MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
            sqlConnection.Close();
            this.Close();
        }
    }
}
