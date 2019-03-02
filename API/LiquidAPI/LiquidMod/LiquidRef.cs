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

        public byte liquidType
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

        public byte liquidAmount
        {
            get => tile.liquid;
            set => tile.liquid = value;
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
                    liquidType = 0;
                }
                else if (tile.lava())
                {
                    liquidType = 1;
                }
                else if (tile.honey())
                {
                    liquidType = 2;
                }
            }
            else
            {
                liquidType = 255;
            }
        }

        public bool checkingLiquid()
        {
            return liquidType == 255;
        }

        public bool liquids(byte index)
        {
            if (index <= 2)
            {
                switch (index)
                {
                    case 0:
                        return liquidType == 0;
                    case 1:
                        return liquidType == 1;
                    case 2:
                        return liquidType == 2;
                }
            }
            return LiquidCore.liquidGrid[x, y][index];
        }

        public byte liquidsType()
        {
            return liquidType;
        }

        public void setLiquidsState(byte index, bool value)
        {
            if (index <= 2)
            {
                switch (index)
                {
                    case 0:
                        liquidType = 0;
                        tile.liquidType(0);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                    case 1:
                        liquidType = 1;
                        tile.lava(value);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                    case 2:
                        liquidType = 2;
                        tile.honey(value);
                        LiquidCore.liquidGrid[x, y][index] = value;
                        break;
                }
            }
            else
            {
                liquidType = index;
                LiquidCore.liquidGrid[x, y][index] = value;
            }
        }

        public byte getLiquidAmount()
        {
            return tile.liquid;
        }

        public bool noLiquid()
        {
            return tile.liquid == 0;
        }
    }
}
