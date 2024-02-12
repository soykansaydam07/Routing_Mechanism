using Routing_Mechanism.Services.Interfaces;
using System;

namespace Routing_Mechanism.Services
{
    public class ConsoleLog : ILog
    {

        public ConsoleLog(int a)
        {
            
        }

        public void Log()
        {
            Console.WriteLine("Console Tarafına Loglama yapıldı");
        }
    }
}
