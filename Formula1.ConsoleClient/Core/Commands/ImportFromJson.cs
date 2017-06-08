namespace Formula1.ConsoleClient.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Data;
    using JsonModels;
    using Models;
    using Providers;

    public class ImportFromJson : ICommand
    {
        private Formula1Context db = Engine.DataBase;

        // example input command: "import from json (pathFile) drivers"
        public void Execute(IList<string> parameters)
        {
            var filePath = parameters[0];
            var json = JsonReader.Instance.Read(filePath);
            parameters.RemoveAt(0);
            var modelName = string.Join("", parameters);
            switch (modelName.ToLower())
            {
                case "drivers": this.ImportDrivers(json); break;
                case "constructors": this.ImportConstructors(json); break;
                case "circuits": this.ImportCircuits(json); break;
                case "races": this.ImportRaces(json); break;
                case "seasons": this.ImportSeasons(json); break;
                default: throw new ArgumentException("Invalid data to import");
            }
        }

        private void ImportSeasons(string json)
        {
            //to do
        }

        private void ImportRaces(string json)
        {
            //to do
        }

        private void ImportCircuits(string json)
        {
            //to do
        }

        private void ImportConstructors(string json)
        {
            var serializer = new JavaScriptSerializer();
            var constructorsJson = serializer.Deserialize<List<ConstructorsJson>>(json); //try deserialize??

            var savedNationalities = db.Nationaties.Select(n => n.Name).ToList();

            foreach (var constructorJson in constructorsJson)
            {
                // somewhere to check for null property in constructorJson??
                var nationality = this.GetNationality(db, constructorJson.Nationality, savedNationalities);
                var constructor = new Constructor() { Name = constructorJson.ConstructorName, Nationality = nationality, InformationUrl = constructorJson.About };
                db.Constructors.AddOrUpdate(d => d.Name, constructor);
                db.SaveChanges();
            }
        }

        private void ImportDrivers(string json)
        {
            var serializer = new JavaScriptSerializer();
            var driversJson = serializer.Deserialize<List<DriverJson>>(json);

            var savedNationalities = db.Nationaties.Select(n => n.Name).ToList();

            foreach (var driverJson in driversJson)
            {
                var nationality = this.GetNationality(db, driverJson.Nationality, savedNationalities);
                var driver = new Driver() { Name = driverJson.DriverName, Nationality = nationality,InformationUrl=driverJson.About };
                db.Drivers.AddOrUpdate(d => d.Name, driver);
                db.SaveChanges();
            }
        }

        private Nationality GetNationality(Formula1Context db, string nationalityName, ICollection<string> addedNationalities)
        {
            if (addedNationalities.Contains(nationalityName))
            {
                return db.Nationaties.FirstOrDefault(n => n.Name == nationalityName);
            }
            else
            {
                addedNationalities.Add(nationalityName);
                var nationality = new Nationality()
                {
                    Name = nationalityName
                };

                db.Nationaties.AddOrUpdate(n => n.Name, nationality);
                db.SaveChanges();
                return nationality;
            }
        }

    }
}
