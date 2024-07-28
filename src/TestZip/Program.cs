using CommandLine;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestZip.Commands;

namespace TestZip;

static class Program
{
    private static IHost? _host;

    static async Task Main(string[] args)
    {
        _host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
            {
                // Register your dependencies here
                builder.RegisterType<ExecutionSummary>().As<ExecutionSummary>().SingleInstance();
                builder.RegisterType<AppRunner>().As<AppRunner>();
                builder.RegisterType<TestArchiveCommandHandler>().AsSelf();
                builder.RegisterType<ZipProcess>().AsSelf();
            })
            // .ConfigureServices((hostContext, services) =>
            // {
            //     // Configure any additional services
            //     //services.AddHostedService<Worker>();
            // })
            .Build();

        var app = _host.Services.GetService<AppRunner>();

        (await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(app.RunOptions))
            .WithNotParsed(HandleParseError);
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        Console.WriteLine("Command line arguments are invalid. Use --help for usage information.");
    }
}