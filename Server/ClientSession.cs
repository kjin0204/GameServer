using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{

	

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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "가나다" };
            var att = new PlayerInfoReq.Skill() { level = 1, duration = 3.0f, id = 100 };
            att.attributes.Add(new PlayerInfoReq.Skill.Attribute() { att = 606 });
            packet.skills.Add(att);
            packet.skills.Add(new PlayerInfoReq.Skill() { level = 2, duration = 4.0f, id = 101 });
            //         packet.skills.Add(new PlayerInfoReq.Skill() { level = 3, duration = 5.0f, id = 102 });
            //         packet.skills.Add(new PlayerInfoReq.Skill() { level = 4, duration = 6.0f, id = 103 });

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
            Thread.Sleep(3000);
            Disconneted();
            Disconneted();
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
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
