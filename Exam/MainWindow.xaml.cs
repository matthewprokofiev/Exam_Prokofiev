using System;
using System.Windows;
using System.Windows.Controls;

namespace Exam
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetSelectedLevel()
        {
            if (rbPUPlus.IsChecked == true) return "ПУ+";
            if (rbPU.IsChecked == true) return "ПУ";
            return "БУ";
        }

        private void Level_Changed(object sender, RoutedEventArgs e)
        {
            if (tbM4 == null || tbM5 == null) return;

            string level = GetSelectedLevel();
            int count = ExamLogic.GetModuleCount(level);

            tbM4.IsEnabled = count >= 4;
            tbM5.IsEnabled = count >= 5;

            if (!tbM4.IsEnabled) tbM4.Clear();
            if (!tbM5.IsEnabled) tbM5.Clear();
        }
>
        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string level = GetSelectedLevel();
                int count = ExamLogic.GetModuleCount(level);

                TextBox[] boxes = { tbM1, tbM2, tbM3, tbM4, tbM5 };
                int total = 0;

                for (int module = 1; module <= count; module++)
                {
                    string text = boxes[module - 1].Text.Trim();

                    if (!int.TryParse(text, out int score))
                    {
                        MessageBox.Show(
                            "Введите целое число баллов за модуль " + module + ".",
                            "Ошибка ввода",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

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
                MessageBox.Show(ex.Message, "Ошибка данных",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
