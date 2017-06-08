namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using Contracts;

    public class ConsoleWriter : IWriter
    {
        private static ConsoleWriter instance;

        private ConsoleWriter()
        {
        }

        public static ConsoleWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleWriter();
                }

                return instance;
            }
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
