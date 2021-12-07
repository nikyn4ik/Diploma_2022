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
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    sqlConnection.Open();

                String query = "SELECT COUNT(*) FROM [dbo].[authorization] WHERE Login=@lg AND Password=@pass"; //@1

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@lg", System.Data.SqlDbType.NVarChar).Value = login.Text;
                sqlCommand.Parameters.AddWithValue("@pass", System.Data.SqlDbType.NVarChar).Value = password.Password;

                int count = Convert.ToInt32(sqlCommand.ExecuteScalar());

                if (count == 1)
                {

                    MessageBox.Show("Welcome " + login.Text, "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or password incorrect.", "Username or password incorrect.", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }
        private void Login_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }



        //private void Tooltip(object sender, RoutedEventArgs e) //caps
        //{
        //    {
        //        if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
        //        {
        //            if (PasswordBox.ToolTip == null)
        //            {
        //                ToolTip tt = new ToolTip();
        //                tt.Content = "Warning: CapsLock is on";
        //                tt.PlacementTarget = sender as UIElement; ;
        //                tt.Placement = PlacementMode.Custom;
        //                PasswordBox.ToolTip = tt;
        //                tt.IsOpen = true;
        //            }
        //        }
        //        else
        //        {
        //            var currentToolTip = PasswordBox.ToolTip as ToolTip;
        //            if (currentToolTip != null)
        //            {
        //                currentToolTip.IsOpen = false;
        //            }

        //            PasswordBox.ToolTip = null;
        //        }
        //    }
        //}
        /*      <Button
           x:Name="ss"
           Width="185"
           Height="30"
           Margin="0,-30,-320,0"
           Click="Tooltip"
           Content="Tooltip">


           <Button.ToolTip>
               <ToolTip>
                   <Label
                       Content = "CapsLock Enabled"
                       Foreground="White"
                       Visibility="{Binding CapsVisibility, Mode=TwoWay}" />
               </ToolTip>
           </Button.ToolTip>
       </Button> */
    }
}
