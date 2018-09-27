using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using TerrariaUltraApocalypse.API.FurnaceRework;
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;

namespace TerrariaUltraApocalypse.API.TerraEnergy.UI
{
    class UIEnergyBar : UIElement
    {

        private Texture2D energyBar;
        private Texture2D fullEnergyBar;

        public Core boundCore;

        public UIEnergyBar(Core boundCore)
        {
            this.boundCore = boundCore;
        }

        

        public override void OnInitialize()
        {
            energyBar = TerrariaUltraApocalypse.instance.GetTexture("API/TerraEnergy/Texture/TerraEnergyBar");
            fullEnergyBar = TerrariaUltraApocalypse.instance.GetTexture("API/TerraEnergy/Texture/TerraEnergyBarFilled");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle innerDim = base.GetInnerDimensions();
            Vector2 picturePos = new Vector2(innerDim.X, innerDim.Y);
            Rectangle r = energyBar.Bounds;


            spriteBatch.Draw(energyBar, new Vector2(innerDim.X + 386, innerDim.Y + 14), r, Color.White, 0f, r.Size(), new Vector2(1f, 0.5f), SpriteEffects.None, 0f);
            if (IsMouseHovering)
            {

                if (boundCore is BudgetCore)
                {

                    Main.hoverItemName = boundCore.getCurrentEnergyLevel() + " / " +
                                         boundCore.getMaxEnergyLevel() + " Fuel";
                }
                else
                {
                    Main.hoverItemName = boundCore.getCurrentEnergyLevel() + " / " +
                                         boundCore.getMaxEnergyLevel() + " TE";
                }
                
                //spriteBatch.Draw(Main.itemTexture[ItemID.Actuator], new Vector2(Main.mouseX, Main.mouseY + 20), r, Color.White, 0f, r.Size(), new Vector2(1f, 0.5f), SpriteEffects.None, 0f);
                //spriteBatch.DrawString(Main.fontMouseText, currentEntity.energy.getCurrentEnergyLevel() + " / " + currentEntity.energy.getMaxEnergyLevel() + " TE", new Vector2(Main.mouseX, Main.mouseY + 20), Color.White);
            }

            float percent = (boundCore.getCurrentEnergyLevel() * 100 / boundCore.getMaxEnergyLevel());
            Rectangle sourceRectangle = new Rectangle(0, 0, (int) (386 * (percent / 100)), 28);
            spriteBatch.Draw(fullEnergyBar, new Vector2(innerDim.X + 386, innerDim.Y + 14), sourceRectangle, Color.White, 0f, r.Size(), new Vector2(1f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
