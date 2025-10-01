using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace NumberInputApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ValidationText.Text = "Введите число (пример: -12.34 или 0,56)";
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = NumberTextBox.Text;

            // Пустая строка - допустима
            if (string.IsNullOrEmpty(text))
            {
                ClearValidationError();
                return;
            }

            // Проверяем формат регулярным выражением
            if (IsValidNumberFormat(text))
            {
                ClearValidationError();
            }
            else
            {
                ShowValidationError("Некорректный формат числа!");
            }
        }

        private bool IsValidNumberFormat(string text)
        {
            // Регулярное выражение для вещественного числа со знаком
            // -?     - необязательный знак минуса
            // \d*    - ноль или более цифр (целая часть)
            // [,.]?  - необязательный разделитель (точка или запятая)
            // \d*    - ноль или более цифр (дробная часть)
            Regex regex = new Regex(@"^-?\d*[,.]?\d*$");
            return regex.IsMatch(text);
        }

        private void ShowValidationError(string message)
        {
            ValidationText.Text = message;
            ValidationText.Foreground = System.Windows.Media.Brushes.Red;
        }

        private void ClearValidationError()
        {
            ValidationText.Text = "Корректный формат числа";
            ValidationText.Foreground = System.Windows.Media.Brushes.Green;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NumberTextBox.Text = Math.PI.ToString();
            double.TryParse()
        }
    }
}