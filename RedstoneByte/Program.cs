using System;
using System.Diagnostics;
using System.Net;
using RedstoneByte.Utils;

namespace RedstoneByte
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            RedstoneByte.Start().Wait();
            watch.Stop();
            RedstoneByte.Logger.Info("Done! It took {0}ms.", watch.ElapsedMilliseconds);
            var flag = true;
            while (flag)
            {
                switch (Console.ReadLine())
                {
                    case string str when str.StartsWith("server", StringComparison.OrdinalIgnoreCase):
                        if (str.StartsWith("servers add", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var data = str.Substring(11).Split(' ');
                            var ep = data[1].Split(':');
                            var info = new ServerInfo(new IPEndPoint(IPAddress.Parse(ep[0]), Convert.ToInt32(ep[1])),
                                data[0].ToLowerInvariant());
                            RedstoneByte.Logger.Info("Adding {0} with address {1}", info.Name, info.EndPoint);
                            ServerQueue.AddLast(info);
                        }
                        else if (str.StartsWith("servers remove", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var data = str.Substring(13).ToLowerInvariant();
                            RedstoneByte.Logger.Info("Removing {0}", data);
                            ServerQueue.Remove(data);
                        }
                        break;

                    case string str when str.StartsWith("stop", StringComparison.OrdinalIgnoreCase):
                        flag = false;
                        break;
                }
            }
            RedstoneByte.Stop().Wait();
        }
    }
}