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

        private bool isActive;
        private int pillarVelocity = 0;
        private float scale = 0.8f;
        private Texture2D pillarS = Main.npcTexture[NPCID.LunarTowerSolar];
        private bool pillarDirection = true;

        public override void Activate(Vector2 position, params object[] args)
        {
            if (!Main.gameMenu)
            {
                TUAPlayer p = Main.LocalPlayer.GetModPlayer<TUAPlayer>();
                if (p != null)
                {
                    if (p.currentDimension == "solar") {
                        isActive = true;
                    }
                }
            }
            /*for (int i = 0; i < Main.npc.Length; i++) {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.EyeofCthulhu) {
                    EoCUp = true;
                }
            }*/
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            //pillarS = Main.npcTexture[NPCID.LunarTowerSolar];
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(25, 0, 0) * 0.9f);
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"), new Vector2(Main.screenPosition.X - Main.LocalPlayer.position.X / 2, 100f + pillarVelocity / 5), ModLoader.GetTexture("Terraria/npc_517").Bounds, Color.White * 0.15f, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"), new Vector2(100f, 100f + pillarVelocity / 5) , ModLoader.GetTexture("Terraria/npc_517").Bounds, Color.White * 0.15f, 0, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"), new Vector2(250f, 400f + pillarVelocity / 20), ModLoader.GetTexture("Terraria/npc_517").Bounds, Color.White * 0.3f, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"), new Vector2(1200f, 200f + pillarVelocity / 5), ModLoader.GetTexture("Terraria/npc_517").Bounds, Color.White * 0.4f, 0, new Vector2(0, 0), 0.69f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"), new Vector2(600f, 300f + pillarVelocity / 5), ModLoader.GetTexture("Terraria/npc_517").Bounds, Color.White * 0.1f, 0, new Vector2(0, 0), 0.15f, SpriteEffects.None, 0f);
        }

        public override bool IsActive()
        {
            return isActive;
            //return EoCUp || EoAUp;
        }

        public override void Reset()
        {
            EoCUp = false;
            EoAUp = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (pillarDirection)
            {
                pillarVelocity--;
            }
            else {
                pillarVelocity++;
            }

            if (pillarVelocity == 50 || pillarVelocity == -50) {
                pillarDirection = !pillarDirection;
            }
        }
    }
}
