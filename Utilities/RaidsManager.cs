using Terraria;
using System.Collections.Generic;
using TUA.Raids;
using TUA.Raids.UI;

namespace TUA.Utilities
{
    public static class RaidsManager
    {
        public static HashSet<RaidsPanel> Panels { get; private set; }

        public static void Fill()
        {
            // Type no longer matters, the order of which panels are added here does
            Panels = new HashSet<RaidsPanel>
            {
                new RaidsPanel(RaidsID.None, () => RaidsWorld.currentRaid == RaidsID.None),
                new RaidsPanel(RaidsID.TheGreatHellRide, () => !Main.hardMode && !TUAWorld.Wasteland),
                new RaidsPanel(RaidsID.TheWrathOfTheWasteland, () => !Main.hardMode && TUAWorld.Wasteland),
                new RaidsPanel(RaidsID.TheEyeOfDestruction, () => TUAWorld.ApoMoonDowned && !TUAWorld.EoADowned)
            };
        }

        public static void Clear()
        {
            Panels.Clear();
        }
    }
}
