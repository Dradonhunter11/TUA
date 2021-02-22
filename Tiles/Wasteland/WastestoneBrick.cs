using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Tiles.Wasteland
{
    class WastestoneBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation tileTranslation = CreateMapEntryName();
            tileTranslation.SetDefault("Wastestone Brick");
            AddMapEntry(new Color(173, 255, 47));
        }
    }
}
