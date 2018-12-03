using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrariaUltraApocalypse.Tiles.Furniture.Coins
{
    abstract class TUACoins : ModTile
    {
        public abstract int coinDropID { get; }
        public abstract int coinProjectileID { get; }

        public override void SetDefaults()
        {
            Main.tilePile[Type] = true;
            //TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            //TileObjectData.addTile(Type);
            soundStyle = 18;
            dustType = DustID.PlatinumCoin;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 14, 14, coinDropID);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Main.tile[i, j];
            Tile tile2 = Main.tile[i, j - 1];
            Tile tile3 = Main.tile[i, j + 1];
            int tileType = tile.type;
            if (!WorldGen.noTileActions && tile.active() && (tileType == Type))
            {
                if (Main.netMode == 0)
                {
                    if (tile3 != null && !tile3.active())
                    {
                        bool flag18 = !(tile2.active() && (TileID.Sets.BasicChest[(int)tile2.type] || TileID.Sets.BasicChestFake[(int)tile2.type] || tile2.type == 323 || TileLoader.IsDresser(tile2.type)));
                        if (flag18)
                        {
                            int damage = 10;
                            int projectileType = 0;
                            if (tileType == Type)
                            {
                                projectileType = coinProjectileID;
                                damage = 0;
                            }
                            tile.ClearTile();
                            int num77 = Projectile.NewProjectile((float)(i * 16 + 8), (float)(j * 16 + 8), 0f, 0.41f, projectileType, damage, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num77].ai[0] = 1f;
                            WorldGen.SquareTileFrame(i, j, true);
                        }
                    }
                }
                else if (Main.netMode == 2 && tile3 != null && !tile3.active())
                {
                    bool flag19 = !(tile2.active() && (TileID.Sets.BasicChest[(int)tile2.type] || TileID.Sets.BasicChestFake[(int)tile2.type] || tile2.type == 323 || TileLoader.IsDresser(tile2.type)));
                    if (flag19)
                    {
                        int damage2 = 10;
                        int projectileType = 0;
                        if (tileType == Type)
                        {
                            projectileType = coinProjectileID;
                            damage2 = 0;
                        }

                        tile.active(false);
                        bool flag20 = false;
                        for (int m = 0; m < 1000; m++)
                        {
                            if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == projectileType && Math.Abs(Main.projectile[m].timeLeft - 3600) < 60 && Main.projectile[m].Distance(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 10))) < 4f)
                            {
                                flag20 = true;
                                break;
                            }
                        }
                        if (!flag20)
                        {
                            int num79 = Projectile.NewProjectile((float)(i * 16 + 8), (float)(j * 16 + 8), 0f, 2.5f, projectileType, damage2, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num79].velocity.Y = 0.5f;
                            Projectile expr_7AAA_cp_0 = Main.projectile[num79];
                            expr_7AAA_cp_0.position.Y = expr_7AAA_cp_0.position.Y + 2f;
                            Main.projectile[num79].netUpdate = true;
                        }
                        NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                        WorldGen.SquareTileFrame(i, j, true);
                    }
                }
            }
            return true;
        }
    }
}
