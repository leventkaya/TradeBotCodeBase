using BtcTurk.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;

namespace TradeBot.DataCollection
{
    public static class BtcTurk
    {
        public static async void GetData()
        {
            var sock = new BtcTurkSocketClient(new BtcTurkSocketClientOptions { LogVerbosity = LogVerbosity.Debug });

            /* Public Socket Endpoints: */
            var subs = new List<UpdateSubscription>();
            {
                var subscription = await sock.SubscribeToTickerAsync("BTCUSDT", (ticker) =>
                {
                    if (ticker != null)
                    {
                        WriteFile.WriteFileCsvBtcTurk(ticker);
                        Console.WriteLine($"Channel: {ticker.Channel} Event:{ticker.Event} Type:{ticker.Type} >> " +
                        $"B:{ticker.Bid} A:{ticker.Ask} PS:{ticker.PairSymbol} NS:{ticker.NumeratorSymbol} DS:{ticker.DenominatorSymbol} " +
                        $"O:{ticker.Open} H:{ticker.High} L:{ticker.Low} LA:{ticker.Close} V:{ticker.Volume} " +
                        $"AV:{ticker.AveragePrice} D:{ticker.DailyChange} DP:{ticker.DailyChangePercent} PId:{ticker.PairId} Ord:{ticker.OrderNum} ");
                    }
                });
                subs.Add(subscription.Data);
            }
        }
    }
}
