using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleFileSync.Console
{
    public class ConsoleWriterLog : ILog
    {
        public void Error(Exception ex)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(ex);
            System.Console.ResetColor();
        }

        public void Error(string message, params string[] args)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message, args);
            System.Console.ResetColor();
        }

        public void Info(string message, params string[] args)
        {
            System.Console.WriteLine(message, args);
        }
    }

    
}
