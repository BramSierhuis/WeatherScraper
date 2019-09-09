﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Globalization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace WeatherScraper
{
    class Program
    {
        private const string API_KEY = "80bb1b8d7cb2f1ef93bdfd3fa9df9990"; //The api key used to log in on openweathermap
        private const string ForecastUrl = //The url used to get the forecasts
            "http://api.openweathermap.org/data/2.5/forecast?q=Heemskerk,nl@&mode=xml&units=metric&APPID=" + API_KEY;

        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static List<Lesson> lessons = new List<Lesson>(); //All the upcoming lessons
        static List<Forecast> forecasts = new List<Forecast>(); //The forecasts of tomorrow

        static DateTime tomorrow = DateTime.Now.AddDays(1);


        static void Main(string[] args)
        {
            GetUpcommingLessons(); //Add all upcoming lessons to the list
            GetForecasts(); //Add all forecasts to the list

            foreach(Lesson l in lessons)
            {
                l.PrintAll();
            }

            foreach(Forecast f in forecasts)
            {
                f.PrintAll();
            }

            Console.ReadKey();
        }

        static void GetForecasts()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    ExtractForecast(client.DownloadString(ForecastUrl));
                }
                catch (WebException ex)
                {
                    Console.WriteLine("WebException: " + ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void GetUpcommingLessons()
        {
            #region APIExampleCode
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
                Console.WriteLine();
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            #endregion

            // List events.
            Events events = request.Execute();

            if (events.Items != null && events.Items.Count > 0) //If there are any events planned
            {
                foreach (var eventItem in events.Items)
                {
                    try
                    {
                        DateTime date = (DateTime)eventItem.Start.DateTime; //Get the date and time of the event

                        if (date.Date == tomorrow.Date)
                        { //We only care about the lessons of tomorrow
                            string name = eventItem.Summary; //The name of the planned event
                            string classRoom = eventItem.Location.ToString(); //The classroom of the event

                            Lesson lesson = new Lesson(name, date, classRoom);
                            lessons.Add(lesson);
                        }
                    }
                    catch (Exception e)
                    {
                        //An error will be thrown if there is no location set. Only lessons have locaton set
                    }
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
        }

        static void ExtractForecast(string xml)
        {
            XmlDocument xml_doc = new XmlDocument();
            xml_doc.LoadXml(xml);

            foreach (XmlNode time_node in xml_doc.SelectNodes("//time"))
            {
                // Get the time in UTC.
                DateTime forecastTime =
                    DateTime.Parse(time_node.Attributes["from"].Value,
                        null, DateTimeStyles.AssumeUniversal);

                if(tomorrow.Date == forecastTime.Date)
                {
                    double precipitation;

                    try
                    {
                        XmlNode temp_node = time_node.SelectSingleNode("temperature");
                        double temp = double.Parse(temp_node.Attributes["value"].Value, CultureInfo.InvariantCulture);

                        XmlNode wind_node = time_node.SelectSingleNode("windSpeed");
                        double windSpeedInMph = double.Parse(wind_node.Attributes["mps"].Value, CultureInfo.InvariantCulture);

                        //Check if the rain node is empty, if so set rain as 0
                        try
                        {
                            XmlNode precipitation_node = time_node.SelectSingleNode("precipitation");
                            precipitation = double.Parse(precipitation_node.Attributes["value"].Value, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            precipitation = 0;
                        }

                        Forecast forecast = new Forecast(temp, forecastTime, precipitation, windSpeedInMph);
                        forecasts.Add(forecast);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}
