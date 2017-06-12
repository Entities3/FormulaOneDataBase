namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using Contracts;

    public class ConsoleReader : IReader
    {       
        public string Read(string path)
        {
            string lineInput = Console.ReadLine();
            return lineInput;
        }
    }
}
