using ServerCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    

    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipaddr = ipHost.AddressList[0]; //아이피가 여러개 있을수 있으며 배열로 ip를 반환함
            IPEndPoint endPoint = new IPEndPoint(ipaddr, 7777);

            Connector conneter = new Connector();
            conneter.Connect(endPoint, () => { return new ServerSession(); });

            while (true)
            {
                try
                {
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(100);
            }
        }
    }
}
