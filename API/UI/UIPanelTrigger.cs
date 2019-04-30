using Microsoft.Xna.Framework.Graphics;

namespace TUA.API.UI
{
    class UIPanelTrigger : CustomizableUIPanel
    {
        public bool isVisible;

        public UIPanelTrigger(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                base.Draw(spriteBatch);
            }
        }

        
    }
}
