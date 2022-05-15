using System;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Diploma_2022
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled)==KeyStates.Toggled)
            {
                capsLabel.Visibility = Visibility.Visible;  
            }
            else
            {
                capsLabel.Visibility = Visibility.Hidden;
            }
            Keyboard.AddKeyDownHandler(Application.Current.MainWindow, HandlerSub);
        }
        private void HandlerSub(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            { capsLabel.Visibility = e.IsToggled ? Visibility.Visible : Visibility.Hidden;
            
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=localhost; Initial Catalog=diploma_db; Integrated Security=True");
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    sqlConnection.Open();

                var query = "SELECT COUNT(*) FROM [dbo].[authorization] WHERE Login=@lg AND Password=@pass";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@lg", System.Data.SqlDbType.NVarChar).Value = login.Text;
                sqlCommand.Parameters.AddWithValue("@pass", System.Data.SqlDbType.NVarChar).Value = password.Password;

                int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                    if (count == 1)
                    {
                    sqlConnection.Open();
                    var query1 = "SELECT FIO FROM [dbo].[authorization] WHERE Login=@lg AND Password=@pass";
                    SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                    sqlCommand1.Parameters.AddWithValue("@lg", System.Data.SqlDbType.NVarChar).Value = login.Text;
                    sqlCommand1.Parameters.AddWithValue("@pass", System.Data.SqlDbType.NVarChar).Value = password.Password;
                    var reader = sqlCommand1.ExecuteReader();
                    var FIO = "";
                    if (reader.Read())
                    {
                         FIO = reader["FIO"].ToString(); 
                    }
                    var Login = login.Text;
                    var Password = password.Password;
                    var window = new MainWindow(FIO);

                    MessageBox.Show(
                         "Добро пожаловать" + ", " + FIO, //login.Text,
                         "Severstal Infocom",
                         MessageBoxButton.OK,
                         MessageBoxImage.Information);
                        window.Show();
                        this.Hide();
                    }
                    else 
                    {
                    MessageBox.Show(
                            "Введен неверный логин или пароль.",
                            "Severstal Infocom", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Login_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
