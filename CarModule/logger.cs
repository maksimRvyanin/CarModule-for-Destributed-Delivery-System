using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarModule
{
    class Logger
    {
        string logFileName; //file's name equals car's Id.
        public Logger(string fileName)
        {
            logFileName = fileName;
        }
        public void WriteLogInFile(string logString)
        {
            string toLogFile = String.Format("{0:d/M/yyyy HH:mm:ss}  {1}\r\n", DateTime.Now, logString);
            File.WriteAllText(@"C:\logs\" + logFileName + ".txt", toLogFile);
        }
    }
}
