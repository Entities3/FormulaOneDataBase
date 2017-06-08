namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Contracts;

    public class CommandParser : ICommandParser
    {
        private static ICommandParser instance;

        private CommandParser()
        {
        }

        public static ICommandParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommandParser();
                }
                return instance;
            }
        }

        public ICommand ParseCommand(string fullCommand)
        {
            List<string> commandNameParts = fullCommand.Split(' ').ToList();
            if (commandNameParts.Count < 3)
            {
                throw new ArgumentException("Invalid command!");
            }
            commandNameParts.GetRange(0, 3);
            
            string commandName = string.Join("", commandNameParts);
            TypeInfo commandTypeInfo = this.FindCommand(commandName);
            ICommand command = Activator.CreateInstance(commandTypeInfo) as ICommand;

            return command;
        }

        public IList<string> ParseParameters(string fullCommand)
        {
            List<string> commandParts = fullCommand.Split(' ').ToList();
            if (commandParts.Count < 3)
            {
                throw new ArgumentException("Invalid command!");
            }
            commandParts.RemoveRange(0, 3);

            if (commandParts.Count() == 0)
            {
                return null;
            }

            return commandParts;
        }

        private TypeInfo FindCommand(string commandName)
        {
            Assembly currentAssembly = this.GetType().GetTypeInfo().Assembly;
            TypeInfo commandTypeInfo = currentAssembly.DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(ICommand)))
                .Where(type => type.Name.ToLower().Contains(commandName.ToLower()))
                .SingleOrDefault();

            if (commandTypeInfo == null)
            {
                throw new ArgumentException("The passed command is not found!");
            }

            return commandTypeInfo;
        }
    }
}
