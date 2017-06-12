namespace Formula1.ConsoleClient.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Contracts;
    using Providers;
    using Ninject;
    using log4net;
    using Container;
    using Ninject.Parameters;

    public class CommandParser : ICommandParser
    {
        private readonly IKernel kernel = new StandardKernel(new Formula1NinjectModule());

        public ICommand ParseCommand(string fullCommand)
        {
            List<string> commandNameParts = fullCommand.Split(' ').ToList();
            if (commandNameParts.Count < 1)
            {
                throw new ArgumentException("Invalid command!");
            }

            string commandName = commandNameParts[0];
            TypeInfo commandTypeInfo = this.FindCommand(commandName);
            string sourceName = commandNameParts[2];

            //     object[] args = new object[] { this.kernel.Get<IDesrializer>(),
            //         this.kernel.Get<IReader>("JsonReader"),
            //         this.kernel.Get<IWriter>(),
            //         this.kernel.Get<ILogger>() };
            ICommand command = this.kernel.Get<ICommand>(commandName);
                //,new[] {            new ConstructorArgument("pdf", this.kernel.Get<IDesrializer>(sourceName))});

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
