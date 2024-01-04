using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace Diploma_2022.Windows
{
    public partial class AddDelivery : Window
    {
        List<Models.Storage> list = new();
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        SqlDataReader db;

        public AddDelivery()
        {
            InitializeComponent();
            deliv();
            delivstor();
            delivDone();
        }
        public void showdata()
        {
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[delivery]", sqlConnection);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            DeliveryGrid.DataContext = dt;
            DeliveryGrid.ItemsSource = dt.DefaultView;
        }

        private void product_standart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deliv();
        }
        private void deliv()
        {
            SqlCommand cmd = new SqlCommand("SELECT product_standard FROM [dbo]. delivery", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                product_standart.Items.Add(db.GetValue(0));
            }
            product_standart.Items.Add("1");
            sqlConnection.Close();
        }

        private void Storage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delivstor();
        }
        private void delivstor()
        {
            SqlCommand cmd = new SqlCommand("SELECT name_storage FROM [dbo].storage", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();
            while (db.Read())
            {
                Storage.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void DateDelivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sqlConnection.Open();
            String query = "INSERT INTO [dbo]. delivery values(@date_of_delivery);";
            SqlCommand createCommand = new SqlCommand(query, sqlConnection);
            createCommand.Parameters.AddWithValue("@date_of_delivery", DateDelivery.Text);
            createCommand.ExecuteNonQuery();
            MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
            sqlConnection.Close();
            showdata();
        }

        private void Done_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delivDone();
        }

        private void delivDone()
        {
            SqlCommand cmd = new SqlCommand("SELECT done_delivery FROM  [dbo].confirmation", sqlConnection);
            sqlConnection.Open();
            cmd.CommandType = CommandType.Text;
            db = cmd.ExecuteReader();

            while (db.Read())
            {
                Done.Items.Add(db.GetValue(0));
            }
            sqlConnection.Close();
        }

        private void outpdfButton(object sender, RoutedEventArgs e)
        {
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("Order.pdf", FileMode.Create));
            sqlConnection.Open();
            doc.Open();

            var sql = "SELECT * FROM [dbo]. storage";
            var cmd = new SqlCommand(sql, sqlConnection);

            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
            var table = new PdfPTable(DeliveryGrid.Columns.Count);// создание таблицы
            var cell = new PdfPCell(new Phrase("БД " + "Склад.pdf" + ", таблица№" + 1, font))// создание первой ячейки с фразой, которую мы хотим
            {
                Colspan = DeliveryGrid.Columns.Count,
                HorizontalAlignment = 1,
                Border = 0
            };
            table.AddCell(cell);
            for (int j = 0; j < DeliveryGrid.Columns.Count; j++)//проходимся циклом по каж.слобцу 
            {
                cell = new PdfPCell(new Phrase(DeliveryGrid.Columns[j].ToString(), font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }
            foreach (var storages in list)
            {
                table.AddCell(new Phrase(storages.id_storage.ToString(), font));
                table.AddCell(new Phrase(storages.name_storage, font));
                table.AddCell(new Phrase(storages.address, font));
                table.AddCell(new Phrase(storages.phone_storage, font));
                table.AddCell(new Phrase(storages.remainder, font));
                table.AddCell(new Phrase(storages.date_add_storage.ToString(), font));
            }

            doc.Add(table);
            doc.Close();
            MessageBox.Show("Pdf-документ сохранен", "Severstal Infocom");
            sqlConnection.Close();
        }

        private void out_excel_button(object sender, RoutedEventArgs e)
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

        private void product_standart_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void DeliveryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery]";
            cmd.Connection = sqlConnection;

            SqlDataAdapter deliverygrid = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            deliverygrid.Fill(dt);
            DeliveryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void DateDelivery_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime DateDelivery = (DateTime)this.DatePicker.SelectedDate;
        }
    }
}