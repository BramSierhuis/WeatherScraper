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
        public double windSpeedInKmh;

        public Forecast(double temp, DateTime date, double amountOfRain, double windSpeedInMps)
        {
            this.temp = temp;
            this.date = date;
            this.amountOfRain = amountOfRain;
            windSpeedInKmh = windSpeedInMps * 3.6;
        }

        public void PrintAll()
        {
            Console.WriteLine(date + " t:" + temp.ToString("0.0") + " r:" + amountOfRain.ToString("0.000") + " w:" + windSpeedInKmh.ToString("0.000"));
        }
    }
}
