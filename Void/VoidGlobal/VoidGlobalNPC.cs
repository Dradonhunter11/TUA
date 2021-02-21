using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TUA.Void.VoidGlobal
{
    internal class VoidGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {

                VoidPlayer player;
                if (Main.netMode == 0)
                {
                    player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
                    if (!player.voidTier2Unlocked)
                    {
                        TUA.BroadcastMessage("You feel that the darkness is taking more space...", new Color(0, 0, 0));
                        player.voidTier2Unlocked = true;
                    }
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
                        if (!player.voidTier2Unlocked)
                        {
                            TUA.BroadcastMessage("You feel that the darkness is taking more space...", new Color(0, 0, 0));
                            player.voidTier2Unlocked = true;
                        }
                    }
                }
            }
            else if (npc.type == NPCID.MoonLordCore)
            {

                VoidPlayer player;
                if (Main.netMode == 0)
                {
                    player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
                    if (player.voidTier2Unlocked && !player.voidTier3Unlocked)
                    {
                        TUA.BroadcastMessage("The void inside you is getting bigger...", new Color(0, 0, 0));
                        player.voidTier3Unlocked = true;
                    }
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
                        if (player.voidTier2Unlocked && !player.voidTier3Unlocked)
                        {
                            TUA.BroadcastMessage("The void inside you is getting bigger...", new Color(0, 0, 0));
                            player.voidTier3Unlocked = true;
                        }
                    }
                }
            }


            if (npc.boss && Main.rand.Next(5) == 0)
            {
                TUA.BroadcastMessage("You are assimilating the darkness...", new Color(0, 0, 0));
                VoidPlayer player;
                if (Main.netMode == 0)
                {

                    player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
                    player.AddVoidAffinity(5);
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
                        player.AddVoidAffinity(5);
                    }
                }
            }
        }
    }
}
