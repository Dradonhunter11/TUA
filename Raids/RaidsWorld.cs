using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.Raids
{
    class RaidsWorld : ModWorld
    {
        public static RaidsType currentRaids = RaidsType.noActiveRaids;
        public static int stage = 0;
        public static Dictionary<RaidsType, String> raidsName = raidsName = new Dictionary<RaidsType, string>
            {
                { RaidsType.noActiveRaids, "No active raids" },
                { RaidsType.theGreatHellRide, "The Great Hell Ride!" },
                { RaidsType.theWrathOfTheWasteland, "The Wrath of the Wasteland!" },
                { RaidsType.theEyeOfDestruction, "The God of Destruction" }
            };
        public static List<string> hasTalkedToGuide;

        public override TagCompound Save() => new TagCompound
            {
                ["currentRaids"] = (byte)currentRaids,
                ["stage"] = stage,
                ["hasTalkedToGuide"] = hasTalkedToGuide
            };

        public override void Load(TagCompound tag)
        {
            currentRaids = (RaidsType) tag.GetByte("currentRaids");
            stage = tag.GetInt("stage");
            hasTalkedToGuide = (List<string>)tag.GetList<string>("hasTalkedToGuide");
        }
    }

    public enum RaidsType : byte
    {
        noActiveRaids = 0,
        theGreatHellRide = 1,
        theWrathOfTheWasteland = 2,
        theEyeOfDestruction = 3
    }
}
