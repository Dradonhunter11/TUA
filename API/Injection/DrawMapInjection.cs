using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dimlibs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using TUA.Utilities;

namespace TUA.API.Injection
{
    class DrawMapInjection
    {
        private static MethodInfo originalMethod;

        private static MethodInfo PostDrawFullscreenMap;

        public static void inject()
        {
            MethodInfo OriginalDrawMap =
                ReflManager<Type>.GetItem("TMain").GetMethod("DrawMap", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo NewDrawMap =
                typeof(DrawMapInjection).GetMethod("DrawMap", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            originalMethod = OriginalDrawMap;

            RuntimeHelpers.PrepareMethod(OriginalDrawMap.MethodHandle);
            RuntimeHelpers.PrepareMethod(NewDrawMap.MethodHandle);

            PostDrawFullscreenMap = ReflManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.ModHooks").GetMethod("PostDrawFullscreenMap", BindingFlags.Static | BindingFlags.NonPublic);
            

            IntPtr ptr = OriginalDrawMap.MethodHandle.Value + IntPtr.Size * 2;
            IntPtr ptr2 = NewDrawMap.MethodHandle.Value + IntPtr.Size * 2;
            int value = ptr.ToInt32();
            Marshal.WriteInt32(ptr, Marshal.ReadInt32(ptr2));
            Marshal.WriteInt32(ptr2, Marshal.ReadInt32(new IntPtr(value)));
        }

        public static void Revert()
        {

            MethodInfo NewDrawMap =
                typeof(DrawMapInjection).GetMethod("DrawMap", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            RuntimeHelpers.PrepareMethod(originalMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(NewDrawMap.MethodHandle);

            IntPtr ptr = originalMethod.MethodHandle.Value + IntPtr.Size * 2;
            IntPtr ptr2 = NewDrawMap.MethodHandle.Value + IntPtr.Size * 2;
            int value = ptr.ToInt32();
            Marshal.WriteInt32(ptr, Marshal.ReadInt32(ptr2));
            Marshal.WriteInt32(ptr2, Marshal.ReadInt32(new IntPtr(value)));
        }


        protected void DrawMap()
        {
            MethodInfo DrawPlayerHead = ReflManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.Main")
        .GetMethod("DrawPlayerHead", BindingFlags.Instance | BindingFlags.NonPublic);

            MethodInfo DrawMap_FindChestName = ReflManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.Main")
                .GetMethod("DrawMap_FindChestName", BindingFlags.Instance | BindingFlags.NonPublic);


            FieldInfo mapDeathTextureInfo =
                ReflManager<Type>.GetItem("TMain").GetField("mapDeathTexture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapDeathTexture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG1TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG1Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG1Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG2TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG2Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG2Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG3TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG3Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG3Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG4TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG4Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG4Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG5TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG5Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG5Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG6TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG6Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG6Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG7TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG7Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG7Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG8TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG8Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG8Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG9TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG9Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG9Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG10TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG10Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG10Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG11TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG11Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG11Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG12TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG12Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG12Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG13TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG13Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG13Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG14TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG14Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG14Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            FieldInfo mapBG15TextureInfo = ReflManager<Type>.GetItem("TMain").GetField("mapBG15Texture", BindingFlags.Instance | BindingFlags.NonPublic);
            Texture2D mapBG15Texture = (Texture2D)mapDeathTextureInfo.GetValue(Main.instance);

            string text = "";
            if (!Main.mapEnabled)
            {
                return;
            }
            if (!Main.mapReady)
            {
                return;
            }
            float num = 0f;
            float num2 = 0f;
            float num3 = num;
            float num4 = num2;
            byte b = 255;
            int arg_40_0 = Main.maxTilesX / Main.textureMaxWidth;
            int num5 = Main.maxTilesY / Main.textureMaxHeight;
            float num6 = (float)Lighting.offScreenTiles;
            float num7 = (float)Lighting.offScreenTiles;
            float num8 = (float)(Main.maxTilesX - Lighting.offScreenTiles - 1);
            float num9 = (float)(Main.maxTilesY - Lighting.offScreenTiles - 42);
            float num10 = 0f;
            float num11 = 0f;
            num6 = 10f;
            num7 = 10f;
            num8 = (float)(Main.maxTilesX - 10);
            num9 = (float)(Main.maxTilesY - 10);
            for (int i = 0; i < Main.instance.mapTarget.GetLength(0); i++)
            {
                for (int j = 0; j < Main.instance.mapTarget.GetLength(1); j++)
                {
                    if (Main.instance.mapTarget[i, j] != null)
                    {
                        if (Main.instance.mapTarget[i, j].IsContentLost && !Main.mapWasContentLost[i, j])
                        {
                            Main.mapWasContentLost[i, j] = true;
                            Main.refreshMap = true;
                            Main.clearMap = true;
                        }
                        else if (!Main.instance.mapTarget[i, j].IsContentLost && Main.mapWasContentLost[i, j])
                        {
                            Main.mapWasContentLost[i, j] = false;
                        }
                    }
                }
            }
            num = 200f;
            num2 = 300f;
            float num12 = 0f;
            float num13 = 0f;
            float num14 = num8 - 1f;
            float num15 = num9 - 1f;
            float num16;
            if (Main.mapFullscreen)
            {
                num16 = Main.mapFullscreenScale;
            }
            else if (Main.mapStyle == 1)
            {
                num16 = Main.mapMinimapScale;
            }
            else
            {
                num16 = Main.mapOverlayScale;
            }
            bool flag = false;
            Matrix transformMatrix = Main.UIScaleMatrix;
            if (Main.mapStyle != 1)
            {
                transformMatrix = Matrix.Identity;
            }
            if (Main.mapFullscreen)
            {
                transformMatrix = Matrix.Identity;
            }
            if (!Main.mapFullscreen && num16 > 1f)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
                flag = true;
            }
            if (Main.mapFullscreen)
            {
                if (Main.mouseLeft && Main.instance.IsActive && !CaptureManager.Instance.UsingMap)
                {
                    if (Main.mouseLeftRelease)
                    {
                        Main.grabMapX = (float)Main.mouseX;
                        Main.grabMapY = (float)Main.mouseY;
                    }
                    else
                    {
                        float num17 = (float)Main.mouseX - Main.grabMapX;
                        float num18 = (float)Main.mouseY - Main.grabMapY;
                        Main.grabMapX = (float)Main.mouseX;
                        Main.grabMapY = (float)Main.mouseY;
                        num17 *= 0.06255f;
                        num18 *= 0.06255f;
                        Main.mapFullscreenPos.X = Main.mapFullscreenPos.X - num17 * (16f / Main.mapFullscreenScale);
                        Main.mapFullscreenPos.Y = Main.mapFullscreenPos.Y - num18 * (16f / Main.mapFullscreenScale);
                    }
                }
                Main.player[Main.myPlayer].mouseInterface = true;
                float num19 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
                if (Main.mapFullscreenScale < num19)
                {
                    Main.mapFullscreenScale = num19;
                }
                if (Main.mapFullscreenScale > 16f)
                {
                    Main.mapFullscreenScale = 16f;
                }
                num16 = Main.mapFullscreenScale;
                b = 255;
                if (Main.mapFullscreenPos.X < num6)
                {
                    Main.mapFullscreenPos.X = num6;
                }
                if (Main.mapFullscreenPos.X > num8)
                {
                    Main.mapFullscreenPos.X = num8;
                }
                if (Main.mapFullscreenPos.Y < num7)
                {
                    Main.mapFullscreenPos.Y = num7;
                }
                if (Main.mapFullscreenPos.Y > num9)
                {
                    Main.mapFullscreenPos.Y = num9;
                }
                float num20 = Main.mapFullscreenPos.X;
                float num21 = Main.mapFullscreenPos.Y;
                if (Main.resetMapFull)
                {
                    Main.resetMapFull = false;
                    num20 = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
                    num21 = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
                    Main.mapFullscreenPos.X = num20;
                    Main.mapFullscreenPos.Y = num21;
                }
                num20 *= num16;
                num21 *= num16;
                num = -num20 + (float)(Main.screenWidth / 2);
                num2 = -num21 + (float)(Main.screenHeight / 2);
                num += num6 * num16;
                num2 += num7 * num16;
                //5 for small, 7.6ish for medium, 10 for large?
                float num22 = (float)(Main.maxTilesX / 840);
                num22 *= Main.mapFullscreenScale;
                //Whahaatttt
                float num23 = num;
                float num24 = num2;
                float num25 = (float)Main.mapTexture.Width;
                float num26 = (float)Main.mapTexture.Height;
                //AHHHH
                if (Main.maxTilesX == 8400)
                {
                    num22 *= 0.999f;
                    num23 -= 40.6f * num22;
                    num24 = num2 - 5f * num22;
                    num25 -= 8.045f;
                    num25 *= num22;
                    num26 += 0.12f;
                    num26 *= num22;
                    if ((double)num22 < 1.2)
                    {
                        num26 += 1f;
                    }
                }
                else if (Main.maxTilesX == 6400)
                {
                    num22 *= 1.09f;
                    num23 -= 38.8f * num22;
                    num24 = num2 - 3.85f * num22;
                    num25 -= 13.6f;
                    num25 *= num22;
                    num26 -= 6.92f;
                    num26 *= num22;
                    if ((double)num22 < 1.2)
                    {
                        num26 += 2f;
                    }
                }
                else if (Main.maxTilesX == 6300)
                {
                    num22 *= 1.09f;
                    num23 -= 39.8f * num22;
                    num24 = num2 - 4.08f * num22;
                    num25 -= 26.69f;
                    num25 *= num22;
                    num26 -= 6.92f;
                    num26 *= num22;
                    if ((double)num22 < 1.2)
                    {
                        num26 += 2f;
                    }
                }
                else if (Main.maxTilesX == 4200)
                {
                    num22 *= 0.998f;
                    num23 -= 37.3f * num22;
                    num24 -= 1.7f * num22;
                    num25 -= 16f;
                    num25 *= num22;
                    num26 -= 8.31f;
                    num26 *= num22;
                }
                else
                {
                    //num23 == map x
                    //num24 == map y
                    //num25 == map width
                    //num26 == map height
                    num23 -= 40f * num22;
                    num24 -= 2f * num22;
                    num25 -= 24f;
                    num25 *= num22;
                    num26 -= 8f;
                    num26 *= num22;
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                flag = true;
                Texture2D modTexture = PlayerHooks.GetMapBackgroundImage(Main.player[Main.myPlayer]);
                if (modTexture != null)
                {
                    Main.spriteBatch.Draw(modTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                }
                else if (Main.screenPosition.Y > (float)((Main.maxTilesY - 232) * 16))
                {
                    Main.spriteBatch.Draw(mapBG3Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                }
                else if (Main.player[Main.myPlayer].ZoneDungeon)
                {
                    Main.spriteBatch.Draw(mapBG5Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                }
                else if (Main.tile[(int)(Main.player[Main.myPlayer].Center.X / 16f), (int)(Main.player[Main.myPlayer].Center.Y / 16f)].wall == 87)
                {
                    Main.spriteBatch.Draw(mapBG14Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                }
                else if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0)
                {
                    if (Main.player[Main.myPlayer].ZoneSnow)
                    {
                        Main.spriteBatch.Draw(mapBG4Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                    }
                    else if (Main.player[Main.myPlayer].ZoneJungle)
                    {
                        Main.spriteBatch.Draw(mapBG13Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.sandTiles > 1000)
                    {
                        Main.spriteBatch.Draw(mapBG15Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(mapBG2Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Microsoft.Xna.Framework.Color.White);
                    }
                }
                else
                {
                    int num27 = (int)((Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f);
                    if (Main.player[Main.myPlayer].ZoneCorrupt)
                    {
                        Main.spriteBatch.Draw(mapBG6Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.player[Main.myPlayer].ZoneCrimson)
                    {
                        Main.spriteBatch.Draw(mapBG7Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.player[Main.myPlayer].ZoneHoly)
                    {
                        Main.spriteBatch.Draw(mapBG8Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if ((double)(Main.screenPosition.Y / 16f) < Main.worldSurface + 10.0 && (num27 < 380 || num27 > Main.maxTilesX - 380))
                    {
                        Main.spriteBatch.Draw(mapBG11Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.player[Main.myPlayer].ZoneSnow)
                    {
                        Main.spriteBatch.Draw(mapBG12Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.player[Main.myPlayer].ZoneJungle)
                    {
                        Main.spriteBatch.Draw(mapBG9Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else if (Main.sandTiles > 1000)
                    {
                        Main.spriteBatch.Draw(mapBG10Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(mapBG1Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.bgColor);
                    }
                }
                Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)num23, (int)num24, (int)num25, (int)num26);
                Main.spriteBatch.Draw(Main.mapTexture, destinationRectangle, Microsoft.Xna.Framework.Color.White);
                if (num16 < 1f)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin();
                    flag = false;
                }

                DimPlayer p = Main.player[Main.myPlayer].GetModPlayer<DimPlayer>();

                Main.spriteBatch.DrawString(Main.fontMouseText, "Current dimension : " + Dimlibs.Dimlibs.getPlayerDim(), new Vector2(Main.screenWidth / 2 - 75, 10), Color.White);
            }
            else if (Main.mapStyle == 1)
            {
                Main.miniMapWidth = 240;
                Main.miniMapHeight = 240;
                Main.miniMapX = Main.screenWidth - Main.miniMapWidth - 52;
                Main.miniMapY = 90;
                float arg_B10_0 = (float)Main.miniMapHeight / (float)Main.maxTilesY;
                if ((double)Main.mapMinimapScale < 0.2)
                {
                    Main.mapMinimapScale = 0.2f;
                }
                if (Main.mapMinimapScale > 3f)
                {
                    Main.mapMinimapScale = 3f;
                }
                if ((double)Main.mapMinimapAlpha < 0.01)
                {
                    Main.mapMinimapAlpha = 0.01f;
                }
                if (Main.mapMinimapAlpha > 1f)
                {
                    Main.mapMinimapAlpha = 1f;
                }
                num16 = Main.mapMinimapScale;
                b = (byte)(255f * Main.mapMinimapAlpha);
                num = (float)Main.miniMapX;
                num2 = (float)Main.miniMapY;
                num3 = num;
                num4 = num2;
                float num28 = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                float num29 = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                num10 = -(num28 - (float)((int)((Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * num16;
                num11 = -(num29 - (float)((int)((Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * num16;
                num14 = (float)Main.miniMapWidth / num16;
                num15 = (float)Main.miniMapHeight / num16;
                num12 = (float)((int)num28) - num14 / 2f;
                num13 = (float)((int)num29) - num15 / 2f;
                float num30 = (float)Main.maxTilesY + num13;
                num30 *= num16;
                float x = num3 - 6f;
                float y = num4 - 6f;
                Main.spriteBatch.Draw(Main.miniMapFrame2Texture, new Vector2(x, y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.miniMapFrame2Texture.Width * 2, Main.miniMapFrame2Texture.Height * 2)), new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);


            }
            else if (Main.mapStyle == 2)
            {
                float num31 = (float)Main.screenWidth / (float)Main.maxTilesX;
                if (Main.mapOverlayScale < num31)
                {
                    Main.mapOverlayScale = num31;
                }
                if (Main.mapOverlayScale > 16f)
                {
                    Main.mapOverlayScale = 16f;
                }
                if ((double)Main.mapOverlayAlpha < 0.01)
                {
                    Main.mapOverlayAlpha = 0.01f;
                }
                if (Main.mapOverlayAlpha > 1f)
                {
                    Main.mapOverlayAlpha = 1f;
                }
                num16 = Main.mapOverlayScale;
                b = (byte)(255f * Main.mapOverlayAlpha);
                int arg_D61_0 = Main.maxTilesX;
                int arg_D67_0 = Main.maxTilesY;
                float num32 = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
                float num33 = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
                num32 *= num16;
                num33 *= num16;
                num = -num32 + (float)(Main.screenWidth / 2);
                num2 = -num33 + (float)(Main.screenHeight / 2);
                num += num6 * num16;
                num2 += num7 * num16;
            }
            if (Main.mapStyle == 1 && !Main.mapFullscreen)
            {
                if (num12 < num6)
                {
                    num -= (num12 - num6) * num16;
                }
                if (num13 < num7)
                {
                    num2 -= (num13 - num7) * num16;
                }
            }
            num14 = num12 + num14;
            num15 = num13 + num15;
            if (num12 > num6)
            {
                num6 = num12;
            }
            if (num13 > num7)
            {
                num7 = num13;
            }
            if (num14 < num8)
            {
                num8 = num14;
            }
            if (num15 < num9)
            {
                num9 = num15;
            }
            float num34 = (float)Main.textureMaxWidth * num16;
            float num35 = (float)Main.textureMaxHeight * num16;
            float num36 = num;
            float num37 = 0f;
            for (int k = 0; k <= Main.mapTargetX - 1; k++)
            {
                if ((float)((k + 1) * Main.textureMaxWidth) > num6 && (float)(k * Main.textureMaxWidth) < num6 + num8)
                {
                    for (int l = 0; l <= num5; l++)
                    {
                        if ((float)((l + 1) * Main.textureMaxHeight) > num7 && (float)(l * Main.textureMaxHeight) < num7 + num9)
                        {
                            float num38 = num + (float)((int)((float)k * num34));
                            float num39 = num2 + (float)((int)((float)l * num35));
                            float num40 = (float)(k * Main.textureMaxWidth);
                            float num41 = (float)(l * Main.textureMaxHeight);
                            float num42 = 0f;
                            float num43 = 0f;
                            if (num40 < num6)
                            {
                                num42 = num6 - num40;
                            }
                            else
                            {
                                num38 -= num6 * num16;
                            }
                            if (num41 < num7)
                            {
                                num43 = num7 - num41;
                                num39 = num2;
                            }
                            else
                            {
                                num39 -= num7 * num16;
                            }
                            num38 = num36;
                            float num44 = (float)Main.textureMaxWidth;
                            float num45 = (float)Main.textureMaxHeight;
                            float num46 = (float)((k + 1) * Main.textureMaxWidth);
                            float num47 = (float)((l + 1) * Main.textureMaxHeight);
                            if (num46 >= num8)
                            {
                                num44 -= num46 - num8;
                            }
                            if (num47 >= num9)
                            {
                                num45 -= num47 - num9;
                            }
                            num38 += num10;
                            num39 += num11;
                            if (num44 > num42)
                            {
                                Main.spriteBatch.Draw(Main.instance.mapTarget[k, l], new Vector2(num38, num39), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)num42, (int)num43, (int)num44 - (int)num42, (int)num45 - (int)num43)), new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), 0f, default(Vector2), num16, SpriteEffects.None, 0f);
                            }
                            num37 = (float)((int)num44 - (int)num42) * num16;
                        }
                        if (l == num5)
                        {
                            num36 += num37;
                        }
                    }
                }
            }
            if (flag)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
            }
            if (!Main.mapFullscreen)
            {
                if (Main.mapStyle == 2)
                {
                    float num48 = (num16 * 0.2f * 2f + 1f) / 3f;
                    if (num48 > 1f)
                    {
                        num48 = 1f;
                    }
                    num48 *= Main.UIScale;
                    for (int m = 0; m < 200; m++)
                    {
                        if (Main.npc[m].active && Main.npc[m].townNPC)
                        {
                            int num49 = NPC.TypeToHeadIndex(Main.npc[m].type);
                            if (num49 > 0)
                            {
                                SpriteEffects effects = SpriteEffects.None;
                                if (Main.npc[m].direction > 0)
                                {
                                    effects = SpriteEffects.FlipHorizontally;
                                }
                                float num50 = (Main.npc[m].position.X + (float)(Main.npc[m].width / 2)) / 16f * num16;
                                float num51 = (Main.npc[m].position.Y + (float)(Main.npc[m].height / 2)) / 16f * num16;
                                num50 += num;
                                num51 += num2;
                                num50 -= 10f * num16;
                                num51 -= 10f * num16;
                                Main.spriteBatch.Draw(Main.npcHeadTexture[num49], new Vector2(num50, num51), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.npcHeadTexture[num49].Width, Main.npcHeadTexture[num49].Height)), new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), 0f, new Vector2((float)(Main.npcHeadTexture[num49].Width / 2), (float)(Main.npcHeadTexture[num49].Height / 2)), num48, effects, 0f);
                            }
                        }
                        if (Main.npc[m].active && Main.npc[m].GetBossHeadTextureIndex() != -1)
                        {
                            float bossHeadRotation = Main.npc[m].GetBossHeadRotation();
                            SpriteEffects bossHeadSpriteEffects = Main.npc[m].GetBossHeadSpriteEffects();
                            Vector2 vector = Main.npc[m].Center + new Vector2(0f, Main.npc[m].gfxOffY);
                            if (Main.npc[m].type == 134)
                            {
                                Vector2 vector2 = Main.npc[m].Center;
                                int num52 = 1;
                                int num53 = (int)Main.npc[m].ai[0];
                                while (num52 < 15 && Main.npc[num53].active && Main.npc[num53].type >= 134 && Main.npc[num53].type <= 136)
                                {
                                    num52++;
                                    vector2 += Main.npc[num53].Center;
                                    num53 = (int)Main.npc[num53].ai[0];
                                }
                                vector2 /= (float)num52;
                                vector = vector2;
                            }
                            int bossHeadTextureIndex = Main.npc[m].GetBossHeadTextureIndex();
                            float num54 = vector.X / 16f * num16;
                            float num55 = vector.Y / 16f * num16;
                            num54 += num;
                            num55 += num2;
                            num54 -= 10f * num16;
                            num55 -= 10f * num16;
                            Main.spriteBatch.Draw(Main.npcHeadBossTexture[bossHeadTextureIndex], new Vector2(num54, num55), null, new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), bossHeadRotation, Main.npcHeadBossTexture[bossHeadTextureIndex].Size() / 2f, num48, bossHeadSpriteEffects, 0f);
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
                    for (int n = 0; n < 255; n++)
                    {
                        if (Main.player[n].active && !Main.player[n].dead && n != Main.myPlayer && ((!Main.player[Main.myPlayer].hostile && !Main.player[n].hostile) || (Main.player[Main.myPlayer].team == Main.player[n].team && Main.player[n].team != 0) || n == Main.myPlayer))
                        {
                            float num56 = (Main.player[n].position.X + (float)(Main.player[n].width / 2)) / 16f * num16;
                            float num57 = Main.player[n].position.Y / 16f * num16;
                            num56 += num;
                            num57 += num2;
                            num56 -= 6f;
                            num57 -= 2f;
                            num57 -= 2f - num16 / 5f * 2f;
                            num56 -= 10f * num16;
                            num57 -= 10f * num16;
                            Object[] array = { Main.player[n], num56, num57, (float)b / 255f, num48 };
                            DrawPlayerHead.Invoke(Main.instance, array);
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin();
                }
                if (Main.mapStyle == 1)
                {
                    float num58 = num3 - 6f;
                    float num59 = num4 - 6f;
                    float num60 = (num16 * 0.25f * 2f + 1f) / 3f;
                    if (num60 > 1f)
                    {
                        num60 = 1f;
                    }
                    for (int num61 = 0; num61 < 200; num61++)
                    {
                        if (Main.npc[num61].active && Main.npc[num61].townNPC)
                        {
                            int num62 = NPC.TypeToHeadIndex(Main.npc[num61].type);
                            if (num62 > 0)
                            {
                                SpriteEffects effects2 = SpriteEffects.None;
                                if (Main.npc[num61].direction > 0)
                                {
                                    effects2 = SpriteEffects.FlipHorizontally;
                                }
                                float num63 = ((Main.npc[num61].position.X + (float)(Main.npc[num61].width / 2)) / 16f - num12) * num16;
                                float num64 = ((Main.npc[num61].position.Y + Main.npc[num61].gfxOffY + (float)(Main.npc[num61].height / 2)) / 16f - num13) * num16;
                                num63 += num3;
                                num64 += num4;
                                num64 -= 2f * num16 / 5f;
                                if (num63 > (float)(Main.miniMapX + 12) && num63 < (float)(Main.miniMapX + Main.miniMapWidth - 16) && num64 > (float)(Main.miniMapY + 10) && num64 < (float)(Main.miniMapY + Main.miniMapHeight - 14))
                                {
                                    Main.spriteBatch.Draw(Main.npcHeadTexture[num62], new Vector2(num63 + num10, num64 + num11), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.npcHeadTexture[num62].Width, Main.npcHeadTexture[num62].Height)), new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), 0f, new Vector2((float)(Main.npcHeadTexture[num62].Width / 2), (float)(Main.npcHeadTexture[num62].Height / 2)), num60, effects2, 0f);
                                    float num65 = num63 - (float)(Main.npcHeadTexture[num62].Width / 2) * num60;
                                    float num66 = num64 - (float)(Main.npcHeadTexture[num62].Height / 2) * num60;
                                    float num67 = num65 + (float)Main.npcHeadTexture[num62].Width * num60;
                                    float num68 = num66 + (float)Main.npcHeadTexture[num62].Height * num60;
                                    if ((float)Main.mouseX >= num65 && (float)Main.mouseX <= num67 && (float)Main.mouseY >= num66 && (float)Main.mouseY <= num68)
                                    {
                                        text = Main.npc[num61].FullName;
                                    }
                                }
                            }
                        }
                        if (Main.npc[num61].active && Main.npc[num61].GetBossHeadTextureIndex() != -1)
                        {
                            float bossHeadRotation2 = Main.npc[num61].GetBossHeadRotation();
                            SpriteEffects bossHeadSpriteEffects2 = Main.npc[num61].GetBossHeadSpriteEffects();
                            Vector2 vector3 = Main.npc[num61].Center + new Vector2(0f, Main.npc[num61].gfxOffY);
                            if (Main.npc[num61].type == 134)
                            {
                                Vector2 vector4 = Main.npc[num61].Center;
                                int num69 = 1;
                                int num70 = (int)Main.npc[num61].ai[0];
                                while (num69 < 15 && Main.npc[num70].active && Main.npc[num70].type >= 134 && Main.npc[num70].type <= 136)
                                {
                                    num69++;
                                    vector4 += Main.npc[num70].Center;
                                    num70 = (int)Main.npc[num70].ai[0];
                                }
                                vector4 /= (float)num69;
                                vector3 = vector4;
                            }
                            int bossHeadTextureIndex2 = Main.npc[num61].GetBossHeadTextureIndex();
                            float num71 = (vector3.X / 16f - num12) * num16;
                            float num72 = (vector3.Y / 16f - num13) * num16;
                            num71 += num3;
                            num72 += num4;
                            num72 -= 2f * num16 / 5f;
                            if (num71 > (float)(Main.miniMapX + 12) && num71 < (float)(Main.miniMapX + Main.miniMapWidth - 16) && num72 > (float)(Main.miniMapY + 10) && num72 < (float)(Main.miniMapY + Main.miniMapHeight - 14))
                            {
                                Main.spriteBatch.Draw(Main.npcHeadBossTexture[bossHeadTextureIndex2], new Vector2(num71 + num10, num72 + num11), null, new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), bossHeadRotation2, Main.npcHeadBossTexture[bossHeadTextureIndex2].Size() / 2f, num60, bossHeadSpriteEffects2, 0f);
                                float num73 = num71 - (float)(Main.npcHeadBossTexture[bossHeadTextureIndex2].Width / 2) * num60;
                                float num74 = num72 - (float)(Main.npcHeadBossTexture[bossHeadTextureIndex2].Height / 2) * num60;
                                float num75 = num73 + (float)Main.npcHeadBossTexture[bossHeadTextureIndex2].Width * num60;
                                float num76 = num74 + (float)Main.npcHeadBossTexture[bossHeadTextureIndex2].Height * num60;
                                if ((float)Main.mouseX >= num73 && (float)Main.mouseX <= num75 && (float)Main.mouseY >= num74 && (float)Main.mouseY <= num76)
                                {
                                    text = Main.npc[num61].GivenOrTypeName;
                                }
                            }
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                    for (int num77 = 0; num77 < 255; num77++)
                    {
                        if (Main.player[num77].active && ((!Main.player[Main.myPlayer].hostile && !Main.player[num77].hostile) || (Main.player[Main.myPlayer].team == Main.player[num77].team && Main.player[num77].team != 0) || num77 == Main.myPlayer))
                        {
                            float num78 = ((Main.player[num77].position.X + (float)(Main.player[num77].width / 2)) / 16f - num12) * num16;
                            float num79 = ((Main.player[num77].position.Y + Main.player[num77].gfxOffY + (float)(Main.player[num77].height / 2)) / 16f - num13) * num16;
                            num78 += num3;
                            num79 += num4;
                            num78 -= 6f;
                            num79 -= 6f;
                            num79 -= 2f - num16 / 5f * 2f;
                            num78 += num10;
                            num79 += num11;
                            if (Main.screenPosition.X != Main.leftWorld + 640f + 16f && Main.screenPosition.X + (float)Main.screenWidth != Main.rightWorld - 640f - 32f && Main.screenPosition.Y != Main.topWorld + 640f + 16f && Main.screenPosition.Y + (float)Main.screenHeight <= Main.bottomWorld - 640f - 32f && num77 == Main.myPlayer && Main.zoomX == 0f && Main.zoomY == 0f)
                            {
                                num78 = num3 + (float)(Main.miniMapWidth / 2);
                                num79 = num4 + (float)(Main.miniMapHeight / 2);
                                num79 -= 3f;
                                num78 -= 4f;
                            }
                            if (!Main.player[num77].dead && num78 > (float)(Main.miniMapX + 6) && num78 < (float)(Main.miniMapX + Main.miniMapWidth - 16) && num79 > (float)(Main.miniMapY + 6) && num79 < (float)(Main.miniMapY + Main.miniMapHeight - 14))
                            {
                                Object[] array = { Main.player[num77], num78, num79, (float)b / 255f, num60 };
                                DrawPlayerHead.Invoke(Main.instance, array);

                                if (num77 != Main.myPlayer)
                                {
                                    float num80 = num78 + 4f - 14f * num60;
                                    float num81 = num79 + 2f - 14f * num60;
                                    float num82 = num80 + 28f * num60;
                                    float num83 = num81 + 28f * num60;
                                    if ((float)Main.mouseX >= num80 && (float)Main.mouseX <= num82 && (float)Main.mouseY >= num81 && (float)Main.mouseY <= num83)
                                    {
                                        text = Main.player[num77].name;
                                    }
                                }
                            }
                            if (Main.player[num77].showLastDeath)
                            {
                                num78 = (Main.player[num77].lastDeathPostion.X / 16f - num12) * num16;
                                num79 = (Main.player[num77].lastDeathPostion.Y / 16f - num13) * num16;
                                num78 += num3;
                                num79 += num4;
                                num79 -= 2f - num16 / 5f * 2f;
                                num78 += num10;
                                num79 += num11;
                                if (num78 > (float)(Main.miniMapX + 8) && num78 < (float)(Main.miniMapX + Main.miniMapWidth - 18) && num79 > (float)(Main.miniMapY + 8) && num79 < (float)(Main.miniMapY + Main.miniMapHeight - 16))
                                {
                                    Main.spriteBatch.Draw(mapDeathTexture, new Vector2(num78, num79), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, mapDeathTexture.Width, mapDeathTexture.Height)), Microsoft.Xna.Framework.Color.White, 0f, new Vector2((float)mapDeathTexture.Width * 0.5f, (float)mapDeathTexture.Height * 0.5f), num60, SpriteEffects.None, 0f);
                                    float num84 = num78 + 4f - 14f * num60;
                                    float num85 = num79 + 2f - 14f * num60;
                                    num84 -= 4f;
                                    num85 -= 4f;
                                    float num86 = num84 + 28f * num60;
                                    float num87 = num85 + 28f * num60;
                                    if ((float)Main.mouseX >= num84 && (float)Main.mouseX <= num86 && (float)Main.mouseY >= num85 && (float)Main.mouseY <= num87)
                                    {
                                        TimeSpan timeSpan = DateTime.Now - Main.player[num77].lastDeathTime;
                                        text = Language.GetTextValue("Game.PlayerDeathTime", Main.player[num77].name, Lang.LocalizedDuration(timeSpan, false, false));
                                    }
                                }
                            }
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                    Main.spriteBatch.Draw(Main.miniMapFrameTexture, new Vector2(num58, num59), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.miniMapFrameTexture.Width, Main.miniMapFrameTexture.Height)), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                    for (int num88 = 0; num88 < 3; num88++)
                    {
                        float num89 = num58 + 148f + (float)(num88 * 26);
                        float num90 = num59 + 234f;
                        if ((float)Main.mouseX > num89 && (float)Main.mouseX < num89 + 22f && (float)Main.mouseY > num90 && (float)Main.mouseY < num90 + 22f)
                        {
                            Main.spriteBatch.Draw(Main.miniMapButtonTexture[num88], new Vector2(num89, num90), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.miniMapButtonTexture[num88].Width, Main.miniMapButtonTexture[num88].Height)), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                            if (!PlayerInput.IgnoreMouseInterface)
                            {
                                Main.player[Main.myPlayer].mouseInterface = true;
                                if (Main.mouseLeft)
                                {
                                    if (Main.mouseLeftRelease)
                                    {
                                        Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                    }
                                    if (num88 == 0)
                                    {
                                        Main.mapMinimapScale = 1.25f;
                                    }
                                    else if (num88 == 1)
                                    {
                                        Main.mapMinimapScale *= 0.975f;
                                    }
                                    else if (num88 == 2)
                                    {
                                        Main.mapMinimapScale *= 1.025f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Main.mapFullscreen)
            {
                int num91 = (int)((-num + (float)Main.mouseX) / num16 + num6);
                int num92 = (int)((-num2 + (float)Main.mouseY) / num16 + num7);
                bool flag2 = false;
                if ((float)num91 < num6)
                {
                    flag2 = true;
                }
                if ((float)num91 >= num8)
                {
                    flag2 = true;
                }
                if ((float)num92 < num7)
                {
                    flag2 = true;
                }
                if ((float)num92 >= num9)
                {
                    flag2 = true;
                }
                if (!flag2 && Main.Map[num91, num92].Light > 40)
                {


                    int type = (int)Main.Map[num91, num92].Type;
                    int num93 = (int)MapHelper.tileLookup[21];
                    int num94 = (int)MapHelper.tileLookup[441];
                    int num95 = MapHelper.tileOptionCounts[21];
                    int num96 = (int)MapHelper.tileLookup[467];
                    int num97 = (int)MapHelper.tileLookup[468];
                    int num98 = MapHelper.tileOptionCounts[467];
                    int num99 = (int)MapHelper.tileLookup[88];
                    int num100 = MapHelper.tileOptionCounts[88];
                    LocalizedText[] chestType = Lang.chestType;
                    LocalizedText[] chestType2 = Lang.chestType2;
                    if (type >= num93 && type < num93 + num95)
                    {
                        Tile tile = Main.tile[num91, num92];
                        if (tile != null)
                        {
                            int num101 = num91;
                            int num102 = num92;
                            if (tile.frameX % 36 != 0)
                            {
                                num101--;
                            }
                            if (tile.frameY % 36 != 0)
                            {
                                num102--;
                            }

                            Object[] array = { chestType, tile, num101, num102, 36 };

                            text = (string)DrawMap_FindChestName.Invoke(Main.instance, array);
                        }
                    }
                    else if (type >= num96 && type < num96 + num98)
                    {
                        Tile tile2 = Main.tile[num91, num92];
                        if (tile2 != null)
                        {
                            int num103 = num91;
                            int num104 = num92;
                            if (tile2.frameX % 36 != 0)
                            {
                                num103--;
                            }
                            if (tile2.frameY % 36 != 0)
                            {
                                num104--;
                            }

                            Object[] array = { chestType2, tile2, num103, num104, 36 };
                            text = (string)DrawMap_FindChestName.Invoke(Main.instance, array);
                        }
                    }
                    else if (type >= num94 && type < num94 + num95)
                    {
                        Tile tile3 = Main.tile[num91, num92];
                        if (tile3 != null)
                        {
                            int num105 = num91;
                            int num106 = num92;
                            if (tile3.frameX % 36 != 0)
                            {
                                num105--;
                            }
                            if (tile3.frameY % 36 != 0)
                            {
                                num106--;
                            }
                            text = chestType[(int)(tile3.frameX / 36)].Value;
                        }
                    }
                    else if (type >= num97 && type < num97 + num98)
                    {
                        Tile tile4 = Main.tile[num91, num92];
                        if (tile4 != null)
                        {
                            int num107 = num91;
                            int num108 = num92;
                            if (tile4.frameX % 36 != 0)
                            {
                                num107--;
                            }
                            if (tile4.frameY % 36 != 0)
                            {
                                num108--;
                            }
                            text = chestType2[(int)(tile4.frameX / 36)].Value;
                        }
                    }
                    else if (type >= num99 && type < num99 + num100)
                    {
                        //patch file: num91, num92
                        Tile tile5 = Main.tile[num91, num92];
                        if (tile5 != null)
                        {
                            int num109 = num91;
                            int num110 = num92;
                            num109 -= (int)(tile5.frameX % 54 / 18);
                            if (tile5.frameY % 36 != 0)
                            {
                                num110--;
                            }
                            int num111 = Chest.FindChest(num109, num110);
                            if (num111 < 0)
                            {
                                text = Lang.dresserType[0].Value;
                            }
                            else if (Main.chest[num111].name != "")
                            {
                                text = Lang.dresserType[(int)(tile5.frameX / 54)].Value + ": " + Main.chest[num111].name;
                            }
                            else
                            {
                                text = Lang.dresserType[(int)(tile5.frameX / 54)].Value;
                            }
                        }
                    }
                    else
                    {
                        text = Lang.GetMapObjectName(type);
                        text = Lang._mapLegendCache.FromTile(Main.Map[num91, num92], num91, num92);
                    }
                }
                float num112 = (num16 * 0.25f * 2f + 1f) / 3f;
                if (num112 > 1f)
                {
                }
                num112 = Main.UIScale;
                for (int num113 = 0; num113 < 200; num113++)
                {
                    if (Main.npc[num113].active && Main.npc[num113].townNPC)
                    {
                        int num114 = NPC.TypeToHeadIndex(Main.npc[num113].type);
                        if (num114 > 0)
                        {
                            SpriteEffects effects3 = SpriteEffects.None;
                            if (Main.npc[num113].direction > 0)
                            {
                                effects3 = SpriteEffects.FlipHorizontally;
                            }
                            float num115 = (Main.npc[num113].position.X + (float)(Main.npc[num113].width / 2)) / 16f * num16;
                            float num116 = (Main.npc[num113].position.Y + Main.npc[num113].gfxOffY + (float)(Main.npc[num113].height / 2)) / 16f * num16;
                            num115 += num;
                            num116 += num2;
                            num115 -= 10f * num16;
                            num116 -= 10f * num16;
                            Main.spriteBatch.Draw(Main.npcHeadTexture[num114], new Vector2(num115, num116), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.npcHeadTexture[num114].Width, Main.npcHeadTexture[num114].Height)), new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), 0f, new Vector2((float)(Main.npcHeadTexture[num114].Width / 2), (float)(Main.npcHeadTexture[num114].Height / 2)), num112, effects3, 0f);
                            float num117 = num115 - (float)(Main.npcHeadTexture[num114].Width / 2) * num112;
                            float num118 = num116 - (float)(Main.npcHeadTexture[num114].Height / 2) * num112;
                            float num119 = num117 + (float)Main.npcHeadTexture[num114].Width * num112;
                            float num120 = num118 + (float)Main.npcHeadTexture[num114].Height * num112;
                            if ((float)Main.mouseX >= num117 && (float)Main.mouseX <= num119 && (float)Main.mouseY >= num118 && (float)Main.mouseY <= num120)
                            {
                                text = Main.npc[num113].FullName;
                            }
                        }
                    }
                    if (Main.npc[num113].active && Main.npc[num113].GetBossHeadTextureIndex() != -1)
                    {
                        float bossHeadRotation3 = Main.npc[num113].GetBossHeadRotation();
                        SpriteEffects bossHeadSpriteEffects3 = Main.npc[num113].GetBossHeadSpriteEffects();
                        Vector2 vector5 = Main.npc[num113].Center + new Vector2(0f, Main.npc[num113].gfxOffY);
                        if (Main.npc[num113].type == 134)
                        {
                            Vector2 vector6 = Main.npc[num113].Center;
                            int num121 = 1;
                            int num122 = (int)Main.npc[num113].ai[0];
                            while (num121 < 15 && Main.npc[num122].active && Main.npc[num122].type >= 134 && Main.npc[num122].type <= 136)
                            {
                                num121++;
                                vector6 += Main.npc[num122].Center;
                                num122 = (int)Main.npc[num122].ai[0];
                            }
                            vector6 /= (float)num121;
                            vector5 = vector6;
                        }
                        int bossHeadTextureIndex3 = Main.npc[num113].GetBossHeadTextureIndex();
                        float num123 = vector5.X / 16f * num16;
                        float num124 = vector5.Y / 16f * num16;
                        num123 += num;
                        num124 += num2;
                        num123 -= 10f * num16;
                        num124 -= 10f * num16;
                        Main.spriteBatch.Draw(Main.npcHeadBossTexture[bossHeadTextureIndex3], new Vector2(num123, num124), null, new Microsoft.Xna.Framework.Color((int)b, (int)b, (int)b, (int)b), bossHeadRotation3, Main.npcHeadBossTexture[bossHeadTextureIndex3].Size() / 2f, num112, bossHeadSpriteEffects3, 0f);
                        float num125 = num123 - (float)(Main.npcHeadBossTexture[bossHeadTextureIndex3].Width / 2) * num112;
                        float num126 = num124 - (float)(Main.npcHeadBossTexture[bossHeadTextureIndex3].Height / 2) * num112;
                        float num127 = num125 + (float)Main.npcHeadBossTexture[bossHeadTextureIndex3].Width * num112;
                        float num128 = num126 + (float)Main.npcHeadBossTexture[bossHeadTextureIndex3].Height * num112;
                        if ((float)Main.mouseX >= num125 && (float)Main.mouseX <= num127 && (float)Main.mouseY >= num126 && (float)Main.mouseY <= num128)
                        {
                            text = Main.npc[num113].GivenOrTypeName;
                        }
                    }
                }
                bool flag3 = false;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                for (int num129 = 0; num129 < 255; num129++)
                {
                    if (Main.player[num129].active && ((!Main.player[Main.myPlayer].hostile && !Main.player[num129].hostile) || (Main.player[Main.myPlayer].team == Main.player[num129].team && Main.player[num129].team != 0) || num129 == Main.myPlayer) && Main.player[num129].showLastDeath)
                    {
                        float num130 = (Main.player[num129].lastDeathPostion.X / 16f - num12) * num16;
                        float num131 = (Main.player[num129].lastDeathPostion.Y / 16f - num13) * num16;
                        num130 += num;
                        num131 += num2;
                        num131 -= 2f - num16 / 5f * 2f;
                        num130 -= 10f * num16;
                        num131 -= 10f * num16;
                        Main.spriteBatch.Draw(mapDeathTexture, new Vector2(num130, num131), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, mapDeathTexture.Width, mapDeathTexture.Height)), Microsoft.Xna.Framework.Color.White, 0f, new Vector2((float)mapDeathTexture.Width * 0.5f, (float)mapDeathTexture.Height * 0.5f), num112, SpriteEffects.None, 0f);
                        float num132 = num130 + 4f - 14f * num112;
                        float num133 = num131 + 2f - 14f * num112;
                        float num134 = num132 + 28f * num112;
                        float num135 = num133 + 28f * num112;
                        if ((float)Main.mouseX >= num132 && (float)Main.mouseX <= num134 && (float)Main.mouseY >= num133 && (float)Main.mouseY <= num135)
                        {
                            TimeSpan timeSpan2 = DateTime.Now - Main.player[num129].lastDeathTime;
                            text = Language.GetTextValue("Game.PlayerDeathTime", Main.player[num129].name, Lang.LocalizedDuration(timeSpan2, false, false));
                        }
                    }
                }
                for (int num136 = 0; num136 < 255; num136++)
                {
                    if (Main.player[num136].active && ((!Main.player[Main.myPlayer].hostile && !Main.player[num136].hostile) || (Main.player[Main.myPlayer].team == Main.player[num136].team && Main.player[num136].team != 0) || num136 == Main.myPlayer))
                    {
                        float num137 = ((Main.player[num136].position.X + (float)(Main.player[num136].width / 2)) / 16f - num12) * num16;
                        float num138 = ((Main.player[num136].position.Y + Main.player[num136].gfxOffY + (float)(Main.player[num136].height / 2)) / 16f - num13) * num16;
                        num137 += num;
                        num138 += num2;
                        num137 -= 6f;
                        num138 -= 2f;
                        num138 -= 2f - num16 / 5f * 2f;
                        num137 -= 10f * num16;
                        num138 -= 10f * num16;
                        float num139 = num137 + 4f - 14f * num112;
                        float num140 = num138 + 2f - 14f * num112;
                        float num141 = num139 + 28f * num112;
                        float num142 = num140 + 28f * num112;
                        if (!Main.player[num136].dead)
                        {
                            Object[] array = { Main.player[num136], num137, num138, (float)b / 255f, num112 };

                            DrawPlayerHead.Invoke(Main.instance, array);
                            if ((float)Main.mouseX >= num139 && (float)Main.mouseX <= num141 && (float)Main.mouseY >= num140 && (float)Main.mouseY <= num142)
                            {
                                text = Main.player[num136].name;
                                if (num136 != Main.myPlayer && Main.player[Main.myPlayer].team > 0 && Main.player[Main.myPlayer].team == Main.player[num136].team && Main.netMode == 1 && Main.player[Main.myPlayer].HasUnityPotion())
                                {
                                    flag3 = true;
                                    if (!Main.instance.unityMouseOver)
                                    {
                                        Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                    }
                                    Main.instance.unityMouseOver = true;

                                    Object[] array2 = { Main.player[num136], num137, num138, 2f, num112 + 0.5f };
                                    DrawPlayerHead.Invoke(Main.instance, array2);

                                    text = Language.GetTextValue("Game.TeleportTo", Main.player[num136].name);
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        Main.mouseLeftRelease = false;
                                        Main.mapFullscreen = false;
                                        Main.player[Main.myPlayer].UnityTeleport(Main.player[num136].position);
                                        Main.player[Main.myPlayer].TakeUnityPotion();
                                    }
                                }
                            }
                        }
                    }
                }
                if (!flag3 && Main.instance.unityMouseOver)
                {
                    Main.instance.unityMouseOver = false;
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                PlayerInput.SetZoom_UI();
                int num143 = 10;
                int num144 = Main.screenHeight - 40;
                if (Main.showFrameRate)
                {
                    num144 -= 15;
                }
                int num145 = 0;
                int num146 = 130;
                if (Main.mouseX >= num143 && Main.mouseX <= num143 + 32 && Main.mouseY >= num144 && Main.mouseY <= num144 + 30)
                {
                    num146 = 255;
                    num145 += 4;
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                        Main.mapFullscreen = false;
                    }
                }
                Main.spriteBatch.Draw(Main.mapIconTexture[num145], new Vector2((float)num143, (float)num144), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.mapIconTexture[num145].Width, Main.mapIconTexture[num145].Height)), new Microsoft.Xna.Framework.Color(num146, num146, num146, num146), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

                //Attempted fix, not sure if it will work.

                object[] args = new object[] { text };
                PostDrawFullscreenMap.Invoke(null, args);
                text = (string)args[0];

                Vector2 bonus = Main.DrawThickCursor(false);
                Main.DrawCursor(bonus, false);
            }
            if (text != "")
            {
                Main.instance.MouseText(text, 0, 0, -1, -1, -1, -1);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            PlayerInput.SetZoom_Unscaled();
            TimeLogger.DetailedDrawTime(9);
        }
    }
}
