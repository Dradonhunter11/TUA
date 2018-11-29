using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.World;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.World
{
    class OreSpread : ModWorld
    {

        public override void PostUpdate()
        {
            
            if (Main.netMode != 1)
            {
                for (int k = 0; k < Main.maxTilesX * Main.maxTilesY * 3E-05 * Main.worldRate; k++)
                {
                    int x = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int y = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1);
                    if (Main.tile[x, y] != null && Main.tile[x, y].liquid <= 32 && Main.tile[x, y].nactive())
                    {
                        UpdateTile(x, y);
                    }
                }
                for (int k = 0; k < Main.maxTilesX * Main.maxTilesY * 1.5E-05 * Main.worldRate; k++)
                {
                    int x = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                    int y = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                    if (Main.tile[x, y] != null && Main.tile[x, y].liquid <= 32 && Main.tile[x, y].nactive())
                    {
                        UpdateTile(x, y);
                    }
                }

            }
        }


        private void UpdateTile(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (!tile.inActive() && (tile.type == 6 || tile.type == 7 || tile.type == 8 || tile.type == 9 || tile.type == 22 || tile.type == 58 || tile.type == 107 || tile.type == 108 || tile.type == 110 || tile.type == 166 || tile.type == 167 || tile.type == 168 || tile.type == 169 || tile.type == 204 || tile.type == 221 || tile.type == 222 || tile.type == 223 && WorldGen.genRand.Next(100) == 0))
            {
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    int toX = x + WorldGen.genRand.Next(-3, 4);
                    int toY = y + WorldGen.genRand.Next(-3, 4);
                    bool tileChanged = false;
                    int targetType = Main.tile[toX, toY].type;
                    if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 6;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 7;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        for (int j = toX - 1; j <= toX + 1; j++)
                        {
                            for (int k = toY - 1; k <= toY + 1; k++)
                            {
                                if (!Main.tile[j, k].active())
                                {
                                    tileChanged = true;
                                }
                                if (Main.tile[j, k].lava() && Main.tile[j, k].liquid > 0)
                                {
                                    tileChanged = false;
                                    j = toX + 1;
                                    break;
                                }
                            }
                        }
                        if (tileChanged)
                        {
                            Main.tile[toX, toY].type = 8;
                        }
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 9;
                        tileChanged = true;
                    }
                    else if (targetType == 25)
                    {
                        Main.tile[toX, toY].type = 22;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 58;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 107;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 108;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 110;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 166;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 167;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 168;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 169;
                        tileChanged = true;
                    }
                    else if (targetType == 203)
                    {
                        Main.tile[toX, toY].type = 204;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 221;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 222;
                        tileChanged = true;
                    }
                    else if (targetType == 1)
                    {
                        Main.tile[toX, toY].type = 223;
                        tileChanged = true;
                    }
                    if (tileChanged)
                    {
                        if (WorldGen.genRand.Next(1000) == 0)
                        {
                            flag = true;
                        }
                        WorldGen.SquareTileFrame(toX, toY, true);
                        NetMessage.SendTileSquare(-1, toX, toY, 1);
                    }
                }
            }
        }
    }
}
