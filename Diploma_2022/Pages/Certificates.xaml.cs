using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Diploma_2022.Add;
using System.Collections.ObjectModel;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Diploma_2022.Models;
//using System.Windows.Documents;

namespace Diploma_2022.Pages
{
    /// <summary>
    /// Interaction logic for Certificates.xaml
    /// </summary>
    public partial class Certificates : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SPUTNIK; Initial Catalog=diploma_db; Integrated Security=True");
        DataTable dt = new DataTable("diploma_db");
        ObservableCollection<Certificates> certificates = new ObservableCollection<Certificates>();
        public Certificates()
        {
            InitializeComponent();
            Certificates_DataGrid_SelectionChanged();

            certificates = new ObservableCollection<Certificates>();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddCertificates();
            window.ShowDialog();
            Show();
            update();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            object item = CertificatesGrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данный сертификат из базы?", "Sevestal Infocom", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        break;

                    case MessageBoxResult.Yes:
                        if (CertificatesGrid.SelectedItems.Count > 0)
                        {
                            DataRowView drv = (DataRowView)CertificatesGrid.SelectedItem;
                            string certific = drv.Row[0].ToString();
                            SqlCommand cmd = new SqlCommand("DELETE FROM qua_certificate WHERE id_qua_certificate=@id", sqlConnection);
                            cmd.Parameters.AddWithValue("@id", certific);
                            cmd.ExecuteNonQuery();
                            sqlConnection.Close();
                            update();
                        }
                        MessageBox.Show("Удален!", "Severstal Infocom");
                        break;
                }
            }
        }

        private void Certificates_DataGrid_SelectionChanged()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            DataTable tap = new DataTable();
            cmd.CommandText = "SELECT * FROM [dbo].[qua_certificate]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter order = new SqlDataAdapter(cmd);
            new SqlDataAdapter(cmd.CommandText, sqlConnection).Fill(tap);
            order.Fill(dt);
            List<int> result = new List<int>();
            result = tap.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("id_qua_certificate")).ToList();
            certificates = new ObservableCollection<Certificates>();
            CertificatesGrid.ItemsSource = dt.DefaultView;
        }
        protected void update()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [dbo].[qua_certificate]";
            cmd.Connection = sqlConnection;
            SqlDataAdapter cert = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("diploma_db");
            cert.Fill(dt);
            CertificatesGrid.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void polee_TextChanged(object sender, TextChangedEventArgs e)
        {
            CertificatesGrid.Items.Refresh();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Severstal"].ConnectionString;
            try
            {
                SqlConnection cmds = new SqlConnection(ConnectionString);
                string cmd = "SELECT * FROM [dbo].[qua_certificate] WHERE standard_per_mark like '" + pole.Text + "%'";
                cmds.Open();
                SqlCommand sqlcom = new SqlCommand(cmd, cmds);
                SqlDataAdapter cert = new SqlDataAdapter(sqlcom);
                DataTable dt = new DataTable("qua_certificate");
                cert.Fill(dt);
                CertificatesGrid.ItemsSource = dt.DefaultView;
                cert.Update(dt);
                cmds.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не найдено в системе.", "Severstal Infocom", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PDFOut(int ID_cert)
        {
            object item = CertificatesGrid.SelectedItem;
            DataRowView drv = (DataRowView)CertificatesGrid.SelectedItem;
            string id_qua_certificate = drv.Row[0].ToString();
            string ID = (CertificatesGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT id_qua_certificate, standard_per_mark, product_standard, manufacturer, date_add_certificate FROM qua_certificate WHERE id_qua_certificate = @id_qua_certificate";
            cmd.Parameters.AddWithValue("@id_qua_certificate", id_qua_certificate);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader dr = null;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int i = 0;

                string ttf = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                Document doc1 = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc1, new FileStream("PDF\\Сертификат соответствия " + ID + ".pdf", FileMode.Create));
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
                Chunk c5 = new Chunk(" " + "                                                        Сертификат соответствия", font);
                Chunk c6 = new Chunk(" " + "", font);
                Chunk c7 = new Chunk(" " + "ID сертификата:   " + dr[0], font);
                Chunk c8 = new Chunk(" " + "", font);
                Chunk c9 = new Chunk(" " + "Стандарт на марку:  " + dr[1], font);
                Chunk c10 = new Chunk(" " + "", font);
                Chunk c11 = new Chunk(" " + "Стандарт на продукцию:  " + dr[2], font);
                Chunk c12 = new Chunk(" " + "", font);
                Chunk c13 = new Chunk(" " + "Изготовитель:  " + dr[3], font);
                Chunk c14 = new Chunk(" " + "", font);
                Chunk c15 = new Chunk(" " + "Дата добавления:  " + dr[4], font);
                Chunk c16 = new Chunk(" " + "", font);
                Chunk c17 = new Chunk(" " + "", font);

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

                doc1.CloseDocument();
                doc1.Close();
                doc1.Dispose();
                ++i;
            }
            dr.Close();
            sqlConnection.Close();
            MessageBox.Show("PDF-документ сохранен", "Severstal Infocom");
        }

        private void outpdf(object sender, RoutedEventArgs e)
        {
            var selectedIndex = CertificatesGrid.SelectedIndex;
            if (selectedIndex != -1)
                PDFOut(selectedIndex);
            else MessageBox.Show("Выберите нужную строчку!", "Severstal Infocom");
        }
    }
}
