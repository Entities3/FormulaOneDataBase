namespace Formula1.ConsoleClient.Core.Contracts
{
    using System.Collections.Generic;
    using JsonModels;

    public interface IDesrializer
    {
        IList<ConstructorJson> DeserializeConstructor(string data);

        IList<DriverJson> DeserializeDriver(string data);

        IList<RaceJson> DeserializeRace(string data);

        IList<SeasonJson> DeserializeSeason(string data);

        IList<DriverResultJson> DeserializeDriverResult(string data);

        IList<GrandPrixJson> DeserializeGrandPrix(string data);

        IList<ParticipantJson> DeserializeParticipant(string data);
    }
}
