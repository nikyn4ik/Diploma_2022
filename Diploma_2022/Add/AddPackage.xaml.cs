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
using System.Globalization;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddPackage.xaml
    /// </summary>
    public partial class AddPackage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;

        int IdOrder;
        public AddPackage(int idOrder)
        {
            InitializeComponent();
            IdOrder = idOrder;
        }

        private void date_package_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_package = (DateTime)this.DatePicker.SelectedDate;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (mark_package.Text != "" && date_package.Text != "" && type_model.Text != "")
            {
                query = "UPDATE [dbo].[package] SET type_model=@type_model, mark_package=@mark_package, date_package=@date_package WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@type_model", type_model.Text);
                createCommand.Parameters.AddWithValue("@mark_package", mark_package.Text);
                createCommand.Parameters.AddWithValue("@date_package", Convert.ToDateTime(date_package.Text));
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker.SelectedDate = DateTime.Now;
        }
    }
}
