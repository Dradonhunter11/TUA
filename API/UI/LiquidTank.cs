using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TUA.API.CustomInventory.TileEntityContainer;

namespace TUA.API.UI
{
    class LiquidTank : UIElement
    {
        private Texture2D backTexture;
        private Texture2D frontTexture;
        private ITank tank;
        

        public LiquidTank(Texture2D texture, ITank tank)
        {
            backTexture = texture.SubImage(0, 0, 20, 28);
            frontTexture = texture.SubImage(20, 0, 20, 28);
            this.tank = tank;
        }

        public LiquidTank(Texture2D textureFront, Texture2D textureBack, ITank tank)
        {
            backTexture = textureBack;
            frontTexture = textureFront;
            this.tank = tank;
        }

        public override void OnInitialize()
        {
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle style = GetInnerDimensions();
            float calculateLiquidQuantity = tank.GetCurrentAmount() * 100 / tank.GetMaxCapacity();

            spriteBatch.Draw(backTexture, style.Position(), Color.White);
            spriteBatch.Draw();
            if (IsMouseHovering)
            {
                Main.hoverItemName = tank.GetCurrentAmount() + " mB";
            }   
        }
    }
}
