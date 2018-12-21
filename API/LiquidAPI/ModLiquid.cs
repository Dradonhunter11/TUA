using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;

namespace TerrariaUltraApocalypse.API.LiquidAPI
{
    public class ModLiquid
    {
        private readonly Liquid l = new Liquid();
        public bool gravity = true;
        public int customDelay = 1; //Default value, aka 
        public Dictionary<Func<bool>, Texture2D> altTexture;
        

        public Liquid liquid
        {
            get
            {
                return l;
            }
        }

        public virtual Texture2D texture
        {
            get { return null; }
        }
        internal int liquidIndex;

        public virtual void Update()
        {
        }

        //Normally trigger if gravity is at false
        public virtual bool CustomPhysic()
        {
            return false;
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
