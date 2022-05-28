using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddStorage.xaml
    /// </summary>
    public partial class AddStorage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public AddStorage()
        {
            InitializeComponent();
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
            if (namesclad.Text != "" && addresssklad.Text != "" && phonesklad.Text != "" && FIO_responsible.Text != "" && DatePicker.Text != "")
            {
                query = "INSERT INTO storage (name_storage, address, phone_storage, FIO_responsible_person, date_add_storage) VALUES (@name_storage, @address, @phone_storage, @FIO_responsible_person, @date_add_storage)";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@name_storage", namesclad.Text);
                createCommand.Parameters.AddWithValue("@address", addresssklad.Text);
                createCommand.Parameters.AddWithValue("@phone_storage", phonesklad.Text);
                createCommand.Parameters.AddWithValue("@FIO_responsible_person", FIO_responsible.Text);
                createCommand.Parameters.AddWithValue("@date_add_storage", Convert.ToDateTime(DatePicker.Text));
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
