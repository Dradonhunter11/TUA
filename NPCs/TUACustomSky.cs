using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.NPCs
{
    class TUACustomSky : CustomSky
    {
        private bool EoCUp;
        private bool EoAUp;

        public override void Activate(Vector2 position, params object[] args)
        {
            for (int i = 0; i < Main.npc.Length; i++) {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.EyeofCthulhu) {
                    EoCUp = true;
                }
            }
        }

        public override void Deactivate(params object[] args)
        {
            EoCUp = false;
            EoAUp = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (EoCUp) {
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(30, 30, 30) * 0.5f);
            }
        }

        public override bool IsActive()
        {
            return EoCUp || EoAUp;
        }

        public override void Reset()
        {
            EoCUp = false;
            EoAUp = false;
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
