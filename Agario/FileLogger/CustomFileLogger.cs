///<summary>
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      3/29/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source.
/// 
///</summary>

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileLogger
{

    /// <summary>
    /// This class provides a way for custom messages to be written to a Log .txt file.
    /// </summary>
    public class CustomFileLogger : ILogger
    {
        private StreamWriter file;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="categoryName">This parameter should be provided by the Services Builder.</param>
        public CustomFileLogger(string categoryName)
        {
            file = new StreamWriter($"Log_{categoryName}.txt", true);
        }

        //From what I was able to gather, the BeginScope is used to help determine where the log information
        //came from. When using it, it kind of gives a history of where the log information originated, if
        //it's nested in multiple processes.
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// This method writes the Log event to the given .txt file, in the following format:
        /// 
        /// Date Time (Thread ID) - LogLevel - message
        /// 
        /// All parameters should be provided by the Services Builder.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string shortLogLevel = logLevel.ToString().Substring(0, 5);
            string closeMessage = ". This message was produced by a Log" + logLevel + " call";
            try
            {
                file.WriteLine(ShowTimeAndThreadClass.ShowTimeAndThread() + "- " + shortLogLevel + " - " + formatter(state, exception) + closeMessage);
            }
            catch
            {
            }
        }

        /// <summary>
        /// The Dispose method to close the file after the writing has finished.
        /// </summary>
        public void Dispose()
        {
            file.Flush();
            file.Close();
        }
    }
}
