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
    /// Логика взаимодействия для ReasonForSendingDefectProduct.xaml
    /// </summary>
    public partial class ReasonForSendingDefectProduct : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        int IdOrder;
        public string FIO;
        public ReasonForSendingDefectProduct(int idOrder, string fIO_work)
        {
            InitializeComponent();
            IdOrder = idOrder;
            FIO = fIO_work;
            DatePicker.DisplayDate = DateTime.Today;
            DatePicker.Text = DateTime.Today.ToString();
        }

        private void date_of_defect_product_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_of_defect_product = (DateTime)this.DatePicker.SelectedDate;
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
            if (reasonbrak.Text != "" && DatePicker.Text != "")
            {
                query = "INSERT INTO defect_product (id_order, FIO, reasons_for_sending, product_for_sending) VALUES (@id, @FIO, @reasons_for_sending, @product_for_sending)";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@FIO", FIO.ToString());
                createCommand.Parameters.AddWithValue("@reasons_for_sending", reasonbrak.Text);
                createCommand.Parameters.AddWithValue("@product_for_sending", Convert.ToDateTime(DatePicker.Text));
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
