using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Diploma_2022
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            UpdateCapsLockVisibility();
            Keyboard.AddKeyDownHandler(Application.Current.MainWindow, HandlerSub);
        }

        private void HandlerSub(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            {
                UpdateCapsLockVisibility();
            }
        }

        private void UpdateCapsLockVisibility()
        {
            capsLabel.Visibility = (Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    var query = "SELECT COUNT(*) FROM [dbo].[authorizations] WHERE Login=@lg AND Password_hash=@password_hash";
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@lg", login.Text);
                        sqlCommand.Parameters.AddWithValue("@password_hash", Users.HashPassword(password.Password));

                        int count = Convert.ToInt32(sqlCommand.ExecuteScalar());

                        if (count == 1)
                        {
                            var fioQuery = "SELECT FIO FROM [dbo].[authorizations] WHERE Login=@lg";
                            using (SqlCommand fioCommand = new SqlCommand(fioQuery, sqlConnection))
                            {
                                fioCommand.Parameters.AddWithValue("@lg", login.Text);
                                string FIO = fioCommand.ExecuteScalar()?.ToString();

                                var window = new MainWindow();
                                window.lplogin.Text = FIO;

                                MessageBox.Show(
                                    $"Добро пожаловать, {FIO}",
                                    "Severstal Infocom",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                window.Show();
                                Hide();
                            }
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Login_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

    }
}
