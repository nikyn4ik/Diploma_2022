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
        //public IObservableCollection<Certificates> Cert { get; set; } = new();
        int IdOrder;
        public AddAttestationToOrder(int idOrder)
        {
            InitializeComponent();
            fillComboBoxStandart();
            standard_mark_Select();
            product_standard_Select();
            IdOrder = idOrder;
        }
        private void date_add_attest_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_add_attest = (DateTime)this.DatePicker.SelectedDate;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (standard_mark.Text != "" && access_standart.Text != "" && product_standard.Text != "" && date_add_attest.Text != "")
            {
                query = "UPDATE [dbo].[orders] SET standard_per_mark=@standard_per_mark, product_standard=@product_standard,access_standart=@access_standart, date_add_certificate=@date_add_certificate WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@standard_per_mark", standard_mark.Text);
                createCommand.Parameters.AddWithValue("@product_standard", product_standard.Text);
                createCommand.Parameters.AddWithValue("@access_standart", access_standart.Text);
                createCommand.Parameters.AddWithValue("@date_add_certificate", Convert.ToDateTime(date_add_attest.Text));
                updateAtest(createCommand);
            }
            else
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
            }
        }
        private void updateAtest(SqlCommand createCommand)
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
    }
}
