using System;

namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        /// <summary>
        /// Формирует список свободных временных интервалов
        /// </summary>
        /// <param name="startTimes">Список начал занятых промежутков времени</param>
        /// <param name="durations">Длительность работы на занятых промежутках времени</param>
        /// <param name="beginWorkingTime">Время начала работы менеджера</param>
        /// <param name="endWorkingTime">Время окончания работы менеджера</param>
        /// <param name="consultationTime">Стандартное время работы</param>
        /// <returns>Строка со списком свободных промежутков времени</returns>
        public static string[] AvaliablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            if (beginWorkingTime > endWorkingTime)
            {
                return null;
            }

            string[] times = new string[0];

            while (beginWorkingTime < endWorkingTime)
            {
                bool check = true;
                for (int i = 0; i < startTimes.Length; i++)
                {
                    if (startTimes[i].TotalMinutes >= beginWorkingTime.TotalMinutes &&
                        startTimes[i].TotalMinutes < beginWorkingTime.Add(new TimeSpan(0, consultationTime, 0)).TotalMinutes)
                    {
                        check = false;
                        beginWorkingTime = startTimes[i].Add(new TimeSpan(0, durations[i], 0));
                        break;
                    }
                }

                if (check)
                {
                    Array.Resize(ref times, times.Length + 1);
                    times[times.Length - 1] = beginWorkingTime.Hours + ":" + beginWorkingTime.Minutes + "-";
                    beginWorkingTime = beginWorkingTime.Add(new TimeSpan(0, consultationTime, 0));
                    times[times.Length - 1] += beginWorkingTime.Hours + ":" + beginWorkingTime.Minutes;
                }

                if (beginWorkingTime.Add(new TimeSpan(0, consultationTime, 0)) >= endWorkingTime)
                {
                    break;
                }
            }

            return times;
        }
    }
}