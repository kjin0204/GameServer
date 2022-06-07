﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{

    //{0} 패킷 이름
    //{1} 멤버 변수들
    //{2} 멤버 변수 Read
    //{3} 멤버 변수 Write
    class PaketFormat
    {
        public static string paketFormat =
@"
class {0}
{{
    {1}

    public void Read(ArraySegment<byte> segment)
    {{
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array,segment.Offset,segment.Count);

        ushort count = 0;
        //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
        count += sizeof(ushort);
        //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + 2);
        count += sizeof(ushort);;

        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        bool success = true;
        ushort count = 0;

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.{0});
        count += sizeof(ushort);
        {3}
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }}
}}
";
        //{0} 변수 형식
        //{1} 변수 이름
        public static string memberFormat =
@"
public {0} {1}
";


        //{0} 변수 이름
        //{1} To ~ 변수 형식
        //{2} 변수 형식
        public static string readFormat =
@"
this.{0} = BitConverter.To{1}(s.Slice(count, s.Length - count));
count += sizeof({2});
";

        //{0} 변수 이름
        public static string readStringFormat =
@"
ushort {0}Len = (ushort)BitConverter.ToInt64(s.Slice(count, s.Length - count));
count += 2;
{0} = Encoding.Unicode.GetString(segment.Array, count, {0}Len);
count += {0}Len;
";


        //{0} 변수 이름
        //{1} 변수 형식
        public static string writeFormat =
@"
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.{0});
count += sizeof({1});
";



        //{0} 변수 이름
        public static string writeStringFormat =
@"
ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), {0}Len);
count += {0}Len;
count += sizeof(ushort);
";

    }
}