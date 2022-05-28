using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Diploma_2022.Pages;
using Diploma_2022.Models;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Diploma_2022.Add
{
    public partial class AddDelivery : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");

        int IdOrder;
        public AddDelivery(int idOrder)
        {
            InitializeComponent();
            fillComboBoxSearly_delivery();
            fillComboBoxStatus();
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
            if (DatePicker.Text != "" && early_delivery.Text != "" && status.Text != "")
            {
                query = "UPDATE [dbo].[delivery] SET date_of_delivery=@date_of_delivery, early_delivery=@early_delivery WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand.Parameters.AddWithValue("@date_of_delivery", Convert.ToDateTime(DatePicker.Text));
                createCommand.Parameters.AddWithValue("@early_delivery", early_delivery.Text);
                createCommand.ExecuteNonQuery();
                query = "UPDATE [dbo].[orders] SET status_order=@status_order WHERE id_order=@id";
                SqlCommand createCommand1 = new SqlCommand(query, sqlConnection);
                createCommand1.Parameters.AddWithValue("@status_order", status.Text);
                createCommand1.Parameters.AddWithValue("@id", IdOrder.ToString());
                createCommand1.ExecuteNonQuery();               
                update(createCommand);
                this.Close();
            }
            
            else
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
                }
        }
        private void fillComboBoxStatus()
        {
            status.Items.Add("Заказ на выполнении");
            status.Items.Add("Заказ выполнен");
        }
        private void update(SqlCommand createCommand)
        {
            createCommand.ExecuteNonQuery();
            MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
            sqlConnection.Close();
            this.Close();
        }

        private void fillComboBoxSearly_delivery()
        {
            early_delivery.Items.Add("Да");
            early_delivery.Items.Add("Нет");
        }
    }
}