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
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            var window = new Add.AddDelivery();
            window.ShowDialog();
            Show();
        }

        private void outButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = DeliveryGrid.SelectedIndex;
            if (selectedIndex != -1)
                PDFOut(selectedIndex);
            else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
        private void PDFOut(int cellId)
        {
            object item = DeliveryGrid.SelectedItem;
            string ID = (DeliveryGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("Delivery" + ID + ".pdf", FileMode.Create));
            doc.Open();

            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
            var table = new PdfPTable(DeliveryGrid.Columns.Count);// создание таблицы
            var cell = new PdfPCell(new Phrase("DELIVERY ORDER " + " # " + ID))// создание первой ячейки с фразой, которую мы хотим
            {
                Colspan = DeliveryGrid.Columns.Count,
                HorizontalAlignment = 1,
                Border = 0
            };
            table.AddCell(cell);

            for (int j = 0; j < DeliveryGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                cell = new PdfPCell(new Phrase(DeliveryGrid.Columns[j].Header.ToString()));
                var headerCell = cell.Phrase[0].ToString();
                cell = new PdfPCell(new Phrase(headerCell, font));
                cell.BackgroundColor = BaseColor.BLACK;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }
            for (int j = 0; j < DeliveryGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                string sr = (DeliveryGrid.SelectedCells[j].Column.GetCellContent(item) as TextBlock).Text;
                cell = new PdfPCell(new Phrase(sr, font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }

            doc.Add(table);
            doc.Close();
            MessageBox.Show("PDF-документ сохранен", "Severstal Infocom");

        }
    }
}
