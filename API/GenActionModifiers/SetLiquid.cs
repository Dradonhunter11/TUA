using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using TUA.API.LiquidAPI.LiquidMod;

namespace TUA.API.GenActionModifiers
{
    public class SetModdedLiquid : GenAction
    {

        private readonly byte _type;
        private readonly byte _value;

        public SetModdedLiquid(byte type = 0, byte value = 255)
        {
            this._value = value;
            this._type = type;
        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            LiquidCore.liquidGrid[x, y].data = _type;
            GenBase._tiles[x, y].liquid = this._value;
            return this.UnitApply(origin, x, y, args);
        }
    }
}
