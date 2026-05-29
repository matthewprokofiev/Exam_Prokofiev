using System;

namespace Exam
{
    /// <summary>
    /// Класс с бизнес-логикой подсчёта баллов демонстрационного экзамена.
    /// Логика вынесена из интерфейса: все методы принимают простые типы
    /// (int, double, string) и возвращают результат. При некорректных
    /// входных данных методы выбрасывают <see cref="ArgumentException"/>.
    /// </summary>
    public static class ExamLogic
    {
        /// <summary>
        /// Возвращает максимальное количество баллов за указанный модуль.
        /// Модуль 1 = 10, 2 = 15, 3 = 25, 4 = 25, 5 = 25.
        /// </summary>
        /// <param name="module">Номер модуля (1..5).</param>
        /// <exception cref="ArgumentException">Если номер модуля вне диапазона 1..5.</exception>
        public static int GetModuleMax(int module)
        {
            switch (module)
            {
                case 1: return 10;
                case 2: return 15;
                case 3: return 25;
                case 4: return 25;
                case 5: return 25;
                default:
                    throw new ArgumentException("Номер модуля должен быть от 1 до 5.");
            }
        }

        /// <summary>
        /// Возвращает количество модулей для выбранного уровня экзамена:
        /// "БУ" = 3, "ПУ" = 4, "ПУ+" = 5.
        /// </summary>
        /// <param name="level">Код уровня экзамена ("БУ", "ПУ", "ПУ+").</param>
        /// <exception cref="ArgumentException">Если уровень не распознан.</exception>
        public static int GetModuleCount(string level)
        {
            switch (level)
            {
                case "БУ": return 3;
                case "ПУ": return 4;
                case "ПУ+": return 5;
                default:
                    throw new ArgumentException("Неизвестный уровень экзамена.");
            }
        }

        /// <summary>
        /// Возвращает максимально возможную сумму баллов для уровня экзамена:
        /// БУ = 50, ПУ = 75, ПУ+ = 100.
        /// </summary>
        /// <param name="level">Код уровня экзамена ("БУ", "ПУ", "ПУ+").</param>
        /// <exception cref="ArgumentException">Если уровень не распознан.</exception>
        public static int GetMaxScore(string level)
        {
            int count = GetModuleCount(level);
            int max = 0;
            for (int module = 1; module <= count; module++)
            {
                max += GetModuleMax(module);
            }
            return max;
        }

        /// <summary>
        /// Проверяет балл за конкретный модуль. Балл должен быть от 0
        /// до максимума по модулю включительно. Возвращает тот же балл,
        /// если он корректен.
        /// </summary>
        /// <param name="score">Введённый балл.</param>
        /// <param name="module">Номер модуля (1..5).</param>
        /// <exception cref="ArgumentException">
        /// Если балл отрицательный или превышает максимум модуля.
        /// </exception>
        public static int ValidateScore(int score, int module)
        {
            int max = GetModuleMax(module);
            if (score < 0)
            {
                throw new ArgumentException("Балл не может быть отрицательным.");
            }
            if (score > max)
            {
                throw new ArgumentException(
                    "Балл за модуль " + module + " не может превышать " + max + ".");
            }
            return score;
        }

        /// <summary>
        /// Вычисляет процент выполнения работы: totalScore / maxScore * 100.
        /// </summary>
        /// <param name="totalScore">Набранная сумма баллов.</param>
        /// <param name="maxScore">Максимально возможная сумма баллов.</param>
        /// <returns>Процент выполнения (0..100).</returns>
        /// <exception cref="ArgumentException">
        /// Если максимум не положителен, сумма отрицательна или превышает максимум.
        /// </exception>
        public static double CalculatePercent(int totalScore, int maxScore)
        {
            if (maxScore <= 0)
            {
                throw new ArgumentException("Максимальный балл должен быть положительным.");
            }
            if (totalScore < 0)
            {
                throw new ArgumentException("Сумма баллов не может быть отрицательной.");
            }
            if (totalScore > maxScore)
            {
                throw new ArgumentException("Сумма баллов не может превышать максимум.");
            }
            return (double)totalScore / maxScore * 100.0;
        }

        /// <summary>
        /// Переводит процент выполнения в оценку по 5-балльной шкале:
        /// 80..100 = 5, 60..79 = 4, 40..59 = 3, 0..39 = 2.
        /// </summary>
        /// <param name="percent">Процент выполнения (0..100).</param>
        /// <exception cref="ArgumentException">Если процент вне диапазона 0..100.</exception>
        public static int GetGrade(double percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentException("Процент должен быть в диапазоне от 0 до 100.");
            }
            if (percent >= 80) return 5;
            if (percent >= 60) return 4;
            if (percent >= 40) return 3;
            return 2;
        }

        /// <summary>
        /// Возвращает текстовое описание оценки.
        /// </summary>
        /// <param name="grade">Оценка по 5-балльной шкале (2..5).</param>
        /// <exception cref="ArgumentException">Если оценка вне диапазона 2..5.</exception>
        public static string GetGradeText(int grade)
        {
            switch (grade)
            {
                case 5: return "отлично";
                case 4: return "хорошо";
                case 3: return "удовлетворительно";
                case 2: return "неудовлетворительно";
                default:
                    throw new ArgumentException("Оценка должна быть от 2 до 5.");
            }
        }
    }
}
