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

    public static class RaidsID
    {
        public const byte None = 0;
        public const byte TheGreatHellRide = 1;
        public const byte TheWrathOfTheWasteland = 2;
        public const byte TheEyeOfDestruction = 3;
        // public const byte DryadsRequest = 4;
        public static string[] raidsName = new string[]
            {
                "No active raids",
                "The Great Hell Ride!",
                "The Wrath of the Wasteland!",
                "The God of Destruction"
            };
    }
}
