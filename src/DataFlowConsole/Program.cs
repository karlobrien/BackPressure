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
            Console.WriteLine("Type Ctrl-C to quit ....");
            CustomBlockQueue();

            _quitEvent.WaitOne();

            //Clean up the remaining items
        }

        public static void CustomBlockQueue()
        {
            var bq = new CustomBlockingQueue<int>(10);

            Task producer = Task.Run(() =>
            {
                for(var i = 0; ; i++)
                {
                    if (!bq.Enqueue(i))
                        break;
                    Console.WriteLine($"Producer enqueuing {i} ");
                }
                Console.WriteLine("Producer has completed......");
            });

            Task consumerZero = Task.Run(() =>
            {
                    for (; ;)
                    {
                        Thread.Sleep(100);
                        int x = 0;
                        if ( !bq.Dequeue( out x ) )
                            break;
                        Console.WriteLine($"{0} dequeued {x}");
                    }
                    Console.WriteLine($"Consumer {0} has completed......");
            });

            Task consumerOne = Task.Run(() =>
            {
                for (; ;)
                {
                    Thread.Sleep(100);
                    int x = 0;
                    if ( !bq.Dequeue( out x ) )
                        break;
                    Console.WriteLine($"{1} dequeued {x}");
                }
                Console.WriteLine($"Consumer {1} has completed......");
            });


            Task consumerTwo = Task.Run(() =>
            {
                for (; ;)
                {
                    Thread.Sleep(100);
                    int x = 0;
                    if ( !bq.Dequeue( out x ) )
                        break;
                    Console.WriteLine($"{2} dequeued {x}");
                }
                Console.WriteLine($"Consumer {2} has completed......");
            });

            Task consumerThree = Task.Run(() =>
            {
                    for (; ;)
                    {
                        Thread.Sleep(100);
                        int x = 0;
                        if ( !bq.Dequeue( out x ) )
                            break;
                        Console.WriteLine($"{3} dequeued {x}");
                    }
                    Console.WriteLine($"Consumer {3} has completed......");
            });

            Thread.Sleep( 1000 );

            Console.WriteLine( "Quitting" );

            bq.Stop();

        }

        public static void DotNetBlockingCollection()
        {
            Console.WriteLine("Kick off a blocking collection");

            var prod = Task.Run(() => BlockExample.Producer());
            var con = Task.Run(() => BlockExample.Consumer());
        }
    }
}
