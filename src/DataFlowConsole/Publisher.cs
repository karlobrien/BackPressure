using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace DataFlowConsole
{
    public class Publisher //<T> where T : struct
    {
        private IObservable<PriceBar> _pub;

        public Publisher()
        {
            Random random = new Random();

            Dictionary<long, string> symbols = new Dictionary<long, string>();
            symbols.Add(1, "VOD LN");
            symbols.Add(2, "IBIC LN");
            symbols.Add(3, "SEM FP");
            symbols.Add(4, "KAR GY");
            symbols.Add(5, "EO9F GY");

            DateTime mkdtDate = DateTime.UtcNow;
            PriceBar pBar = new PriceBar();
            pBar.Close = 99;
            pBar.High = 102;
            pBar.Low = 98;
            pBar.Open = 100;
            pBar.Volume = 50;
            pBar.Date = mkdtDate;

            _pub = Observable.Interval(TimeSpan.FromMilliseconds(250))
                .Select(t => {
                    GeneratePrices.GenerateRandomBar(pBar);
                    return pBar;
                });
        }

        public IObservable<PriceBar> GetMarketDataObs(string symbol)
        {
            return _pub;
        }
    }

    public class PriceBar
    {
            public DateTime Date { get; set; }
            public double Open { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Close { get; set; }
            public long Volume { get; set; }

            public override string ToString()
            {
                return $"Date: {Date}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}";
            }
    }

    public static class GeneratePrices
    {
        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static void GenerateRandomBar(PriceBar newBar)
        {
            double fluct = 0.025;
            double volFluct = 0.40;

            //Open is equal to the previous close
            newBar.Open = newBar.Close;
            newBar.Close = GetRandomNumber(newBar.Close - newBar.Close * fluct, newBar.Close + newBar.Close * fluct);
            newBar.High = GetRandomNumber(Math.Max(newBar.Close, newBar.Open), Math.Max(newBar.Close, newBar.Open) + Math.Abs(newBar.Close - newBar.Open) * fluct);
            newBar.Low = GetRandomNumber(Math.Min(newBar.Close, newBar.Open), Math.Min(newBar.Close, newBar.Open) - Math.Abs(newBar.Close - newBar.Open) * fluct);
            newBar.Volume = (long)GetRandomNumber(newBar.Volume - newBar.Volume * volFluct, newBar.Volume + newBar.Volume * volFluct);
        }
    }
    public struct MarketData
    {
        public MarketData(DateTime tickDate, long seqId, string symbol, decimal bidPrice,
            long bidSize, decimal askPrice, long askSize)
        {
            TickDate = tickDate;
            SeqId = seqId;
            Symbol = symbol;
            BidPrice = bidPrice;
            BidSize = bidSize;
            AskPrice = askPrice;
            AskSize = askSize;
        }
        public DateTime TickDate { get; }
        public long SeqId { get; }
        public string Symbol { get; }
        public decimal BidPrice { get; }
        public long BidSize { get; }
        public decimal AskPrice { get; }
        public long AskSize { get; }
    }
}