using System;
using System.Collections.Concurrent;

namespace DataFlowConsole
{
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
}
