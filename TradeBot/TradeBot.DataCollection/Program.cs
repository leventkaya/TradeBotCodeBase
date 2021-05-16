using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TradeBot.DataCollection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Thread th1 = new Thread(new ThreadStart(Binance.GetData));
                Thread th2 = new Thread(new ThreadStart(BtcTurk.GetData));
                Thread th3 = new Thread(new ThreadStart(Paribu.GetData));

                th1.Start();
                th2.Start();
                th3.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR:{ex.Message}-------{DateTime.Now}");
            }

        }
    }
}
