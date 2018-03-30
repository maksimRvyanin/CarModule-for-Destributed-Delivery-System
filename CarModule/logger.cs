using System;
using System.IO;

namespace CarModule
{
    class Logger
    {
        string logFileName; //file's name equals car's Id.
        public Logger(string fileName)
        {
            logFileName = fileName;
        }
        public void Log(string logString)
        {
            string toLogFile = String.Format("{0:d/M/yyyy HH:mm:ss}  {1}\r\n", DateTime.Now, logString);
            File.AppendAllText(@"C:\logs\" + logFileName + ".txt", toLogFile);
        }
    }
}
