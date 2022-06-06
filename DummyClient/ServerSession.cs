using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
    }
    class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2
    }

    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"Conneted to {endPoint.ToString()}");

            //for (int i = 0; i < 5; i++)
            {
                PlayerInfoReq knight = new PlayerInfoReq() { size = 4, packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001 };
                ArraySegment<byte> s = SendBufferHelper.Open(4096);

                bool success = true;
                ushort count = 0;

                count += 2;
                success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), knight.packetId);
                count += 2;
                success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), knight.playerId);
                count += 8;

                success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);
                ArraySegment<byte> sendBuffe = SendBufferHelper.Close(count);

                if (success)
                    Send(sendBuffe);

                //byte[] buffer = BitConverter.GetBytes(knight.size);
                //byte[] buffer2 = BitConverter.GetBytes(knight.packetId);
                //byte[] buffer3 = BitConverter.GetBytes(knight.playerId);
                //Array.Copy(buffer, 0, s.Array, s.Offset + count, buffer.Length);
                //count += 2;
                //Array.Copy(buffer2, 0, s.Array, s.Offset + count, buffer2.Length);
                //count += 2;
                //Array.Copy(buffer3, 0, s.Array, s.Offset + count, buffer3.Length);
                //count += 8;
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
}
