using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
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
            type_model_Select();
            mark_package_Select();

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
            if (mark_package.Text != "" && DatePicker.Text != "" && type_model.Text != "")
            {  
                query = "UPDATE [dbo].[package] SET type_model=@type_model, mark_package=@mark_package, date_package=@date_package WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);                
                createCommand.Parameters.AddWithValue("@id", IdOrder.ToString());                 
                createCommand.Parameters.AddWithValue("@type_model", type_model.Text);                 
                createCommand.Parameters.AddWithValue("@mark_package", mark_package.Text);
                createCommand.Parameters.AddWithValue("@date_package", DatePicker.Text);
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

        private void type_model_Select()
        {
            SqlCommand cmd = new SqlCommand("SELECT type_model FROM [dbo].[container_package]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                type_model.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void mark_package_Select()
        {
            SqlCommand cmd = new SqlCommand("SELECT mark_package FROM [dbo].[container_package]", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                mark_package.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();

        }

        private void type_model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (type_model.SelectedIndex > -1)
            {
                sqlConnection.Open();
                var ind = Convert.ToString(type_model.SelectedValue); 
                //конвертируем полученный индекс в строку и получаем необходимый текст из выбранного индекса
                var cmd = new SqlCommand("SELECT mark_package from container_package WHERE type_model='" + ind.ToString() + " '", sqlConnection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mark_package.SelectedItem = reader.GetString(0);
                }
                reader.Close();
            }
            sqlConnection.Close();
        }
    }
}
