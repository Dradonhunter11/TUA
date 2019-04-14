using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Items.Spells.BiomeSpell
{
    public abstract class BaseBiomeSpellProjectile : ModProjectile
    {
        public override string Texture => "TUA/Texture/Projectiles/DefaultProjectile";

        public virtual void Convert(int x, int y)
        {
            ConversionTypes(out byte wall, out ushort dirt, out ushort stone);
            Terraform(x, y, wall, dirt, stone);
        }

        public Color color = Color.White;

        public override void SetDefaults()
        {
            projectile.width = 33;
            projectile.height = 33;
            projectile.scale = 3f;
            projectile.damage = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.MaxUpdates = 1;
            projectile.timeLeft = 50;
            projectile.tileCollide = false;
            projectile.aiStyle = 0;
        }

        public sealed override void AI()
        {
            base.AI();
            int topPosition = (int) (projectile.position.Y / 16) - 1;
            int leftPosition = (int) (projectile.position.X / 16) - 1;
            int rightPosition = (int) ((projectile.position.X + (float)projectile.width) / 16) + 2;
            int bottomPosition = (int) ((projectile.position.Y + (float) projectile.height) / 16) + 2;

            if (leftPosition < 0)
            {
                leftPosition = 0;
            }

            if (topPosition < 0)
            {
                topPosition = 0;
            }

            if (rightPosition > Main.maxTilesX)
            {
                rightPosition = Main.maxTilesX;
            }

            if (bottomPosition > Main.maxTilesY)
            {
                bottomPosition = Main.maxTilesY;
            }

            for (int x = leftPosition; x < rightPosition; x++) {
                for (int y = topPosition; y < bottomPosition; y++)
                {
                    Convert(x, y);
                }
            }

            for (int k = 0; k < 2; k++) Dust.NewDust(projectile.position, 1, 1, 54, 0, 0, 0, color, 1f);
        }

        public abstract void ConversionTypes(out byte wall, out ushort dirt, out ushort stone);

        public static void Terraform(int x, int y, byte wall, ushort dirt, ushort stone)
        {
            Tile tile = Main.tile[x, y];
            if (tile.wall == WallID.Dirt || WallID.Sets.Corrupt[tile.wall] || WallID.Sets.Crimson[tile.wall])
            {
                TileSpreadUtils.ChangeWall(x, y, wall);
            }

            if (tile.type == TileID.Dirt || TileID.Sets.Conversion.Grass[tile.type]
                || tile.type == TileID.SnowBlock, || TileID.Sets.Conversion.Sand[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, dirt);
            }

            if (TileID.Sets.Conversion.Stone[tile.type]
                || TileID.Sets.Conversion.Sandstone[tile.type] || TileID.Sets.Conversion.HardenedSand[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, stone);
            }
        }
    }
}
