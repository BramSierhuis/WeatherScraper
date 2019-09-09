using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherScraper
{
    class Lesson
    {
        public string name;
        public DateTime date;
        public string location;

        public Lesson(string name, DateTime date, string location)
        {
            this.name = name;
            this.date = date;
            this.location = location;
        }

        public void PrintAll()
        {
            Console.WriteLine(name + " " + date + " " + location);
        }
    }
}
