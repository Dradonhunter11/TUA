using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TUA.API.Inventory.TileEntityContainer;

namespace TUA.API.UI
{
    public class LiquidTank : UIElement
    {
        private readonly ITank _tank;

        public LiquidTank(Texture2D texture, ITank tank)
        {
            BackTexture = texture.SubImage(0, 0, 20, 28);
            FrontTexture = texture.SubImage(20, 0, 20, 28);

            _tank = tank;
        }

        public LiquidTank(Texture2D textureFront, Texture2D textureBack, ITank tank)
        {
            BackTexture = textureBack;
            FrontTexture = textureFront;

            _tank = tank;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle style = GetInnerDimensions();
            float calculateLiquidQuantity = _tank.GetCurrentAmount() * 100 / _tank.GetMaxCapacity();

            spriteBatch.Draw(BackTexture, style.Position(), Color.White);

            if (IsMouseHovering)
                Main.hoverItemName = _tank.GetCurrentAmount() + " mB";
        }

        public Texture2D BackTexture { get; }
        public Texture2D FrontTexture { get; }
    }
}
