using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace TradeBot.DataCollection
{
    public static class Paribu
    {
        public static void GetData()
        {

            while (true)
            {
                if (DateTime.Now.Second == 0)
                {
                    var webRequest = WebRequest.Create("https://www.paribu.com/ticker") as HttpWebRequest;
                    if (webRequest == null)
                    {
                        return;
                    }

                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "Nothing";

                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var contributorsAsJson = sr.ReadToEnd();
                            var orderResponse = Coins.FromJson(contributorsAsJson);
                            //var BTC_USDT= orderResponse.Where(x=>x.Key=="BTC_USDT").Select(i => $"{i.Key}: {i.Value.Last}").ToList().ForEach(System.Console.WriteLine);
                            var BTC_USDT = orderResponse.Where(x => x.Key == "BTC_USDT").Select(i => $"{i.Value.Last}").First();
                            // orderResponse üzerinden devam... 
                            WriteFile.WriteFileCsvParibu(BTC_USDT);

                        }
                    }
                    System.Threading.Thread.Sleep(10000);
                }
            }
        }
        public static void GetMarket()
        {
            while (true)
            {
                var webRequest = WebRequest.Create("https://v3.paribu.com/app/markets/btc-tl?interval=1000") as HttpWebRequest;
                if (webRequest == null)
                {
                    return;
                }

                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";

                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var contributorsAsJson = sr.ReadToEnd();
                        var orderResponse = Coins.FromJson(contributorsAsJson);

                        // orderResponse üzerinden otomasyon 
                    }
                }
                System.Threading.Thread.Sleep(60000);
                System.Console.Clear();
                Console.ReadLine();
            }
        }
    }
    public partial class Coins
    {
        [JsonProperty("lowestAsk")]
        public double LowestAsk { get; set; }

        [JsonProperty("highestBid")]
        public double HighestBid { get; set; }

        [JsonProperty("low24hr")]
        public double Low24Hr { get; set; }

        [JsonProperty("high24hr")]
        public double High24Hr { get; set; }

        [JsonProperty("avg24hr")]
        public double Avg24Hr { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("last")]
        public double Last { get; set; }

        [JsonProperty("change")]
        public double Change { get; set; }

        [JsonProperty("percentChange")]
        public double PercentChange { get; set; }

        [JsonProperty("chartData")]
        public object[] ChartData { get; set; }
    }

    public partial class Coins
    {
        public static Dictionary<string, Coins> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, Coins>>(json, Converter.Settings);
    }

    public partial class Data
    {
        [JsonProperty("orderBook")]
        public OrderBook OrderBook { get; set; }

        [JsonProperty("marketMatches")]
        public MarketMatch[] MarketMatches { get; set; }

        [JsonProperty("charts")]
        public Charts Charts { get; set; }
    }

    public partial class Charts
    {
        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("interval")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Interval { get; set; }

        [JsonProperty("t")]
        public object[] T { get; set; }

        [JsonProperty("c")]
        public object[] C { get; set; }

        [JsonProperty("v")]
        public object[] V { get; set; }
    }

    public partial class MarketMatch
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("trade")]
        public Trade Trade { get; set; }
    }

    public partial class OrderBook
    {
        [JsonProperty("buy")]
        public Dictionary<string, double> Buy { get; set; }

        [JsonProperty("sell")]
        public Dictionary<string, double> Sell { get; set; }
    }

    public enum Trade { Buy, Sell };


    public static class Serialize
    {
        public static string ToJson(this Coins self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
        {
            TradeConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class TradeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Trade) || t == typeof(Trade?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "buy":
                    return Trade.Buy;
                case "sell":
                    return Trade.Sell;
            }
            throw new Exception("Cannot unmarshal type Trade");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Trade)untypedValue;
            switch (value)
            {
                case Trade.Buy:
                    serializer.Serialize(writer, "buy");
                    return;
                case Trade.Sell:
                    serializer.Serialize(writer, "sell");
                    return;
            }
            throw new Exception("Cannot marshal type Trade");
        }

        public static readonly TradeConverter Singleton = new TradeConverter();
    }


}
