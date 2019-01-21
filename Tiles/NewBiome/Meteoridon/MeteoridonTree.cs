using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonTree : ModTree
    {

        private Mod mod
        {
            get { return ModLoader.GetMod("TUA"); }
        }

        

        public override int DropWood()
        {
            
            return mod.ItemType("MeteoridonWoodPlank");
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Texture/Tree/MeteoridonTree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft,
            ref int yOffset)
        {
            return mod.GetTexture("Texture/Tree/MeteoridonTree_Tops");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Texture/Tree/MeteoridonTree_Branches");
        }
    }
}
