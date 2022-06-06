using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
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
        public string name;

        public struct SkillInfo
        {
            public int id;
            public short level;
            public float duration;

            public bool Write(Span<byte> s, ref ushort count)
            {
                bool success = true;

                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.id);
                count += sizeof(int);
                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.level);
                count += sizeof(short);
                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.duration);
                count += sizeof(float);

                return success;
            }


            public SkillInfo Read(ReadOnlySpan<byte> s, ref ushort count)
            {
                this.id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
                count += sizeof(int);
                this.level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
                count += sizeof(short);
                this.duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
                count += sizeof(float);

                return this;
            }
        }

        public List<SkillInfo> skills = new List<SkillInfo>();

        public override void Read(ArraySegment<byte> segment)
        {
            ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

            ushort count = 0;
            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += 2;
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + 2);
            count += 2;

            playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));

            count += 8;

            //string
            ushort stringLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            count += 2;
            name = Encoding.Unicode.GetString(segment.Array, count, stringLen);
            count += stringLen;

            skills.Clear();
            //skills
            ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            count += 2;

            for (int i = 0; i < skillLen; i++)
            {
                skills.Add(new SkillInfo().Read(s, ref count));
            }
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            bool success = true;
            ushort count = 0;

            count += 2;
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.PlayerInfoReq);
            count += 2;
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
            count += 8;


            //string StringLen를 string값 앞에 넣어주기 위해 2바이트 뒤에 string 입력
            ushort stringLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), stringLen);
            count += stringLen;
            count += sizeof(ushort);

            //skill
            ushort skillLne = (ushort)skills.Count;
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), skillLne);
            count += sizeof(ushort);
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i].Write(s, ref count);
            }

            success &= BitConverter.TryWriteBytes(s, count);
            if (success == false)
                return null;

            return SendBufferHelper.Close(count);
        }
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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "가나다" };
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { level = 1, duration = 3.0f, id = 100 });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { level = 2, duration = 4.0f, id = 101 });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { level = 3, duration = 5.0f, id = 102 });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { level = 4, duration = 6.0f, id = 103 });

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
            int count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
            count += 2;

            switch((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    PlayerInfoReq p = new PlayerInfoReq();
                    p.Read(buffer);

                    Console.WriteLine($"PlayerInfoReq : {p.playerId},{p.name} ");
                    foreach(PlayerInfoReq.SkillInfo data in p.skills)
                    {
                        Console.WriteLine($"SkillList : {data.id},{data.level},{data.duration} ");
                    }
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
