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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FileLogger
{
    /// <summary>
    /// This class generates a date and time stamp for a given thread.
    /// </summary>
    class ShowTimeAndThreadClass
    {

        /// <summary>
        /// Method that generates a date and time stamp for a given thread.
        /// </summary>
        /// <returns></returns>
        public static string ShowTimeAndThread()
        {
            string date_time_threadID = DateTime.Now.ToString(@"yyyy-MM-dd h:mm:ss tt") + " (" + Thread.CurrentThread.ManagedThreadId + ") ";
            return date_time_threadID;
        }
    }
}
