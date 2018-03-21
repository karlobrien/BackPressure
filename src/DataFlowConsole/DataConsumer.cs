
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using Serilog;

namespace DataFlowConsole
{
    public class DataCapture : IDisposable
    {
        public BlockingCollection<PriceBar> PriceQueue { get; private set;}
        private ILogger _logger;
        private IDisposable _pubDispose;
        public DataCapture(Publisher publisher, ILogger logger)
        {
            if (publisher == null)
                throw new ArgumentNullException();
            if(logger == null)
                throw new ArgumentNullException();

            _logger = logger;

            PriceQueue = new BlockingCollection<PriceBar>(boundedCapacity: 10);

            _pubDispose = publisher.GetMarketDataObs("")
                .Subscribe(OnNext, OnError, OnComplete);
        }

        private void OnNext(PriceBar priceBar)
        {
            PriceQueue.Add(priceBar);
        }

        private void OnError(Exception ex)
        {
            _logger.Error(ex.Message);
        }

        private void OnComplete()
        {
            PriceQueue.CompleteAdding();
        }

        public void Dispose()
        {
            _pubDispose.Dispose();
        }
    }

}