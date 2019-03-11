using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TUA.API.UI;

namespace TUA.UI.Cobblegen
{
    class CobbleGenUI : UIState
    {
        private CustomizableUIPanel panel;
        private UIImageButton exitButton;

        private static Texture2D background = TUA.instance.GetTexture("Texture/UI/Panel");

        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;

        public override void OnInitialize()
        {
            panel = new CustomizableUIPanel(background);


            exitButton = new UIImageButton(TUA.instance.GetTexture("Texture/X_ui"));
            exitButton.Width.Set(20f, 0);
            exitButton.Height.Set(22f, 0);
            exitButton.HAlign = 1f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            
        }
    }
}
