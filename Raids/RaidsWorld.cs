using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.Raids
{
    class RaidsWorld : ModWorld
    {
        public static byte currentRaid;
        public static int stage;
        public static List<string> hasTalkedToGuide;

        public override void Initialize()
        {
            currentRaid = 0;
            stage = 0;
            hasTalkedToGuide = new List<string>();
        }

        public override TagCompound Save() => new TagCompound
            {
                ["currentRaids"] = currentRaid,
                ["stage"] = stage,
                ["hasTalkedToGuide"] = hasTalkedToGuide
            };

        public override void Load(TagCompound tag)
        {
            currentRaid = tag.GetByte("currentRaids");
            stage = tag.GetInt("stage");
            hasTalkedToGuide = (List<string>)tag.GetList<string>("hasTalkedToGuide");
        }
    }

    public enum RaidsType : byte
    {
        None = 0,
        theGreatHellRide = 1,
        theWrathOfTheWasteland = 2,
        theEyeOfDestruction = 3,
        DryadsRequest
    }

    public static class RaidsID
    {
        public const byte None = 0;
        public const byte TheGreatHellRide = 1;
        public const byte TheWrathOfTheWasteland = 2;
        public const byte TheEyeOfDestruction = 3;
        public const byte DryadsRequest = 4;
        public static Dictionary<byte, string> raidsName = new Dictionary<byte, string>
            {
                { None, "No active raids" },
                { TheGreatHellRide, "The Great Hell Ride!" },
                { TheWrathOfTheWasteland, "The Wrath of the Wasteland!" },
                { TheEyeOfDestruction, "The God of Destruction" }
            };
    }
}
