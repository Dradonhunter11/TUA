using MonoMod.RuntimeDetour.HookGen;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Raids
{

    internal class RaidsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool giveRaidsDialog = false;

        public delegate void SetChatButtonsReplacementDelegate(ref string focusText, ref string focusText2);
        public static void SetChatButtonsReplacement(ref string focusText, ref string focusText2)
        {
            NPCLoader.SetChatButtons(ref focusText, ref focusText2);
            var npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (npc.type == NPCID.Guide)
            {
                focusText = "Raids";
                if (TerrariaUltraApocalypse.instance.GetModWorld<RaidsWorld>().currentRaids != RaidsType.noActiveRaids)
                {
                    RaidsType raids = TerrariaUltraApocalypse.instance.GetModWorld<RaidsWorld>().currentRaids;
                    if (raids == RaidsType.theGreatHellRide)
                    {
                        focusText = "The Great Hell Ride";
                    }

                    if (raids == RaidsType.theWrathOfTheWasteland)
                    {
                        focusText = "The Wrath of the Wasteland";
                    }
                }
            }

            if (npc.type == NPCID.Cyborg)
            {
                focusText2 = "Upgrade weapon";
            }
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (firstButton && npc.type == NPCID.Guide)
            {
                TerrariaUltraApocalypse.raidsInterface.SetState(new UI.RaidsUI());
                TerrariaUltraApocalypse.raidsInterface.IsVisible = !TerrariaUltraApocalypse.raidsInterface.IsVisible;
                /*if (Main.ActiveWorldFileData.HasCorruption)
                {
                    if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        
                        Main.npcChatText = "Come back when you'll have my doll, I mean the sacred doll!";
                    }
                    else
                    {
                        mod.GetModWorld<RaidsWorld>().currentRaids = RaidsType.theGreatHellRide;
                        Main.NewText(Main.LocalPlayer.name + " has started the great hell ride raids!", new Microsoft.Xna.Framework.Color(186, 85, 211));
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
                        mod.GetModWorld<RaidsWorld>().currentRaids = RaidsType.theWrathOfTheWasteland;
                        Main.NewText(Main.LocalPlayer.name + " has started the wrath of the wasteland raids!");
                        Main.npcChatText = "I heard thing been happening in the wasteland, the core is apparently not happy and is menacing to destroy the world.\nYour goal is to calm down the heart of the wasteland but you'll need some stuff first.";
                    }
                }*/

                giveRaidsDialog = true;
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Guide)
            {
                chat = "";

                /*if (!NPC.downedBoss3)
                {
                    chat = "Come back when you'll have rescued the cursed man";
                    return;
                }*/
                if (Main.ActiveWorldFileData.HasCorruption)
                {
                    if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        chat = "Come back when you'll have my doll, I mean the sacred doll!";
                    }
                    else
                    {
                        chat = "Hello, are you ready for a great hell ride? It's for sure gonna be fun! \nIf you see the great wall, tell me, I never seen it since I explode everytime someone summons it.";
                    }
                }
                else
                {
                    if (!Main.LocalPlayer.inventory.Any(i => i.type == mod.ItemType("GuideVoodooDoll")))
                    {
                        chat = "Come back when you have my doll, and I mean the sacred doll!";
                    }
                    else
                    {
                        chat = "I heard things have been happening in the wasteland, the core is apparently not happy and is menacing to destroy the world.\nYour goal is to calm down the heart of the wasteland but you'll need some stuff first.";
                    }
                }
            }
        }
    }
}
