using ServerCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Conneted to {endPoint.ToString()}");

            for (int i = 0; i < 5; i++)
            {
                Packet knight = new Packet() { size = 4, packetId = 7 };
                ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
                byte[] buffer = BitConverter.GetBytes(knight.size);
                byte[] buffer2 = BitConverter.GetBytes(knight.packetId);
                Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
                Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
                ArraySegment<byte> sendBuffe = SendBufferHelper.Close(knight.size);


                Send(sendBuffe);
            }

            Thread.Sleep(1000);

            Disconneted();
            Disconneted();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string ReceiveData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {ReceiveData}");

            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {

            Console.WriteLine($"SendData Byte : {numOfBytes }");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipaddr = ipHost.AddressList[0]; //아이피가 여러개 있을수 있으며 배열로 ip를 반환함
            IPEndPoint endPoint = new IPEndPoint(ipaddr, 7777);

            Connector conneter = new Connector();
            conneter.Connect(endPoint, () => { return new GameSession(); });

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
