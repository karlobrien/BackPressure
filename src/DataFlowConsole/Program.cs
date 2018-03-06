using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlowConsole
{
    public class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            Console.WriteLine("Kick off a blocking collection");

            var prod = Task.Run(() => BlockExample.Producer());
            var con = Task.Run(() => BlockExample.Consumer());

            _quitEvent.WaitOne();

            //Clean up the remaining items
        }
    }
}
