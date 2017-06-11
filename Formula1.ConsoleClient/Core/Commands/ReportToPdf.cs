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
        private Formula1Context db = new Formula1Context();

        // example input command:"report to pdf get all drivers"
        public void Execute(IList<string> parameters)
        {

            //to do
        }

        private IDictionary<string, string> GetAllDrivers()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Drivers.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });
            return result;
        }

        private IDictionary<string, string> GetAllConstructors()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            db.Constructors.ForEach(d =>
            {
                result[d.Name] = d.InformationUrl;
            });
            return result;
        }

        private IDictionary<string, string> GetConstructorStangingsForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> constructors = new HashSet<string>(db.SeasonsParticipants.Where(s => s.Season.Year == season).Select(s => s.Constructor.Name).ToList().Distinct());
            foreach (string constructor in constructors)
            {
                result[constructor] = db.Races.Where(r => r.Constructor.Name == constructor).Select(r => r.Score).ToList().Sum().ToString();
            }
            result.OrderBy(x => x.Value);
            return result;
        }

        private IDictionary<string, string> GetCurrentConstructorsStandings()
        {
            string currentSeason = GetCurrentSeason();
            IDictionary<string, string> result = GetConstructorStangingsForSeason(currentSeason);
            return result;
        }

        private IDictionary<string, string> GetDriversStandingsForSeason(string season)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> drivers = new HashSet<string>(db.SeasonsParticipants.Where(s => s.Season.Year == season).Select(s => s.Driver.Name).ToList().Distinct());
            foreach (string driver in drivers)
            {
                result[driver] = GetDriverScoreForSeason(season);
            }
            result.OrderBy(x => x.Value);
            return result;
        }

        private string GetDriverScoreForSeason(string season)
        {
            throw new NotImplementedException();
        }

        private IDictionary<string, string> GetCurrentDriversStandings()
        {
            string currentSeason = GetCurrentSeason();
            IDictionary<string, string> result = GetDriversStandingsForSeason(currentSeason);
            return result;
        }

        private IDictionary<string, string> GetDriverActiveSeasons(string driverName)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            HashSet<string> seasons = new HashSet<string>(db.Drivers.Where(d => d.Name == driverName).FirstOrDefault().Seasons.Select(s => s.Year).ToList().Distinct());

           foreach
            return result;
        }

        private void GetConstructorActiveSeasons(string constructorName)
        {
            // return type
            //to do
        }

        private void GetRaceResults(string season, string circuit)
        {
            // return type
            //to do
        }

        private string GetCurrentSeason()
        {
            string currentSeason = db.Seasons.OrderByDescending(s => s.Year).FirstOrDefault().Year.ToString();
            return currentSeason;
        }


        // and many others
    }
}
