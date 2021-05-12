using Binance.Net;
using Binance.Net.Objects.Spot.MarketStream;
using System;

namespace TradeBot.DataCollection
{
    public static class Binance
    {
        public static async void GetData()
        {

            var socketClient = new BinanceSocketClient();

            await socketClient.Spot.SubscribeToSymbolTickerUpdatesAsync("BTCUSDT", data =>
            {
                WriteFile.WriteFileCsvBinance((BinanceStreamTick)data);
                Console.WriteLine($"Time:{data.CloseTime} Time2:{DateTime.Now} Price:{data.LastPrice}");
            });

        }
    }
}
