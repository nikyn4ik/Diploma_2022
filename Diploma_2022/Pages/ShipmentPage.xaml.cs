using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Diploma_2022.Add;


namespace Diploma_2022.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShipmentPage.xaml
    /// </summary>
    public partial class ShipmentPage : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");

        public ShipmentPage()
        {
            InitializeComponent();
            Shipment_DataGrid_SelectionChanged();

        }
        private void Shipment_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[shipment]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter shipment = new SqlDataAdapter(cmd);
            shipment.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView;
        }
        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShipmentGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[shipment] WHERE id_order like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter shipments = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("shipment");
                shipments.Fill(dt);
                ShipmentGrid.ItemsSource = dt.DefaultView;
                shipments.Update(dt);
                cmds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не найдено в системе.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            object item = ShipmentGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
                string ID_Orders = drv.Row[1].ToString();
                var select = "SELECT COUNT(*) FROM [dbo].[delivery] WHERE id_order=@id";
                SqlCommand sqlCommand = new SqlCommand(select, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", ID_Orders);
                int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                if(count == 0)
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[delivery] (id_order, id_storage) ((SELECT id_order, id_storage FROM shipment WHERE id_order=@id))", sqlConnection);
                    cmd.Parameters.AddWithValue("@id", ID_Orders);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Заказ успешно отправлен в доставку!", "Severstal Infocom");
                    sqlConnection.Close();
                    update();
                }
                else
                {
                    MessageBox.Show("Данный заказ ранее уже был отправлен в доставку", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        protected void update()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[shipment]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter ship = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            ship.Fill(dt);
            ShipmentGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private void outButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = ShipmentGrid.SelectedIndex;
            if (selectedIndex != -1)
                PDFOut(selectedIndex);
            else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
        private void PDFOut(int ID_orders)
        {
            object item = ShipmentGrid.SelectedItem;
            DataRowView drv = (DataRowView)ShipmentGrid.SelectedItem;
            string ID_Orders = drv.Row[1].ToString();
            string ID = (ShipmentGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT orders.id_order,orders.name_product, (SELECT name_storage as 'Наименование склад' FROM storage where id_storage=shipment.id_storage), (SELECT address as 'адрес склад' FROM storage where id_storage=shipment.id_storage), (SELECT FIO_responsible_person as 'Фио ответственн склад' FROM storage where id_storage=shipment.id_storage), (SELECT name_transport as 'Наименование' FROM transport where id_transport=shipment.id_transport), (SELECT number_transport as 'Номер транспорта' FROM transport where id_transport=shipment.id_transport) FROM orders INNER JOIN package ON orders.id_order =@id INNER JOIN shipment ON shipment.id_order =@id INNER JOIN delivery ON delivery.id_order =@id WHERE orders.id_order = @id";
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
                PdfWriter.GetInstance(doc1, new FileStream("PDF\\Накладная отгрузка " + ID + ".pdf", FileMode.Create));
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
                Chunk c5 = new Chunk(" " + "                                                        Накладная | Упаковка", font);
                Chunk c6 = new Chunk(" " + "", font);
                Chunk c7 = new Chunk(" " + "ID заказа:   " + dr[0], font);
                Chunk c8 = new Chunk(" " + "", font);
                Chunk c9 = new Chunk(" " + "Продукт:  " + dr[1], font);
                Chunk c10 = new Chunk(" " + "", font);
                Chunk c11 = new Chunk(" " + "Склад:  " + dr[2], font);
                Chunk c12 = new Chunk(" " + "", font);
                Chunk c13 = new Chunk(" " + "Адрес:  " + dr[3], font);
                Chunk c14 = new Chunk(" " + "", font);
                Chunk c15 = new Chunk(" " + "ФИО ответственного:  " + dr[4], font);
                Chunk c16 = new Chunk(" " + "", font);
                Chunk c17 = new Chunk(" " + "Транспорт:  " + dr[5], font);
                Chunk c18 = new Chunk(" " + "", font);
                Chunk c19 = new Chunk(" " + "Номер:  " + dr[6], font);
                Chunk c20 = new Chunk(" " + "", font);
                Chunk c21 = new Chunk(" " + "", font);

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
                Phrase ph20 = new Phrase();

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

                doc1.CloseDocument();
                doc1.Close();
                doc1.Dispose();
                ++i;
            }
            dr.Close();
            sqlConnection.Close();
            MessageBox.Show("PDF-документ сохранен", "Severstal Infocom");
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            object item = ShipmentGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                string ID = (ShipmentGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var window = new AddShipment(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
        }
    }
}
