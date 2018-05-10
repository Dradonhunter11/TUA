using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary;
using Terraria.ModLoader;
using Terraria;
using TerrariaUltraApocalypse.Biomes;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrariaUltraApocalypse
{
    class TUAPlayer : ModPlayer
    {
        public static bool meteoridonZone = false;
        public static bool blueSoul = false;
        public static Vector2 arenaCenter;

        public static bool arenaActive = false;

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
            if (arenaActive) {
                
                arenaActive = false;
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " think he was more powerful than the god of destruction... really?");
            }
            
            return true;
        }

        public override void UpdateDead()
        {
            player.respawnTimer = 1;
        }

        
    }
}
