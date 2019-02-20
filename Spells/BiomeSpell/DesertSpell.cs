using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TUA.API;

namespace TUA.Spells.BiomeSpell
{
    internal class DesertSpell : BaseBiomeSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Desolation");
            Tooltip.SetDefault("A spell created by the humans a long time ago.");
            Tooltip.AddLine("The legends say that any land touching it get scorched");
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
            item.shoot = mod.ProjectileType("DesertSpellProjectile");
            
        }

        public override bool Cast(Player player)
        {
            return false;
        }

        public override bool GetColor(out Color color)
        {
            color = new Color(237, 201, 175);
            return true;
        }
    }

    internal class DesertSpellProjectile : BaseBiomeSpellProjectile
    {
        public override void Convert(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile.wall == WallID.Dirt || WallID.Sets.Corrupt[tile.wall] || WallID.Sets.Crimson[tile.wall])
            {
                TileSpreadUtils.ChangeWall(x, y, WallID.Sandstone);
            }

            if (tile.type == TileID.Dirt || TileID.Sets.Conversion.Grass[tile.type] || tile.type == TileID.SnowBlock)
            {
                TileSpreadUtils.ChangeTile(x, y, TileID.Sand);
            }

            if (TileID.Sets.Conversion.Stone[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, TileID.Sandstone);
            }

            if (TileID.Sets.Conversion.Ice[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, TileID.HardenedSand);
            }

        }
    }
}
