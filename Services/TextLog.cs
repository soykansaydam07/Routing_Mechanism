using Routing_Mechanism.Services.Interfaces;
using System;

namespace Routing_Mechanism.Services
{
    public class TextLog : ILog
    {
        public TextLog()
        {
            
        }

        public void Log()
        {
            Console.WriteLine("Text Loglaması yapılmıştır");
        }
    }
}
