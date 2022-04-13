using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace Diploma_2022.Add
{
    public partial class AddDelivery : Window
    {
        List<Models.Storage> list = new();
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        SqlDataReader db;
        public AddDelivery()
        {
            InitializeComponent();
        }
        public void showdata()
        {
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [dbo].[delivery]", sqlConnection);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            DeliveryGrid.DataContext = dt;
            DeliveryGrid.ItemsSource = dt.DefaultView;
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

        private void outpdfButton(object sender, RoutedEventArgs e)
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

        private void out_excel_button(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
                var selectedIndex = DeliveryGrid.SelectedIndex;
                if (selectedIndex != -1) ;
                else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new FileStream("Excel.xlsx", FileMode.Create);
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.Add("new sheet");
            sqlConnection.Open();

            var sql = "SELECT orders.id_order, package.id_model, package.color_package, qua_certificate.standard_per_mark, qua_certificate.access_standart, qua_certificate.product_standard, shipment.shipment_total_amount_tons, orders.consignee, storage.name_storage, package.date_package, qua_certificate.date_add_certificate, shipment.date_of_shipments, delivery.date_of_delivery FROM orders, package, qua_certificate, shipment, storage, delivery";
            var cmd = new SqlCommand(sql, sqlConnection);
            var reader = cmd.ExecuteReader();
            int count = 2;

            ws.Cells["A1"].Value = "ID заказа";
            ws.Cells["B1"].Value = "ID модель (упаковка)";
            ws.Cells["C1"].Value = "Цвет (упаковка)";
            ws.Cells["D1"].Value = "Стандарт на марку (сертификация)";
            ws.Cells["E1"].Value = "Проверка на качество (сертификация)";
            ws.Cells["F1"].Value = "Стандарты продукта (сертификация)";
            ws.Cells["G1"].Value = "Кол-во тонн (отгрузка)"; //
            ws.Cells["H1"].Value = "Грузоперевозчик (заказ)";
            ws.Cells["I1"].Value = "Склад (склад)";
            ws.Cells["J1"].Value = "Дата упаковки (упаковка)";
            ws.Cells["K1"].Value = "Дата сертификации (сертификация)";
            ws.Cells["L1"].Value = "Дата отгрузки (отгрузка)";
            ws.Cells["M1"].Value = "Дата доставки (доставка)";
            ws.Cells["N1"].Value = "Статус заказа (заказ)";
            ws.Cells["O1"].Value = "Ранняя отгрузка (доставка)";


            while (reader.Read())
            {
                ws.Cells[$"A{count}"].Value = reader.GetValue("id_order");
                ws.Cells[$"B{count}"].Value = reader.GetValue("id_model");
                ws.Cells[$"C{count}"].Value = reader.GetValue("color_package");
                ws.Cells[$"D{count}"].Value = reader.GetValue("standard_per_mark");
                ws.Cells[$"E{count}"].Value = reader.GetValue("access_standart");
                ws.Cells[$"F{count}"].Value = reader.GetValue("product_standard");
                ws.Cells[$"G{count}"].Value = reader.GetValue("shipment_total_amount_tons");
                ws.Cells[$"H{count}"].Value = reader.GetValue("consignee");
                ws.Cells[$"I{count}"].Value = reader.GetValue("name_storage");

                ws.Cells[$"J{count}"].Value = reader.GetValue("date_package");
                ws.Cells[$"J{count}"].Style.Numberformat.Format = "yyyy-mm-dd";

                ws.Cells[$"K{count}"].Value = reader.GetValue("date_add_certificate");
                ws.Cells[$"K{count}"].Style.Numberformat.Format = "yyyy-mm-dd";

                ws.Cells[$"L{count}"].Value = reader.GetValue("date_of_shipments");
                ws.Cells[$"L{count}"].Style.Numberformat.Format = "yyyy-mm-dd";


                ws.Cells[$"M{count}"].Value = reader.GetValue("date_of_delivery");
                ws.Cells[$"M{count}"].Style.Numberformat.Format = "yyyy-mm-dd";


                ws.Cells[$"N{count}"].Value = reader.GetValue("status_order");

                ws.Cells[$"O{count}"].Value = reader.GetValue("early_delivery");
                count++;
            }
            package.Save();
            MessageBox.Show("EXCEL-таблица сохранена", "Severstal Infocom");
            sqlConnection.Close();
        }

        private void product_standart_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void DeliveryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
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

        private void Button_add(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            {
                sqlConnection.Open();
                String query = "UPDATE [dbo].[delivery],[dbo].[orders] SET delivery.date_of_delivery=@date_of_delivery, delivery.early_delivery=@early_delivery, orders.status=@status_task  WHERE id_order=@id";
                SqlCommand createCommand = new SqlCommand(query, sqlConnection);
                createCommand.Parameters.AddWithValue("@date_of_delivery", DateDelivery.Text);
                createCommand.Parameters.AddWithValue("@status_task", status.Text);
                createCommand.Parameters.AddWithValue("@early_delivery", early_delivery.Text);
                createCommand.ExecuteNonQuery();
                MessageBox.Show("Сохранено!", "Severstal Infocom", MessageBoxButton.OK);
                sqlConnection.Close();
                showdata();
            }
        }

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            status.Items.Add("Да");
            status.Items.Add("Нет");
        }

        private void early_delivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            early_delivery.Items.Add("Да");
            early_delivery.Items.Add("Нет");
        }
    }
}