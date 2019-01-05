using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API
{
    public static class ModExtension
    {
        private static int defaultSpawnRate = 600;
        private static int defaultMaxSpawns = 5;
        private static bool noSpawnCycle = false;
        private static int spawnRate = defaultSpawnRate;
        private static int maxSpawns = defaultMaxSpawns;
        private static int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
        private static int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
        public static int safeRangeX = (int)((double)(NPC.sWidth / 16) * 0.52);
        public static int safeRangeY = (int)((double)(NPC.sHeight / 16) * 0.52);
        private static int activeRangeX = (int)((double)NPC.sWidth * 2.1);
        private static int activeRangeY = (int)((double)NPC.sHeight * 2.1);
        private static int townRangeX = NPC.sWidth;
        private static int townRangeY = NPC.sHeight;
        private static int spawnSpaceX = 3;
        private static int spawnSpaceY = 3;

        public static void AddLine(this ModTranslation self, string text, int culture = 0)
        {
            string currentText;
            if (culture == 0)
            {
                currentText = self.GetDefault();
                currentText += text + "\n";
                self.SetDefault(currentText);
            }
            else
            {
                currentText = self.GetTranslation(culture);
                currentText += text + "\n";
                self.AddTranslation(culture, currentText);
            }
        }

        public static void Reset(this ModTranslation self, int culture = 0)
        {
            string currentText;
            if (culture == 0)
            {
                self.SetDefault("");
            }
            else
            {
                self.AddTranslation(culture, "");
            }
        }

        public static void IgnoreArmor(this Player self)
        {
            self.armorPenetration = 0;

        }

        public static void IgnoreArmor(this NPC self)
        {
            self.checkArmorPenetration(0);
        }

        /// <summary>
        /// Scan for an item in the player inventory that match the item item type and the item quantity
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemType"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static Item scanForItemInInventory(this Player player, int itemType, int quantity) {
            foreach (Item i in player.inventory)
            {
                if (i.type == itemType && i.stack == quantity)
                {
                    return i;
                }
            }
            return null;
        }

        public static bool IsFull(this Object[,] self)
        {
            int rowCount = self.GetLength(0);
            int columnCount = self.GetLength(1);
            int validCase = 0;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (self[i, j] != null)
                    {
                        validCase++;
                    }
                }
            }
            return validCase == rowCount * columnCount;
        }

        public static String ToString(this ModTranslation self)
        {
            GameCulture culture = Language.ActiveCulture;
            if (culture == GameCulture.English || self.GetTranslation(culture) == null)
            {
                return self.GetDefault();
            }

            return self.GetTranslation(culture);
        }
    }
}