using Dimlibs;
using Terraria.ID;

namespace TUA.Dimension
{
    public static class DimensionUtil
    {
        public static readonly int[] VortexEnemy = new int[] { NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier };
        public static readonly int[] SolarEnemy = new int[] { NPCID.SolarCorite, NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider };
        public static readonly int[] StardustEnemy = new int[] { NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustSpiderBig, NPCID.StardustWormHead };
        public static readonly int[] NebulaEnemy = new int[] { NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier };

        public static bool PlayerInSolar => Dimlibs.Dimlibs.getPlayerDim() == "Solar";
    }
}
