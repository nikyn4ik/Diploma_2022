using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using Diploma_2022.Models;
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
using System.Configuration;
using System.Globalization;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddAttestationToOrder.xaml
    /// </summary>
    public partial class AddAttestationToOrder : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        int IdOrder;
        public string FIO;
        double thickness_mm;
        double width_mm;
        double length_mm;
        public AddAttestationToOrder(int idOrder)
        {
            InitializeComponent();
            IdOrder = idOrder;
            standard_mark_Select();
            DatePicker.DisplayDate = DateTime.Today;
            DatePicker.Text = DateTime.Today.ToString();
        }
       
        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (Convert.ToDateTime(DatePicker.Text) < DateTime.Today)
            {
                MessageBox.Show("Дата меньше текущей", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();

                return;
            }
             if (standard_mark.Text != "" && units.Text != "" && product_standard.Text != "" && DatePicker.Text != "")
            {
                var query1 = "SELECT id_qua_certificate FROM [dbo].[qua_certificate] WHERE standard_per_mark=@standard_per_mark";
                SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                sqlCommand1.Parameters.AddWithValue("@standard_per_mark", SqlDbType.NVarChar).Value = standard_mark.Text;
                var reader = sqlCommand1.ExecuteReader();

                var data = "";
                if (reader.Read())
                {
                    data = reader["id_qua_certificate"].ToString();
                }
                sqlConnection.Close();
                sqlConnection.Open();
                query = "UPDATE [dbo].[orders] SET id_qua_certificate=@id_sert, units=@units WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@id_sert", data);
                createCommand.Parameters.AddWithValue("@units", units.Text);
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

        private void standard_mark_Select()
        {
            SqlCommand cmd1 = new SqlCommand("SELECT thickness_mm, width_mm, length_mm FROM [dbo].[orders] where id_order = @id", sqlConnection);
            sqlConnection.Open();
            cmd1.Parameters.AddWithValue("@id", IdOrder.ToString());
            db = cmd1.ExecuteReader();
            while (db.Read())
            {
                thickness_mm = (double)db.GetValue(0);
                width_mm = (double)db.GetValue(1);
                length_mm = (double)db.GetValue(2);
            }
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[qua_certificate], [dbo].[cert_directory] WHERE [dbo].[qua_certificate].id_qua_certificate = [dbo].[cert_directory].id_qua_certificate", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                double min = Convert.ToDouble(db.GetValue(7).ToString());
                double max = Convert.ToDouble(db.GetValue(8).ToString());
                if (thickness_mm > min && thickness_mm < max && width_mm > min && width_mm < max && length_mm > min && length_mm < max) 
                {
                    standard_mark.Items.Add(db.GetValue(1));
                    product_standard.Items.Add(db.GetValue(2));
                }
                    
            }
            sqlConnection.Close();
        }
        private void standard_mark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (standard_mark.SelectedIndex > -1)
            {
                sqlConnection.Open();
                var ind = Convert.ToString(standard_mark.SelectedValue);
                var cmd = new SqlCommand("SELECT product_standard, units from qua_certificate, cert_directory WHERE qua_certificate.standard_per_mark='" + ind.ToString() + " ' AND [dbo].[qua_certificate].standard_per_mark = [dbo].[cert_directory].standard_per_mark", sqlConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product_standard.SelectedItem=reader.GetString(0);
                    units.Text = reader.GetString(1);
                }
                reader.Close();
            }
            sqlConnection.Close();
        }
    }
}