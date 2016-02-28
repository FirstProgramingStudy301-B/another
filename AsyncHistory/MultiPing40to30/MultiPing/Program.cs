using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPing
{
    class Program
    {
        public static void Main(string[] args)
        {
            //var beginDNS = new BeginDNS();
            //beginDNS.Run();

            var tskDNS = new TaskPing();
            tskDNS.Run();
        }
    }
}
