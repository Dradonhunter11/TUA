using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;
using TUA.API;

namespace TUA.Tiles.Machine
{
    class DivinePortal : TUATile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Origin = new Point16(7, 7);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 8;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }
    }
}
