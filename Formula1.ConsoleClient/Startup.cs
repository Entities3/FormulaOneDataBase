namespace Formula1.ConsoleClient
{
    using Core;
    using Core.Providers;
    using JsonModels;
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class Startup
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine(null, null, null, null);
            engine.Start();
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

