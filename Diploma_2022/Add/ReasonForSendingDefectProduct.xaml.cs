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
    /// Логика взаимодействия для ReasonForSendingDefectProduct.xaml
    /// </summary>
    public partial class ReasonForSendingDefectProduct : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        int IdOrder;
        public ReasonForSendingDefectProduct(int idOrder)
        {
            InitializeComponent();
            IdOrder = idOrder;
        }

        private void date_of_defect_product_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_of_defect_product = (DateTime)this.DatePicker.SelectedDate;
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "";
            if (reasonbrak.Text != "" && date_of_defect_product.Text != "")
            {
                query = "UPDATE [dbo].[defect_product] SET reasons_for_sending=@reasons_for_sending, product_for_sending=@product_for_sending WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@standard_per_mark", reasonbrak.Text);
                createCommand.Parameters.AddWithValue("@product_for_sending", Convert.ToDateTime(date_of_defect_product.Text));
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
    }
}
