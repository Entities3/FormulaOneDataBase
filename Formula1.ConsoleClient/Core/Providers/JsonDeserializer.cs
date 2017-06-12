namespace Formula1.ConsoleClient.Core.Providers
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Contracts;
    using JsonModels;

    public class JsonDeserializer : IDesrializer
    {
        public IList<ConstructorJson> DeserializeConstructor(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<ConstructorJson> constructors = serializer.Deserialize<List<ConstructorJson>>(json);

            return constructors;
        }

        public IList<DriverJson> DeserializeDriver(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<DriverJson> drivers = serializer.Deserialize<List<DriverJson>>(json);

            return drivers;
        }

        public IList<DriverResultJson> DeserializeDriverResult(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<DriverResultJson> driversResult = serializer.Deserialize<List<DriverResultJson>>(json);

            return driversResult;
        }

        public IList<GrandPrixJson> DeserializeGrandPrix(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<GrandPrixJson> grandPrixesResult = serializer.Deserialize<List<GrandPrixJson>>(json);

            return grandPrixesResult;
        }

        public IList<ParticipantJson> DeserializeParticipant(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<ParticipantJson> participantsResult = serializer.Deserialize<List<ParticipantJson>>(json);

            return participantsResult;
        }

        public IList<RaceJson> DeserializeRace(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<RaceJson> racesResult = serializer.Deserialize<List<RaceJson>>(json);

            return racesResult;
        }

        public IList<SeasonJson> DeserializeSeason(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<SeasonJson> seasonsResult = serializer.Deserialize<List<SeasonJson>>(json);

            return seasonsResult;
        }
    }
}
