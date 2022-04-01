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
using System.Data;
using System.IO;
using System.Configuration;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для DeliveryPage.xaml
    /// </summary>
        public partial class DeliveryPage : Window
        {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public DeliveryPage()
        {
            InitializeComponent();
            DeliveryGrid_SelectionChanged();
        }

        private void DeliveryGrid_SelectionChanged()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery], [dbo].[shipment]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter deliv = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            deliv.Fill(dt);
            DeliveryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void UpdButton(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery], [dbo].[shipment]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter deliv = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            deliv.Fill(dt);
            DeliveryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        //private void cancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var window = new DeliveryPage();
        //    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить доставку?", "Sevestal Infocom", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        //    switch (result)
        //    {
        //        case MessageBoxResult.No:
        //            MessageBox.Show("Заявка НЕ была отменена", "Severstal Infocom");
        //            break;
        //        case MessageBoxResult.Yes:
        //            MessageBox.Show("Заявка отменена", "Severstal Infocom");
        //            this.Hide();
        //            DataRowView drv = (DataRowView)DeliveryGrid.SelectedItem;
        //            string delivery = drv.Row[0].ToString();
        //            sqlConnection.Open();
        //            SqlCommand cmd = new SqlCommand("DELETE FROM delivery WHERE id_delivery=@id", sqlConnection);
        //            cmd.Parameters.AddWithValue("@id", delivery);
        //            cmd.ExecuteNonQuery();
        //            DeliveryGrid_SelectionChanged();
        //            window.Show();
        //            break;
        //        case MessageBoxResult.Cancel:
        //            break;
        //    }
        //}

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            DeliveryGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[delivery] WHERE id_delivery like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter deliv = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("delivery");
                deliv.Fill(dt);
                DeliveryGrid.ItemsSource = dt.DefaultView;
                deliv.Update(dt);
                cmds.Close();
            }
        }

        private void editButton(object sender, RoutedEventArgs e)
        {
            Hide();
            var window = new Windows.AddDelivery();
            window.ShowDialog();
            Show();
        }

        private void outButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
