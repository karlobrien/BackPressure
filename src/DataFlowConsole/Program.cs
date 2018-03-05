using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
        }
    }

    public class BlockExample
    {
        private static BlockingCollection<int> data = new BlockingCollection<int>(boundedCapacity:5);
        public static void Producer()
        {
            for (int ctr = 0; ctr < 10; ctr++)
            {
               data.Add(ctr);
               Console.WriteLine($"Added ctr: {ctr}");
            }
        }

        public static void Consumer()
        {
            foreach (var item in data.GetConsumingEnumerable())
            {
                Console.WriteLine($"Removed {item}");
                Console.ReadLine();
            }
        }
    }

    public class BothWays
    {
        public static void Produce(BufferBlock<int> queue, IEnumerable<int> values)
        {
            foreach (var value in values)
            {
                queue.Post(value);
            }

            queue.Complete();
        }

        public static async Task<IEnumerable<int>> Consume(BufferBlock<int> queue)
        {
            var ret = new List<int>();
            while (await queue.OutputAvailableAsync())
            {
                ret.Add(await queue.ReceiveAsync());
            }

            return ret;
        }

    }
}
