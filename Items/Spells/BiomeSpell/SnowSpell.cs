using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TUA.API;

namespace TUA.Items.Spells.BiomeSpell
{
    class SnowSpell : BaseBiomeSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Blizzard");
            Tooltip.SetDefault("A spell created by the humans a long time ago.");
            Tooltip.AddLine("The legends say that any land touching it get instantly frozen");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.useAnimation = 2;
            item.useTime = 2;
            item.useStyle = 5;
            item.UseSound = SoundID.Item13;
            item.knockBack = 4;
            item.damage = 30;
            item.autoReuse = true;
            item.noMelee = true;
            item.magic = true;
            item.mana = 20;
            item.shootSpeed = 13;
            item.shoot = mod.ProjectileType("SnowSpellProjectile");

        }

        public override bool Cast(Player player)
        {
            return false;
        }

        public override bool GetColor(out Color color)
        {
            color = Color.White;
            return true;
        }
    }

    class SnowSpellProjectile : BaseBiomeSpellProjectile
    {
        public override void ConversionTypes(out byte wall, out ushort dirt, out ushort stone)
        {
            wall = WallID.SnowWallUnsafe;
            dirt = TileID.SnowBlock;
            stone = TileID.IceBlock;
        }
    }
}
