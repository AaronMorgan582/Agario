using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ViewController
{
    static class RunGUI
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (CustomFileLogProvider provider = new CustomFileLogProvider())
            {
                ServiceCollection services = new ServiceCollection();

                services.AddLogging(configure =>
                {
                    configure.AddProvider(provider);
                    configure.AddConsole();
                    configure.SetMinimumLevel(LogLevel.Information);
                });

                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    ILogger<Client_and_GUI> logger = serviceProvider.GetRequiredService<ILogger<Client_and_GUI>>();

                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Client_and_GUI(logger));
                }
            }

        }
    }
}