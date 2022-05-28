using System;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Interaction logic for AddCertificates.xaml
    /// </summary>
    public partial class AddCertificates : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public AddCertificates()
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
                if (DatePicker.Text != "" && standard_per_mark.Text != "" && manufacturer.Text != "" && product_standard.Text != "")
                {
                query = "INSERT INTO qua_certificate (standard_per_mark, manufacturer, product_standard, date_add_certificate) VALUES (@standard_per_mark, @manufacturer, @product_standard, @date_add_certificate)";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@standard_per_mark", standard_per_mark.Text);
                createCommand.Parameters.AddWithValue("@manufacturer", manufacturer.Text);
                createCommand.Parameters.AddWithValue("@product_standard", product_standard.Text);
                createCommand.Parameters.AddWithValue("@date_add_certificate", Convert.ToDateTime(DatePicker.Text));
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
