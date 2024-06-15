using System;
using System.Windows;
using System.Linq;
using piris.DomainService.lpml;
using piris.DomainService.Objects;
using piris.DomainService;
using System.ServiceModel;

namespace TPCHR
{
    public class PositionObject
    {
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public PositionType PositionType { get; set; }
        public int PositionValue { get; set; }
        public DateTime DayOfBuy { get; set; }
        public double PositionPrice { get; set; }
        public double PriceCurrency { get; set; }
    }
    public enum PositionType
    {
        Components = 0,
        Devices = 1,
        Accessories = 2
    }

    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {

        private bool _downloadTable = false;
        public Menu()
        {
            InitializeComponent();
            Loaded += Window_loaded;

        }
        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            cbPosType.ItemsSource = Enum.GetValues(typeof(PositionType));
            cbPosType.SelectedIndex = 0;
        }
        private void bQuotes_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр CentralBankService
            CentralBankService centralBankService = new CentralBankService();

            // Получаем котировки для USD, EUR и CNY
            string[] currencies = { "USD", "EUR", "CNY" };
            foreach (var currency in currencies)
            {
                double value = 1; // Пример значения для конвертации (в данном случае, 1)
                ConverterObject result = centralBankService.ConvertValue(value, currency);

                // Формируем строку для добавления в lbQuotes
                string quote = $"{1 / result.currencyValue}";

                // Добавляем строку в lbQuotes
                lbQuotes.Items.Add($" 1 {(result.currencyName)} = {quote}  RUB");
            }

            // Устанавливаем первый элемент в качестве выбранного
            lbQuotes.SelectedIndex = 0;
        }


        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            PositionObject item = new PositionObject();

            try
            {
                if (!new Validation().ValidateData(tbPosName.Text, cbPosType.SelectedItem, tbDayOfBuy.Text, tbPosValue.Text, tbPosPrice.Text, lbQuotes.SelectedItem))
                {
                    return; // Если данные неверные, прерываем выполнение метода
                }

                // Если данные прошли валидацию, продолжаем выполнение кода
                item.PositionName = tbPosName.Text;
                item.PositionType = (PositionType)cbPosType.SelectedItem;
                string data = tbDayOfBuy.Text;
                item.DayOfBuy = DateTime.ParseExact(data, "dd.MM.yyyy", null);
                item.PositionValue = int.Parse(tbPosValue.Text);
                item.PositionPrice = double.Parse(tbPosPrice.Text);

                // Присвоение идентификатора позиции
                item.PositionID = dgMain.Items.Count + 1;

                // Дополнительная проверка перед использованием lbQuotes.SelectedItem
                if (lbQuotes.SelectedItem != null)
                {
                    string[] strings = lbQuotes.SelectedItem.ToString().Split(' ');
                    item.PriceCurrency = Math.Round(item.PositionPrice * item.PositionValue / double.Parse(strings[0]), 2);
                }
                
                

                // Добавление элемента в DataGrid
                dgMain.Items.Add(item);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка!" + '\n' + "Некорректный формат числа или пустое поле ввода.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        /// <summary>
        /// кнопка загразузить таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDownload_Table_Click(object sender, RoutedEventArgs e)
        {
            if (_downloadTable == false)
            {
                DownloadTable(@"C:\Users\kivip\OneDrive\Desktop\Товары.xlsx");
                return;
            }
            else
            {
                MessageBox.Show("Таблица уже добавлена!");
                return;
            }
        }


        private void DownloadTable(string xlsxpath)
        {

            if (lbQuotes.SelectedItem == null)
            {
                MessageBox.Show("Получите котировки!");
                return;
            }
            var workbook = new ClosedXML.Excel.XLWorkbook(xlsxpath);

            var worksheet = workbook.Worksheets.Worksheet(1);
            // Получаем используемые строки в таблице
            var usedRows = worksheet.RowsUsed();

            // Проверяем, есть ли элементы в DataGrid
            int initialPositionID = 1;
            if (dgMain.Items.Count > 0)
            {
                // Если есть, то находим максимальный PositionID и увеличиваем его на 1
                initialPositionID = dgMain.Items.OfType<PositionObject>().Max(item => item.PositionID) + 1;
            }

            // Перебираем каждую строку с данными
            foreach (var row in usedRows.Skip(1)) // Пропускаем заголовок
            {
                PositionObject item = new PositionObject();
                {
                    // Присваиваем PositionID
                    item.PositionID = initialPositionID++;

                    // Остальные поля заполняем из Excel-файла
                    item.PositionName = row.Cell(2).GetValue<string>();
                    item.PositionType = (PositionType)row.Cell(3).GetValue<int>();
                    string data = row.Cell(5).GetValue<string>();
                    item.DayOfBuy = DateTime.ParseExact(data, "dd.MM.yyyy", null);
                    item.PositionValue = row.Cell(4).GetValue<int>();
                    item.PositionPrice = (double)row.Cell(6).GetValue<int>();

                    // Вычисляем валюту
                    if (lbQuotes.SelectedItem != null)
                    {
                        string selectedItem = lbQuotes.SelectedItem.ToString();

                        // Разделяем строку по пробелам
                        string[] parts = selectedItem.Split(' ');

                        // Извлекаем курс валюты (первый элемент после " = ")
                        string ratePart = parts[4];


                        double currencyRate = double.Parse(ratePart);

                        item.PriceCurrency = Math.Round(item.PositionPrice * item.PositionValue / currencyRate, 2);
                    }
                };

                // Добавляем созданный объект в DataGrid
                dgMain.Items.Add(item);
            }
            _downloadTable = true;
        }
    }

}
