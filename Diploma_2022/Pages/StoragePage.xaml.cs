//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Data.SqlClient;
//using System.Data;
//using System.IO;
//using System.Configuration;
//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    public partial class StoragePage : Window
    {
        // string connectionString;
        // SqlDataAdapter Dstorage;
        //DataTable dt;
        List<Models.Storage> list = new();
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        public StoragePage()
        {
            InitializeComponent();
            Storage_DataGrid_SelectionChanged();
            //StorageGrid.ItemsSource = list;
           // var ppp = CodePagesEncodingProvider.Instance;
            //Encoding.RegisterProvider(ppp);

            
            // SqlDataAdapter adpt;
            //DataTable dt;
        }

        private void Storage_DataGrid_SelectionChanged()
        {
           // SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[storage]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter storage = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            storage.Fill(dt);
            StorageGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            Windows.AddStorage taskWindow = new Windows.AddStorage();
            taskWindow.Show();
            Storage_DataGrid_SelectionChanged();
        }

        private void deleteButton(object sender, RoutedEventArgs e)
        {
            if (StorageGrid.SelectedItems.Count > 0)
            {
                DataRowView drv = (DataRowView)StorageGrid.SelectedItem;
                string storage = drv.Row[0].ToString();
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", storage);
                cmd.ExecuteNonQuery();
                Storage_DataGrid_SelectionChanged();
            }
        }

        private void UpdButton(object sender, RoutedEventArgs e)
        {
            StorageGrid.Items.Refresh();

        }
        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[storage] WHERE id_storage like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter storages = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("storage");
                storages.Fill(dt);
                StorageGrid.ItemsSource = dt.DefaultView;
                storages.Update(dt);
                cmds.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            StorageGrid.Items.Refresh();
        }

        private void StorageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void out_excel_button(object sender, RoutedEventArgs e) //
        {
            ExportToExcel();
        } 
        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new FileStream("Excel.xlsx", FileMode.Create);
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.Add("new sheet");
            sqlConnection.Open();

            var sql = "SELECT * FROM storage";
            var cmd = new SqlCommand(sql, sqlConnection);
            var reader = cmd.ExecuteReader();
            int count = 2;

            ws.Cells["A1"].Value = "Id склад";
            ws.Cells["B1"].Value = "Имя";
            ws.Cells["C1"].Value = "Адрес";
            ws.Cells["D1"].Value = "Телефон";
            ws.Cells["E1"].Value = "Дата";
            ws.Cells["F1"].Value = "SAP код";
            ws.Cells["G1"].Value = "Остаток";

            while (reader.Read())
            {
                ws.Cells[$"A{count}"].Value = reader.GetValue("id_storage");
                ws.Cells[$"B{count}"].Value = reader.GetValue("name_storage");
                ws.Cells[$"C{count}"].Value = reader.GetValue("address");
                ws.Cells[$"D{count}"].Value = reader.GetValue("phone_storage");
                ws.Cells[$"E{count}"].Value = reader.GetValue("date_of_entrance");
                ws.Cells[$"E{count}"].Style.Numberformat.Format = "yyyy-mm-dd";
                ws.Cells[$"F{count}"].Value = reader.GetValue("SAP_product_code");
                ws.Cells[$"G{count}"].Value = reader.GetValue("remainder");
                count++;
            }
            package.Save();
            MessageBox.Show("Excel-таблица сохранена", "Severstal Infocom");
            sqlConnection.Close();
        }

        private void outpdfButton(object sender, RoutedEventArgs e) //
        {
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("pdfTables.pdf", FileMode.Create));
            sqlConnection.Open();
            doc.Open();

            var sql = "SELECT * FROM storage";
            var cmd = new SqlCommand(sql, sqlConnection);

            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
            var table = new PdfPTable(StorageGrid.Columns.Count);// создание таблицы
            var cell = new PdfPCell(new Phrase("БД " + "Склад.pdf" + ", таблица№" + 1, font))// создание первой ячейки с фразой, которую мы хотим
            {
                Colspan = StorageGrid.Columns.Count,
                HorizontalAlignment = 1,
                Border = 0
            };
            table.AddCell(cell);
            for (int j = 0; j < StorageGrid.Columns.Count; j++)//проходимся циклом по каж.слобцу 
            {
                cell = new PdfPCell(new Phrase(StorageGrid.Columns[j].ToString(), font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }
            foreach (var storages in list)
            {
                table.AddCell(new Phrase(storages.id_storage.ToString(), font));
                table.AddCell(new Phrase(storages.name_storage, font));
                table.AddCell(new Phrase(storages.address, font));
                table.AddCell(new Phrase(storages.phone_storage, font));
                table.AddCell(new Phrase(storages.date_of_entrance.ToString(), font));
                table.AddCell(new Phrase(storages.SAP_product_code, font));
                table.AddCell(new Phrase(storages.remainder, font));
            }

            doc.Add(table);
            doc.Close();
            MessageBox.Show("Pdf-документ сохранен", "Severstal Infocom");
            sqlConnection.Close();
        }
    }
    }
