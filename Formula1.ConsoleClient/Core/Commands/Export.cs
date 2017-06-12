namespace Formula1.ConsoleClient.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using WebGrease.Css.Extensions;

    public class Export : ICommand
    {
        private readonly ISerializer serializer; //db??

        private Formula1Context db;
        private ICollection<string> headers;

        public Export(ISerializer serializer)
        {
            this.serializer = serializer;
            this.db = new Formula1Context();
            this.headers = new HashSet<string>();
        }

        //export to pdf getDriversStandingForSeason 2017
        // example input command:"report to pdf (path) getalldrivers 2017"
        public void Execute(IList<string> parameters)
        {
            string filePath = parameters[0];
            parameters.RemoveAt(0);
            string command = parameters[0];
            parameters.RemoveAt(0);

            switch (command.ToLower())
            {
                case "getalldrivers": serializer.Export(filePath, this.GetAllDrivers(), headers); break;
                case "getallconstructors": serializer.Export(filePath, this.GetAllConstructors(), headers); break;
                case "getconstructorsstangingforseason": serializer.Export(filePath, GetConstructorsStangingForSeason(parameters[0]), headers); break;
                case "getcurrentconstructorstandings": serializer.Export(filePath, GetCurrentConstructorsStandings(), headers); break;
                case "getdriversstandingforseason": serializer.Export(filePath, GetDriversStandingForSeason(string.Join(" ",parameters)), headers); break;
                case "getcurrentdriversstandings": serializer.Export(filePath, GetCurrentDriversStandings(), headers); break;
                case "getdriveractiveseasons": serializer.Export(filePath, GetDriverActiveSeasons(string.Join(" ", parameters)), headers); break;
                case "getconstructoractiveseasons": serializer.Export(filePath, GetConstructorActiveSeasons(string.Join(" ", parameters)), headers); break;
                case "getraceresults":
                    string season = parameters[0];
                    parameters.RemoveAt(0);
                    serializer.Export(filePath, GetRaceResults(season, string.Join(" ", parameters)), headers); break;

                default: throw new ArgumentException("Invalid data to export");
            }
        }

        private IDictionary<string, string> GetAllDrivers()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            this.db.Drivers.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

            headers.Add("Driver");
            headers.Add("Information");

            return result;
        }

        private IDictionary<string, string> GetAllConstructors()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            this.db.Constructors.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

            headers.Add("Constructor");
            headers.Add("Information");

            return result;
        }

        private IDictionary<string, string> GetConstructorsStangingForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> constructors =
                new HashSet<string>(this.db.SeasonsParticipants
                .Where(s => s.Season.Year == season)
                .Select(s => s.Constructor.Name).ToList().Distinct());

            foreach (string constructor in constructors)
            {
                result[constructor] = GetConstructorScoreForSeason(constructor, season);
            }

            headers.Add("Constructor");
            headers.Add("Score");

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        private string GetConstructorScoreForSeason(string constructorName, string season)
        {
            string result = string.Empty;
            result = this.db.Races.Where(r => r.Constructor.Name == constructorName).Where(r => r.Season.Year == season).Select(r => r.Score).ToList().Sum().ToString();
            //     db.Constructors.Where(d => d.Name == constructorName).FirstOrDefault()
            //         .Races.Where(r => r.Season.Year == season)
            //         .Select(r => r.Score).ToList().Sum().ToString();

            return result;
        }

        private IDictionary<string, string> GetCurrentConstructorsStandings()
        {
            string currentSeason = GetCurrentSeason();
            IDictionary<string, string> result = GetConstructorsStangingForSeason(currentSeason);

            headers.Add("Constructor");
            headers.Add("Current Score");

            return result;
        }

        private IDictionary<string, string> GetDriversStandingForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> drivers =
                new HashSet<string>(this.db.SeasonsParticipants
                .Where(s => s.Season.Year == season)
                .Select(s => s.Driver.Name).ToList().Distinct());
            foreach (string driver in drivers)
            {
                result[driver] = GetDriverScoreForSeason(driver, season);
            }

            headers.Add("Driver");
            headers.Add("Score");

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        private string GetDriverScoreForSeason(string driverName, string season)
        {
            string result = string.Empty;
            result = this.db.Races.Where(r => r.Driver.Name == driverName).Where(r => r.Season.Year == season).Select(r => r.Score).ToList().Sum().ToString();
            //    db.Drivers.Where(d => d.Name == driverName)
            //        .FirstOrDefault()
            //        .Races.Where(r => r.Season.Year == season)
            //        .Select(r => r.Score).ToList().Sum().ToString();

            return result;
        }

        private IDictionary<string, string> GetCurrentDriversStandings()
        {
            string currentSeason = GetCurrentSeason();
            Console.WriteLine(currentSeason);
            IDictionary<string, string> result = GetDriversStandingForSeason(currentSeason);

            headers.Add("Driver");
            headers.Add("Current Score");

            return result;
        }

        private IDictionary<string, string> GetDriverActiveSeasons(string driverName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> seasons = new HashSet<string>(this.db.SeasonsParticipants.Where(s => s.Driver.Name == driverName)
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

            headers.Add("Season");
            headers.Add("Score");

            var sort = result.OrderBy(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        private IDictionary<string, string> GetConstructorActiveSeasons(string constructorName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> seasons = new HashSet<string>(this.db.SeasonsParticipants.Where(s => s.Constructor.Name == constructorName)
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

            headers.Add("Season");
            headers.Add("Score");

            var sort = result.OrderBy(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        private IDictionary<string, string> GetRaceResults(string season, string grandPrixName)
        {
            Console.WriteLine(season);
            Console.WriteLine(grandPrixName);
            IDictionary<string, string> result = new Dictionary<string, string>();
            this.db.Races.Where(r => r.Season.Year == season).Where(r => r.GrandPrix.Name == grandPrixName)
                .ForEach(r =>
                {
                    result[$"Driver: {r.Driver.Name}/ Constructor: {r.Constructor.Name}"] = r.Score.ToString();
                });

            headers.Add("Participant");
            headers.Add("Score");

            var sort = result.OrderByDescending(x => int.Parse(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            return sort;
        }

        private string GetCurrentSeason()
        {
            string currentSeason = this.db.Seasons.OrderByDescending(s => s.Year).FirstOrDefault().Year.ToString();
            return currentSeason;
        }

        // and many others
    }
}
