using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TUA.CustomScreenShader
{
    class ScreenFog
    {
        private static int _fogTimer = 300;
        private static int _fogTimer2 = 300;

        public static void Update(Texture2D texture)
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

        public static void Draw(Texture2D texture, float fogOpacity1, float fogOpacity2)
        {
            Viewport dimension = Main.graphics.GraphicsDevice.Viewport;
            Main.spriteBatch.Begin();
            for (int i = 0; i < dimension.Width + _fogTimer; i += texture.Width)
            {
                for (int j = 0; j < dimension.Height; j += texture.Height)
                {
                    Main.spriteBatch.Draw(texture, new Rectangle(i - _fogTimer, j, 512, 512), null, Color.White * fogOpacity1, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
            }


            for (int i = 0; i < Main.screenWidth + _fogTimer2; i += texture.Width)
            {
                for (int j = 0; j < Main.screenHeight; j += texture.Height)
                {
                    Main.spriteBatch.Draw(texture, new Rectangle(i - _fogTimer2, j, 512, 512), null, Color.White * fogOpacity2, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.End();
        }
    }
}
