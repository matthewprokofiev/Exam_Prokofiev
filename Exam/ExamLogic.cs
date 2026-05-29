using System;

namespace Exam
{
    public static class ExamLogic
    {
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
