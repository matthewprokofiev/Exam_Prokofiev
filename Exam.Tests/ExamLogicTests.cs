using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exam;

namespace Exam.Tests
{
    /// <summary>
    /// Автоматизированные тесты класса логики ExamLogic.
    /// Тестируются только методы класса логики. Набор покрывает:
    /// корректные данные, граничные значения и некорректные данные (исключения).
    /// </summary>
    [TestClass]
    public class ExamLogicTests
    {
        // ===================== GetModuleMax =====================
        [TestMethod]
        public void GetModuleMax_Module1_Returns10()
        {
            Assert.AreEqual(10, ExamLogic.GetModuleMax(1));
        }

        [TestMethod]
        public void GetModuleMax_Module3_Returns25()
        {
            Assert.AreEqual(25, ExamLogic.GetModuleMax(3));
        }

        [TestMethod]
        public void GetModuleMax_Module6_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.GetModuleMax(6));
        }

        // ===================== GetModuleCount =====================
        [TestMethod]
        public void GetModuleCount_BU_Returns3()
        {
            Assert.AreEqual(3, ExamLogic.GetModuleCount("БУ"));
        }

        [TestMethod]
        public void GetModuleCount_PUPlus_Returns5()
        {
            Assert.AreEqual(5, ExamLogic.GetModuleCount("ПУ+"));
        }

        [TestMethod]
        public void GetModuleCount_UnknownLevel_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.GetModuleCount("XX"));
        }

        // ===================== GetMaxScore =====================
        [TestMethod]
        public void GetMaxScore_BU_Returns50()
        {
            Assert.AreEqual(50, ExamLogic.GetMaxScore("БУ"));
        }

        [TestMethod]
        public void GetMaxScore_PU_Returns75()
        {
            Assert.AreEqual(75, ExamLogic.GetMaxScore("ПУ"));
        }

        [TestMethod]
        public void GetMaxScore_PUPlus_Returns100()
        {
            Assert.AreEqual(100, ExamLogic.GetMaxScore("ПУ+"));
        }

        // ===================== ValidateScore =====================
        [TestMethod]
        public void ValidateScore_MaxForModule1_ReturnsSame()
        {
            Assert.AreEqual(10, ExamLogic.ValidateScore(10, 1));
        }

        [TestMethod]
        public void ValidateScore_Negative_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.ValidateScore(-1, 1));
        }

        [TestMethod]
        public void ValidateScore_AboveMaxModule1_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.ValidateScore(11, 1));
        }

        // ===================== CalculatePercent =====================
        [TestMethod]
        public void CalculatePercent_FullScore_Returns100()
        {
            Assert.AreEqual(100.0, ExamLogic.CalculatePercent(50, 50), 0.0001);
        }

        [TestMethod]
        public void CalculatePercent_HalfScore_Returns50()
        {
            Assert.AreEqual(50.0, ExamLogic.CalculatePercent(50, 100), 0.0001);
        }

        [TestMethod]
        public void CalculatePercent_ScoreAboveMax_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.CalculatePercent(60, 50));
        }

        [TestMethod]
        public void CalculatePercent_ZeroMax_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.CalculatePercent(0, 0));
        }

        // ===================== GetGrade (границы шкалы) =====================
        [TestMethod]
        public void GetGrade_80Percent_Returns5()
        {
            Assert.AreEqual(5, ExamLogic.GetGrade(80));
        }

        [TestMethod]
        public void GetGrade_JustBelow80_Returns4()
        {
            Assert.AreEqual(4, ExamLogic.GetGrade(79.99));
        }

        [TestMethod]
        public void GetGrade_40Percent_Returns3()
        {
            Assert.AreEqual(3, ExamLogic.GetGrade(40));
        }

        [TestMethod]
        public void GetGrade_JustBelow40_Returns2()
        {
            Assert.AreEqual(2, ExamLogic.GetGrade(39.99));
        }

        [TestMethod]
        public void GetGrade_Above100_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.GetGrade(101));
        }

        // ===================== GetGradeText =====================
        [TestMethod]
        public void GetGradeText_5_ReturnsExcellent()
        {
            Assert.AreEqual("отлично", ExamLogic.GetGradeText(5));
        }

        [TestMethod]
        public void GetGradeText_InvalidGrade_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ExamLogic.GetGradeText(1));
        }

        // ===================== Комплексные сценарии =====================
        [TestMethod]
        public void Scenario_BU_FullScore_GivesGrade5()
        {
            // БУ: 10 + 15 + 25 = 50 из 50 -> 100% -> 5
            int total = ExamLogic.ValidateScore(10, 1)
                      + ExamLogic.ValidateScore(15, 2)
                      + ExamLogic.ValidateScore(25, 3);
            int max = ExamLogic.GetMaxScore("БУ");
            double percent = ExamLogic.CalculatePercent(total, max);

            Assert.AreEqual(50, total);
            Assert.AreEqual(5, ExamLogic.GetGrade(percent));
        }

        [TestMethod]
        public void Scenario_PU_LowScore_GivesGrade2()
        {
            // ПУ: 10 из 75 -> ~13% -> 2
            int max = ExamLogic.GetMaxScore("ПУ");
            double percent = ExamLogic.CalculatePercent(10, max);

            Assert.IsTrue(percent < 40);
            Assert.AreNotEqual(5, ExamLogic.GetGrade(percent));
            Assert.AreEqual(2, ExamLogic.GetGrade(percent));
        }
    }
}
