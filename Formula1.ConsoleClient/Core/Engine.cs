namespace Formula1.ConsoleClient.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Commands;
    using Contracts;
    using Data;
    using Data.Migrations;
    using Providers;
    using Bytes2you.Validation;

    public class Engine
    {
        private const string TerminationCommand = "end";
        private const string NullProvidersExceptionMessage = "cannot be null.";

        private Formula1Context db;
        private ICommandParser parser;
        private IReader reader;
        private IWriter writer;
        private ILogger logger;

        public Engine(IReader reader, IWriter writer, ILogger logger, ICommandParser parser)
        {
            this.Reader = reader ?? ConsoleReader.Instance;
            this.Writer = writer ?? ConsoleWriter.Instance;
            this.Logger = logger ?? FileLogger.Instance;
            this.Parser = parser ?? CommandParser.Instance;
            this.DataBase = new Formula1Context();
        }

        public Formula1Context DataBase
        {
            get
            {
                return db;
            }
            set
            {
                // here???
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<Formula1Context, Configuration>());
                db = value;
                db.Database.CreateIfNotExists();
            }
        }

        public IReader Reader
        {
            get
            {
                return this.reader;
            }

            set
            {
                Guard.WhenArgument(value, "Engine Reader provider").IsNull().Throw();
                this.reader = value;
            }
        }

        public IWriter Writer
        {
            get
            {
                return this.writer;
            }

            set
            {
                Guard.WhenArgument(value, "Engine Writer provider").IsNull().Throw();
                this.writer = value;
            }
        }

        public ILogger Logger
        {
            get
            {
                return this.logger;
            }

            set
            {
                Guard.WhenArgument(value, "Engine Logger provider").IsNull().Throw();
                this.logger = value;
            }
        }

        public ICommandParser Parser
        {
            get
            {
                return this.parser;
            }

            set
            {
                Guard.WhenArgument(value, "Engine Parser provider").IsNull().Throw();
                this.parser = value;
            }
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
