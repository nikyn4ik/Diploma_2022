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
        List<StoragePage> list = new();


        public StoragePage()
        {
            InitializeComponent();
            Storage_DataGrid_SelectionChanged();


            var ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            SqlDataAdapter adpt;
            DataTable dt;
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        }

        private void Storage_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
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
                SqlConnection con = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM storage WHERE id_storage=@id", con);
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



        private void out_excel_button(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        //    List<StoragePage> list = new ();
        //StorageGrid.ItemsSource = list;
        } //
         private void ExportToExcel()
        {
        //    // List<StoragePage> list = new ();
        //    StorageGrid.ItemsSource = list;
        //    using var package = new ExcelPackage("Text.xlsx");

        //    var ws = package.Workbook.Worksheets.Add("new sheet");
        //    ws.Cells["A1"].Value = "id_storage";
        //    ws.Cells["B1"].Value = "name_storage";
        //    ws.Cells["C1"].Value = "address";
        //    ws.Cells["D1"].Value = "phone_storage";
        //    ws.Cells["E1"].Value = "date_of_entrance";
        //    ws.Cells["F1"].Value = "SAP_product_code";
        //    ws.Cells["G1"].Value = "remainder";

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        ws.Cells[$"A{i + 2}"].Value = list[i].id_storage;
        //        ws.Cells[$"B{i + 2}"].Value = list[i].name_storage;
        //        ws.Cells[$"C{i + 2}"].Value = list[i].address;
        //        ws.Cells[$"D{i + 2}"].Value = list[i].phone_storage;
        //        ws.Cells[$"E{i + 2}"].Value = list[i].date_of_entrance;
        //        ws.Cells[$"F{i + 2}"].Value = list[i].SAP_product_code;
        //        ws.Cells[$"G{i + 2}"].Value = list[i].remainder;
        //    }
        //    package.Save();
        //    MessageBox.Show("Excel-таблица сохранена");

        }//

        private void outpdfButton(object sender, RoutedEventArgs e)
        {
        //    using var doc = new Document();
        //    PdfWriter.GetInstance(doc, new FileStream("pdfTables.pdf", FileMode.Create));
        //    doc.Open();

        //    var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H,BaseFont.NOT_EMBEDDED);
        //    var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
        //    var table = new PdfPTable(StorageGrid.Columns.Count);// создание таблицы
        //    var cell = new PdfPCell(new Phrase("БД " + "Склад.pdf" + ", таблица№" + 1, font))// создание первой ячейки с фразой, которую мы хотим
        //    {
        //        Colspan = StorageGrid.Columns.Count,
        //        HorizontalAlignment = 1,
        //        Border = 0
        //    };
        //    table.AddCell(cell);
        //    for (int j =0; j <StorageGrid.Columns.Count; j++)//проходимся цикломпо каж.слобцу 
        //    {
        //        cell = new PdfPCell(new Phrase(StorageGrid.Columns[j].ToString(), font));
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        table.AddCell(cell);    
        //    }
        //    foreach (var storages in list)
        //    {

        //        table.AddCell(new Phrase(storages.id_storage.ToString(), font));
        //        table.AddCell(new Phrase(storages.name_storage, font));
        //        table.AddCell(new Phrase(storages.address, font));
        //        table.AddCell(new Phrase(storages.phone_storage, font));
        //        table.AddCell(new Phrase(storages.date_of_entrance.ToString(), font));
        //        table.AddCell(new Phrase(storages.SAP_product_code, font));
        //        table.AddCell(new Phrase(storages.remainder, font));
        //    }
        //    doc.Add(table);
        //    doc.Close();
        //    MessageBox.Show("Pdf-документ сохранен");
        }//
    }
    }
