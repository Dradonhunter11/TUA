using Terraria;
using System.Collections.Generic;
using TUA.Raids;
using TUA.Raids.UI;
using System.Linq;

namespace TUA.Utilities
{
    public static class RaidsManager
    {
        public static HashSet<RaidsPanel> Panels { get; private set; }

        public static void Fill()
        {
            Panels = new HashSet<RaidsPanel>
            {
                // 'false' doesn't matter, the none always shows up if no other raid is selected
                new RaidsPanel(RaidsID.None, () => false),
                new RaidsPanel(RaidsID.TheGreatHellRide, () => Main.hardMode && TUAWorld.Wasteland),
                new RaidsPanel(RaidsID.TheWrathOfTheWasteland, () => Main.hardMode && !TUAWorld.Wasteland),
                new RaidsPanel(RaidsID.ApoMoon, () => TUAWorld.ApoMoonDowned)
                new RaidsPanel(RaidsID.TheEyeOfDestruction, () => TUAWorld.EoADowned)
            };
        }

        public static void Clear()
        {
            Panels.Clear();
        }

        public static bool IsComplete(int raid)
        {
            return Panels.FirstOrDefault(x => x.RaidsType == raid).Complete();
        }
    }
}
