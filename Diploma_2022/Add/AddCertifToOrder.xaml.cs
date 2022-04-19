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
using System.Globalization;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddCertifToOrder.xaml
    /// </summary>
    public partial class AddCertifToOrder : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;

        int IdOrder;
        public AddCertifToOrder(int idOrder)
        {
            InitializeComponent();
            fillComboBoxStandart();
            IdOrder = idOrder;
        }
        private void date_add_certificate_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_add_certificate = (DateTime)this.DatePicker.SelectedDate;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (standard_mark.Text != "" && access_standart.Text != "")
            {
                query = "UPDATE [dbo].[qua_certificate] SET standard_per_mark=@standard_per_mark, access_standart=@access_standart, date_add_certificate=@date_add_certificate WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@standard_per_mark", standard_mark.Text);
                createCommand.Parameters.AddWithValue("@access_standart", access_standart.Text);
                createCommand.Parameters.AddWithValue("@date_add_certificate", Convert.ToDateTime(date_add_certificate.Text));
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                updateCert(createCommand);
            }
            else
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
            }
        }
        private void updateCert(SqlCommand createCommand)
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
    }
}
