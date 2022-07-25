using System;
using System.Collections.Generic;
using System.Text;

namespace TrilogyAvivaTest.Services.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void LogLine(string message);
    }
}
