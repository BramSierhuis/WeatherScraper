using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherScraper
{
    class Forecast
    {
        public double temp;
        public DateTime date;
        public double amountOfRain;

        public Forecast(double temp, DateTime date, double amountOfRain)
        {
            this.temp = temp;
            this.date = date;
            this.amountOfRain = amountOfRain;
        }

        public void PrintAll()
        {
            Console.WriteLine(date + " " + temp + " " + amountOfRain);
        }
    }
}
