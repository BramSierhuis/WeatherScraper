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
            Console.WriteLine("Date and time of Forecast: " + date);
            Console.WriteLine("Temperature: " + temp.ToString("0.0") + " Degrees Celcius");
            Console.WriteLine("Amount of rain: " + amountOfRain.ToString("0.0") + "mm");
            Console.WriteLine("Windspeed: " + windSpeedInKmh.ToString("0.0") + "km/h");
        }
    }
}
