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

        public Forecast(double temp, DateTime date, double amountOfRain, double windSpeedInMph)
        {
            this.temp = temp;
            this.date = date;
            this.amountOfRain = amountOfRain;
            windSpeedInKmh = windSpeedInMph * 1.609344;
        }

        public void PrintAll()
        {
            Console.WriteLine(date + " t:" + temp + " r:" + amountOfRain + " w:" + windSpeedInKmh);
        }
    }
}
