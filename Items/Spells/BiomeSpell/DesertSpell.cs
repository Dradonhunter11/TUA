using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TUA.API;

namespace TUA.Items.Spells.BiomeSpell
{
    public class DesertSpell : BaseBiomeSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Desolation");
            Tooltip.SetDefault("A spell created by the humans a long time ago.");
            Tooltip.AddLine("Legend says it wields a flame so great, it " +
                "\ncan purge the land it scorches.");
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

    public class DesertSpellProjectile : BaseBiomeSpellProjectile
    {
        public override void ConversionTypes(out byte wall, out ushort dirt, out ushort stone)
        {
            wall = WallID.Sandstone;
            dirt = TileID.Sand;
            stone = TileID.Sandstone;
        }

        public override void Convert(int x, int y)
        {
            base.Convert(x, y);

            if (TileID.Sets.Conversion.Ice[Main.tile[x, y].type])
            {
                TileSpreadUtils.ChangeTile(x, y, TileID.HardenedSand);
            }
        }
    }
}
