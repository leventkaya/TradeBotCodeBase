using Binance.Net.Objects.Spot.MarketStream;
using BtcTurk.Net.Objects.SocketObjects;
using System;
using System.IO;

namespace TradeBot.DataCollection
{
    public static class WriteFile
    {
        #region WriteCsvFile
        static string BTC_TURK_CSV_PATH = @"C:\Users\Raşit\Desktop\btcTurk.csv";
        static string BINANCE_CSV_PATH = @"C:\Users\Raşit\Desktop\binance.csv";
        static string PARIBU_CSV_PATH = @"C:\Users\Raşit\Desktop\paribu.csv";
        public static void WriteFileCsvBinance(BinanceStreamTick data)
        {
            if (DateTime.Now.Second == 0)
            {
                string createText = string.Format("{0};{1}{2}", DateTime.Now, FormatDecimal(data.LastPrice, 0), Environment.NewLine);
                if (!File.Exists(BINANCE_CSV_PATH))
                {
                    File.WriteAllText(BINANCE_CSV_PATH, createText);
                }
                else
                {
                    File.AppendAllText(BINANCE_CSV_PATH, createText);
                }
            }
        }
        public static void WriteFileCsvParibu(string data)
        {
            if (DateTime.Now.Second == 0)
            {
                string createText = string.Format("{0};{1}{2}", DateTime.Now, FormatDecimal(Convert.ToDecimal(data), 0), Environment.NewLine);
                if (!File.Exists(PARIBU_CSV_PATH))
                {
                    File.WriteAllText(PARIBU_CSV_PATH, createText);
                }
                else
                {
                    File.AppendAllText(PARIBU_CSV_PATH, createText);
                }
            }
        }
        public static void WriteFileCsvBtcTurk(BtcTurkStreamTickerSingle data)
        {
            if (DateTime.Now.Second == 0)
            {
                string createText = string.Format("{0};{1}{2}", DateTime.Now, FormatDecimal(data.Close, 0), Environment.NewLine);
                if (!File.Exists(BTC_TURK_CSV_PATH))
                {
                    File.WriteAllText(BTC_TURK_CSV_PATH, createText);
                }
                else
                {
                    File.AppendAllText(BTC_TURK_CSV_PATH, createText);
                }
            }
        }
        #endregion

        #region WriteTxtFile
        static string BTC_TURK_PATH = @"C:\Users\Raşit\Desktop\btcTurk.txt";
        static string BINANCE_PATH = @"C:\Users\Raşit\Desktop\binance.txt";
        static string PARIBU_PATH = @"C:\Users\Raşit\Desktop\paribu.txt";
        public static void WriteFileParibu(string data)
        {
            if (DateTime.Now.Second == 0)
            {
                if (!File.Exists(BTC_TURK_PATH))
                {

                    string createText = data + " " + DateTime.Now + Environment.NewLine;
                    File.WriteAllText(PARIBU_PATH, createText);
                }
                else
                {
                    string appendText = data + " " + DateTime.Now + Environment.NewLine;
                    File.AppendAllText(PARIBU_PATH, appendText);
                }
            }
        }
        public static void WriteFileBtcTurk(BtcTurkStreamTickerSingle data)
        {
            if (DateTime.Now.Second == 0)
            {
                if (!File.Exists(BTC_TURK_PATH))
                {

                    string createText = data.Close + " " + DateTime.Now + Environment.NewLine;
                    File.WriteAllText(BTC_TURK_PATH, createText);
                }
                else
                {
                    string appendText = data.Close + " " + DateTime.Now + Environment.NewLine;
                    File.AppendAllText(BTC_TURK_PATH, appendText);
                }
            }
        }
        public static void WriteFileBinance(BinanceStreamTick data)
        {
            if (DateTime.Now.Second == 0)
            {
                if (!File.Exists(BINANCE_PATH))
                {
                    string createText = data.LastPrice + " " + DateTime.Now + " " + data.CloseTime + Environment.NewLine;
                    File.WriteAllText(BINANCE_PATH, createText);
                }
                else
                {
                    string appendText = data.LastPrice + " " + DateTime.Now + " " + data.CloseTime + Environment.NewLine;
                    File.AppendAllText(BINANCE_PATH, appendText);
                }
            }
        }
        #endregion
        public static string FormatDecimal(this decimal value, int decimalSeparator = 2)
        {
            return value.ToString(string.Format("0.{0}", new string('0', decimalSeparator)));
        }
    }
}
