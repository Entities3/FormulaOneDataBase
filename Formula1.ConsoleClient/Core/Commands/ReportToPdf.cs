namespace Formula1.ConsoleClient.Core.Commands
{
    using System.Collections.Generic;
    using Data;

    public class ReportToPdf : ICommand
    {
        private Formula1Context db = new Formula1Context();

        // example input command:"report to pdf get all drivers"
        public void Execute(IList<string> parameters)
        {

            //to do
        }

        private void GetAllDrivers()
        {
            // return type
            //to do
        }

        private void GetAllConstructors()
        {
            // return type
            //to do
        }

        private void GetConstructorStangingsForSeason(string season)
        {
            // return type
            //how to get the season parameter? get from parameters or add ReadLine() to ask for it?
            //to do
        }

        private void GetCurrentConstructorsStandings()
        {
            // return type
            //to do
        }

        private void GetDriversStandingsForSeason(string season)
        {
            // return type
            //to do
        }

        private void GetCurrentDriversStandings()
        {
            // return type
            //to do
        }

        private void GetDriverActiveSeasons(string driverName)
        {
            // return type
            //to do
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

        // and many others
    }
}
