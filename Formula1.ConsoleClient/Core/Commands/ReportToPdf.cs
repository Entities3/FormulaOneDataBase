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
            string command = parameters[1];
            if (parameters.Count > 2)
            {
                string additionalParam = parameters[2];
            }

            switch (command.ToLower())
            {
                case "getalldrivers": exporter.Export(filePath, GetAllDrivers(), headers); break;
                    //case "constructors": this.ImportConstructors(json); break;
                    //case "circuits": this.ImportCircuits(json); break;
                    //case "races": this.ImportRaces(json); break;
                    //case "seasons": this.ImportSeasons(json); break;
                    //default: throw new ArgumentException("Invalid data to import");
            }
        }

        public static IDictionary<string, string> GetAllDrivers()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Drivers.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

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
            return sort;
        }

        public static string GetConstructorScoreForSeason(string constructorName, string season)
        {
            string result = string.Empty;
            result = db.Races.Where(r => r.Constructor.Name == constructorName).Where(r => r.Season.Year == season).Select(r => r.Score).ToList().Sum().ToString();
            //     db.Constructors.Where(d => d.Name == constructorName).FirstOrDefault()
            //         .Races.Where(r => r.Season.Year == season)
            //         .Select(r => r.Score).ToList().Sum().ToString();

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
