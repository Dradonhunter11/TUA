using Terraria;
using Terraria.ModLoader;

namespace TUA.Tiles.EoA
{
    class Arena : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            this.minPick = 99999;
            AddMapEntry(new Microsoft.Xna.Framework.Color(0, 0, 0));
        }
    }
}
