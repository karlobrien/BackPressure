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

            CustomBlockQueue();

            _quitEvent.WaitOne();

            //Clean up the remaining items
        }

        public static void CustomBlockQueue()
        {
            var bq = new CustomBlockingQueue<int>(10);

            Task producer = new Task(() =>
            {
                for(var i = 0; ; i++)
                {
                    if (!bq.Enqueue(i))
                        break;
                    Console.WriteLine($"{i} >>");
                }
                Console.WriteLine("Producer has completed......");
            });
            producer.Start();

            Task consumer = new Task(() =>
            {
                    for (; ;)
                    {
                        Thread.Sleep(100);
                        int x = 0;
                        if ( !bq.Dequeue( out x ) )
                            break;
                        Console.WriteLine($"{0} < {x}");
                    }
                    Console.WriteLine($"Consumer {0} has completed......");
            });
            consumer.Start();

            Task consumerTwo = new Task(() =>
            {
                for (; ;)
                {
                    Thread.Sleep(100);
                    int x = 0;
                    if ( !bq.Dequeue( out x ) )
                        break;
                    Console.WriteLine($"{1} < {x}");
                }
                Console.WriteLine($"Consumer {1} has completed......");
            });
            consumerTwo.Start();


            Task consumerThree = new Task(() =>
            {
                for (; ;)
                {
                    Thread.Sleep(100);
                    int x = 0;
                    if ( !bq.Dequeue( out x ) )
                        break;
                    Console.WriteLine($"{2} < {x}");
                }
                Console.WriteLine($"Consumer {2} has completed......");
            });
            consumerThree.Start();

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
