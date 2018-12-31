using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse
{
    class TUAPlayer : ModPlayer
    {
        public static bool AugmendVortex = false;

        public static bool arenaActive = false;

        public bool noImmunityDebuff;

        public override void Load(TagCompound tag)
        {
            Main.item = new Item[Main.itemTexture.Length];
            base.Load(tag);
        }

        public void setWorldPath()
        {

            if (Dimlibs.Dimlibs.getPlayerDim() == "solar")
            {
                Main.WorldPath = Main.SavePath + "/World/solar";

            }
            else if (Dimlibs.Dimlibs.getPlayerDim() == "overworld")
            {
                Main.WorldPath = Main.SavePath + "/World";
            }
        }


        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (arenaActive)
            {

                arenaActive = false;
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " think he was more powerful than the god of destruction... really?");
            }
            setWorldPath();
            return true;
        }

        public override void UpdateBiomeVisuals()
        {
            if (Dimlibs.Dimlibs.getPlayerDim() != null) {
                bool inSolar = Dimlibs.Dimlibs.getPlayerDim() == "Solar";
                player.ManageSpecialBiomeVisuals("TerrariaUltraApocalypse:TUAPlayer", false, player.Center);
                bool inStardust = Dimlibs.Dimlibs.getPlayerDim() == "Stardust";
                player.ManageSpecialBiomeVisuals("TerrariaUltraApocalypse:StardustPillar", inStardust, player.Center);
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
            //player.respawnTimer = 1;
        }

        
    }
}
