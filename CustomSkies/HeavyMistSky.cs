using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace TerrariaUltraApocalypse.CustomSkies
{
    internal class HeavyMistSky : CustomSky
    {
        private bool _isLeaving = false;
        private bool _isActive = false;
        private readonly float fogOpacity1 = 0.5f;
        private readonly float fogOpacity2 = 0.4f;
        private int _fogTimer = 300;
        private int _fogTimer2 = 300;
        private Texture2D texture = TerrariaUltraApocalypse.instance.GetTexture("CustomScreenShader/HeavyMist");

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
            _isLeaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            _isActive = false;
            _isLeaving = true;
        }

        public override void Update(GameTime gameTime)
        {
            _fogTimer--;
            _fogTimer2 -= 3;
            if (_fogTimer <= 0)
            {
                _fogTimer = texture.Width;
            }

            if (_fogTimer2 <= 0)
            {
                _fogTimer2 = texture.Width;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Viewport dimension = Main.graphics.GraphicsDevice.Viewport;

            for (int i = 0; i < dimension.Width + _fogTimer; i += texture.Width)
            {
                for (int j = 0; j < dimension.Height; j += texture.Height)
                {
                    spriteBatch.Draw(texture, new Rectangle(i - _fogTimer, j, 512, 512), null, Color.White * fogOpacity1, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
            }


            for (int i = 0; i < Main.screenWidth + _fogTimer2; i += texture.Width)
            {
                for (int j = 0; j < Main.screenHeight; j += texture.Height)
                {
                    spriteBatch.Draw(texture, new Rectangle(i - _fogTimer2, j, 512, 512), null, Color.White * fogOpacity2, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
            }
        }

        

        public override bool IsActive()
        {
            return _isActive;
        }

        public override void Reset()
        {
            _isActive = false;
        }
    }
}
