
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse.Raids
{
    class RaidsWorld : ModWorld
    {
        internal RaidsType currentRaids = RaidsType.noActiveRaids;
        internal int stage = 0;
        internal static Dictionary<RaidsType, String> raidsName = new Dictionary<RaidsType, string>();

        public override void Initialize()
        {
            loadRaidsName();
        }


        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("currentRaids", (byte)currentRaids);
            tag.Add("stage", stage);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            currentRaids = (RaidsType) tag.GetByte("currentRaids");
            stage = tag.GetInt("stage");
            loadRaidsName();
        }

        internal void loadRaidsName()
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
