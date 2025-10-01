using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Task2
{
    /// <summary>
    /// Главное окно приложения для вычисления квадратного корня методом Ньютона
    /// </summary>
    public partial class MainWindow : Window
    {
        // Переменные для хранения состояния вычислений
        private decimal currentGuess;      // Текущее приближение корня
        private decimal numberDecimal;     // Число, из которого извлекается корень (в формате decimal для точности)
        private int iterationCount;        // Счетчик выполненных итераций
        private double exactValue;         // Точное значение корня, вычисленное стандартным методом Math.Sqrt
        private List<IterationData> iterationsList; // Список для хранения информации о всех итерациях

        /// <summary>
        /// Класс для хранения данных об одной итерации метода Ньютона
        /// </summary>
        public class IterationData
        {
            public int Номер { get; set; }         // Номер итерации
            public string Приближение { get; set; } // Текущее значение приближения
            public string Погрешность { get; set; } // Разница с предыдущим приближением
            public string Разница { get; set; }    // Разница с точным значением Math.Sqrt
        }

        /// <summary>
        /// Конструктор главного окна - инициализация компонентов
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Инициализация списка для хранения итераций
            iterationsList = new List<IterationData>();
            // Подписка на событие изменения выбора в комбобоксе
            initialGuessComboBox.SelectionChanged += InitialGuessComboBox_SelectionChanged;
        }

        /// <summary>
        /// Обработчик изменения выбора метода начального приближения
        /// </summary>
        private void InitialGuessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Показываем поле для ввода пользовательского значения только при выборе соответствующего пункта
            if (initialGuessComboBox.SelectedIndex == 2)
                customGuessTextBox.Visibility = Visibility.Visible;
            else
                customGuessTextBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Вычислить полностью" - выполняет все итерации до достижения точности
        /// </summary>
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            StartCalculation(true);
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Следующая итерация" - выполняет только одну итерацию
        /// </summary>
        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            StartCalculation(false);
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сброс" - очищает все результаты и сбрасывает состояние
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка списка итераций
            iterationsList.Clear();
            iterationsDataGrid.ItemsSource = null;
            // Сброс отображаемых результатов
            frameworkLabel.Content = "-";
            newtonLabel.Content = "-";
            statusTextBlock.Text = "Вычисления сброшены. Введите новое число.";
            iterationCount = 0;
        }

        /// <summary>
        /// Основной метод начала вычислений
        /// </summary>
        private void StartCalculation(bool fullCalculation)
        {
            string inputText = inputTextBox.Text.Replace(".", ",");

            // ПРОВЕРКА 1: Проверяем как double
            if (!double.TryParse(inputText, out double numberDouble))
            {
                MessageBox.Show("Пожалуйста, введите корректное число");
                return;
            }


            // ПРОВЕРКА 2: Проверяем что число положительное И не ноль
            if (numberDouble <= 0)
            {
                MessageBox.Show("Пожалуйста, введите положительное число (больше нуля)");
                return;
            }

            // ПРОВЕРКА 3: Проверяем как decimal
            if (!decimal.TryParse(inputText, out numberDecimal))
            {
                MessageBox.Show("Пожалуйста, введите корректное десятичное число");
                return;
            }

            // ПРОВЕРКА 4: Дополнительная проверка для decimal
            if (numberDecimal <= 0)
            {
                MessageBox.Show("Пожалуйста, введите положительное число (больше нуля)");
                return;
            }

            // Теперь безопасно вычисляем корень
            exactValue = Math.Sqrt(numberDouble);
            frameworkLabel.Content = $"{exactValue:F15}";

            // Начало новых вычислений
            if (iterationCount == 0)
            {
                iterationsList.Clear();
                currentGuess = GetInitialGuess();
                AddIteration(0, currentGuess, 0, (decimal)exactValue - currentGuess);
                iterationCount = 1;
            }

            // Выбор режима вычисления
            if (fullCalculation)
                CalculateFull();
            else
                CalculateStep();
        }

        /// <summary>
        /// Получение начального приближения в зависимости от выбранного метода
        /// </summary>
        private decimal GetInitialGuess()
        {
            // Простой выбор начального приближения по выбранному методу
            if (initialGuessComboBox.SelectedIndex == 1)
                return numberDecimal / 2;  // Классический метод: число/2

            if (initialGuessComboBox.SelectedIndex == 2)
            {
                // Пользовательское значение - парсим введенное число
                string customText = customGuessTextBox.Text.Replace(".", ",");
                if (decimal.TryParse(customText, out decimal customGuess))
                    return customGuess;
                else
                    return 1;  // Значение по умолчанию при ошибке парсинга
            }

            // Умный алгоритм (автоматический) - используется по умолчанию
            return SmartInitialGuess(numberDecimal);
        }

        /// <summary>
        /// Умный алгоритм выбора начального приближения через определение порядка числа
        /// Основан на определении порядка числа в двоичной системе счисления
        /// </summary>
        private decimal SmartInitialGuess(decimal number)
        {
            if (number <= 0) return 1;

            // Для чисел < 1 используем 1 как приближение для √temp
            if (number < 1)
            {
                decimal temp = number;
                int shifts = 0;

                while (temp < 1)
                {
                    temp *= 10;
                    shifts++;
                }

                // number = temp × 10^(-shifts)
                // √number ≈ 1 × 10^(-shifts/2)  (предполагаем √temp ≈ 1)
                return (decimal)Math.Pow(10, -shifts / 2.0);
            }

            // Для чисел ≥ 1 - двоичный подход 
            decimal num = number;
            int exponent = 0;

            while (num >= 2)
            {
                num /= 2;
                exponent++;
            }

            decimal guess = 1m;
            int halfExponent = exponent / 2;

            for (int i = 0; i < halfExponent; i++)
            {
                guess *= 2;
            }

            return guess;
        }

        /// <summary>
        /// Полное вычисление методом Ньютона до достижения заданной точности
        /// Использует итерационную формулу: xₙ₊₁ = (xₙ + a/xₙ) / 2
        /// </summary>
        private void CalculateFull()
        {
            decimal guess = currentGuess;
            decimal delta = 0.0000000000000000000000000001m;
            int maxIterations = 100;


            for (int i = 0; i < maxIterations; i++)
            {
                // ЗАЩИТА ОТ ДЕЛЕНИЯ НА НОЛЬ
                if (guess == 0)
                {
                    MessageBox.Show("Ошибка: начальное приближение равно нулю");
                    iterationCount = 0;
                    return;
                }

                decimal result = (guess + numberDecimal / guess) / 2;
                decimal error = Math.Abs(result - guess);
                decimal difference = (decimal)exactValue - result;

                AddIteration(iterationCount, result, error, difference);

                if (error <= delta)
                {
                    newtonLabel.Content = $"{result:F28}";
                    statusTextBlock.Text = $"Вычисление завершено. Итераций: {iterationCount}";
                    iterationCount = 0;
                    return;
                }

                guess = result;
                iterationCount++;
            }

            newtonLabel.Content = $"{guess:F28}";
            statusTextBlock.Text = "Достигнут предел итераций";
            iterationCount = 0;
        }

        /// <summary>
        /// Пошаговое вычисление методом Ньютона (одна итерация за раз)
        /// Позволяет наблюдать за процессом сходимости метода
        /// </summary>
        private void CalculateStep()
        {
            // ЗАЩИТА ОТ ДЕЛЕНИЯ НА НОЛЬ
            if (currentGuess == 0)
            {
                MessageBox.Show("Ошибка: текущее приближение равно нулю");
                iterationCount = 0;
                return;
            }

            decimal result = (currentGuess + numberDecimal / currentGuess) / 2;
            decimal error = Math.Abs(result - currentGuess);
            decimal difference = (decimal)exactValue - result;

            AddIteration(iterationCount, result, error, difference);

            currentGuess = result;
            iterationCount++;

            decimal delta = 0.0000000000000000000000000001m;
            if (error <= delta)
            {
                newtonLabel.Content = $"{result:F28}";
                statusTextBlock.Text = $"Вычисление завершено! Итераций: {iterationCount}";
                iterationCount = 0;
            }
            else
            {
                newtonLabel.Content = $"{result:F15}";
                statusTextBlock.Text = $"Итерация {iterationCount}. Погрешность: {error:E10}";
            }
        }

        /// <summary>
        /// Добавление информации об итерации в таблицу результатов
        /// </summary>
        private void AddIteration(int iteration, decimal guess, decimal error, decimal difference)
        {
            // Создание объекта с данными об итерации
            var data = new IterationData
            {
                Номер = iteration,
                Приближение = guess.ToString("F15"),      // Формат: фиксированная точка с 15 знаками
                Погрешность = error.ToString("E10"),      // Формат: экспоненциальный с 10 знаками
                Разница = difference.ToString("E10")      // Формат: экспоненциальный с 10 знаками
            };

            // Добавление в список и обновление отображения таблицы
            iterationsList.Add(data);
            iterationsDataGrid.ItemsSource = null;        // Сброс привязки
            iterationsDataGrid.ItemsSource = iterationsList; // Установка новой привязки
            // Прокрутка к последней добавленной строке для удобства просмотра
            iterationsDataGrid.ScrollIntoView(data);
        }

        /// <summary>
        /// Обработчик нажатия клавиш на форме
        /// Позволяет управлять вычислениями с клавиатуры
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка пробела для выполнения следующей итерации
            if (e.Key == Key.Space)
            {
                StepButton_Click(sender, e);
            }
        }
    }
}
