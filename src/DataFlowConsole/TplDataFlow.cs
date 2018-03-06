using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowConsole
{
    public class TplDataFlow
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
