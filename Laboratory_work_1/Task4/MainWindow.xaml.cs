using System;
using System.Windows;

namespace Task4
{
    /// <summary>
    /// Простое WPF-приложение для сложения двух чисел.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Выполняет сложение двух чисел, введённых пользователем.
        /// </summary>
        /// <param name="sender">Источник события (кнопка)</param>
        /// <param name="e">Аргументы события</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Преобразуем текст из TextBox в числа
                double number1 = double.Parse(number1TextBox.Text.Trim());
                double number2 = double.Parse(number2TextBox.Text.Trim());

                // Складываем
                double sum = number1 + number2;

                // Отображаем результат
                resultTextBlock.Text = $"{sum}";
            }
            catch (Exception ex)
            {
                resultTextBlock.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Очищает все поля ввода и результат.
        /// </summary>
        /// <param name="sender">Источник события (кнопка)</param>
        /// <param name="e">Аргументы события</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            number1TextBox.Text = "";
            number2TextBox.Text = "";
            resultTextBlock.Text = "";
        }
    }
}