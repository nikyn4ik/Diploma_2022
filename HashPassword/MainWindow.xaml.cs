using HashPassword.Properties;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashPassword
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            ApplicationContext db = new ApplicationContext();
            authorization Authorization = new authorization();

            if (string.IsNullOrWhiteSpace(Log.Text) || string.IsNullOrWhiteSpace(Pass.Password) || string.IsNullOrWhiteSpace(FIO.Text))
            {
                MessageBox.Show("Введите значения", "Severstal Infocom", MessageBoxButton.OK);
            }
            else
            {
                var log = Log.Text;
                var pass = md5.hashPassword(Pass.Password);
                var fio = FIO.Text;
                if (CheckLog())
                {
                    return;
                }
                Authorization.login = log;
                Authorization.password_hash = pass;
                Authorization.FIO = fio;
                db.authorization.Add(Authorization);
                db.SaveChanges();
                Log.Clear();
                Pass.Clear();
                FIO.Clear();
                MessageBox.Show("Пользователь успешно добавлен!", "Severstal Infocom", MessageBoxButton.OK);
            }
        }

        private Boolean CheckLog()
        {
            var log = Log.Text;
            ApplicationContext db = new ApplicationContext();
            var login_check = db.authorization.Where(p => p.login == log).ToList();
            if
                (login_check.Count > 0)
            {
                MessageBox.Show($"Пользователь уже существует. Введите другой логин!", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}

