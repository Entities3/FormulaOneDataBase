namespace Formula1.ConsoleClient.Core.Commands
{
    using System.Collections.Generic;
    using Data;
    using System.Linq;
    using WebGrease.Css.Extensions;
    using System;
    using Providers;

    public class ReportToPdf : ICommand
    {
        private static Formula1Context db = new Formula1Context();
        private static PdfExporter exporter = new PdfExporter();
        private static ICollection<string> headers = new HashSet<string>();

        //export to pdf getDriversStandingForSeason 2017
        // example input command:"report to pdf (path) getalldrivers 2017"
        public void Execute(IList<string> parameters)
        {


            string filePath = parameters[0];
            parameters.RemoveAt(0);
            string command = parameters[0];
            parameters.RemoveAt(0);

            string[] additionalParam = new string[parameters.Count];

            int i = 0;

            while  (parameters.Count > 0)
                {
                    additionalParam[i] = parameters[i];
                    parameters.RemoveAt(0);
                    i++;
                }


            switch (command.ToLower())
            {
                case "getalldrivers": exporter.Export(filePath, GetAllDrivers(), headers); break;
                case "getallconstructors": exporter.Export(filePath, GetAllConstructors(), headers); break;
                case "getconstructorsstangingforseason": exporter.Export(filePath, GetConstructorsStangingForSeason(additionalParam[0]), headers); break;
               // case "getconstructorscoreforseason": exporter.Export(filePath, GetConstructorScoreForSeason(additionalParam[0], additionalParam[1]), headers); break;
                case "getcurrentconstructorsctandings": exporter.Export(filePath, GetCurrentConstructorsStandings(), headers); break;
                case "getdriversstandingforseason": exporter.Export(filePath, GetDriversStandingForSeason(additionalParam[0]), headers); break;
                case "getcurrentdriversstandings": exporter.Export(filePath, GetCurrentDriversStandings(), headers); break;
                case "getdriveractiveseasons": exporter.Export(filePath, GetDriverActiveSeasons(additionalParam[0]), headers); break;
                case "getconstructoractiveseasons": exporter.Export(filePath, GetConstructorActiveSeasons(additionalParam[0]), headers); break;
                case "GetRaceResults": exporter.Export(filePath, GetRaceResults(additionalParam[0], additionalParam[1]), headers); break;

                default: throw new ArgumentException("Invalid data to export");
            }
        }

        public static IDictionary<string, string> GetAllDrivers()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Drivers.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

            headers.Clear();
            headers.Add("Driver Name");
            headers.Add("Information Url");

            return result;
        }

        public static IDictionary<string, string> GetAllConstructors()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Constructors.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

            headers.Clear();
            headers.Add("Constructor Name");
            headers.Add("Information Url");

            return result;
        }

        public static IDictionary<string, string> GetConstructorsStangingForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> constructors =
                new HashSet<string>(db.SeasonsParticipants
                .Where(s => s.Season.Year == season)
                .Select(s => s.Constructor.Name).ToList().Distinct());

            foreach (string constructor in constructors)
            {
                result[constructor] = GetConstructorScoreForSeason(constructor, season);
            }

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);

            headers.Clear();
            headers.Add("Constructor Name");
            headers.Add("Score");

            return sort;
        }

        public static string GetConstructorScoreForSeason(string constructorName, string season)
        {
            string result = string.Empty;
            result = db.Races.Where(r => r.Constructor.Name == constructorName).Where(r => r.Season.Year == season).Select(r => r.Score).ToList().Sum().ToString();
            //     db.Constructors.Where(d => d.Name == constructorName).FirstOrDefault()
            //         .Races.Where(r => r.Season.Year == season)
            //         .Select(r => r.Score).ToList().Sum().ToString();

            headers.Clear();
            headers.Add("Score");

            return result;
        }

        public static IDictionary<string, string> GetCurrentConstructorsStandings()
        {
            string currentSeason = GetCurrentSeason();
            IDictionary<string, string> result = GetConstructorsStangingForSeason(currentSeason);

            return result;
        }

        public static IDictionary<string, string> GetDriversStandingForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> drivers =
                new HashSet<string>(db.SeasonsParticipants
                .Where(s => s.Season.Year == season)
                .Select(s => s.Driver.Name).ToList().Distinct());
            foreach (string driver in drivers)
            {
                result[driver] = GetDriverScoreForSeason(driver, season);
            }

            headers.Clear();
            headers.Add("Driver Name");
            headers.Add("Score");

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        public static string GetDriverScoreForSeason(string driverName, string season)
        {
            string result = string.Empty;
            result = db.Races.Where(r => r.Driver.Name == driverName).Where(r => r.Season.Year == season).Select(r => r.Score).ToList().Sum().ToString();
            //    db.Drivers.Where(d => d.Name == driverName)
            //        .FirstOrDefault()
            //        .Races.Where(r => r.Season.Year == season)
            //        .Select(r => r.Score).ToList().Sum().ToString();

            return result;
        }

        public static IDictionary<string, string> GetCurrentDriversStandings()
        {
            string currentSeason = GetCurrentSeason();
            Console.WriteLine(currentSeason);
            IDictionary<string, string> result = GetDriversStandingForSeason(currentSeason);

            return result;
        }

        private IDictionary<string, string> GetDriverActiveSeasons(string driverName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> seasons = new HashSet<string>(db.SeasonsParticipants.Where(s => s.Driver.Name == driverName)
                  .Select(s => s.Season.Year).ToList());

            foreach (string season in seasons)
            {
                result[season] = GetDriverScoreForSeason(driverName, season);
            }
            //      db.Drivers.Where(d => d.Name == driverName)
            //          .FirstOrDefault().Seasons
            //          .ForEach(s =>
            //          {
            //              result[$"Season {s.Year}"] = GetDriverScoreForSeason(driverName, s.Year);
            //          });

            headers.Clear();
            headers.Add("Driver Name");
            headers.Add("Season");

            var sort = result.OrderBy(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        public static IDictionary<string, string> GetConstructorActiveSeasons(string constructorName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> seasons = new HashSet<string>(db.SeasonsParticipants.Where(s => s.Constructor.Name == constructorName)
                  .Select(s => s.Season.Year).ToList());

            foreach (string season in seasons)
            {
                result[season] = GetConstructorScoreForSeason(constructorName, season);
            }
            //      db.Constructors.Where(d => d.Name == constructorName)
            //          .FirstOrDefault().Seasons
            //          .ForEach(s =>
            //          {
            //              result[$"Season {s.Year}"] = GetConstructorScoreForSeason/(constructorName, /s.Year);
            //          });

            headers.Clear();
            headers.Add("Constructor Name");
            headers.Add("Season Score");

            var sort = result.OrderBy(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        public static IDictionary<string, string> GetRaceResults(string season, string grandPrixName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Races.Where(r => r.Season.Year == season).Where(r => r.GrandPrix.Name == grandPrixName)
                .ForEach(r =>
                {
                    result[$"Driver: {r.Driver.Name}/ Constructor: {r.Constructor.Name}"] = r.Score.ToString();
                });

            headers.Clear();
            headers.Add("Driver Name");
            headers.Add("Constructor");

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        public static string GetCurrentSeason()
        {
            string currentSeason = db.Seasons.OrderByDescending(s => s.Year).FirstOrDefault().Year.ToString();
            return currentSeason;
        }


        // and many others
    }
}
