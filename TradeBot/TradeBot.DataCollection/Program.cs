using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TradeBot.DataCollection
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Binance.GetData();
            BtcTurk.GetData();
            Paribu.GetData();


        }
    }
}
