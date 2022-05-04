using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using Diploma_2022.Models;
using Diploma_2022.Add;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            cmd.CommandText = "SELECT * FROM [dbo].[delivery]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter deliv = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            deliv.Fill(dt);
            DeliveryGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        protected void update()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[delivery]";
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
            object item = DeliveryGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите нужную строчку", "Severstal Infocom");
            else
            {
                string ID = (DeliveryGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                var window = new Add.AddDelivery(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
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
            if (!Directory.Exists("PDF"))
                Directory.CreateDirectory("PDF");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            object item = DeliveryGrid.SelectedItem;
            string ID = (DeliveryGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("PDF\\Delivery" + ID + ".pdf", FileMode.Create));
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
        }

        private void ExportToExcel()
        {
            if (!Directory.Exists("EXCEL"))
                Directory.CreateDirectory("EXCEL");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new FileStream("EXCEL\\ORDER.xlsx", FileMode.Create);
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.Add("ORDER"); //id order
            sqlConnection.Open();

            var sql = "SELECT orders.id_order, orders.syst_c3, orders.log_c3, orders.thickness_mm, orders.width_mm, orders.length_mm, orders.name_product, orders.consignee, orders.status_order, " +
                "orders.id_payer, orders.access_standart, orders.id_qua_certificate, qua_certificate.standard_per_mark, qua_certificate.product_standard, qua_certificate.date_add_certificate, " +
                "package.mark_package, package.type_model, package.date_package, shipment.id_transport, shipment.shipment_total_amount_tons, shipment.date_of_shipments, shipment.id_storage, " +
                "transport.name_transport, transport.number_transport, storage.name_storage, storage.address, storage.phone_storage, storage.FIO_responsible_person, delivery.id_delivery, " +
                "delivery.early_delivery, delivery.date_of_delivery " +
                "FROM orders, qua_certificate, package, shipment, transport, storage, delivery";
            var cmd = new SqlCommand(sql, sqlConnection);
            var reader = cmd.ExecuteReader();
            int count = 2;

            //order
            ws.Cells["A1"].Value = "ID заказа";
            ws.Cells["B1"].Value = "ID аттестации";
            ws.Cells["C1"].Value = "ID заказчика";
            ws.Cells["D1"].Value = "СИСТ #СЗ";
            ws.Cells["E1"].Value = "ЛОГ #СЗ";
            ws.Cells["F1"].Value = "Толщина продукта";
            ws.Cells["G1"].Value = "Длина продукта"; //
            ws.Cells["H1"].Value = "Ширина продукта";
            ws.Cells["I1"].Value = "Название продукта";
            ws.Cells["J1"].Value = "Грузоперевозчик";
            ws.Cells["K1"].Value = "Статус заказа";
            ws.Cells["L1"].Value = "Пройдена проверка на качество";

            //qua_certificate
            ws.Cells["M1"].Value = "Стандарт на марку";
            ws.Cells["N1"].Value = "Стандарт продукта";
            ws.Cells["O1"].Value = "Дата аттестации";

            //transport
            ws.Cells["P1"].Value = "Транспорт";
            ws.Cells["Q1"].Value = "Номер транспорта";

            //storage
            ws.Cells["R1"].Value = "Склад";
            ws.Cells["S1"].Value = "Адрес склада";
            ws.Cells["T1"].Value = "Телефон склада";
            ws.Cells["U1"].Value = "ФИО ответственного за склад";

            //delivery
            ws.Cells["V1"].Value = "ID доставки";
            ws.Cells["W1"].Value = "Ранняя доставка";
            ws.Cells["X1"].Value = "Дата доставки";


            while (reader.Read())
            {
                //order
                ws.Cells[$"A{count}"].Value = reader.GetValue("id_order");
                ws.Cells[$"B{count}"].Value = reader.GetValue("id_qua_certificate");
                ws.Cells[$"C{count}"].Value = reader.GetValue("id_payer");
                ws.Cells[$"D{count}"].Value = reader.GetValue("syst_c3");
                ws.Cells[$"E{count}"].Value = reader.GetValue("log_c3");
                ws.Cells[$"F{count}"].Value = reader.GetValue("thickness_mm");
                ws.Cells[$"G{count}"].Value = reader.GetValue("length_mm");
                ws.Cells[$"H{count}"].Value = reader.GetValue("width_mm");
                ws.Cells[$"I{count}"].Value = reader.GetValue("name_product");
                ws.Cells[$"J{count}"].Value = reader.GetValue("consignee");
                ws.Cells[$"K{count}"].Value = reader.GetValue("status_order");
                ws.Cells[$"L{count}"].Value = reader.GetValue("access_standart");


                //qua_certificate
                ws.Cells[$"M{count}"].Value = reader.GetValue("standard_per_mark");
                ws.Cells[$"N{count}"].Value = reader.GetValue("product_standard");
                ws.Cells[$"O{count}"].Value = reader.GetValue("date_add_certificate");
                ws.Cells[$"O{count}"].Style.Numberformat.Format = "yyyy-mm-dd";

                //transport
                ws.Cells[$"P{count}"].Value = reader.GetValue("name_transport");
                ws.Cells[$"Q{count}"].Value = reader.GetValue("number_transport");

                //storage
                ws.Cells[$"R{count}"].Value = reader.GetValue("name_storage");
                ws.Cells[$"S{count}"].Value = reader.GetValue("address"); 
                ws.Cells[$"T{count}"].Value = reader.GetValue("phone_storage");
                ws.Cells[$"U{count}"].Value = reader.GetValue("FIO_responsible_person");

                //delivery
                ws.Cells[$"V{count}"].Value = reader.GetValue("id_delivery");
                ws.Cells[$"W{count}"].Value = reader.GetValue("early_delivery");
                ws.Cells[$"X{count}"].Value = reader.GetValue("date_of_delivery");
                ws.Cells[$"X{count}"].Style.Numberformat.Format = "yyyy-mm-dd";
                count++;
            }
            package.Save();
            MessageBox.Show("EXCEL-таблица сохранена", "Severstal Infocom");
            sqlConnection.Close();
        }
    }
}
