using Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API
{
    public static class ModExtension
    {
        private static int 
            _defaultSpawnRate = 600,
            _defaultMaxSpawns = 5,
            _spawnSpaceX = 3,
            _spawnSpaceY = 3;

        private static bool _noSpawnCycle = false;

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
            if (culture == 0)
                self.SetDefault("");
            else
                self.AddTranslation(culture, "");
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
        public static Item ScanForItemInInventory(this Player player, int itemType, int quantity) {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];

                if (item.type == itemType && item.stack == quantity)
                    return item;
            }

            return null;
        }

        public static bool IsFull(this object[,] array)
        {
            int rowCount = array.GetLength(0);
            int columnCount = array.GetLength(1);

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                    if (array[i, j] == null)
                        return false;

            return true;
        }

        public static bool IsFull(this object[] array)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == null)
                    return false;

            return true;
        }

        public static ushort TileID(this Mod mod, string tileName)
        {
            return (ushort) mod.TileType(tileName);
        }

        public static void MergeTile(this ModTile self, int otherTile)
        {
            Main.tileMerge[self.Type][otherTile] = true;
        }

        public static void PutModItemInInventory(this Player player, ModItem item, int selItem = -1)
        {
            for (int i = 0; i < 58; i++)
            {
                Item inventoryItem = player.inventory[i];

                if (inventoryItem.stack > 0 && inventoryItem.type == item.item.type && inventoryItem.stack < inventoryItem.maxStack)
                {
                    inventoryItem.stack++;
                    return;
                }
            }

            if (selItem >= 0 && (player.inventory[selItem].type == 0 || player.inventory[selItem].stack <= 0))
            {
                ModItem newItem = item.Clone();
                newItem.item.maxStack = 1;
                player.inventory[selItem].SetDefaults(newItem.item.type, false);
                typeof(Item).GetField("modItem", BindingFlags.Public | BindingFlags.Instance).SetValue(player.inventory[selItem], newItem);
                return;
            }

            ModItem newModItem = item.Clone();
            newModItem.item.maxStack = 1;
            player.inventory[selItem].SetDefaults(newModItem.item.type, false);

            Item newItem2 = new Item();
            newItem2.SetDefaults(item.item.type, false);
            typeof(Item).GetField("modItem", BindingFlags.Public | BindingFlags.Instance).SetValue(newItem2, newModItem);
            Item item3 = player.GetItem(player.whoAmI, newItem2, false, false);
            if (item3.stack > 0)
            {
                int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, item.item.type, 1, false, 0, true, false);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
                    return;
                }
            }
            else
            {
                newItem2.position.X = player.Center.X - (float)(newItem2.width / 2);
                newItem2.position.Y = newItem2.Center.Y - (float)(newItem2.height / 2);
                newItem2.active = true;
                ItemText.NewText(newItem2, 0, false, false);
            }
        }

        public static Texture2D SubFrameImage(this Texture2D texture, int frameNumber, int frame)
        {
            RenderTarget2D target = new RenderTarget2D(Main.instance.GraphicsDevice, texture.Width, texture.Height / frame);
            Main.graphics.GraphicsDevice.SetRenderTarget(target);
            Main.spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, texture.Height / frameNumber * frame, texture.Width, texture.Height / frameNumber), Color.White);
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            return target;
        }

        public static Texture2D SubImage(this Texture2D texture, int x, int y, int width, int height)
        {
            RenderTarget2D target = new RenderTarget2D(Main.instance.GraphicsDevice, width, height);
            Main.graphics.GraphicsDevice.SetRenderTarget(target);
            Main.spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(x, y, width, height), Color.White);
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            return target;
        }
    }
}