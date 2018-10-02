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
using Terraria.GameInput;

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

        public override void SetupStartInventory(IList<Item> items)
        {
            if (this.Name == "Dradon")
            {
                Item item = new Item();
                item.SetDefaults(3543);
                item.stack = 1;
                items.Add(item);
            }

            

            for (int i = 0; i < Main.itemTexture.Length; i++) {
                Item item = new Item();
                item.SetDefaults(i);
                item.stack = 1000;
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
                bool inSolar = Dimlibs.Dimlibs.getPlayerDim() == "Solar";
                player.ManageSpecialBiomeVisuals("TerrariaUltraApocalypse:TUAPlayer", inSolar, player.Center);
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
