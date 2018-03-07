using System;
using DataFlowConsole;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace SimpleUnitTest
{
    public class SimpleTests
    {
        [Fact]
        public async Task ConsumerReceivesCorrectValues()
        {
            // Define the mesh.
            var queue = new BufferBlock<int>();

            // Start the producer and consumer.
            var values = Enumerable.Range(0, 10);
            TplDataFlow.Produce(queue, values);
            var consumer = TplDataFlow.Consume(queue);

            // Wait for everything to complete.
            await Task.WhenAll(consumer, queue.Completion);

            // Ensure the consumer got what the producer sent (in the correct order).
            var results = await consumer;
            Assert.True(results.SequenceEqual(values));
        }
    }
}