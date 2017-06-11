using System.Collections.Generic;

namespace Formula1.ConsoleClient.JsonModels
{
    public class RaceJson // to fix
    {
        public string Season { get; set; }

        public string GrandPrix { get; set; }

        public ICollection<DriverResultJson> Results { get; set; }
    }
}
