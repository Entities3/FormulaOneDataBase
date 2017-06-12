namespace Formula1.ConsoleClient.Core.Contracts
{
    using System.Collections.Generic;

    public interface IExporter
    {
        void Export(string path, IDictionary<string, string> list, IEnumerable<string> headers);
    }
}
