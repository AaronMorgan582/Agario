/// <summary>
/// 
/// Author:    Aaron Morgan and Xavier Davis
/// Partner:   None
/// Date:      4/14/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500, Aaron Morgan and Xavier Davis
/// 
/// We, Aaron Morgan and Xavier Davis, certify that we wrote this code from scratch and did not copy it in part
/// or in whole from another source.
/// 
/// </summary>

using System;
using System.Windows.Forms;
using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ViewController
{
    static class RunGUI
    {
        /// <summary>
        ///  The main entry point for the Agario application.
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