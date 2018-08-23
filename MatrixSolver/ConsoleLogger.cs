using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver
{
    public class ConsoleLogger
    {
        private readonly string _context;
        public ConsoleLogger(string context)
        {
            _context = context;
        }

        public void Log(string logMessage)
        {
            StringBuilder msg = ComposeMessage("LOG");
            msg.Append(logMessage);

            Console.WriteLine(msg.ToString());
        }

        public void Error(string errorMessage)
        {
            StringBuilder msg = ComposeMessage("ERR");
            msg.Append(errorMessage);

            Console.WriteLine(msg.ToString());
        }

        private StringBuilder ComposeMessage(string level)
        {
            StringBuilder msg = new StringBuilder(string.Format("[{0}] | ", level));
            msg.Append(string.Format("{0} | ", DateTime.Now.ToString()));
            msg.Append(string.Format("{0} | ", _context));

            return msg;
        }
    }
}
