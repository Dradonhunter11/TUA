using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria.ModLoader;
using Terraria;
using Terraria.World;
using Terraria.ID;
using TerrariaUltraApocalypse;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace TerrariaUltraApocalypse
{
    class MainModifier : Main
    {
        SpriteFont Font1;
        Vector2 FontPos;

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/UI/ButtonDelete"), new Vector2(80.0f, 80.8f), Color.Black);
            spriteBatch.End();
            
            Main instance2 = Main.instance;
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Font1 = Content.Load<SpriteFont>("Courier New");

            /*FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 2);*/
            //System.IO.File.WriteAllText(@"C:\TerrariaTag\g.txt", "I got loaded!");
            base.LoadContent();
        }
    }
}
