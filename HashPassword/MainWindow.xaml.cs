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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext db = new ApplicationContext();
            authorizations Authorizations = new authorizations();
           
            if (string.IsNullOrWhiteSpace(Log.Text) || string.IsNullOrWhiteSpace(Pass.Password) || string.IsNullOrWhiteSpace(FIO.Text))
            {
                MessageBox.Show("Необходимо заполнить все данные");
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
                Authorizations.login = log;
                Authorizations.password_hash = pass;
                Authorizations.FIO = fio;
                db.authorizations.Add(Authorizations);
                db.SaveChanges();
                Log.Clear();
                Pass.Clear();
                FIO.Clear();
                MessageBox.Show("Пользователь успешно добавлен!", "Успешно.");


            }
        }



        private Boolean CheckLog()
        {
            var log = Log.Text;
            ApplicationContext db = new ApplicationContext();
            var login_check = db.authorizations.Where(p => p.login == log).ToList();
            if
                (login_check.Count > 0)
            {
                MessageBox.Show($"Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }



    }
}

