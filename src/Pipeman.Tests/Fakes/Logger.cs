using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pipeman.Tests.Fakes
{
    public class Logger
    {
        private readonly Action<string> _log;
        private readonly List<string> _logs;
        public IEnumerable<string> Logs => _logs;

        public Logger(Action<string> log=null)
        {
            _log = log ?? (p=>Debug.WriteLine(p));
            _logs= new List<string>();
        }

        public void Log(string msg)
        {
            _log(msg);
            _logs.Add(msg);
        }
    }
}