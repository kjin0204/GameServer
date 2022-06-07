using System;
using System.Xml;

namespace PacketGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true, //주석무시
                IgnoreWhitespace = true // 스페이스바 무시
            };


            using (XmlReader r = XmlReader.Create("PDL.xml", settings))
            {
                r.MoveToContent(); //헤더를 건너뜀

                while (r.Read())
                {
                    //Depth : 엘리멘트의 깊이  ,Element : 패킷에 시작, EndElement : 패킷에 마지막
                    if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                        ParsePacket(r);
                    //Console.WriteLine(r.Name + " " + r["name"]);
                }
            }

        }

        public static void ParsePacket(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.EndElement)
                return;

            if (r.Name.ToLower() != "packet")
            {
                Console.WriteLine("Invalid packet node");
                return;
            }
            string packetName = r["name"];
            if (string.IsNullOrEmpty(packetName))
            {
                Console.WriteLine("Packet without name");
                return;
            }

            ParseMembers(r);
        }

        public static void ParseMembers(XmlReader r)
        {
            string packetName = r["name"];

            int depth = r.Depth + 1;
            while(r.Read())
            {
                if (r.Depth != depth)
                    break;

                string memeberName = r["name"];
                if(string.IsNullOrEmpty(memeberName))
                {
                    Console.WriteLine("Member without name");
                    return;
                }

                string memberType = r.Name.ToLower();

                switch(memberType)
                {
                    case "bool":
                    case "byte":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                    case "string":
                    case "list":
                        break;
                    default:
                        break;
                }
            }
            

        }
    }
}
