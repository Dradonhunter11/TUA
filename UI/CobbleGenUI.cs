/*
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TUA.API.UI;

namespace TUA.UI
{
    class CobbleGenUI : UIState
    {
        private CustomizableUIPanel panel;
        private UIImageButton exitButton;

        private static readonly Texture2D background = TUA.instance.GetTexture("Texture/UI/Panel");

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
*/