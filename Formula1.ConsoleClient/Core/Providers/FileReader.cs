namespace Formula1.ConsoleClient.Core.Providers
{
    using System.IO;
    using Contracts;

    public class FileReader : IReader
    {       
        public string Read(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string data = r.ReadToEnd();
                return data;
            }
        }        
    }
}
