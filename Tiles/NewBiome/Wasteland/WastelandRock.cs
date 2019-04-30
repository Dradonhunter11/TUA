using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Wasteland
{
    class WastelandRock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(64, 47, 73));
        }
    }
}
