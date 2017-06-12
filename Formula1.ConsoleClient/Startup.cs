namespace Formula1.ConsoleClient
{
    using Container;
    using Core;
    using Core.Commands;
    using Core.Providers;
    using Data;
    using JsonModels;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Script.Serialization;

    public class Startup
    {
        

        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new Formula1NinjectModule());

            IEngine engine = kernel.Get<IEngine>();
            engine.Start();

            //var result = ReportToPdf.GetCurrentSeason();

            //Console.WriteLine(result);
            //    foreach (var key in result.Keys)
            //    {
            //        Console.WriteLine($"{key} ---- {result[key]}");
            //    }

            //var season = "2015";
            //var db = new Formula1Context();
            //var result = new Dictionary<string, string>();
            //HashSet<string> constructors = new HashSet<string>(db.SeasonsParticipants.Where(s => s.Season.Year == season).Select(s => s.Constructor.Name).ToList().Distinct());
            //foreach (string constructor in constructors)
            //{
            //    result[constructor] = db.Races.Where(r => r.Constructor.Name == constructor).Select(r=>r.Score).ToList().Sum().ToString();
            //}

            //foreach (string constructor in constructors)
            //{
            //    Console.WriteLine(constructor + " " + result[constructor]);
            //}



            //
            //    string filePath = @"C:\Users\Acer\Downloads\1.json";
            //    string json = JsonReader.Instance.Read(filePath);
            //
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //    IList<SeasonJson> constructorsJson = serializer.Deserialize<List<SeasonJson>>//(json);
            //    Console.WriteLine(constructorsJson.Count);
            //    Console.WriteLine(constructorsJson[0].Year);
            //    Console.WriteLine(constructorsJson[0].Review);
            //    Console.WriteLine(constructorsJson[0].Participants[3].Constructor);
        }
    }
}

