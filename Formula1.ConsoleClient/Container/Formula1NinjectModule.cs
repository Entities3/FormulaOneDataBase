namespace Formula1.ConsoleClient.Container
{
    using Core;
    using Core.Commands;
    using Core.Contracts;
    using Core.Providers;
    using log4net;
    using Ninject;
    using Ninject.Modules;

    public class Formula1NinjectModule : NinjectModule
    {
        public override void Load()
        {           
            this.Bind<IReader>().To<ConsoleReader>().InSingletonScope().Named("ConsoleReader");
            this.Bind<IReader>().To<FileReader>().Named("FileReader");
            this.Bind<IWriter>().To<ConsoleWriter>().InSingletonScope();

            this.Bind<IEngine>().To<Engine>().InSingletonScope().WithConstructorArgument("reader", Kernel.Get<IReader>("ConsoleReader"));

            this.Bind<ILog>().ToMethod(x => LogManager.GetLogger(x.Request.Target.Member.DeclaringType));
            this.Bind<ILogger>().To<FileLogger>().InSingletonScope();

            this.Bind<ICommand>().To<Import>().Named("import").WithConstructorArgument("reader", Kernel.Get<IReader>("FileReader"));
            this.Bind<ICommand>().To<Export>().Named("export");
			      this.Bind<ICommand>().To<Delete>().Named("delete");

            this.Bind<ISerializer>().To<PdfSerializer>().InSingletonScope();
            this.Bind<IDesrializer>().To<JsonDeserializer>().InSingletonScope();

            this.Bind<ICommandParser>().To<CommandParser>();
        }
    }
}
