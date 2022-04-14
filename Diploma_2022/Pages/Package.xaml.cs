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
    /// Логика взаимодействия для Package.xaml
    /// </summary>
    public partial class Package : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");
        List<Models.Package> list = new();
        public Package()
        {
            InitializeComponent();
            Package_DataGrid_SelectionChanged();
            var ppp = CodePagesEncodingProvider.Instance;   
            Encoding.RegisterProvider(ppp);     
        }
        private void Package_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[package]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter pack = new SqlDataAdapter(cmd);
            pack.Fill(dt);
            PackageGrid.ItemsSource = dt.DefaultView;
        }
        private void Buttontoshipment(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PackageGrid.SelectedItems.Count > 0)
                {
                    sqlConnection.Open();
                    DataRowView drv = (DataRowView)PackageGrid.SelectedItem;
                    string ID_Orders = drv.Row[0].ToString();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[shipment] (id_order) ((SELECT id_order FROM package WHERE id_order=@id))", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", ID_Orders);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Заказ успешно отправлен в отгрузку!", "Severstal Infocom");
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Данный заказ ранее уже был отправлен в отгрузку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void outButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = PackageGrid.SelectedIndex;
            if (selectedIndex != -1)
             PDFOut(selectedIndex);
            else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
        private void PDFOut(int cellId)
        {
            object item = PackageGrid.SelectedItem;
            string ID = (PackageGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("Package" + ID + ".pdf", FileMode.Create));
            doc.Open();

            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
            var table = new PdfPTable(PackageGrid.Columns.Count);// создание таблицы
            var cell = new PdfPCell(new Phrase("PACKAGE ORDER " + " # " + ID))// создание первой ячейки с фразой, которую мы хотим
            {
                Colspan = PackageGrid.Columns.Count,
                HorizontalAlignment = 1,
                Border = 0
            };
            table.AddCell(cell);

            for (int j = 0; j < PackageGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                cell = new PdfPCell(new Phrase(PackageGrid.Columns[j].Header.ToString()));
                var headerCell = cell.Phrase[0].ToString();
                cell = new PdfPCell(new Phrase(headerCell, font));
                cell.BackgroundColor = BaseColor.BLACK;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }
            for (int j = 0; j < PackageGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                string sr = (PackageGrid.SelectedCells[j].Column.GetCellContent(item) as TextBlock).Text;
                cell = new PdfPCell(new Phrase(sr, font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }

            doc.Add(table);
            doc.Close();
            MessageBox.Show("Pdf-документ сохранен", "Severstal Infocom");

        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Add.AddPackage taskWindow = new Add.AddPackage();
            taskWindow.Show();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[package] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter pack = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("package");
                pack.Fill(dt);
                PackageGrid.ItemsSource = dt.DefaultView;
                pack.Update(dt);
                cmds.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            PackageGrid.Items.Refresh();
        }
    }
    }

