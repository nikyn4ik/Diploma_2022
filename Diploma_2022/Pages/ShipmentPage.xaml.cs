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
        private void PDFOut(int cellId)
        {
            object item = ShipmentGrid.SelectedItem;
            string ID = (ShipmentGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            using var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream( "Shipment" + ID + ".pdf", FileMode.Create));
            //string outputFile = Path.Combine(@"PDF\", doc);
            doc.Open();

            var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL); //создание базовых font/шрифтов
            var table = new PdfPTable(ShipmentGrid.Columns.Count);// создание таблицы
            var cell = new PdfPCell(new Phrase("SHIPMENT ORDER " + " # " + ID))// создание первой ячейки с фразой, которую мы хотим
            {
                Colspan = ShipmentGrid.Columns.Count,
                HorizontalAlignment = 1,
                Border = 0
            };
            table.AddCell(cell);

            for (int j = 0; j < ShipmentGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                cell = new PdfPCell(new Phrase(ShipmentGrid.Columns[j].Header.ToString()));
                var headerCell = cell.Phrase[0].ToString();
                cell = new PdfPCell(new Phrase(headerCell, font));
                cell.BackgroundColor = BaseColor.BLACK;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }
            for (int j = 0; j < ShipmentGrid.Columns.Count; j++)//проходимся циклом по каж.сtolбцу 
            {
                string sr = (ShipmentGrid.SelectedCells[j].Column.GetCellContent(item) as TextBlock).Text;
                cell = new PdfPCell(new Phrase(sr, font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                font.Color = BaseColor.WHITE;
                table.AddCell(cell);
            }

            doc.Add(table);
            doc.Close();
            MessageBox.Show("PDF-документ сохранен", "Severstal Infocom");

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            object item = ShipmentGrid.SelectedItem;
            if (item == null)
                MessageBox.Show("Выберите строчку", "Severstal Infocom");
            else
            {
                string ID = (ShipmentGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                var window = new AddShipment(Convert.ToInt32(ID));
                window.ShowDialog();
                Show();
                update();
            }
        }
    }
}
