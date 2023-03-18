using SF2022User_NN_Lib;

namespace ShopTest
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void AvaliablePeriods_NotNullResult()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            Assert.IsNotNull(Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [TestMethod]
        public void AvaliablePeriods_NullResult()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(18, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(8, 0, 0);
            int consultationTime = 30;

            Assert.IsNull(Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration10()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 10;

            int expected = 48;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration20()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 20;

            int expected = 23;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration30()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            int expected = 16;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration40()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 40;

            int expected = 11;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration50()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 50;

            int expected = 8;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualCountPeriodsDuration60()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 60;

            int expected = 7;
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime).Length);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualFirstAvaliable()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 40;

            string expected = "8:0-8:40";
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime)[0]);
        }

        [TestMethod]
        public void AvaliablePeriods_EqualFirstAvaliableWhenNotFirst()
        {
            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(8, 20, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0) };
            startTimes[0] = new TimeSpan(8, 20, 0);
            startTimes[1] = new TimeSpan(15, 0, 0);
            startTimes[2] = new TimeSpan(15, 30, 0); int[] durations = new int[] { 30, 30, 50 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 40;

            string expected = "8:50-9:30";
            Assert.AreEqual(expected, Calculations.AvaliablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime)[0]);
        }
    }
}