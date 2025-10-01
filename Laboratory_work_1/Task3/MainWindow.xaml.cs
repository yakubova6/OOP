using System;
using System.Windows;
using System.Windows.Controls;

namespace Task3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Настройка начальных значений
            fromBaseComboBox.SelectedIndex = 0;
            toBaseComboBox.SelectedIndex = 1;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = inputTextBox.Text.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    resultTextBlock.Text = "Введите число";
                    return;
                }

                // Определяем исходную систему счисления
                int fromBase = GetBaseFromComboBox(fromBaseComboBox);
                int toBase = GetBaseFromComboBox(toBaseComboBox);

                // Конвертируем
                string result = ConvertNumber(input, fromBase, toBase);
                resultTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                resultTextBlock.Text = $"Ошибка: {ex.Message}";
            }
        }

        private int GetBaseFromComboBox(ComboBox comboBox)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0: return 10;  // Десятичная
                case 1: return 2;   // Двоичная
                case 2: return 8;   // Восьмеричная
                case 3: return 16;  // Шестнадцатеричная
                case 4: return -1;  // Римская
                default: return 10;
            }
        }

        private string ConvertNumber(string input, int fromBase, int toBase)
        {
            // Если конвертируем в римскую систему
            if (toBase == -1)
            {
                if (fromBase != 10)
                {
                    // Сначала конвертируем в десятичную
                    int decimalNumber = Convert.ToInt32(input, fromBase);
                    return DecimalToRoman(decimalNumber);
                }
                return DecimalToRoman(int.Parse(input));
            }

            // Если конвертируем из римской системы
            if (fromBase == -1)
            {
                int decimalNumber = RomanToDecimal(input);
                if (toBase == 10) return decimalNumber.ToString();
                return Convert.ToString(decimalNumber, toBase).ToUpper();
            }

            // Обычная конвертация между системами счисления
            if (fromBase == 10)
            {
                int number = int.Parse(input);
                return Convert.ToString(number, toBase).ToUpper();
            }
            else
            {
                // Конвертируем в десятичную, затем в нужную систему
                int decimalNumber = Convert.ToInt32(input, fromBase);
                if (toBase == 10) return decimalNumber.ToString();
                return Convert.ToString(decimalNumber, toBase).ToUpper();
            }
        }

        private string DecimalToRoman(int number)
        {
            if (number < 1 || number > 3999)
                throw new ArgumentException("Число должно быть от 1 до 3999");

            string[] thousands = { "", "M", "MM", "MMM" };
            string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

            return thousands[number / 1000] +
                   hundreds[(number % 1000) / 100] +
                   tens[(number % 100) / 10] +
                   ones[number % 10];
        }

        private int RomanToDecimal(string roman)
        {
            roman = roman.ToUpper();
            int result = 0;
            int previousValue = 0;

            for (int i = roman.Length - 1; i >= 0; i--)
            {
                int currentValue = RomanCharToValue(roman[i]);


                if (currentValue < previousValue)
                    result -= currentValue;
                else
                    result += currentValue;

                previousValue = currentValue;
            }

            return result;
        }

        private int RomanCharToValue(char c)
        {
            switch (c)
            {
                case 'I': return 1;
                case 'V': return 5;
                case 'X': return 10;
                case 'L': return 50;
                case 'C': return 100;
                case 'D': return 500;
                case 'M': return 1000;
                default: throw new ArgumentException($"Неверный римский символ: {c}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Text = "";
            resultTextBlock.Text = "";
        }
    }
}