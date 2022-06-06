using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace DummyClient
{
    abstract class Packet
    {
        public ushort size;
        public ushort packetId;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> s);
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;

        public override void Read(ArraySegment<byte> s)
        {
            int count = 0;
            ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(s.Array, s.Offset + 2);
            count += 2;

            long playerID = BitConverter.ToUInt16(s.Array, s.Offset + 4);
            BitConverter.ToUInt64(new ReadOnlySpan<byte>(s.Array, s.Offset + count, s.Count - count));

            count += 8;

            Console.WriteLine($"PlayerInfoReq : {playerID} ");
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> s = SendBufferHelper.Open(4096);

            bool success = true;
            ushort count = 0;

            count += 2;
            success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), (ushort)PacketID.PlayerInfoReq);
            count += 2;
            success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), this.playerId);
            count += 8;

            success = BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);
           

            if (success == false)
                return null;

            return SendBufferHelper.Close(count);
        }
    }
    //class PlayerInfoOk : Packet
    //{
    //    public int hp;
    //    public int attack;
    //}

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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001 };


            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = packet.Write();
                if (s != null)
                    Send(s);

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
