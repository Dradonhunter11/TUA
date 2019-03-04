using Terraria;

namespace TUA.API.LiquidAPI.LiquidMod
{
    internal class LiquidRef
    {

        public int x;
        public int y;

        public byte _liquidType;
        public byte _liquidAmount;

        public Tile tile;

        public byte liquidType
        {
            get
            {
                return _liquidType;
            }
            set
            {
                _liquidType = value;
            }
        }

        public byte liquidAmount
        {
            get { return _liquidAmount; }
            set
            {
                tile.liquid = value;
                _liquidAmount = value;
            }
            
        }

        public LiquidRef(int x, int y)
        {
            if (Main.tile[x, y] == null)
            {
                Main.tile[x, y] = new Tile();
            }
            tile = Main.tile[x, y];
            this.x = x;
            this.y = y;
            

            if (tile != null)
            {
                if (tile.bTileHeader == 159)
                {
                    _liquidType = 0;
                }
                else if (tile.lava())
                {
                    _liquidType = 1;
                }
                else if (tile.honey())
                {
                    _liquidType = 2;
                }
                this._liquidType = tile.liquidType();
                this._liquidAmount = tile.liquid;
            }
            else
            {
                _liquidType = 255;
            }
        }

        public bool CheckingLiquid()
        {
            return liquidType == 255;
        }

        public bool Liquids(byte index)
        {

            switch (index)
            {
                case 0:
                    return tile.liquidType() == 0;
                case 1:
                    return tile.liquidType() == 1;
                case 2:
                    return tile.liquidType() == 2;
                default:
                    return LiquidCore.liquidGrid[x, y][index];
            }
        }

        public byte liquidsType()
        {
            return liquidType;
        }

        public void SetLiquidsState(byte index, bool value)
        {
            switch (index)
            {
                case 0:
                    _liquidType = 0;
                    tile.liquidType(0);
                    LiquidCore.liquidGrid[x, y][index] = value;
                    break;
                case 1:
                    _liquidType = 1;
                    tile.lava(value);
                    LiquidCore.liquidGrid[x, y][index] = value;
                    break;
                case 2:
                    _liquidType = 2;
                    tile.honey(value);
                    LiquidCore.liquidGrid[x, y][index] = value;
                    break;
                default:
                    _liquidType = index;
                    LiquidCore.liquidGrid[x, y][index] = value;
                    break;
            }
        }

        public byte GetLiquidAmount()
        {
            return tile.liquid;
        }

        public bool NoLiquid()
        {
            return tile.liquid == 0;
        }
    }
}
