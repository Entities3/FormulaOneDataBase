namespace Formula1.ConsoleClient.Core.Providers
{
    using System.IO;
    using Contracts;

    public class JsonReader : IReader
    {
        private static readonly JsonReader instance = new JsonReader();

        private JsonReader()
        {
        }

        public static JsonReader Instance
        {
            get
            {
                return instance;
            }
        }

        public string Read(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}
