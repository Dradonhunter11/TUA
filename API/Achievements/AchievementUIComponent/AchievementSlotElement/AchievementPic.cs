using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrariaUltraApocalypse.API.Achievements.AchievementUIComponent.AchievementSlotElement
{
    class AchievementPic : UIElement
    {
        private Texture2D picture;

        public override void OnInitialize()
        {
            Width.Set(64, 0f);
            Height.Set(64, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            picture = ModLoader.GetMod("TerrariaUltraApocalypse").GetTexture("Achievement/Default");
            CalculatedStyle innerDim = base.GetInnerDimensions();
            Vector2 picturePos = new Vector2(innerDim.X + 5f, innerDim.Y + 5);
            Rectangle r = picture.Bounds;
            

            spriteBatch.Draw(picture, new Vector2(innerDim.X + 56f, innerDim.Y + 54), r, Color.White, 0f, r.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
}
