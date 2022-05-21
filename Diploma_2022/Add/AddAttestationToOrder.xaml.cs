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
        public AddAttestationToOrder(int idOrder)
        {
            InitializeComponent();
            fillComboBoxStandart();
            standard_mark_Select();
            product_standard_Select();
            IdOrder = idOrder;
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
            if (standard_mark.Text != "" && access_standart.Text != "" && product_standard.Text != "" && DatePicker.Text != "")
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
                query = "UPDATE [dbo].[orders] SET id_qua_certificate=@id_sert, access_standart=@access WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@id_sert", data);
                createCommand.Parameters.AddWithValue("@access", access_standart.Text);
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

        private void fillComboBoxStandart()
        {
            access_standart.Items.Add("Да");
            access_standart.Items.Add("Нет");
        }

        private void standard_mark_Select()
        {
            SqlCommand cmd = new SqlCommand("SELECT standard_per_mark FROM [dbo].[qua_certificate]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                standard_mark.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void product_standard_Select()
        {
            SqlCommand cmd = new SqlCommand("SELECT product_standard FROM [dbo].[qua_certificate]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                product_standard.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();

        }

        private void standard_mark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (standard_mark.SelectedIndex > -1)
            {
                sqlConnection.Open();
                var ind = Convert.ToString(standard_mark.SelectedValue);
                var cmd = new SqlCommand("SELECT product_standard from qua_certificate WHERE standard_per_mark='" + ind.ToString() +" '", sqlConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product_standard.SelectedItem=reader.GetString(0);
                }
                reader.Close();
            }
            sqlConnection.Close();
        }
    }
}