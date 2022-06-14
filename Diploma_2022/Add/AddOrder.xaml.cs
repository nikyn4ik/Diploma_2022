using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Diploma_2022.Models;
using System.Data;

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
            IdOrder = idOrder;
            consigneeSelect();
            fillComboBoxStatus();

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
                if (Convert.ToDateTime(DatePicker.Text) < DateTime.Today)
                {
                    MessageBox.Show("Дата меньше текущей", "Severstal Infocom", MessageBoxButton.OK);
                    sqlConnection.Close();

                    return;
                }
                if (consignee.Text != "" && status.Text != "" && DatePicker.Text != "")
                {
                    query = "UPDATE [dbo].[orders] SET status_order=@status_order, date_of_adoption=@date_of_adoption WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    createCommand.Parameters.AddWithValue("@status_order", status.Text);
                    createCommand.Parameters.AddWithValue("@date_of_adoption", Convert.ToDateTime(DatePicker.Text));
                    updateOrder(createCommand);
                }
                else 
                {
                    MessageBox.Show("Введите значения", "Severstal Infocom");
                    sqlConnection.Close();
                }
                    
            }
        }

        private void consigneeSelect()
        {
            SqlCommand cmd = new SqlCommand("SELECT name_consignee FROM [dbo].[orders] WHERE id_order = @id", sqlConnection);
            sqlConnection.Open();
            cmd.Parameters.AddWithValue("@id", IdOrder.ToString());
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                consignee.Text = (string)db.GetString(0);
            }
            sqlConnection.Close();
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
            status.Items.Add("Заказ отменен");
        }
    }
}