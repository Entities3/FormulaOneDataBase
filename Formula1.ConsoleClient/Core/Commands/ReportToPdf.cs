namespace Formula1.ConsoleClient.Core.Commands
{
    using System.Collections.Generic;
    using Data;
    using System.Linq;
    using System.Collections;
    using WebGrease.Css.Extensions;
    using System;

    public class ReportToPdf : ICommand
    {
        private static Formula1Context db = new Formula1Context();

        // example input command:"report to pdf get all drivers"
        public void Execute(IList<string> parameters)
        {

            //to do
        }

        public static IDictionary<string, string> GetAllDrivers()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Drivers.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });

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

            result.OrderBy(x => x.Value); //???
            return result;
        }

        public static string GetConstructorScoreForSeason(string constructorName, string season)
        {
            string result = string.Empty;
            db.Constructors.Where(d => d.Name == constructorName).FirstOrDefault()
                .Races.Where(r => r.Season.Year == season)
                .Select(r => r.Score).ToList().Sum().ToString();

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

            result.OrderBy(x => x.Value);
            return result; //??
        }

        public static string GetDriverScoreForSeason(string driverName, string season)
        {
            string result = string.Empty;
            db.Drivers.Where(d => d.Name == driverName)
                .FirstOrDefault()
                .Races.Where(r => r.Season.Year == season)
                .Select(r => r.Score).ToList().Sum().ToString();

            return result;
        }

        public static IDictionary<string, string> GetCurrentDriversStandings()
        {
            string currentSeason = GetCurrentSeason();
            IDictionary<string, string> result = GetDriversStandingForSeason(currentSeason);

            return result;
        }

        private IDictionary<string, string> GetDriverActiveSeasons(string driverName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Drivers.Where(d => d.Name == driverName)
                .FirstOrDefault().Seasons
                .ForEach(s =>
            {
                result[$"Season {s.Year}"] = GetDriverScoreForSeason(driverName, s.Year);
            });

            result.OrderBy(x => x.Value); //??
            return result;
        }

        public static IDictionary<string, string> GetConstructorActiveSeasons(string constructorName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Constructors.Where(d => d.Name == constructorName)
                .FirstOrDefault().Seasons
                .ForEach(s =>
                {
                    result[$"Season {s.Year}"] = GetConstructorScoreForSeason(constructorName, s.Year);
                });

            result.OrderBy(x => x.Value); //??
            return result;
        }

        public static IDictionary<string, string> GetRaceResults(string season, string grandPrixName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Races.Where(r => r.Season.Year == season).Where(r => r.GrandPrix.Name == grandPrixName)
                .ForEach(r =>
                {
                    result[$"Driver: {r.Driver}/ Constructor: {r.Constructor}"] = r.Score.ToString();
                });

            result.OrderBy(x => x.Value);
            return result;
        }

        public static string GetCurrentSeason()
        {
            string currentSeason = db.Seasons.OrderByDescending(s => s.Year).FirstOrDefault().Year.ToString();
            return currentSeason;
        }


        // and many others
    }
}
