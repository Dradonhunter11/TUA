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

        public LiquidTank(Texture2D texture, int maxCapacity)
        {
            backTexture = texture.SubImage(2, 1);
            frontTexture = texture.SubImage(2, 0);
        }

        public LiquidTank(Texture2D textureFront, Texture2D textureBack, int maxCapacity)
        {

        }

        public override void OnInitialize()
        {
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle style = GetInnerDimensions();
            if (IsMouseHovering)
            {
                Main.hoverItemName = tank.GetCapacity() + " MB";
            }

            
        }

        
    }
}
