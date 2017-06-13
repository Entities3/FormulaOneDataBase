using Formula1.ConsoleClient.Core.Contracts;
using Formula1.Data;
using Formula1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Formula1.ConsoleClient.Core.Commands
{
    public class Delete : ICommand
    {
        private Formula1Context db;
        private IWriter writer;
        private ILogger logger;

        public Delete(IWriter writer, ILogger logger) // not working
        {
            this.db = new Formula1Context();
            this.writer = writer;
            this.logger = logger;
        }
        
        //delete from data driver (name)
        public void Execute(IList<string> parameters)
        {
            string modelToDelete = parameters[0];
            parameters.RemoveAt(0);

            switch (modelToDelete)
            {
                case "driver": this.DeleteDriver(parameters); break;
                case "constructor": this.DeleteConstructor(parameters); break;
                case "season": this.DeleteSeason(parameters); break;
                case "race": this.DeleteRace(parameters); break;
                case "grandprix": this.DeleteGrandPrix(parameters); break;
                default:
                    break;
            }
        }

        private void DeleteRace(IList<string> parameters)
        {
            int seasonIndex = parameters.IndexOf("season");
            int granPrixIndex = parameters.IndexOf("grandprix");
            int driverIndex = parameters.IndexOf("driver");

            string seasonValue = parameters[seasonIndex + 1];
            List<string> grandPrixParameters = parameters.ToList().GetRange(granPrixIndex + 1, driverIndex - granPrixIndex);
            string grandPrixValue = string.Join(" ", grandPrixParameters);

            parameters.ToList().RemoveRange(0,granPrixIndex);
            string driverValue = string.Join(" ", parameters);

            Race race = this.db.Races.Where(r => r.Season.Year == seasonValue).Where(r => r.GrandPrix.Name == grandPrixValue).Where(r => r.Driver.Name == driverValue).FirstOrDefault();

            this.db.Entry(race).State = EntityState.Deleted;
            db.SaveChanges();
            this.writer.WriteLine($"Race: Season {seasonValue}, Grand Prix {grandPrixValue}, Driver {driverValue} was deleted!");
            this.logger.Info($"Race: Season {seasonValue}, Grand Prix {grandPrixValue}, Driver {driverValue} was deleted!");
        }

        private void DeleteGrandPrix(IList<string> parameters)
        {
            throw new NotImplementedException();
        }

        private void DeleteSeason(IList<string> parameters)
        {
            throw new NotImplementedException();
        }

        private void DeleteConstructor(IList<string> parameters)
        {
            throw new NotImplementedException();
        }

        private void DeleteDriver(IList<string> parameters)
        {
            string driverName = string.Join(" ", parameters);

            Driver driver = db.Drivers.FirstOrDefault(d => d.Name == driverName);

            Console.WriteLine(driver.Name);
           this.db.Drivers.Remove(driver);
          //  this.db.Entry(driver).State = EntityState.Deleted;
           db.SaveChanges();
            this.writer.WriteLine($"Driver {driverName} was deleted!");
            this.logger.Info($"Driver {driverName} was deleted!");
        }
    }
}
