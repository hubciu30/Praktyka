using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatyCzas
{
    class CDate
    {

        public CDate(string startTime, string endTime)
        {
            TimeToHome = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + endTime);
            StartTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + startTime);
            TimeToHomeTicks = TimeToHome.Ticks;
            DayInWeek = new Dictionary<string, int>();
            DayInWeek.Add("Monday", 1);
            DayInWeek.Add("Tuesday", 2);
            DayInWeek.Add("Wednesday", 3);
            DayInWeek.Add("Thursday", 4);
            DayInWeek.Add("Friday", 5);
            DayInWeek.Add("Saturday", 6);
            DayInWeek.Add("Sunday", 7);
        }

        private Dictionary<string, int> DayInWeek { get; set; }
        private DateTime StartTime { get; set; }
        private TimeSpan Time { get; set; }
        private DateTime TimeToHome { get; set; }
        private long TimeToHomeTicks { get; set; }
        private string CorrectTime(int time)
        {
            if (time < 10)
            {
                return "0" + time;
            }
            else
            {
                return time.ToString();
            }
        }

        public long GetTick()
        {
            return TimeToHomeTicks - DateTime.Now.Ticks;
        }

        public float GetPercent()
        {
            long currentTime = (((DateTime.Now.Hour * 60) + DateTime.Now.Minute) * 60) + DateTime.Now.Second;
            long finalTime = (((TimeToHome.Hour * 60) + TimeToHome.Minute) * 60) + TimeToHome.Second;
            long startTime = (((StartTime.Hour * 60) + StartTime.Minute) * 60) + StartTime.Second;
            return (currentTime - startTime) / ((float)(finalTime - startTime)) * 100; 
        }

        public string GetWeekendPercent()
        {
            string result = "";
            string today = DateTime.Now.DayOfWeek.ToString();

            foreach(var key in DayInWeek)
            {
                if(key.Key == today)
                {
                    double tmp = Math.Round(key.Value * 100 / 5f, 2);
                    if(tmp > 100)
                    {
                        result = "Weekend";
                    }
                    else if(tmp == 100)
                    {
                        result = "Ostatni dzień!";
                    }
                    else
                    {
                        result = tmp.ToString() + "%";
                    }
                    break;
                }
            }
            return result;
        }

        public string GetTimeToEndWork()
        {
            Time = TimeSpan.FromTicks(TimeToHomeTicks - DateTime.Now.Ticks);
            return String.Format("{0}:{1}:{2}", CorrectTime(Time.Hours), CorrectTime(Time.Minutes), CorrectTime(Time.Seconds));
        }
    }
}
