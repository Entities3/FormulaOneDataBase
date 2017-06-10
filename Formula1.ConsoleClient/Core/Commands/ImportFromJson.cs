namespace Formula1.ConsoleClient.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Data;
    using JsonModels;
    using Models;
    using Providers;

    public class ImportFromJson : ICommand // to fix
    {
        // inject from factory?
        private Formula1Context db = new Formula1Context();
        // private Importer importer=new Importer();

        // example input command: "import from json (pathFile) drivers"
        public void Execute(IList<string> parameters)
        {
            string filePath = parameters[0];
            string json = JsonReader.Instance.Read(filePath);
            parameters.RemoveAt(0);
            string modelName = string.Join("", parameters);
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
            // in other method - "json deserializer"
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<SeasonJson> seasonsJson = serializer.Deserialize<List<SeasonJson>>(json);

            IList<string> savedSeasons = db.Seasons.Select(s => s.Year).ToList();

            int rowToAffectCounter = 0;
            int rowAffectedCounter = 0;

            foreach (SeasonJson seasonJson in seasonsJson)
            {
                if (seasonJson.Season == null)
                {
                    ConsoleWriter.Instance.WriteLine($"Row {rowToAffectCounter} not affected!");
                    FileLogger.Instance.Info($"Row {rowToAffectCounter} not affected!");
                    throw new ArgumentException("Import data row is invalid!");
                }
                Season season = new Season();
                if (!savedSeasons.Contains(seasonJson.Season))
                {
                    season.Year = seasonJson.Season;
                    season.ReviewUrl = seasonJson.Review;
                    db.Seasons.Add(season);
                    savedSeasons.Add(seasonJson.Season);
                    db.SaveChanges();
                    ConsoleWriter.Instance.WriteLine($"Created new season {seasonJson.Season}!");
                    FileLogger.Instance.Info($"Created new season {seasonJson.Season}!");
                }
                else
                {
                    Console.WriteLine("Hi");
                    season = GetSeason(seasonJson.Season);
                }
                foreach (ParticipantJson participantJson in seasonJson.Participants)
                {
                    rowToAffectCounter++;
                    Constructor constructor = GetConstructor(participantJson.Constructor);
                    if (constructor == null)
                    {
                        ConsoleWriter.Instance.WriteLine($"Season Participants: 0 rows affected!");
                        FileLogger.Instance.Info($"Season Participants: 0 rows affected!");
                        throw new ArgumentException($"Constructor {participantJson.Constructor} doesn't exist in data base! Please create a new constructor and import again!");
                    }
                    Driver driver = GetDriver(participantJson.Driver);
                    if (driver == null)
                    {
                        ConsoleWriter.Instance.WriteLine($"Season Participants: 0 rows affected!");
                        FileLogger.Instance.Info($"Season Participants: 0 rows affected!");
                        throw new ArgumentException($"Driver {participantJson.Driver} doesn't exist in data base! Please create a new cdriver and import again!");
                    }
                    rowAffectedCounter++;
                    SeasonParticipants participant = new SeasonParticipants { Season = season, DriverPermanentNumber = int.Parse(participantJson.DriverNumber), Constructor = constructor, Driver = driver };
                    db.SeasonsParticipants.Add(participant);
                }
            }
            // if there are equal rows???
            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Season Participants: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
            FileLogger.Instance.Info($"Season Participants: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");

        }

        private void ImportRaces(string json)
        {
            // in other method - "json deserializer"
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<RaceJson> racesJson = serializer.Deserialize<List<RaceJson>>(json);
            IList<string> savedGrandPrixes = db.GrandPrixes.Select(g => g.Name).ToList();

            int rowToAffectCounter = 0;
            int rowAffectedCounter = 0;

            foreach (RaceJson raceJson in racesJson)
            {
                rowToAffectCounter++;
                Season season = GetSeason(raceJson.Season);
                if (season == null)
                {
                    ConsoleWriter.Instance.WriteLine($"Races: 0 rows affected!");
                    FileLogger.Instance.Info($"Races: 0 rows affected!");
                    throw new ArgumentException($"Season {raceJson.Season} doesn't exist in data base! Please create a new season and import again!");
                }
                //to fix circuit in racejson class
                GrandPrix grandPrix = GetGrandPrix(raceJson.Circuit);
                if (grandPrix == null)
                {
                    grandPrix = ImportGrandPrix(raceJson.Circuit);
                    savedGrandPrixes.Add(raceJson.Circuit);
                }

                foreach (DriverResultJson result in raceJson.Results)
                {
                    Constructor constructor = GetConstructor(result.Constructor);
                    if (constructor == null)
                    {
                        ConsoleWriter.Instance.WriteLine($"Races: 0 rows affected!");
                        FileLogger.Instance.Info($"Races: 0 rows affected!");
                        throw new ArgumentException($"Constructor {result.Constructor} doesn't exist in data base! Please create a new constructor and import again!");
                    }

                    Driver driver = GetDriver(result.Driver);
                    if (driver == null)
                    {
                        ConsoleWriter.Instance.WriteLine($"Race: 0 rows affected!");
                        FileLogger.Instance.Info($"Race: 0 rows affected!");
                        throw new ArgumentException($"Driver {result.Driver} doesn't exist in data base! Please create a new driver and import again!");
                    }

                    string position = result.Position;
                    if (position == null)
                    {
                        ConsoleWriter.Instance.WriteLine($"Race: 0 rows affected!");
                        FileLogger.Instance.Info($"Race: 0 rows affected!");
                        throw new ArgumentException($"Race position could not be null!");
                    }

                    int score;
                    bool isNull = int.TryParse(result.Points, out score);
                    if (!isNull)
                    {
                        ConsoleWriter.Instance.WriteLine($"Race: 0 rows affected!");
                        FileLogger.Instance.Info($"Race: 0 rows affected!");
                        throw new ArgumentException($"Score could not be null!");
                    }

                    rowAffectedCounter++;
                    Race race = new Race { Season = season, GrandPrix=grandPrix, Driver = driver, Constructor = constructor, Position = position, Score = score };
                    db.Races.Add(race);
                }
            }

            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Races: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
            FileLogger.Instance.Info($"Races: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
        }

        private GrandPrix ImportGrandPrix(string grandPrixName)
        {
            GrandPrix grandPrix = new GrandPrix() { Name = grandPrixName };
            db.GrandPrixes.Add(grandPrix);
            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Created new Grand Prix {grandPrixName}!");
            FileLogger.Instance.Info($"Created new Grand Prix {grandPrixName}!");
            return grandPrix;
        }

        private GrandPrix GetGrandPrix(string grandPrixName)
        {
            GrandPrix grandPrix = db.GrandPrixes.FirstOrDefault(g => g.Name == grandPrixName);
            return grandPrix;
        }

        private Circuit GetCircuit(string circuitName)
        {
            Circuit circuit = db.Circuits.FirstOrDefault(c => c.Name == circuitName);
            return circuit;
        }

        private void ImportCircuits(string json)
        {
            // in other method - "json deserializer"
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<CircuitJson> circuitsJson = serializer.Deserialize<List<CircuitJson>>(json);
            IList<string> savedCircuits = db.Circuits.Select(c => c.Name).ToList();
            IList<string> savedCountries = db.Countries.Select(c => c.Name).ToList();

            int rowToAffectCounter = 0;
            int rowAffectedCounter = 0;

            foreach (CircuitJson circuitJson in circuitsJson)
            {
                rowToAffectCounter++;
                if (!savedCircuits.Contains(circuitJson.CircuitName))
                {
                    Country country = GetCountry(circuitJson.Country);
                    if (country == null)
                    {
                        country = ImportCountry(circuitJson.Country);
                        savedCountries.Add(circuitJson.Country);
                    }
                    Circuit circuit = new Circuit() { Name = circuitJson.CircuitName, Country = country, Locality = circuitJson.Locality, InformationUrl = circuitJson.Information };
                    db.Circuits.Add(circuit);
                    savedCircuits.Add(circuitJson.CircuitName);
                    rowAffectedCounter++;
                }
            }

            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Circuits: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
            FileLogger.Instance.Info($"Circuits: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
        }

        private void ImportConstructors(string json)
        {
            // in other method - "json deserializer"
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<ConstructorJson> constructorsJson = serializer.Deserialize<List<ConstructorJson>>(json);

            IList<string> savedNationalities = db.Nationaties.Select(n => n.Name).ToList();
            IList<string> savedConstructors = db.Constructors.Select(n => n.Name).ToList();

            int rowToAffectCounter = 0;
            int rowAffectedCounter = 0;

            foreach (ConstructorJson constructorJson in constructorsJson)
            {
                rowToAffectCounter++;
                if (savedConstructors.Contains(constructorJson.ConstructorName))
                {
                    ConsoleWriter.Instance.WriteLine($"Import data row already exists! Row {rowToAffectCounter} not affected!");
                    FileLogger.Instance.Info($"Import data row already exists! Row {rowToAffectCounter} not affected!");
                }
                else if (constructorJson.ConstructorName == null || constructorJson.Nationality == null)
                {
                    ConsoleWriter.Instance.WriteLine($"Import data row is invalid! Row {rowToAffectCounter} not affected!");
                    FileLogger.Instance.Info($"Import data row is invalid! Row {rowToAffectCounter} not affected!");
                }
                else
                {
                    rowAffectedCounter++;
                    Nationality nationality = this.GetNationality(db, constructorJson.Nationality, savedNationalities);
                    //model factory???
                    Constructor constructor = new Constructor() { Name = constructorJson.ConstructorName, Nationality = nationality, InformationUrl = constructorJson.About };
                    db.Constructors.Add(constructor);
                    savedConstructors.Add(constructorJson.ConstructorName);
                }
            }

            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Constructors: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
            FileLogger.Instance.Info($"Constructors: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
        }

        private void ImportDrivers(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<DriverJson> driversJson = serializer.Deserialize<List<DriverJson>>(json);

            IList<string> savedDrivers = db.Drivers.Select(n => n.Name).ToList();
            IList<string> savedNationalities = db.Nationaties.Select(n => n.Name).ToList();

            int rowToAffectCounter = 0;
            int rowAffectedCounter = 0;

            foreach (DriverJson driverJson in driversJson)
            {
                rowToAffectCounter++;
                if (savedDrivers.Contains(driverJson.DriverName))
                {
                    ConsoleWriter.Instance.WriteLine($"Import data row already exists! Row {rowToAffectCounter} not affected!");
                    FileLogger.Instance.Info($"Import data row already exists! Row {rowToAffectCounter} not affected!");
                }
                else if (driverJson.DriverName == null || driverJson.Nationality == null)
                {
                    ConsoleWriter.Instance.WriteLine($"Import data row is invalid! Row {rowToAffectCounter} not affected!");
                    FileLogger.Instance.Info($"Import data row is invalid! Row {rowToAffectCounter} not affected!");
                }
                else
                {
                    rowAffectedCounter++;
                    Nationality nationality = this.GetNationality(db, driverJson.Nationality, savedNationalities);
                    Driver driver = new Driver() { Name = driverJson.DriverName, Nationality = nationality, InformationUrl = driverJson.About };
                    db.Drivers.Add(driver);
                    savedDrivers.Add(driverJson.DriverName);
                }
            }

            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Drivers: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
            FileLogger.Instance.Info($"Drivers: {rowAffectedCounter}/{rowToAffectCounter} rows affected!");
        }

        private Driver GetDriver(string driverName)
        {
            Driver driver = db.Drivers.FirstOrDefault(d => d.Name == driverName);
            return driver;
        }

        private Constructor GetConstructor(string constructorName)
        {
            Constructor constructor = db.Constructors.FirstOrDefault(c => c.Name == constructorName);
            return constructor;
        }


        private Season GetSeason(string year)
        {
            Season season = db.Seasons.FirstOrDefault(s => s.Year == year);
            return season;
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
                Nationality nationality = new Nationality()
                {
                    Name = nationalityName
                };
                db.Nationaties.Add(nationality);
                ConsoleWriter.Instance.WriteLine($"Nationalities: row affected!");
                FileLogger.Instance.Info($"Nationalities: row affected!");
                db.SaveChanges();
                return nationality;
            }
        }

        private Country ImportCountry(string countryName)
        {
            Country country = new Country() { Name = countryName };
            db.Countries.Add(country);
            db.SaveChanges();
            ConsoleWriter.Instance.WriteLine($"Created new country {countryName}!");
            FileLogger.Instance.Info($"Created new country {countryName}!");
            return country;
        }

        private Country GetCountry(string countryName)
        {
            Country country = db.Countries.FirstOrDefault(c => c.Name == countryName);
            return country;
        }
    }
}
