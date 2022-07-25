using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TrilogyAvivaTest.Services.Logging
{
    public class DebugLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Write(message);
        }

        public void LogLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
