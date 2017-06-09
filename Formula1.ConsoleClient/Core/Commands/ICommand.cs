namespace Formula1.ConsoleClient.Core.Commands
{
    using System.Collections.Generic;
    using System.Data.Entity;

    public interface ICommand
    {
        void Execute(IList<string> parameters);
    }
}
