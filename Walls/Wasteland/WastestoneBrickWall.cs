﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TUA.Walls.Wasteland
{
    class WastestoneBrickWall : ModWall
    {
        public override void SetDefaults()
        {
            drop = mod.ItemType("WastestoneBrickWall");
            ModTranslation wall = CreateMapEntryName();
            wall.SetDefault("Wastestone Brick Wall");
            AddMapEntry(new Color(173, 255, 47), wall);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.8f;
            g = 0.2f;
            b = 0.3f;
        }
    }
}
