using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;

namespace Diploma_2022.Pages
{
    public partial class DeliveryPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public DeliveryPage()
        {
            InitializeComponent();
            DeliveryGrid_SelectionChanged();
        }

        private void DeliveryGrid_SelectionChanged()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[delivery], [dbo].[shipment]", sqlConnection))
                {
                    SqlDataAdapter deliv = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("diploma_db");
                    deliv.Fill(dt);
                    DeliveryGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[delivery], [dbo].[shipment]", sqlConnection))
                {
                    SqlDataAdapter deliv = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("diploma_db");
                    deliv.Fill(dt);
                    DeliveryGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new DeliveryPage();
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить доставку?", "Sevestal Infocom", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        MessageBox.Show("Заявка НЕ была отменена", "Severstal Infocom");
                        break;
                    case MessageBoxResult.Yes:
                        MessageBox.Show("Заявка отменена", "Severstal Infocom");
                        this.Hide();
                        DataRowView drv = (DataRowView)DeliveryGrid.SelectedItem;
                        string delivery = drv.Row[0].ToString();
                        sqlConnection.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM delivery WHERE id_delivery=@id", sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@id", delivery);
                            cmd.ExecuteNonQuery();
                        }
                        DeliveryGrid_SelectionChanged();
                        window.Show();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            DeliveryGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmdText = "SELECT * FROM [dbo].[delivery] WHERE id_delivery like @id";
                using (SqlCommand sqlcom = new SqlCommand(cmdText, sqlConnection))
                {
                    sqlcom.Parameters.AddWithValue("@id", pole.Text + "%");

                    SqlDataAdapter deliv = new SqlDataAdapter(sqlcom);
                    DataTable dt = new DataTable("delivery");
                    deliv.Fill(dt);
                    DeliveryGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
