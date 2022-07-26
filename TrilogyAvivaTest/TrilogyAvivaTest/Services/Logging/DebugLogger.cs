using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace TrilogyAvivaTest.Services.Logging
{
    public class DebugLogger : ILogger
    {
        private readonly int _maxHistory;

        public IEnumerable<string> History { get; }

        public DebugLogger(int maxHistory)
        {
            _maxHistory = maxHistory;
            History = new ObservableCollection<string>();
        }

        public void Log(string message)
        {
            Debug.Write(message);
            var obsColl = (ObservableCollection<string>)History;
            obsColl.Add(message);

            // MAGIC: number.
            if(obsColl.Count > _maxHistory)
                obsColl.RemoveAt(0);
        }

        public void LogLine(string message)
        {
            Log(message + Environment.NewLine);
        }
    }
}
