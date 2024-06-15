using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TPCHR
{
    class Validation
    {
        public bool ValidateData(string posName, object posType, string dayOfBuy, string posValue, string posPrice, object selectedQuote)
        {
            // Проверка имени позиции
            if (string.IsNullOrEmpty(posName))
            {
                MessageBox.Show("Ошибка!" + '\n' + "Введите название позиции!");
                return false;
            }

            // Проверка выбранного типа позиции
            if (posType == null)
            {
                MessageBox.Show("Ошибка!" + '\n' + "Выберите тип позиции!");
                return false;
            }

            // Проверка даты покупки
            if (string.IsNullOrEmpty(dayOfBuy))
            {
                MessageBox.Show("Ошибка!" + '\n' + "Введите дату покупки!");
                return false;
            }
            else
            {
                // Попробуйте разобрать введенную строку в формате "dd.MM.yyyy"
                if (!DateTime.TryParseExact(dayOfBuy, "dd.MM.yyyy", null, DateTimeStyles.None, out _))
                {
                    MessageBox.Show("Ошибка!" + '\n' + "Неверный формат даты покупки! Используйте формат 'dd.MM.yyyy'.");
                    return false;
                }
            }

            // Проверка значения позиции
            if (!int.TryParse(posValue, out int positionValue) || positionValue <= 0)
            {
                MessageBox.Show("Ошибка!" + '\n' + "Введите корректное значение количества товаров");
                return false;
            }

            // Проверка цены позиции
            if (!double.TryParse(posPrice, out double positionPrice) || positionPrice <= 0)
            {
                MessageBox.Show("Ошибка!" + '\n' + "Введите корректную цену позиции!");
                return false;
            }

            // Проверка выбора котировок
            if (selectedQuote == null)
            {
                MessageBox.Show("Ошибка!" + '\n' + "Выберите котировку!");
                return false;
            }

            return true;
        }
    }
}
