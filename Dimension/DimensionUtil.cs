using Terraria.ID;

namespace TUA.Dimension
{
    public static class DimensionUtil
    {
        public static readonly int[] vortexEnemy = new int[] { NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexLarva, NPCID.VortexRifleman, NPCID.VortexSoldier };
        public static readonly int[] solarEnemy = new int[] { NPCID.SolarCorite, NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider };
        public static readonly int[] stardustEnemy = new int[] { NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustSpiderBig, NPCID.StardustWormHead };
        public static readonly int[] nebulaEnemy = new int[] { NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier };

        public static bool PlayerInSolar => Dimlibs.Dimlibs.getPlayerDim() == "Solar";

        public static string CurDim => Dimlibs.Dimlibs.getPlayerDim();

        public static bool PlayerInDim(string dim) => Dimlibs.Dimlibs.getPlayerDim() == dim;
    }
}
