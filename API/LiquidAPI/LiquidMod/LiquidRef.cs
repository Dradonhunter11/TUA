using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TUA.API.LiquidAPI.LiquidMod
{
    class LiquidRef
    {

        public int x;
        public int y;
        public Tile tile;        

        public byte liquid
        {
            get
            {
                return LiquidCore.liquidGrid[x, y];
            }
            set
            {
                LiquidCore.liquidGrid[x, y] = value;
            }
        }

        public LiquidRef(int x, int y)
        {
            tile = Main.tile[x, y];
            this.x = x;
            this.y = y;

            if (tile != null)
            {
                if (tile.bTileHeader == 159)
                {
                    liquid = 0;
                }
                else if (tile.lava())
                {
                    liquid = 1;
                }
                else if (tile.honey())
                {
                    liquid = 2;
                }
            }
            else
            {
                liquid = 255;
            }
        }

        public bool checkingLiquid()
        {
            return liquid == 255;
        }

        public bool liquids(byte index)
        {
            if (index <= 2)
            {
                switch (index)
                {
                    case 0:
                        return liquid == 0;
                    case 1:
                        return liquid == 1;
                    case 2:
                        return liquid == 2;
                }
            }
            return LiquidCore.liquidGrid[x, y][index];
        }

        public byte liquidsType()
        {
            return liquid;
        }

        public void setLiquidsState(byte index, bool value)
        {
            if (index <= 2)
            {
                switch (index)
                {
                    case 0:
                        liquid = 0;
                        tile.liquidType(0);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                    case 1:
                        liquid = 1;
                        tile.lava(value);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                    case 2:
                        liquid = 2;
                        tile.honey(value);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                }
            }
            else
            {
                liquid = index;
                LiquidCore.liquidGrid[x, y][index] = value;
            }
        }

        public byte getLiquidLayer()
        {
            return tile.liquid;
        }

        public bool noLiquid()
        {
            return tile.liquid == 0;
        }
    }
}
