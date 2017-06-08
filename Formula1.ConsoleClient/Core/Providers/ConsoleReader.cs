namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using Contracts;

    public class ConsoleReader : IReader
    {
        private static ConsoleReader instance;

        private ConsoleReader()
        {
        }

        public static ConsoleReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleReader();
                }

                return instance;
            }
        }

        public string Read(string path)
        {
            string lineInput = Console.ReadLine();
            return lineInput;
        }
    }
}
