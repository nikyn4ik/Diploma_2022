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

        int IdOrder;

        Consignee SelectedIdConsignee;

        List<Consignee> Consignee = new List<Consignee>();

        public AddOrder(int idOrder)
        {
            InitializeComponent();
            fillComboBoxStatus();
            consigneeSelect();
            IdOrder = idOrder;
            DatePicker.DisplayDate = DateTime.Today;
            DatePicker.Text = DateTime.Today.ToString();
        }

        private void ChangeSelectedItems()
        {
            var consigneName = consignee.Text.Substring(consignee.Text.IndexOf("| ") + 2);
            SelectedIdConsignee = Consignee.Find(x => x.name_consignee.Contains(consigneName));
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
                    ChangeSelectedItems();
                    query = "UPDATE [dbo].[orders] SET id_consignee=@id_consignee,name_consignee=@name_consignee, status_order=@status_order WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    createCommand.Parameters.AddWithValue("@id_consignee", SelectedIdConsignee.id_consignee);
                    createCommand.Parameters.AddWithValue("@name_consignee", SelectedIdConsignee.name_consignee);
                    createCommand.Parameters.AddWithValue("@status_order", status.Text);
                    updateOrder(createCommand);
                }
                else if (consignee.Text != "" && status.Text == "" && status.Text != "")
                {
                    ChangeSelectedItems();
                    query = "UPDATE [dbo].[orders] SET id_consignee=@id_consignee,name_consignee=@name_consignee, WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@id_consignee", SelectedIdConsignee.id_consignee);
                    createCommand.Parameters.AddWithValue("@name_consignee", SelectedIdConsignee.name_consignee);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else if (consignee.Text == "" && DatePicker.Text != "")
                {
                    ChangeSelectedItems();
                    query = "UPDATE [dbo].[orders] SET status_order=@status_order WHERE id_order=@id";
                    SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                    createCommand.Parameters.AddWithValue("@status_order", status.Text);
                    createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());
                    updateOrder(createCommand);
                }
                else if (consignee.Text == "" && status.Text != "" && DatePicker.Text != "")
                {
                    ChangeSelectedItems();
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

        private void consigneeSelect()
        {
            SqlCommand cmd = new SqlCommand("SELECT name_consignee, FIO_consignee, id_consignee FROM [dbo].[consignee]", sqlConnection);
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                consignee.Items.Add(ds.Tables[0].Rows[i][1] + " | " + ds.Tables[0].Rows[i][0]);
                var items = new Consignee()
                {
                    id_consignee = (int)ds.Tables[0].Rows[i][2],
                    name_consignee = ds.Tables[0].Rows[i][0].ToString(),
            };
                Consignee.Add(items);

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

        //private void date_of_adoption_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    DateTime date_of_adoption = (DateTime)this.DatePicker.DisplayDate;
        //}
    }
}