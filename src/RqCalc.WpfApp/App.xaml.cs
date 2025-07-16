using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.Infrastructure;
using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models;
using RqCalc.Wpf.Windows;
using RqCalc.Wpf.Windows.Route;

namespace RqCalc.WpfApp;

public partial class App
{
    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is ClientException)
        {
            MessageBox.Show("ClientException: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            MessageBox.Show("UnhandledException: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        e.Handled = true;
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        var loadWindow = new LoadWindow();

        var startupCode = e.Args is ["Code", _, ..] ? e.Args[1] : null;

        var versionInfo = FileVersionInfo.GetVersionInfo(typeof(App).Assembly.Location);

        var version = new Version(versionInfo.FileVersion!);

        loadWindow.SetVersion(version);
        loadWindow.Show();

        Task.Run(async () =>
                 {
                     await this.Dispatcher.InvokeAsync(() => loadWindow.SetStatus("Загрузка калькулятора"));

                     var sp = await this.Dispatcher.InvokeAsync(() => BuildServiceProvider(new WpfApplicationSettings(version, startupCode)));

                     await this.Dispatcher.InvokeAsync(() => loadWindow.SetStatus("Создание контекста"));

                     var mainWindow = await this.Dispatcher.InvokeAsync(() => sp.GetRequiredService<MainWindow>());

                     await this.Dispatcher.InvokeAsync(() =>
                                                 {
                                                     mainWindow.Show();
                                                     loadWindow.Close();
                                                 });
                 });
    }

    private static IServiceProvider BuildServiceProvider(WpfApplicationSettings settings)
    {
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", false)
                            .Build();

        return new ServiceCollection()
               .AddRqCalc(configuration)

               .AddSingleton(settings)

               .AddSingleton<WpfApplicationWebRouteSettings>(_ =>
                                                             {
                                                                 var routeSettings = new WpfApplicationWebRouteSettings();

                                                                 configuration.Bind("RouteSettings", routeSettings);

                                                                 return routeSettings;
                                                             })
               .AddSingleton<MainWindow>()
               .AddSingleton<IModelFactory, ModelFactory>()

               .AddSingleton<ICodeRouter, WebRouter>()

               .ValidateDuplicateDeclaration()
               .BuildServiceProvider(
                   new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
    }
}
