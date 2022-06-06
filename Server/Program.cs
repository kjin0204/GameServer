using System;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{

    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    //class LoginIOkPacket : Packet
    //{

    //}

    class GameSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            //byte[] sendBuffe = Encoding.UTF8.GetBytes("welcome to MMORPG Server !...");

            //Packet knight = new Packet() { size = 100, packetId = 10 };
            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(knight.size);
            //byte[] buffer2 = BitConverter.GetBytes(knight.packetId);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            //ArraySegment<byte> sendBuffe = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            //Send(sendBuffe);


            Thread.Sleep(1000);

            Disconneted();
            Disconneted();
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
            Console.WriteLine($"ReceiveId : {id} , size : {size}");

        }

        public override void OnDisconnected(EndPoint endPoint)
        {
        }


        //public override int OnRecv(ArraySegment<byte> buffer)
        //{
        //    string ReceiveData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
        //    Console.WriteLine($"[From Client] {ReceiveData}");

        //    return buffer.Count;
        //}

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"SendData Byte : {numOfBytes }");
        }
    }


    class Program
    {
        static Listener _listener = new Listener();


        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipaddr = ipHost.AddressList[0]; //아이피가 여러개 있을수 있으며 배열로 ip를 반환함
            IPEndPoint endPoint = new IPEndPoint(ipaddr, 7777);

            _listener.init(endPoint, () => { return new GameSession(); });

            while (true)
            {

            }
        }
    }
}
