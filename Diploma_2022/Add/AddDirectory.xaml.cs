using System;
using System.Windows;
using System.Data.SqlClient;
using Diploma_2022.Pages;
using Diploma_2022.Models;
using System.Data;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Interaction logic for AddDirectory.xaml
    /// </summary>
    public partial class AddDirectory : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        public AddDirectory()
        {
            InitializeComponent();
            standard_per_mark_Select();
        }

        private void standard_per_mark_Select()
        {
            SqlCommand cmd = new SqlCommand("SELECT standard_per_mark FROM [dbo].[qua_certificate]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                standard_per_mark.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (standard_per_mark.Text != "" && min.Text != "" && max.Text != "" && units.Text != "" && properties_cert.Text != "")
            {
                var query1 = "SELECT id_qua_certificate FROM [dbo].[qua_certificate] WHERE standard_per_mark=@standard_per_mark";
                SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                sqlCommand1.Parameters.AddWithValue("@standard_per_mark", SqlDbType.NVarChar).Value = standard_per_mark.Text;
                var reader = sqlCommand1.ExecuteReader();

                var data = "";
                if (reader.Read())
                {
                    data = reader["id_qua_certificate"].ToString();
                }
                reader.Close();
                query = "INSERT INTO  [dbo].[cert_directory] (standard_per_mark, min, max, units,id_qua_certificate, properties_cert) VALUES (@standard_per_mark, @min, @max, @units,@id_qua_certificate, @properties_cert)";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@standard_per_mark", standard_per_mark.Text);
                createCommand.Parameters.AddWithValue("@id_qua_certificate", data);
                createCommand.Parameters.Add("@min", SqlDbType.Int).Value = Convert.ToInt32(min.Text);
                createCommand.Parameters.Add("@max", SqlDbType.Int).Value = Convert.ToInt32(max.Text);
                createCommand.Parameters.AddWithValue("@units", units.Text);
                createCommand.Parameters.AddWithValue("@properties_cert", properties_cert.Text);
                update(createCommand);
                this.Close();
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
