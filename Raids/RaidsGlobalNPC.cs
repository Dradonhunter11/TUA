using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Gamepad;
using TUA.Utilities;

namespace TUA.Raids
{

    internal class RaidsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool giveRaidsDialog = false;
        private byte currentGuideText;
        /// <summary>
        /// 0, 1, 2 is "Help", "Crafting", "Raids"
        /// respectively
        /// </summary>
        private static byte guideFocusText;

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.Guide)
            {
                currentGuideText = 0;
                guideFocusText = 0;
            }
        }

        public delegate void SetChatButtonsCustomDelegate(ref string focusText, ref string focusText2);
        public static void SetChatButtonsCustom(ref string focusText, ref string focusText2)
        {
            // NPCLoader.SetChatButtons(ref focusText, ref focusText2);
            var npc = Main.npc[Main.LocalPlayer.talkNPC];
            if (npc.type == NPCID.Guide)
            {
                if (!RaidsWorld.hasTalkedToGuide.Contains(Main.player[Main.myPlayer].GetModPlayer<TUAPlayer>().ID))
                {
                    focusText = "Next";
                    focusText2 = "";
                }

                else
                {
                    switch (guideFocusText)
                    {
                        case 2:
                            focusText = "Raids";
                            if (RaidsWorld.currentRaid != RaidsID.None)
                            {
                                byte raids = RaidsWorld.currentRaid;
                                if (raids == RaidsID.TheGreatHellRide)
                                {
                                    focusText = "The Great Hell Ride";
                                }

                                else if (raids == RaidsID.TheWrathOfTheWasteland)
                                {
                                    focusText = "The Wrath of the Wasteland";
                                }
                            }
                            break;
                        case 1:
                            focusText = "Crafting";
                            break;
                        default:
                            focusText = "Help";
                            break;
                    }
                    focusText2 = "Other";
                }
            }

            else if (npc.type == NPCID.Cyborg)
            {
                // focusText2 = "Upgrade weapon";
            }
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.Guide)
            {
                if (firstButton)
                {
                    if (!RaidsWorld.hasTalkedToGuide.Contains(Main.player[Main.myPlayer].GetModPlayer<TUAPlayer>().ID))
                    {
                        Main.npcChatText = GetGuideStartText();
                        currentGuideText++;
                    }
                    else
                    {
                        switch (guideFocusText)
                        {
                            case 2:
                                TUA.raidsInterface.SetState(new UI.RaidsUI());
                                TUA.raidsInterface.IsVisible = !TUA.raidsInterface.IsVisible;
                                giveRaidsDialog = true;
                                break;
                            case 1:
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                Main.InGuideCraftMenu = true;
                                UILinkPointNavigator.GoToDefaultPage(0);
                                break;
                            default:
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                // If someone can find a better way to do this, do it immediately
                                StaticManager<Type>.GetItem("TMain").GetMethod("HelpText", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                                break;
                        }
                    }
                }

                else
                {
                    if (guideFocusText == 2) guideFocusText = 0;
                    else guideFocusText++;
                }
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Guide)
            {
                /*if (!NPC.downedBoss3)
                {
                    chat = "Come back when you'll have rescued the cursed man";
                    return;
                }*/
                if (!RaidsWorld.hasTalkedToGuide.Contains(Main.player[Main.myPlayer].GetModPlayer<TUAPlayer>().ID))
                {
                    chat = GetGuideStartText();
                }
                else if (Main.ActiveWorldFileData.HasCorruption)
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
                        chat = "I heard that things have been happening in the wasteland, the core is apparently not happy and is menacing to destroy the world.\nYour goal is to calm down the heart of the wasteland but you'll need some stuff first.";
                    }
                }
            }
        }

        private string GetGuideStartText()
        {
            switch (currentGuideText)
            {
                case 0:
                    return "Thousands of years ago, there were 2 races, the gods and the humans.";
                case 1:
                    return "The gods freely terrorized the humans as much as they wanted, with many gods implementing weapons of destruction, torturing, and even using humans as guinea pigs in vile experiments.";
                case 2:
                    return "The humans had technology and even their own magic, but it wasn't enough.";
                case 3:
                    return "But one day, a human had enough of it and went to the search of a way to seal the gods.";
                case 4:
                    return "He gathered about him a group friends, enemies, comrades in arms, all hell-bent on destroying the gods once and for all.";
                case 5:
                    return "Through the power of their will, the group traveled between dimensions, explored multiple worlds, and braved the worst biomes, until they finally defeated the highest of gods and sealed him.";
                case 6:
                    return "Without the power of their fearless leader, the demonic deities fled the land, and humans rejoiced.";
                case 7:
                    return "Thanks to the ravages of time, the names of these heroes has been forgetten, and even the name of their order has been lost.";
                case 8:
                    return "But my father, his father before him, and all the fathers in the sky above knew, that one day, the descendant of the mighty hero would one day come in a dark hour to defeat the gods again.";
                case 9:
                    RaidsWorld.hasTalkedToGuide.Add(Main.player[Main.myPlayer].GetModPlayer<TUAPlayer>().ID);
                    return "I believe that that descendant stands before me now. " +
                        $"{(Main.rand.NextBool() ? "" : "YES YOU IDIOT WHO ELSE.")}";
                default:
                    return "Damn, looks like something went wrong. Report this to the Terraria Ultra Apocalypse developers.";
            }
        }
    }
}
