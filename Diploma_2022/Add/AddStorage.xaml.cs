using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
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
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        public AddStorage()
        {
            InitializeComponent();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (namesclad.Text != "" && addresssklad.Text != "" && phonesklad.Text != "" && FIO_responsible.Text != "" && dataaddsklad.Text != "")
            {
                query = "INSERT INTO [dbo]. storage values(@name_storage, @address, @phone_storage, @FIO_responsible_person, @date_add_storage);";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@name_storage", namesclad.Text);
                createCommand.Parameters.AddWithValue("@address", addresssklad.Text);
                createCommand.Parameters.AddWithValue("@phone_storage", phonesklad.Text);
                createCommand.Parameters.AddWithValue("@FIO_responsible_person", FIO_responsible.Text);
                createCommand.Parameters.AddWithValue("@date_add_storage", Convert.ToDateTime(dataaddsklad.Text));
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

        private void dataaddsklad_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime dataaddsklad = (DateTime)this.DatePicker.SelectedDate;
        }
    }
    }
