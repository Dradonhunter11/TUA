
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.Raids
{
    class RaidsWorld : ModWorld
    {
        internal static RaidsType currentRaids = RaidsType.noActiveRaids;
        internal static int stage = 0;
        internal static Dictionary<RaidsType, String> raidsName = new Dictionary<RaidsType, string>();

        public override void Initialize() => LoadRaidsName();

        public override TagCompound Save() => new TagCompound
            {
                { "currentRaids", (byte)currentRaids },
                { "stage", stage }
            };

        public override void Load(TagCompound tag)
        {
            currentRaids = (RaidsType) tag.GetByte("currentRaids");
            stage = tag.GetInt("stage");
            LoadRaidsName();
        }

        internal void LoadRaidsName()
        {
            raidsName.Clear();
            raidsName.Add(RaidsType.noActiveRaids, "No active raids");
            raidsName.Add(RaidsType.theGreatHellRide, "The Great Hell Ride!");
            raidsName.Add(RaidsType.theWrathOfTheWasteland, "The Wrath of the Wasteland!");
            raidsName.Add(RaidsType.theEyeOfDestruction, "The God of Destruction");
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
