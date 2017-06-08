namespace Formula1.ConsoleClient.Core.Contracts
{
    using System.Collections.Generic;
    using Commands;

    public interface ICommandParser
    {
        ICommand ParseCommand(string fullCommand);

        IList<string> ParseParameters(string fullCommand);
    }
}
