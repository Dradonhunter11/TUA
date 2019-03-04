using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using TUA.API.LiquidAPI.LiquidMod;

namespace TUA.API.LiquidAPI
{
    public class ModLiquid
    {
        public bool gravity = true;
        public int customDelay = 1; //Default value, aka 
        public Dictionary<Func<bool>, Texture2D> altTexture;
        internal int liquidID = 0;
        internal Color liquidColor;

        public Liquid liquid { get; } = new Liquid();

        public virtual Texture2D texture
        {
            get { return null; }
        }
        internal int liquidIndex;

        public virtual void Update()
        {
        }

        //Normally trigger if gravity is at false
        public virtual bool CustomPhysic(int x, int y)
        {
            LiquidRef liquidLeft = LiquidCore.grid[x - 1, y];
            LiquidRef liquidRight = LiquidCore.grid[x + 1, y];
            LiquidRef liquidUp = LiquidCore.grid[x, y - 1];
            LiquidRef liquidDown = LiquidCore.grid[x, y + 1];
            LiquidRef liquidSelf = LiquidCore.grid[x, y];

            if (!Liquid.quickFall)
            {
                if (liquid.delay < 50)
                {
                    ++liquid.delay;
                    return false;
                }
                liquid.delay = 0;
                if (liquidLeft.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidRight.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidUp.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }

                if (liquidDown.liquidsType() == liquidSelf.liquidsType())
                {
                    LiquidExtension.AddModdedLiquidAround(liquid.x, liquid.y);
                }
            }
            return true;
        }

        public virtual float SetLiquidOpacity()
        {
            return 1;
        }
        

        public virtual void PreDrawValueSet(ref bool bg, ref int style, ref float Alpha)
        {
        }

        public virtual void PreDraw(TileBatch batch)
        {
        }

        public virtual void Draw(TileBatch batch)
        {
        }

        public virtual void PostDraw(TileBatch batch)
        {
        }

        public virtual void PlayerInteraction(Player target)
        {
        }

        public virtual void NpcInteraction(NPC target)
        {
        }

        public virtual void ItemInteraction(Item target)
        {
        }

        public virtual void LiquidInteraction(ModLiquid target)
        {
        }
    }
}
