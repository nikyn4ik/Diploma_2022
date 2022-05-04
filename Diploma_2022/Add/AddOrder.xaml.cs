using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
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
using System.Globalization;

namespace Diploma_2022.Add
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;

        int IdOrder;

        public AddOrder(int idOrder)
        {
            InitializeComponent();
            fillComboBoxStatus();
            IdOrder = idOrder;
            DatePicker.DisplayDate = DateTime.Today;
            DatePicker.Text = DateTime.Today.ToString();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                sqlConnection.Open();
                string query = "";
                if (consignee.Text != "" && status.Text != "" && DatePicker.Text != "")
                {
                    query = "UPDATE [dbo].[orders] SET consignee=@consignee, status_order=@status_order  WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@consignee", consignee.Text);
                    createCommand.Parameters.AddWithValue("@status_order", status.Text);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else if (consignee.Text != "" && status.Text == "" && status.Text != "")
                {
                    query = "UPDATE [dbo].[orders] SET consignee=@consignee WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@consignee", consignee.Text);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else if (consignee.Text == "" && DatePicker.Text != "")
                {
                    query = "UPDATE [dbo].[orders] SET status_order=@status_order WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@status_order", status.Text);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else if (consignee.Text == "" && status.Text != "" && DatePicker.Text != "")
                {
                    query = "UPDATE [dbo].[orders] SET date_of_adoption=@date_of_adoption WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@date_of_adoption", Convert.ToDateTime(DatePicker.Text));
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else 
                {
                    MessageBox.Show("Введите значения", "Severstal Infocom");
                    sqlConnection.Close();
                }
                    
            }
        }

        private void updateOrder(SqlCommand createCommand) 
        {
            createCommand.ExecuteNonQuery();
            MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
            sqlConnection.Close();
            this.Close();
        }

        private void fillComboBoxStatus() 
        {
            status.Items.Add("Заказ на выполнении");
            status.Items.Add("Заказ выполнен");
        }

        private void date_of_adoption_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime date_of_adoption = (DateTime)this.DatePicker.DisplayDate;
        }
    }
}