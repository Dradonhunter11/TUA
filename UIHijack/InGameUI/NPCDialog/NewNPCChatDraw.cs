using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.UIHijack.InGameUI.NPCDialog
{
    internal static class NewNPCChatDraw
    {
        public static void GUIChatDraw()
        {
            if (Main.ignoreErrors)
            {
                try
                {
                    if ((Main.npcChatText != "" || Main.player[Main.myPlayer].sign != -1) && !Main.editChest)
                    {
                        GUIChatDrawInner();
                    }
                    return;
                }
                catch (Exception e)
                {
                    TimeLogger.DrawException(e);
                    return;
                }
            }
            if ((Main.npcChatText != "" || Main.player[Main.myPlayer].sign != -1) && !Main.editChest)
            {
                GUIChatDrawInner();
            }
        }

        private static void GUIChatDrawInner()
        {
            MethodInfo HelpText = typeof(Main).GetMethod("HelpText", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo DrawNPCChatButtons =
                typeof(Main).GetMethod("DrawNPCChatButtons", BindingFlags.Static | BindingFlags.NonPublic);
            if (Main.player[Main.myPlayer].talkNPC < 0 && Main.player[Main.myPlayer].sign == -1)
            {
                Main.npcChatText = "";
                return;
            }
            if (Main.netMode == 0 && Main.autoPause && Main.player[Main.myPlayer].talkNPC >= 0)
            {
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 105)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(107);
                }
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 106)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(108);
                }
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 123)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(124);
                }
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 354)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(353);
                }
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 376)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(369);
                }
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 579)
                {
                    Main.npc[Main.player[Main.myPlayer].talkNPC].Transform(550);
                }
            }
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(200, 200, 200, 200);
            int num = (int)((Main.mouseTextColor * 2 + 255) / 3);
            Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(num, num, num, num);
            bool flag = Main.InGameUI.CurrentState is UIVirtualKeyboard && PlayerInput.UsingGamepad;
            int num2;
            string[] array = Utils.WordwrapString(Main.npcChatText, Main.fontMouseText, 460, 10, out num2);
            if (Main.editSign)
            {
                Main.instance.textBlinkerCount++;
                if (Main.instance.textBlinkerCount >= 20)
                {
                    if (Main.instance.textBlinkerState == 0)
                    {
                        Main.instance.textBlinkerState = 1;
                    }
                    else
                    {
                        Main.instance.textBlinkerState = 0;
                    }
                    Main.instance.textBlinkerCount = 0;
                }
                if (Main.instance.textBlinkerState == 1)
                {
                    string[] array2;
                    IntPtr intPtr;
                    (array2 = array)[(int)(intPtr = (IntPtr)num2)] = array2[(int)intPtr] + "|";
                }
                Main.instance.DrawWindowsIMEPanel(new Vector2((float)(Main.screenWidth / 2), 90f), 0.5f);
            }
            num2++;
            Main.spriteBatch.Draw(Main.chatBackTexture, new Vector2((float)(Main.screenWidth / 2 - Main.chatBackTexture.Width / 2), 100f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chatBackTexture.Width, (num2 + 1) * 30)), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.chatBackTexture, new Vector2((float)(Main.screenWidth / 2 - Main.chatBackTexture.Width / 2), (float)(100 + (num2 + 1) * 30)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, Main.chatBackTexture.Height - 30, Main.chatBackTexture.Width, 30)), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            for (int i = 0; i < num2; i++)
            {
                if (array[i] != null)
                {
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, array[i], (float)(170 + (Main.screenWidth - 800) / 2), (float)(120 + i * 30), color2, Microsoft.Xna.Framework.Color.Black, Vector2.Zero, 1f);
                }
            }
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(Main.screenWidth / 2 - Main.chatBackTexture.Width / 2, 100, Main.chatBackTexture.Width, (num2 + 2) * 30);
            int num3 = 120 + num2 * 30 + 30;
            num3 -= 235;
            if (!PlayerInput.UsingGamepad)
            {
                num3 = 9999;
            }
            UIVirtualKeyboard.OffsetDown = num3;
            if (Main.npcChatCornerItem != 0)
            {
                Vector2 vector = new Vector2((float)(Main.screenWidth / 2 + Main.chatBackTexture.Width / 2), (float)(100 + (num2 + 1) * 30 + 30));
                vector -= Vector2.One * 8f;
                Item item = new Item();
                item.netDefaults(Main.npcChatCornerItem);
                float num4 = 1f;
                Texture2D texture2D = Main.itemTexture[item.type];
                if (texture2D.Width > 32 || texture2D.Height > 32)
                {
                    if (texture2D.Width > texture2D.Height)
                    {
                        num4 = 32f / (float)texture2D.Width;
                    }
                    else
                    {
                        num4 = 32f / (float)texture2D.Height;
                    }
                }
                Main.spriteBatch.Draw(texture2D, vector, null, item.GetAlpha(Microsoft.Xna.Framework.Color.White), 0f, new Vector2((float)texture2D.Width, (float)texture2D.Height), num4, SpriteEffects.None, 0f);
                if (item.color != default(Microsoft.Xna.Framework.Color))
                {
                    Main.spriteBatch.Draw(texture2D, vector, null, item.GetColor(item.color), 0f, new Vector2((float)texture2D.Width, (float)texture2D.Height), num4, SpriteEffects.None, 0f);
                }
                if (new Microsoft.Xna.Framework.Rectangle((int)vector.X - (int)((float)texture2D.Width * num4), (int)vector.Y - (int)((float)texture2D.Height * num4), (int)((float)texture2D.Width * num4), (int)((float)texture2D.Height * num4)).Contains(new Microsoft.Xna.Framework.Point(Main.mouseX, Main.mouseY)))
                {
                    Main.instance.MouseText(item.Name, -11, 0, -1, -1, -1, -1);
                }
            }
            num = (int)Main.mouseTextColor;
            color2 = new Microsoft.Xna.Framework.Color(num, (int)((double)num / 1.1), num / 2, num);
            string focusText = "";
            string focusText2 = "";
            int num5 = Main.player[Main.myPlayer].statLifeMax2 - Main.player[Main.myPlayer].statLife;
            for (int j = 0; j < 22; j++)
            {
                int num6 = Main.player[Main.myPlayer].buffType[j];
                if (Main.debuff[num6] && Main.player[Main.myPlayer].buffTime[j] > 5 && BuffLoader.CanBeCleared(num6))
                {
                    num5 += 1000;
                }
            }
            if (Main.player[Main.myPlayer].sign > -1)
            {
                if (Main.editSign)
                {
                    focusText = Lang.inter[47].Value;
                }
                else
                {
                    focusText = Lang.inter[48].Value;
                }
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 20)
            {
                focusText = Lang.inter[28].Value;
                focusText2 = Lang.inter[49].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 207)
            {
                focusText = Lang.inter[28].Value;
                focusText2 = Lang.inter[107].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 453)
            {
                focusText = Lang.inter[28].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 550)
            {
                focusText = Lang.inter[28].Value;
                focusText2 = Language.GetTextValue("UI.BartenderHelp");
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 353)
            {
                focusText = Lang.inter[28].Value;
                focusText2 = Language.GetTextValue("GameUI.HairStyle");
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 368)
            {
                focusText = Lang.inter[28].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 369)
            {
                focusText = Lang.inter[64].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 17 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 19 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 38 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 54 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 107 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 108 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 124 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 142 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 160 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 178 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 207 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 208 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 209 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 227 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 228 || Main.npc[Main.player[Main.myPlayer].talkNPC].type == 229)
            {
                focusText = Lang.inter[28].Value;
                if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 107)
                {
                    focusText2 = Lang.inter[19].Value;
                }
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 37)
            {
                if (!Main.dayTime)
                {
                    focusText = Lang.inter[50].Value;
                }
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 22)
            {
                focusText = Lang.inter[51].Value;
                focusText2 = Lang.inter[25].Value;
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 441)
            {
                if (Main.player[Main.myPlayer].taxMoney <= 0)
                {
                    focusText = Lang.inter[89].Value;
                }
                else
                {
                    string text = "";
                    int num7 = 0;
                    int num8 = 0;
                    int num9 = 0;
                    int num10 = 0;
                    int num11 = Main.player[Main.myPlayer].taxMoney;
                    if (num11 < 0)
                    {
                        num11 = 0;
                    }
                    num5 = num11;
                    if (num11 >= 1000000)
                    {
                        num7 = num11 / 1000000;
                        num11 -= num7 * 1000000;
                    }
                    if (num11 >= 10000)
                    {
                        num8 = num11 / 10000;
                        num11 -= num8 * 10000;
                    }
                    if (num11 >= 100)
                    {
                        num9 = num11 / 100;
                        num11 -= num9 * 100;
                    }
                    if (num11 >= 1)
                    {
                        num10 = num11;
                    }
                    if (num7 > 0)
                    {
                        object obj = text;
                        text = string.Concat(new object[]
                            {
                                obj,
                                num7,
                                " ",
                                Lang.inter[15].Value,
                                " "
                            });
                    }
                    if (num8 > 0)
                    {
                        object obj2 = text;
                        text = string.Concat(new object[]
                            {
                                obj2,
                                num8,
                                " ",
                                Lang.inter[16].Value,
                                " "
                            });
                    }
                    if (num9 > 0)
                    {
                        object obj = text;
                        text = string.Concat(new object[]
                            {
                                obj,
                                num9,
                                " ",
                                Lang.inter[17].Value,
                                " "
                            });
                    }
                    if (num10 > 0)
                    {
                        object obj = text;
                        text = string.Concat(new object[]
                            {
                                obj,
                                num10,
                                " ",
                                Lang.inter[18].Value,
                                " "
                            });
                    }
                    float num12 = (float)Main.mouseTextColor / 255f;
                    if (num7 > 0)
                    {
                        color2 = new Microsoft.Xna.Framework.Color((int)((byte)(220f * num12)), (int)((byte)(220f * num12)), (int)((byte)(198f * num12)), (int)Main.mouseTextColor);
                    }
                    else if (num8 > 0)
                    {
                        color2 = new Microsoft.Xna.Framework.Color((int)((byte)(224f * num12)), (int)((byte)(201f * num12)), (int)((byte)(92f * num12)), (int)Main.mouseTextColor);
                    }
                    else if (num9 > 0)
                    {
                        color2 = new Microsoft.Xna.Framework.Color((int)((byte)(181f * num12)), (int)((byte)(192f * num12)), (int)((byte)(193f * num12)), (int)Main.mouseTextColor);
                    }
                    else if (num10 > 0)
                    {
                        color2 = new Microsoft.Xna.Framework.Color((int)((byte)(246f * num12)), (int)((byte)(138f * num12)), (int)((byte)(96f * num12)), (int)Main.mouseTextColor);
                    }
                    if (text == "")
                    {
                        focusText = Lang.inter[89].Value;
                    }
                    else
                    {
                        text = text.Substring(0, text.Length - 1);
                        focusText = Lang.inter[89].Value + " (" + text + ")";
                    }
                }
            }
            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 18)
            {
                string text2 = "";
                int num13 = 0;
                int num14 = 0;
                int num15 = 0;
                int num16 = 0;
                int num17 = num5;
                if (num17 > 0)
                {
                    num17 = (int)((double)num17 * 0.75);
                    if (num17 < 1)
                    {
                        num17 = 1;
                    }
                }
                if (num17 < 0)
                {
                    num17 = 0;
                }
                num5 = num17;
                if (num17 >= 1000000)
                {
                    num13 = num17 / 1000000;
                    num17 -= num13 * 1000000;
                }
                if (num17 >= 10000)
                {
                    num14 = num17 / 10000;
                    num17 -= num14 * 10000;
                }
                if (num17 >= 100)
                {
                    num15 = num17 / 100;
                    num17 -= num15 * 100;
                }
                if (num17 >= 1)
                {
                    num16 = num17;
                }
                if (num13 > 0)
                {
                    object obj = text2;
                    text2 = string.Concat(new object[]
                        {
                            obj,
                            num13,
                            " ",
                            Lang.inter[15].Value,
                            " "
                        });
                }
                if (num14 > 0)
                {
                    object obj = text2;
                    text2 = string.Concat(new object[]
                        {
                            obj,
                            num14,
                            " ",
                            Lang.inter[16].Value,
                            " "
                        });
                }
                if (num15 > 0)
                {
                    object obj = text2;
                    text2 = string.Concat(new object[]
                        {
                            obj,
                            num15,
                            " ",
                            Lang.inter[17].Value,
                            " "
                        });
                }
                if (num16 > 0)
                {
                    object obj = text2;
                    text2 = string.Concat(new object[]
                        {
                            obj,
                            num16,
                            " ",
                            Lang.inter[18].Value,
                            " "
                        });
                }
                float num18 = (float)Main.mouseTextColor / 255f;
                if (num13 > 0)
                {
                    color2 = new Microsoft.Xna.Framework.Color((int)((byte)(220f * num18)), (int)((byte)(220f * num18)), (int)((byte)(198f * num18)), (int)Main.mouseTextColor);
                }
                else if (num14 > 0)
                {
                    color2 = new Microsoft.Xna.Framework.Color((int)((byte)(224f * num18)), (int)((byte)(201f * num18)), (int)((byte)(92f * num18)), (int)Main.mouseTextColor);
                }
                else if (num15 > 0)
                {
                    color2 = new Microsoft.Xna.Framework.Color((int)((byte)(181f * num18)), (int)((byte)(192f * num18)), (int)((byte)(193f * num18)), (int)Main.mouseTextColor);
                }
                else if (num16 > 0)
                {
                    color2 = new Microsoft.Xna.Framework.Color((int)((byte)(246f * num18)), (int)((byte)(138f * num18)), (int)((byte)(96f * num18)), (int)Main.mouseTextColor);
                }
                if (text2 == "")
                {
                    focusText = Lang.inter[54].Value;
                }
                else
                {
                    text2 = text2.Substring(0, text2.Length - 1);
                    focusText = Lang.inter[54].Value + " (" + text2 + ")";
                }
            }
            NPCLoader.SetChatButtons(ref focusText, ref focusText2);
            TUANPCLoader.ModifyNPCDialogButton(Main.npc[Main.player[Main.myPlayer].talkNPC], ref focusText, ref focusText2);
            if (!flag)
            {
                DrawNPCChatButtons.Invoke(null, new object[] {num, color2, num2, focusText, focusText2});
            }
            if (!PlayerInput.IgnoreMouseInterface)
            {
                if (rectangle.Contains(new Microsoft.Xna.Framework.Point(Main.mouseX, Main.mouseY)))
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                }
                if (Main.mouseLeft && Main.mouseLeftRelease && rectangle.Contains(new Microsoft.Xna.Framework.Point(Main.mouseX, Main.mouseY)))
                {
                    Main.mouseLeftRelease = false;
                    Main.player[Main.myPlayer].releaseUseItem = false;
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (Main.npcChatFocus1)
                    {
                        Main.CloseNPCChatOrSign();
                        return;
                    }
                    if (Main.npcChatFocus2)
                    {
                        if (Main.player[Main.myPlayer].sign != -1)
                        {
                            if (Main.editSign)
                            {
                                Main.SubmitSignText();
                                return;
                            }
                            IngameFancyUI.OpenVirtualKeyboard(1);
                            return;
                        }
                        if (!NPCLoader.PreChatButtonClicked(true))
                            return;
                        NPCLoader.OnChatButtonClicked(true);
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 369)
                        {
                            Main.npcChatCornerItem = 0;
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            bool flag2 = false;
                            if (!Main.anglerQuestFinished && !Main.anglerWhoFinishedToday.Contains(Main.player[Main.myPlayer].name))
                            {
                                int num19 = Main.player[Main.myPlayer].FindItem(Main.anglerQuestItemNetIDs[Main.anglerQuest]);
                                if (num19 != -1)
                                {
                                    Main.player[Main.myPlayer].inventory[num19].stack--;
                                    if (Main.player[Main.myPlayer].inventory[num19].stack <= 0)
                                    {
                                        Main.player[Main.myPlayer].inventory[num19] = new Item();
                                    }
                                    flag2 = true;
                                    Main.PlaySound(24, -1, -1, 1, 1f, 0f);
                                    Main.player[Main.myPlayer].anglerQuestsFinished++;
                                    Main.player[Main.myPlayer].GetAnglerReward();
                                }
                            }
                            Main.npcChatText = Lang.AnglerQuestChat(flag2);
                            if (flag2)
                            {
                                Main.anglerQuestFinished = true;
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(75, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                                }
                                else
                                {
                                    Main.anglerWhoFinishedToday.Add(Main.player[Main.myPlayer].name);
                                }
                                AchievementsHelper.HandleAnglerService();
                                return;
                            }
                        }
                        else
                        {
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 17)
                            {
                                
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 1;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 19)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 2;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 124)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 8;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 142)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 9;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 353)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 18;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 368)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 19;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 453)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 20;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 37)
                            {
                                if (Main.netMode == 0)
                                {
                                    NPC.SpawnSkeletron();
                                }
                                else
                                {
                                    NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f, 0f, 0f, 0, 0, 0);
                                }
                                Main.npcChatText = "";
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 20)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 3;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 38)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 4;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 54)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 5;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 107)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 6;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 108)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 7;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 160)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 10;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 178)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 11;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 207)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 12;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 208)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 13;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 209)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 14;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 227)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 15;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 228)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 16;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 229)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 17;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 22 && NPC.downedBoss3)
                            {
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                HelpText.Invoke(null, new object[]{});
                                return;
                            }
                            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 441)
                            {
                                if (Main.player[Main.myPlayer].taxMoney > 0)
                                {
                                    int k = Main.player[Main.myPlayer].taxMoney;
                                    while (k > 0)
                                    {
                                        if (k > 1000000)
                                        {
                                            int num20 = k / 1000000;
                                            k -= 1000000 * num20;
                                            int number = Item.NewItem((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, 74, num20, false, 0, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                                            }
                                        }
                                        else if (k > 10000)
                                        {
                                            int num21 = k / 10000;
                                            k -= 10000 * num21;
                                            int number2 = Item.NewItem((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, 73, num21, false, 0, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(21, -1, -1, null, number2, 1f, 0f, 0f, 0, 0, 0);
                                            }
                                        }
                                        else if (k > 100)
                                        {
                                            int num22 = k / 100;
                                            k -= 100 * num22;
                                            int number3 = Item.NewItem((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, 72, num22, false, 0, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(21, -1, -1, null, number3, 1f, 0f, 0f, 0, 0, 0);
                                            }
                                        }
                                        else
                                        {
                                            int num23 = k;
                                            if (num23 < 1)
                                            {
                                                num23 = 1;
                                            }
                                            k -= num23;
                                            int number4 = Item.NewItem((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, 71, num23, false, 0, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(21, -1, -1, null, number4, 1f, 0f, 0f, 0, 0, 0);
                                            }
                                        }
                                    }
                                    Main.npcChatText = Lang.dialog(Main.rand.Next(380, 382), false);
                                    Main.player[Main.myPlayer].taxMoney = 0;
                                    return;
                                }
                                Main.npcChatText = Lang.dialog(Main.rand.Next(390, 401), false);
                                return;
                            }
                            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 18)
                            {
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                if (num5 > 0)
                                {
                                    if (Main.player[Main.myPlayer].BuyItem(num5, -1))
                                    {
                                        AchievementsHelper.HandleNurseService(num5);
                                        Main.PlaySound(SoundID.Item4, -1, -1);
                                        Main.player[Main.myPlayer].HealEffect(Main.player[Main.myPlayer].statLifeMax2 - Main.player[Main.myPlayer].statLife, true);
                                        if ((double)Main.player[Main.myPlayer].statLife < (double)Main.player[Main.myPlayer].statLifeMax2 * 0.25)
                                        {
                                            Main.npcChatText = Lang.dialog(227, false);
                                        }
                                        else if ((double)Main.player[Main.myPlayer].statLife < (double)Main.player[Main.myPlayer].statLifeMax2 * 0.5)
                                        {
                                            Main.npcChatText = Lang.dialog(228, false);
                                        }
                                        else if ((double)Main.player[Main.myPlayer].statLife < (double)Main.player[Main.myPlayer].statLifeMax2 * 0.75)
                                        {
                                            Main.npcChatText = Lang.dialog(229, false);
                                        }
                                        else
                                        {
                                            Main.npcChatText = Lang.dialog(230, false);
                                        }
                                        Main.player[Main.myPlayer].statLife = Main.player[Main.myPlayer].statLifeMax2;
                                        for (int l = 0; l < 22; l++)
                                        {
                                            int num24 = Main.player[Main.myPlayer].buffType[l];
                                            if (Main.debuff[num24] && Main.player[Main.myPlayer].buffTime[l] > 0 && BuffLoader.CanBeCleared(num24))
                                            {
                                                Main.player[Main.myPlayer].DelBuff(l);
                                                l = -1;
                                            }
                                        }
                                        return;
                                    }
                                    int num25 = Main.rand.Next(3);
                                    if (num25 == 0)
                                    {
                                        Main.npcChatText = Lang.dialog(52, false);
                                    }
                                    if (num25 == 1)
                                    {
                                        Main.npcChatText = Lang.dialog(53, false);
                                    }
                                    if (num25 == 2)
                                    {
                                        Main.npcChatText = Lang.dialog(54, false);
                                        return;
                                    }
                                }
                                else
                                {
                                    int num26 = Main.rand.Next(3);
                                    if (!ChildSafety.Disabled)
                                    {
                                        num26 = Main.rand.Next(1, 3);
                                    }
                                    if (num26 == 0)
                                    {
                                        Main.npcChatText = Lang.dialog(55, false);
                                        return;
                                    }
                                    if (num26 == 1)
                                    {
                                        Main.npcChatText = Lang.dialog(56, false);
                                        return;
                                    }
                                    if (num26 == 2)
                                    {
                                        Main.npcChatText = Lang.dialog(57, false);
                                        return;
                                    }
                                }
                            }
                            else if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 550)
                            {
                                Main.playerInventory = true;
                                Main.npcChatText = "";
                                Main.npcShop = 21;
                                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                return;
                            }
                        }
                    }
                    else if (Main.npcChatFocus3 && Main.player[Main.myPlayer].talkNPC >= 0)
                    {
                        if (!NPCLoader.PreChatButtonClicked(false))
                            return;
                        NPCLoader.OnChatButtonClicked(false);
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 20)
                        {
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            Main.npcChatText = Lang.GetDryadWorldStatusDialog();
                            return;
                        }
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 22)
                        {
                            Main.playerInventory = true;
                            Main.npcChatText = "";
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            Main.InGuideCraftMenu = true;
                            UILinkPointNavigator.GoToDefaultPage(0);
                            return;
                        }
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 107)
                        {
                            Main.playerInventory = true;
                            Main.npcChatText = "";
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            Main.InReforgeMenu = true;
                            UILinkPointNavigator.GoToDefaultPage(0);
                            return;
                        }
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 353)
                        {
                            Main.OpenHairWindow();
                            return;
                        }
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 207)
                        {
                            Main.npcChatCornerItem = 0;
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            bool gotDye = false;
                            int num27 = Main.player[Main.myPlayer].FindItem(ItemID.Sets.ExoticPlantsForDyeTrade);
                            if (num27 != -1)
                            {
                                Main.player[Main.myPlayer].inventory[num27].stack--;
                                if (Main.player[Main.myPlayer].inventory[num27].stack <= 0)
                                {
                                    Main.player[Main.myPlayer].inventory[num27] = new Item();
                                }
                                gotDye = true;
                                Main.PlaySound(24, -1, -1, 1, 1f, 0f);
                                Main.player[Main.myPlayer].GetDyeTraderReward();
                            }
                            Main.npcChatText = Lang.DyeTraderQuestChat(gotDye);
                            return;
                        }
                        if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 550)
                        {
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            HelpText.Invoke(null, new object[] { });
                            Main.npcChatText = Lang.BartenderHelpText(Main.npc[Main.player[Main.myPlayer].talkNPC]);
                        }
                    }
                }
            }
        }


    }
}
