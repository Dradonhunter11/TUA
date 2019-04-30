using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TUA.Walls.Hell
{
    class AshWall : ModWall
    {
        public override void SetDefaults()
        {
            drop = mod.ItemType("AshWall");
            AddMapEntry(new Color(32, 32, 32));
        }
    }
}
