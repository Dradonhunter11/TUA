using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Liquid;
using TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod;
using TerrariaUltraApocalypse.API.LiquidAPI.Swap;

namespace TerrariaUltraApocalypse.API.LiquidAPI
{
    class LiquidRegistery
    {
        private static LiquidRegistery instance;
        internal static List<ModLiquid> liquidList;
        private static int initialLiquidIndex = 3;
        private static int liquidTextureIndex = 12;
         
        
        public static LiquidRegistery getInstance()
        {
            if (instance == null)
            {
                instance = new LiquidRegistery();
            }

            return instance;
        }

        public void addNewModLiquid(ModLiquid liquid)
        {
            liquid.liquidIndex = initialLiquidIndex;
            initialLiquidIndex++;
            liquidList.Add(liquid);
            Main.liquidTexture[liquidTextureIndex] = liquid.texture;
            liquidTextureIndex++;
        }

        private LiquidRegistery()
        {
            List<Texture2D> liquidTexture = new List<Texture2D>(256);
            liquidTexture.AddRange(Main.liquidTexture);
            Main.liquidTexture = new Texture2D[256];
            List<Texture2D> liquidTexture2 = new List<Texture2D>(256);
            liquidTexture2.AddRange(LiquidRenderer.Instance._liquidTextures);
            LiquidRenderer.Instance._liquidTextures = new Texture2D[256];

            int i = 0;
            foreach (Texture2D t in liquidTexture)
            {
                Main.liquidTexture[i] = t;
                LiquidRenderer.Instance._liquidTextures[i] = t;
                i++;
            }

            i = 0;
            foreach (Texture2D t in liquidTexture2)
            {
                LiquidRenderer.Instance._liquidTextures[i] = t;
                i++;
            }
            liquidList = new List<ModLiquid>();

            
        }

        public static void MassMethodSwap()
        {
            LiquidSwapping.MethodSwap();
            WaterDrawInjection.MethodSwap();
            InternalLiquidDrawInjection.SwapMethod();
        }

        public static void PreDrawValue(ref bool bg, ref int style, ref float Alpha)
        {
            foreach (ModLiquid liquid in liquidList)
            {
                liquid.PreDraw(Main.tileBatch);
            }
        }

        public static void Update()
        {
            foreach (ModLiquid liquid in liquidList)
            {
                liquid.Update();
            }
        }

        public static float setOpacity(LiquidRef liquid)
        {
            for (byte by = 0; by < LiquidRegistery.liquidList.Count; by = (byte)(by + 1))
            {
                if (liquid.liquids((byte) (2 + by)))
                {
                    return liquidList[by].SetLiquidOpacity();
                }
            }
            return 1f;
        }

        public static void PlayerInteraction(byte index, Player target)
        {
            liquidList[index].PlayerInteraction(target);
        }

        public static void NPCInteraction(byte index, NPC target)
        {
            liquidList[index].NpcInteraction(target);
        }

        public static void ItemInteraction(byte index, Item item)
        {
            liquidList[index].ItemInteraction(item);
        }
    }
}
