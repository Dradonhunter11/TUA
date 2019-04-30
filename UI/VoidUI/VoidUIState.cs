using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TUA.Void;

namespace TUA.UI.VoidUI
{
    class VoidUIState : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            VoidPlayer player = Main.LocalPlayer.GetModPlayer<VoidPlayer>();
            float percent = (player.voidAffinity * 100f / player.maxVoidAffinity);

            spriteBatch.Draw(DrawCircle(104, 66, 1f), new Vector2(3, 78), Color.Black);
            spriteBatch.Draw(DrawCircle(100, 70, 1f), new Vector2(5, 80), Color.White);
            spriteBatch.Draw(DrawCircle(100, 70, percent / 100), new Vector2(5, 80), Color.Purple);
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, $"{percent}%", 34f, 117f, Color.MediumPurple, Color.Black, Vector2.Zero, 0.5f);

            Rectangle rec = new Rectangle(3, 78, 104, 104);
            bool isMouseInRec = rec.Contains(Main.MouseScreen.ToPoint());
            Vector2 fontSize = (isMouseInRec) ? Main.fontDeathText.MeasureString($"{player.voidAffinity}/{player.maxVoidAffinity}") * 0.5f : Main.fontDeathText.MeasureString("Void affinity") * 0.4f;

            if (isMouseInRec)
            {
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, $"{player.voidAffinity}/{player.maxVoidAffinity}", (104f / 2f - fontSize.X / 2f), 78f + 104f + fontSize.Y / 2, Color.MediumPurple, Color.Black, Vector2.Zero, 0.5f);
            }
            else
            {
                Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, $"Void affinity", 104f / 2f - fontSize.X / 2f, 78f + 104f + fontSize.Y / 2, Color.MediumPurple, Color.Black, Vector2.Zero, 0.4f);
            }
        }

        public Texture2D DrawCircle(int diameter, int diameterInterior, float percent)
        {
            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiusInterior = diameterInterior / 2f;
            float radiusSquared = radius * radius;
            float radiusSquaredInterior = radiusInterior * radiusInterior;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {

                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    float anglePercent = (percent * MathHelper.TwoPi) - MathHelper.Pi;
                    float angle = (float)Math.Atan2(pos.Y, pos.X);

                    if (anglePercent > angle && pos.LengthSquared() < radiusSquared && pos.LengthSquared() > radiusSquaredInterior)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
