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
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

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
            cmd.CommandText = "SELECT delivery.id_delivery, orders.id_order, orders.name_product, delivery.id_storage, delivery.early_delivery, delivery.date_of_delivery FROM [dbo].[delivery] LEFT JOIN orders ON orders.id_order=delivery.id_order";
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
            cmd.CommandText = "SELECT delivery.id_delivery, orders.id_order, orders.name_product, delivery.id_storage, delivery.early_delivery, delivery.date_of_delivery FROM [dbo].[delivery] LEFT JOIN orders ON orders.id_order=delivery.id_order";
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
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[delivery] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter deliv = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("delivery");
                deliv.Fill(dt);
                DeliveryGrid.ItemsSource = dt.DefaultView;
                deliv.Update(dt);
                cmds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не найдено в системе.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void editButton(object sender, RoutedEventArgs e)
        {
            object item = DeliveryGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите нужную строчку", "Severstal Infocom");
            else
            {
                string ID = (DeliveryGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new AddDelivery(Convert.ToInt32(ID));
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
        private void PDFOut(int ID_orders)
        {
            object item = DeliveryGrid.SelectedItem;
            DataRowView drv = (DataRowView)DeliveryGrid.SelectedItem;
            string ID_Orders = drv.Row[1].ToString();
            string ID = (DeliveryGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT *, (SELECT FIO as 'фио плательщик' FROM payer where id_payer = orders.id_payer), (SELECT standard_per_mark as 'на марку стандарт серт' FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate), (SELECT product_standard as 'продукт стандарт серт' FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate), (SELECT date_add_certificate as 'дата серт' FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate), (SELECT name_storage as 'Наименование склад' FROM storage where id_storage = shipment.id_storage), (SELECT address as 'адрес склад' FROM storage where id_storage = shipment.id_storage), (SELECT FIO_responsible_person as 'Фио ответственн склад' FROM storage where id_storage=shipment.id_storage), (SELECT date_add_storage as 'Дата склад' FROM storage where id_storage=shipment.id_storage), (SELECT phone_storage as 'номер склад' FROM storage where id_storage=shipment.id_storage), (SELECT name_transport as 'Наименование' FROM transport where id_transport=shipment.id_transport), (SELECT number_transport as 'Номер транспорта' FROM transport where id_transport=shipment.id_transport), (SELECT FIO_consignee as 'грузоперевозчик' FROM consignee where id_consignee=orders.id_consignee)  FROM orders INNER JOIN package ON orders.id_order = package.id_order INNER JOIN shipment ON orders.id_order = shipment.id_order INNER JOIN delivery ON orders.id_order = delivery.id_order WHERE orders.id_order = @id";
            cmd.Parameters.AddWithValue("@id", ID_Orders);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader dr = null;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int i = 0;

                string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                Document doc1 = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc1, new FileStream("PDF\\Заказ № " + ID + ".pdf", FileMode.Create));
                doc1.Open();

                System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("Images/SeverstalPDF.jpg", UriKind.Relative));

                var img = System.Drawing.Image.FromStream(res.Stream);

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(img, BaseColor.BLACK);
                jpg.ScaleToFit(300f, 280f);
                jpg.SpacingBefore = 10f;
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_CENTER;

                Chunk c1 = new Chunk(" " + "                                        Сервисный металлоцентр СМЦ-Колпино", font);
                Chunk c2 = new Chunk(" " + "                         ", font);
                Chunk c3 = new Chunk(" " + "Россия, Колпино, Санкт-Петербург, Территория промзоны 'Ижорские завод', д.90, лит. Д, помещение 1-Н", font);
                Chunk c4 = new Chunk(" " + "                         ", font);
                Chunk c5 = new Chunk(" " + "                                                        Информация о заказе", font);
                Chunk c6 = new Chunk(" " + "", font);
                Chunk c7 = new Chunk(" " + "ID заказа:   " + dr[0], font);
                Chunk c8 = new Chunk(" " + "Аттестация пройдена:  " + dr[16], font);
                Chunk c9 = new Chunk(" " + "Заказчик:  " + dr[33], font);
                Chunk c10 = new Chunk(" " + "СИСТ-С3:  " + dr[1], font);
                Chunk c11 = new Chunk(" " + "ЛОГ-СЗ:  " + dr[2], font);
                Chunk c12 = new Chunk(" " + "Продукт:  " + dr[9], font);
                Chunk c13 = new Chunk(" " + "Толщина продукта:  " + dr[6], font);
                Chunk c14 = new Chunk(" " + "Длина продукта:  " + dr[7], font);
                Chunk c15 = new Chunk(" " + "Ширина продукта:  " + dr[8], font);
                Chunk c16 = new Chunk(" " + "Грузоперевозчик:  " + dr[44], font);
                Chunk c17 = new Chunk(" " + "Статус заказа:  " + dr[11], font);
                Chunk c18 = new Chunk(" " + "                         ", font);
                Chunk c19 = new Chunk("    " + "                                                              Сертификация", font);
                Chunk c20 = new Chunk(" " + "Стандарт на марку:  " + dr[34], font);
                Chunk c21 = new Chunk(" " + "Стандарт продукта:  " + dr[35], font);
                Chunk c22 = new Chunk(" " + "Дата аттестации:  " + dr[36], font);
                Chunk c23 = new Chunk(" " + "                         ", font);
                Chunk c24 = new Chunk("       " + "                                                              Транспорт", font);
                Chunk c25 = new Chunk(" " + "Транспорт:  " + dr[42], font);
                Chunk c26 = new Chunk(" " + "Номер:  " + dr[43], font);
                Chunk c27 = new Chunk(" " + "                         ", font);
                Chunk c28 = new Chunk("         " + "                                                                Склад", font);
                Chunk c29 = new Chunk(" " + "Склад:  " + dr[37], font);
                Chunk c30 = new Chunk(" " + "Адрес:  " + dr[38], font);
                Chunk c31 = new Chunk(" " + "Телефон:  " + dr[41], font);
                Chunk c32 = new Chunk(" " + "ФИО ответственного за склад:  " + dr[39], font);
                Chunk c33 = new Chunk(" " + "                         ", font);
                Chunk c34 = new Chunk("       " + "                                                                Доставка", font);
                Chunk c35 = new Chunk(" " + "Ранняя доставка:  " + dr[31], font);
                Chunk c36 = new Chunk(" " + "Дата доставки:  " + dr[30], font);
                Chunk c37 = new Chunk(" " + " ", font);
                Chunk c38 = new Chunk(" " + "Ф.И.О. Получателя (разборчиво)      __________________        __________________", font);
                Chunk c39 = new Chunk(" " + "                                                                          Ф.И.О                                подпись                      ", font);

                Phrase ph1 = new Phrase();
                ph1.Font = new Font(Font.FontFamily.TIMES_ROMAN, 35, Font.BOLD, BaseColor.BLACK);
                Phrase ph2 = new Phrase();
                Phrase ph3 = new Phrase();
                ph3.Font = new Font(Font.FontFamily.TIMES_ROMAN, 15, Font.BOLD, BaseColor.BLACK);
                Phrase ph4 = new Phrase();
                Phrase ph5 = new Phrase();
                ph5.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph6 = new Phrase();
                Phrase ph7 = new Phrase();
                Phrase ph8 = new Phrase();
                Phrase ph9 = new Phrase();
                Phrase ph10 = new Phrase();
                Phrase ph11 = new Phrase();
                Phrase ph12 = new Phrase();
                Phrase ph13 = new Phrase();
                Phrase ph14 = new Phrase();
                Phrase ph15 = new Phrase();
                Phrase ph16 = new Phrase();
                Phrase ph17 = new Phrase();
                Phrase ph18 = new Phrase();
                Phrase ph19 = new Phrase();
                ph19.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph20 = new Phrase();
                Phrase ph21 = new Phrase();
                Phrase ph22 = new Phrase();
                Phrase ph23 = new Phrase();
                Phrase ph24 = new Phrase();
                ph24.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph25 = new Phrase();
                Phrase ph26 = new Phrase();
                Phrase ph27 = new Phrase();
                Phrase ph28 = new Phrase();
                ph28.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph29 = new Phrase();
                Phrase ph30 = new Phrase();
                Phrase ph31 = new Phrase();
                Phrase ph32 = new Phrase();
                Phrase ph33 = new Phrase();
                ph33.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph34 = new Phrase();
                ph34.Font = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
                Phrase ph35 = new Phrase();
                Phrase ph36 = new Phrase();
                Phrase ph37 = new Phrase();
                Phrase ph38 = new Phrase();
                Phrase ph39 = new Phrase();

                ph1.Add(c1);
                ph2.Add(c2);
                ph3.Add(c3);
                ph4.Add(c4);
                ph5.Add(c5);
                ph6.Add(c6);
                ph7.Add(c7);
                ph8.Add(c8);
                ph9.Add(c9);
                ph10.Add(c10);
                ph11.Add(c11);
                ph12.Add(c12);
                ph13.Add(c13);
                ph14.Add(c14);
                ph15.Add(c15);
                ph16.Add(c16);
                ph17.Add(c17);
                ph18.Add(c18);
                ph19.Add(c19);
                ph20.Add(c20);
                ph21.Add(c21);
                ph22.Add(c22);
                ph23.Add(c23);
                ph24.Add(c24);
                ph25.Add(c25);
                ph26.Add(c26);
                ph27.Add(c27);
                ph28.Add(c28);
                ph29.Add(c29);
                ph30.Add(c30);
                ph31.Add(c31);
                ph32.Add(c32);
                ph33.Add(c33);
                ph34.Add(c34);
                ph35.Add(c35);
                ph36.Add(c36);
                ph37.Add(c37);
                ph38.Add(c38);
                ph39.Add(c39);

                Paragraph p1 = new Paragraph();
                p1.Add(ph1);
                Paragraph p2 = new Paragraph();
                p2.Add(ph2);
                Paragraph p3 = new Paragraph();
                p3.Add(ph3);
                Paragraph p4 = new Paragraph();
                p4.Add(ph4);
                Paragraph p5 = new Paragraph();
                p5.Add(ph5);
                Paragraph p6 = new Paragraph();
                p6.Add(ph6);
                Paragraph p7 = new Paragraph();
                p7.Add(ph7);
                Paragraph p8 = new Paragraph();
                p8.Add(ph8);
                Paragraph p9 = new Paragraph();
                p9.Add(ph9);
                Paragraph p10 = new Paragraph();
                p10.Add(ph10);
                Paragraph p11 = new Paragraph();
                p11.Add(ph11);
                Paragraph p12 = new Paragraph();
                p12.Add(ph12);
                Paragraph p13 = new Paragraph();
                p13.Add(ph13);
                Paragraph p14 = new Paragraph();
                p14.Add(ph14);
                Paragraph p15 = new Paragraph();
                p15.Add(ph15);
                Paragraph p16 = new Paragraph();
                p16.Add(ph16);
                Paragraph p17 = new Paragraph();
                p17.Add(ph17);
                Paragraph p18 = new Paragraph();
                p18.Add(ph18);
                Paragraph p19 = new Paragraph();
                p19.Add(ph19);
                Paragraph p20 = new Paragraph();
                p20.Add(ph20);
                Paragraph p21 = new Paragraph();
                p21.Add(ph21);
                Paragraph p22 = new Paragraph();
                p22.Add(ph22);
                Paragraph p23 = new Paragraph();
                p23.Add(ph23);
                Paragraph p24 = new Paragraph();
                p24.Add(ph24);
                Paragraph p25 = new Paragraph();
                p25.Add(ph25);
                Paragraph p26 = new Paragraph();
                p26.Add(ph26);
                Paragraph p27 = new Paragraph();
                p27.Add(ph27);
                Paragraph p28 = new Paragraph();
                p28.Add(ph28);
                Paragraph p29 = new Paragraph();
                p29.Add(ph29);
                Paragraph p30 = new Paragraph();
                p30.Add(ph30);
                Paragraph p31 = new Paragraph();
                p31.Add(ph31);
                Paragraph p32 = new Paragraph();
                p32.Add(ph32);
                Paragraph p33 = new Paragraph();
                p33.Add(ph33);
                Paragraph p34 = new Paragraph();
                p34.Add(ph34);
                Paragraph p35 = new Paragraph();
                p35.Add(ph35);
                Paragraph p36 = new Paragraph();
                p36.Add(ph36);
                Paragraph p37 = new Paragraph();
                p37.Add(ph37);
                Paragraph p38 = new Paragraph();
                p38.Add(ph38);
                Paragraph p39 = new Paragraph();
                p39.Add(ph39);


                doc1.Add(jpg);
                doc1.Add(p1);
                doc1.Add(p2);
                doc1.Add(p3);
                doc1.Add(p4);
                doc1.Add(p5);
                doc1.Add(p6);
                doc1.Add(p7);
                doc1.Add(p8);
                doc1.Add(p9);
                doc1.Add(p10);
                doc1.Add(p11);
                doc1.Add(p12);
                doc1.Add(p13);
                doc1.Add(p14);
                doc1.Add(p15);
                doc1.Add(p16);
                doc1.Add(p17);
                doc1.Add(p18);
                doc1.Add(p19);
                doc1.Add(p20);
                doc1.Add(p21);
                doc1.Add(p22);
                doc1.Add(p23);
                doc1.Add(p24);
                doc1.Add(p25);
                doc1.Add(p26);
                doc1.Add(p27);
                doc1.Add(p28);
                doc1.Add(p29);
                doc1.Add(p30);
                doc1.Add(p31);
                doc1.Add(p32);
                doc1.Add(p33);
                doc1.Add(p34);
                doc1.Add(p35);
                doc1.Add(p36);
                doc1.Add(p37);
                doc1.Add(p38);
                doc1.Add(p39);


                doc1.CloseDocument();
                doc1.Close();
                doc1.Dispose();
                ++i;
            }
            dr.Close();
            sqlConnection.Close();
            MessageBox.Show("PDF-документ сохранен", "Severstal Infocom");
        }

        private void out_excel_button(object sender, RoutedEventArgs e)
        {
            var selectedIndex = DeliveryGrid.SelectedIndex;
            if (selectedIndex != -1)
                ExportToExcel(selectedIndex);
            else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }

        private void ExportToExcel(int ID_orders)
        {
            sqlConnection.Open();
            if (!Directory.Exists("EXCEL"))
                Directory.CreateDirectory("EXCEL");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DataRowView drv = (DataRowView)DeliveryGrid.SelectedItem;
            string ID_Orders = drv.Row[1].ToString();
            using var stream = new FileStream("EXCEL\\Заказ № " + ID_Orders + " .xlsx", FileMode.Create);
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.Add("Заказ № " + ID_Orders);
            var sql = "SELECT *, (SELECT FIO FROM payer where id_payer = orders.id_payer ) AS 'FIO', (SELECT standard_per_mark FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate) AS 'standard_per_mark', (SELECT product_standard FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate) AS 'product_standard', (SELECT manufacturer FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate) AS 'manufacturer', (SELECT date_add_certificate FROM qua_certificate where id_qua_certificate = orders.id_qua_certificate) AS 'date_add_certificate', (SELECT name_storage FROM storage where id_storage = shipment.id_storage) AS 'name_storage', (SELECT address FROM storage where id_storage = shipment.id_storage) AS 'address', (SELECT FIO_responsible_person FROM storage where id_storage=shipment.id_storage) AS 'FIO_responsible_person', (SELECT date_add_storage FROM storage where id_storage=shipment.id_storage) AS 'date_add_storage', (SELECT phone_storage FROM storage where id_storage=shipment.id_storage) AS 'phone_storage', (SELECT name_transport FROM transport where id_transport=shipment.id_transport) AS 'name_transport', (SELECT number_transport FROM transport where id_transport=shipment.id_transport) AS 'number_transport' FROM orders INNER JOIN package ON orders.id_order = package.id_order INNER JOIN shipment ON orders.id_order = shipment.id_order INNER JOIN delivery ON orders.id_order = delivery.id_order WHERE orders.id_order = @id";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@id", ID_Orders);
            var reader = cmd.ExecuteReader();
            int count = 10;

            ws.TabColor = System.Drawing.Color.Black;

            ws.Cells[1, 1].Value = "Сервисный металлоцентр СМЦ-Колпино";
            ws.Cells[1, 1].Style.Font.Size = 16;
            ws.Cells[1, 1].Style.Font.Name = "Times New Roman";

            ws.Cells[2, 1].Value = "Россия, Колпино, Санкт-Петербург, Территория промзоны 'Ижорские завод', д.90, лит. Д, помещение 1-Н";
            ws.Cells[2, 1].Style.Font.Size = 12;
            ws.Cells[2, 1].Style.Font.Name = "Times New Roman";

            ws.Cells[3, 1].Value = "Информация о заказе";
            ws.Cells[3, 1].Style.Font.Size = 16;
            ws.Cells[3, 1].Style.Font.Name = "Times New Roman";


            //order
            ws.Cells[5, 1].Value = "ID заказа" ;
            ws.Cells[5, 1].Style.Font.Size = 12;
            ws.Cells[5, 1].Style.Font.Name = "Calibri";
            ws.Cells[5, 1].Style.Font.Bold = true;

            ws.Cells[5, 4].Value = "Аттестация пройдена";
            ws.Cells[5, 4].Style.Font.Size = 12;
            ws.Cells[5, 4].Style.Font.Name = "Calibri";
            ws.Cells[5, 4].Style.Font.Bold = true;

            ws.Cells[9, 1].Value = "Заказчик";
            ws.Cells[9, 1].Style.Font.Size = 12;
            ws.Cells[9, 1].Style.Font.Name = "Calibri";
            ws.Cells[9, 1].Style.Font.Bold = true;

            ws.Cells[9, 4].Value = "СИСТ #СЗ";
            ws.Cells[9, 4].Style.Font.Size = 12;
            ws.Cells[9, 4].Style.Font.Name = "Calibri";
            ws.Cells[9, 4].Style.Font.Bold = true;

            ws.Cells[13, 1].Value = "ЛОГ #СЗ";
            ws.Cells[13, 1].Style.Font.Size = 12;
            ws.Cells[13, 1].Style.Font.Name = "Calibri";
            ws.Cells[13, 1].Style.Font.Bold = true;

            ws.Cells[13, 4].Value = "Название продукта";
            ws.Cells[13, 4].Style.Font.Size = 12;
            ws.Cells[13, 4].Style.Font.Name = "Calibri";
            ws.Cells[13, 4].Style.Font.Bold = true;

            ws.Cells[16, 1].Value = "Толщина продукта";
            ws.Cells[16, 1].Style.Font.Size = 12;
            ws.Cells[16, 1].Style.Font.Name = "Calibri";
            ws.Cells[16, 1].Style.Font.Bold = true;

            ws.Cells[16, 4].Value = "Длина продукта"; //
            ws.Cells[16, 4].Style.Font.Size = 12;
            ws.Cells[16, 4].Style.Font.Name = "Calibri";
            ws.Cells[16, 4].Style.Font.Bold = true;

            ws.Cells[20, 1].Value = "Ширина продукта";
            ws.Cells[20, 1].Style.Font.Size = 12;
            ws.Cells[20, 1].Style.Font.Name = "Calibri";
            ws.Cells[20, 1].Style.Font.Bold = true;

            ws.Cells[20, 4].Value = "Грузоперевозчик";
            ws.Cells[20, 4].Style.Font.Size = 12;
            ws.Cells[20, 4].Style.Font.Name = "Calibri";
            ws.Cells[20, 4].Style.Font.Bold = true;

            ws.Cells[24, 1].Value = "Статус заказа";
            ws.Cells[24, 1].Style.Font.Size = 12;
            ws.Cells[24, 1].Style.Font.Name = "Calibri";
            ws.Cells[24, 1].Style.Font.Bold = true;

            //////qua_certificate
            ws.Cells[24, 4].Value = "Стандарт на марку";
            ws.Cells[24, 4].Style.Font.Size = 12;
            ws.Cells[24, 4].Style.Font.Name = "Calibri";
            ws.Cells[24, 4].Style.Font.Bold = true;

            ws.Cells[28, 1].Value = "Стандарт продукта";
            ws.Cells[28, 1].Style.Font.Size = 12;
            ws.Cells[28, 1].Style.Font.Name = "Calibri";
            ws.Cells[28, 1].Style.Font.Bold = true;

            ws.Cells[28, 4].Value = "Изготовитель";
            ws.Cells[28, 4].Style.Font.Size = 12;
            ws.Cells[28, 4].Style.Font.Name = "Calibri";
            ws.Cells[28, 4].Style.Font.Bold = true;

            ws.Cells[32, 1].Value = "Дата аттестации";
            ws.Cells[32, 1].Style.Font.Size = 12;
            ws.Cells[32, 1].Style.Font.Name = "Calibri";
            ws.Cells[32, 1].Style.Font.Bold = true;

            ws.Cells[32, 4].Value = "Транспорт"; ////transport
            ws.Cells[32, 4].Style.Font.Size = 12;
            ws.Cells[32, 4].Style.Font.Name = "Calibri";
            ws.Cells[32, 4].Style.Font.Bold = true;

            ws.Cells[36, 1].Value = "Номер транспорта";
            ws.Cells[36, 1].Style.Font.Size = 12;
            ws.Cells[36, 1].Style.Font.Name = "Calibri";
            ws.Cells[36, 1].Style.Font.Bold = true;

            ws.Cells[36, 4].Value = "Склад"; ////storage
            ws.Cells[36, 4].Style.Font.Size = 12;
            ws.Cells[36, 4].Style.Font.Name = "Calibri";
            ws.Cells[36, 4].Style.Font.Bold = true;

            ws.Cells[40, 1].Value = "Адрес склада";
            ws.Cells[40, 1].Style.Font.Size = 12;
            ws.Cells[40, 1].Style.Font.Name = "Calibri";
            ws.Cells[40, 1].Style.Font.Bold = true;

            ws.Cells[40, 4].Value = "Телефон склада";
            ws.Cells[40, 4].Style.Font.Size = 12;
            ws.Cells[40, 4].Style.Font.Name = "Calibri";
            ws.Cells[40, 4].Style.Font.Bold = true;

            ws.Cells[44, 1].Value = "ФИО ответственного за склад";
            ws.Cells[44, 1].Style.Font.Size = 12;
            ws.Cells[44, 1].Style.Font.Name = "Calibri";
            ws.Cells[44, 1].Style.Font.Bold = true;

            ws.Cells[44, 4].Value = "Ранняя доставка";//delivery
            ws.Cells[44, 4].Style.Font.Size = 12;
            ws.Cells[44, 4].Style.Font.Name = "Calibri";
            ws.Cells[44, 4].Style.Font.Bold = true;

            ws.Cells[48, 1].Value = "Дата доставки";
            ws.Cells[48, 1].Style.Font.Size = 12;
            ws.Cells[48, 1].Style.Font.Name = "Calibri";
            ws.Cells[48, 1].Style.Font.Bold = true;



            while (reader.Read())
            {
                //order
                ws.Cells[6, 1].Value = reader.GetValue("id_order");
                ws.Cells[6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[6, 4].Value = reader.GetValue("access_standart");
                ws.Cells[10, 1].Value = reader.GetValue("FIO");
                ws.Cells[10, 4].Value = reader.GetValue("syst_c3");
                ws.Cells[10, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[14, 1].Value = reader.GetValue("log_c3");
                ws.Cells[14, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[14, 4].Value = reader.GetValue("name_product");
                ws.Cells[17, 1].Value = reader.GetValue("thickness_mm");
                ws.Cells[17, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[17, 4].Value = reader.GetValue("length_mm");
                ws.Cells[17, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[21, 1].Value = reader.GetValue("width_mm");
                ws.Cells[21, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[21, 4].Value = reader.GetValue("name_consignee");
                ws.Cells[25, 1].Value = reader.GetValue("status_order");

                //////qua_certificate
                ws.Cells[25, 4].Value = reader.GetValue("standard_per_mark");
                ws.Cells[29, 1].Value = reader.GetValue("product_standard");
                ws.Cells[29, 4].Value = reader.GetValue("manufacturer");
                ws.Cells[33, 1].Value = reader.GetValue("date_add_certificate");
                ws.Cells[33, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[33, 1].Style.Numberformat.Format = "yyyy-mm-dd";
                ws.Cells[33, 4].Value = reader.GetValue("name_transport");////transport
                ws.Cells[37, 1].Value = reader.GetValue("number_transport");
                ws.Cells[37, 4].Value = reader.GetValue("name_storage");////storage
                ws.Cells[41, 1].Value = reader.GetValue("address");
                ws.Cells[41, 4].Value = reader.GetValue("phone_storage");
                ws.Cells[45, 1].Value = reader.GetValue("FIO_responsible_person");
                ws.Cells[45, 4].Value = reader.GetValue("early_delivery");//delivery
                ws.Cells[49, 1].Value = reader.GetValue("date_of_delivery");
                ws.Cells[49, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[49, 1].Style.Numberformat.Format = "yyyy-mm-dd";
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                count++;
            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            package.Save();
            MessageBox.Show("EXCEL-таблица сохранена", "Severstal Infocom");
            sqlConnection.Close();
        }
    }
}
