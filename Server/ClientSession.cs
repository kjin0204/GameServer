using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
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


    class ClientSession : PacketSession
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
            int count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
            count += 2;

            switch((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    long playerID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 4);
                    count += 8;
                    Console.WriteLine($"PlayerInfoReq : {playerID} ");
                    break;
            }
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
}
