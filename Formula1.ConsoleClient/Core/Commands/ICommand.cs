namespace Formula1.ConsoleClient.Core.Commands
{
    using System.Collections.Generic;

    public interface ICommand
    {
        void Execute(IList<string> parameters);
    }
}
