using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Random = System.Random;

namespace TUA.Void.VoidGlobal
{
    class VoidGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                BaseUtility.Chat("You feel that the darkness is taking more space...", new Color(0, 0, 0), true);
                VoidPlayer player;
                if (Main.netMode == 0)
                {
                    player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
                    player.voidTier2Unlocked = true;
                }
                else if (Main.netMode == 2)
                {
                    foreach (Player p in Main.player)
                    {
                        if (p == null)
                        {
                            continue;
                        }

                        player = p.GetModPlayer<VoidPlayer>();
                        player.voidTier2Unlocked = true;
                    }
                }
            }


            if (npc.boss && Main.rand.Next(5) == 0)
            {
                BaseUtility.Chat("You are assimilating the darkness...", new Color(0, 0, 0), true);
                VoidPlayer player;
                if (Main.netMode == 0)
                {

                    player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
                    player.AddVoidAffinity(5);
                }
                else if(Main.netMode == 2)
                {
                    foreach (Player p in Main.player)
                    {
                        if (p == null)
                        {
                            continue;
                        }

                        player = p.GetModPlayer<VoidPlayer>();
                        player.AddVoidAffinity(5);
                    }
                }
            }
        }
    }
}
