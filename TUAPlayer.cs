using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
using System.Reflection;
using System.Reflection.Emit;
using Terraria.ID;
using Dimlibs;

namespace TerrariaUltraApocalypse
{
    class TUAPlayer : ModPlayer
    {
        public static bool meteoridonZone = false;
        public static bool blueSoul = false;
        public static Vector2 arenaCenter;
        public static bool arenaActive = false;

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

        public override void SetupStartInventory(IList<Item> items)
        {
            if (this.Name == "Dradon")
            {
                Item item = new Item();
                item.SetDefaults(3543);
                item.stack = 1;
                items.Add(item);
            }

            base.SetupStartInventory(items);
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
                bool inSolar = Dimlibs.Dimlibs.getPlayerDim() == "solar";
                player.ManageSpecialBiomeVisuals("TerrariaUltraApocalypse:TUAPlayer", inSolar, player.Center);
            }
            
        }

        public override void UpdateDead()
        {
            player.respawnTimer = 1;
        }


    }
}
