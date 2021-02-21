using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace TUA.Dimension.Block
{
    abstract class InfestedTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<InfestedTileEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            AddMapEntry(Color.Gray);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            var TE = ModContent.GetInstance<InfestedTileEntity>();
            for (int i1 = 0; i1 < TE.types.Count; i1++)
            {
                int type = TE.types[i1];
                int npc = NPC.NewNPC(i * 16, j * 16, type);
                while (Main.tile[i, j].nactive())
                {
                    Main.npc[npc].position += new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
                }
            }
            TE.Kill(i, j);
        }

        public override bool AutoSelect(int i, int j, Item item)
        {
            return item.pick != 0;
        }
    }

    abstract class InfestedTileEntity : ModTileEntity
    {
        public IList<int> types;

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == ModContent.TileType<InfestedTile>() 
                && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            // i - 1 and j - 2 come from the fact that the origin of the tile is "new Point16(1, 2);", so we need to pass the coordinates back to the top left tile. If using a vanilla TileObjectData.Style, make sure you know the origin value.
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3); // this is -1, -1, however, because -1, -1 places the 3 diameter square over all the tiles, which are sent to other clients as an update.
                NetMessage.SendData(87, -1, -1, null, i - 1, j - 2, Type, 0f, 0, 0, 0);
                return -1;
            }
            for (int loop = 0; loop < Main.rand.Next(6); loop++)
            {
                                    // TODO: Add some IDs here
                types.Add(Main.rand.Next(new int[] { }));
            }
            return Place(i - 1, j - 2);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(types)] = types
            };
        }

        public override void Load(TagCompound tag)
        {
            types = tag.GetList<int>(nameof(types));
        }
    }
}
