using System;
using System.Windows;
using System.Windows.Controls;

namespace Exam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// Здесь находятся только обработчики событий интерфейса и валидация ввода;
    /// вся расчётная логика вынесена в класс <see cref="ExamLogic"/>.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Определяет выбранный уровень экзамена по состоянию переключателей.
        /// </summary>
        private string GetSelectedLevel()
        {
            if (rbPUPlus.IsChecked == true) return "ПУ+";
            if (rbPU.IsChecked == true) return "ПУ";
            return "БУ";
        }

        /// <summary>
        /// Включает или отключает поля модулей 4 и 5 в зависимости от
        /// выбранного уровня экзамена. Вызывается при смене переключателя.
        /// </summary>
        private void Level_Changed(object sender, RoutedEventArgs e)
        {
            // При первом срабатывании (во время InitializeComponent) поля
            // ещё могут быть не созданы — защищаемся от NullReferenceException.
            if (tbM4 == null || tbM5 == null) return;

            string level = GetSelectedLevel();
            int count = ExamLogic.GetModuleCount(level);

            tbM4.IsEnabled = count >= 4;
            tbM5.IsEnabled = count >= 5;

            // Очищаем недоступные поля, чтобы они не участвовали в расчёте.
            if (!tbM4.IsEnabled) tbM4.Clear();
            if (!tbM5.IsEnabled) tbM5.Clear();
        }

        /// <summary>
        /// Обработчик кнопки «Рассчитать»: считывает баллы, проверяет ввод,
        /// вычисляет сумму, процент и оценку, выводит результат.
        /// </summary>
        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string level = GetSelectedLevel();
                int count = ExamLogic.GetModuleCount(level);

                // Поля модулей в порядке их номеров.
                TextBox[] boxes = { tbM1, tbM2, tbM3, tbM4, tbM5 };
                int total = 0;

                for (int module = 1; module <= count; module++)
                {
                    string text = boxes[module - 1].Text.Trim();

                    // Валидация ввода через int.TryParse.
                    if (!int.TryParse(text, out int score))
                    {
                        MessageBox.Show(
                            "Введите целое число баллов за модуль " + module + ".",
                            "Ошибка ввода",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    // Проверка диапазона; при ошибке метод бросает ArgumentException.
                    ExamLogic.ValidateScore(score, module);
                    total += score;
                }

                int max = ExamLogic.GetMaxScore(level);
                double percent = ExamLogic.CalculatePercent(total, max);
                int grade = ExamLogic.GetGrade(percent);
                string gradeText = ExamLogic.GetGradeText(grade);

                tbResult.Text =
                    "Уровень: " + level + "\n" +
                    "Сумма баллов: " + total + " из " + max + "\n" +
                    "Процент выполнения: " + percent.ToString("0.##") + "%\n" +
                    "Оценка: " + grade + " (" + gradeText + ")";
            }
            catch (ArgumentException ex)
            {
                // Ожидаемые ошибки данных — понятное сообщение пользователю.
                MessageBox.Show(ex.Message, "Ошибка данных",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                // Любая непредвиденная ошибка — приложение не должно «вылетать».
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
