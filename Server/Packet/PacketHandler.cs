using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            Console.WriteLine($"PlayerInfoReq : {p.playerId},{p.name} ");
            foreach (PlayerInfoReq.Skill data in p.skills)
            {
                Console.WriteLine($"SkillList : {data.id},{data.level},{data.duration} ");
            }
        }
    }
}
