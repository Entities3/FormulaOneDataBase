namespace Formula1.ConsoleClient.Core.Contracts
{
    public interface IWriter
    {
        void Write(string message);

        void WriteLine(string message);
    }
}
