using System;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{

    class Program
    {
        static Listener _listener = new Listener();


        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipaddr = ipHost.AddressList[0]; //아이피가 여러개 있을수 있으며 배열로 ip를 반환함
            IPEndPoint endPoint = new IPEndPoint(ipaddr, 7777);

            _listener.init(endPoint, () => { return new ClientSession(); });

            while (true)
            {

            }
        }
    }
}
