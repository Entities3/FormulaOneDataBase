namespace Formula1.ConsoleClient.Core
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Contracts;

    public class Engine :IEngine
    {
        private const string TerminationCommand = "end";
        private const string NullProvidersExceptionMessage = "cannot be null.";

        private ICommandParser parser;
        private IReader reader;
        private IWriter writer;
        private ILogger logger;

        public Engine(IReader reader, IWriter writer, ILogger logger, ICommandParser parser)
        {
            this.reader = reader;
            this.writer = writer;
            this.logger = logger;
            this.parser = parser;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    string commandAsString = this.reader.Read(string.Empty);

                    if (commandAsString == TerminationCommand)
                    {
                        break;
                    }

                    this.ProcessCommand(commandAsString);
                }
                catch (Exception ex)
                {
                    // not logging to file??
                    this.logger.Error(ex.Message);
                    this.writer.WriteLine(ex.Message);
                }
            }
        }

        private void ProcessCommand(string commandAsString)
        {
            if (string.IsNullOrWhiteSpace(commandAsString))
            {
                throw new ArgumentNullException("Command cannot be null or empty.");
            }

            ICommand command = this.parser.ParseCommand(commandAsString);
            IList<string> parameters = this.parser.ParseParameters(commandAsString);

            command.Execute(parameters);
        }
    }
}
