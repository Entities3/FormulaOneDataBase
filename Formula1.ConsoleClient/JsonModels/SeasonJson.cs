namespace Formula1.ConsoleClient.JsonModels
{
    using System.Collections.Generic;

    public class SeasonJson
    {
        public string Season { get; set; }

        public List<ParticipantJson> Participants { get; set; }

        public string Review { get; set; }
    }
}
