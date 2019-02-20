using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Dev;

namespace TUA
{
    class TUAPlayer : ModPlayer
    {
        public static bool AugmendVortex = false;

        public static bool arenaActive = false;

        public bool noImmunityDebuff;


        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (arenaActive)
            {

                arenaActive = false;
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " think he was more powerful than the god of destruction... really?");
            }
            return true;
        }

        public override void UpdateBiomeVisuals()
        {
            if (Dimlibs.Dimlibs.getPlayerDim() != null) {
                bool inSolar = Dimlibs.Dimlibs.getPlayerDim() == "Solar";
                player.ManageSpecialBiomeVisuals("TUA:TUAPlayer", false, player.Center);
                bool inStardust = Dimlibs.Dimlibs.getPlayerDim() == "Stardust";
                player.ManageSpecialBiomeVisuals("TUA:StardustPillar", inStardust, player.Center);
            }
        }

        

        public override void PreUpdate()
        {
            if (noImmunityDebuff)
            {
                player.immune = false;
                player.immuneTime = -1;
            }
        }

        public override void UpdateDead()
        {
            if (SteamID64Checker.Instance.VerifyID() && TerrariaUltraApocalypse.devMode)
            {
                player.respawnTimer = 1; //for faster respawn while debugging
            }
        }

        
    }
}
