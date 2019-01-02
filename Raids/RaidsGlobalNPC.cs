using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Raids
{
    internal class RaidsTUAGlobalNPC : TUAGlobalNPC
    {
        public override void modifyNPCButtonChat(NPC npc, ref string button, ref string button2)
        {
            if (npc.type == NPCID.Guide)
            {
                button = "Raids";
            }
        }
    }

    internal class RaidsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool giveRaidsDialog = false;

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (firstButton && npc.type == NPCID.Guide)
            {

                if (!NPC.downedBoss3)
                {
                    Main.npcChatText = "Come back when you'll have defeated the cursed man";
                    return;
                }
                if (Main.ActiveWorldFileData.HasCorruption)
                {
                    if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        Main.npcChatText = "Come back when you'll have my doll, I mean the sacred doll!";
                    }
                    else
                    {
                        Main.npcChatText = "Hello, are you ready for a great hell ride? It's for sure gonna be fun! \nIf you see the great wall, tell me, I never seen it since I explode everytime someone summon it.";
                    }
                }
                else
                {
                    if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        Main.npcChatText = "Come back when you'll have my doll, I mean the sacred doll!";
                    }
                    else
                    {
                        Main.npcChatText = "I heard thing been happening in the wasteland, the core is apparently not happy and is menacing to destroy the world.\nYour goal is to calm down the heart of the wasteland but you'll need some stuff first.";
                    }
                }

                giveRaidsDialog = true;
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (giveRaidsDialog && npc.type == NPCID.Guide)
            {
                chat = "";
                if (Main.ActiveWorldFileData.HasCorruption)
                {
                    if (!NPC.downedBoss3)
                    {
                        chat = "Come back when you'll have defeated the cursed man";
                    }
                    else if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        chat = "Come back when you'll have my doll, I mean the sacred doll!";
                    }
                    else
                    {
                        chat = "Hello, are you ready for a great hell ride? It's for sure gonna be fun! \nIf you see the great wall, tell me, I never seen it since I explode everytime someone summon it.";
                    }
                }
            }
        }
    }
}
