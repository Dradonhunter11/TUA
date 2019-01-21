using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Dimlibs;

namespace TUA.NPCs
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

        private int maxSpawns = 50;
        private float[] zDistance = new float[50];
        private float[] xPos = new float[50];
        private float[] yPos = new float[50];


        public override void Activate(Vector2 position, params object[] args)
        {
            if (!Main.gameMenu)
            {

                if (Dimlibs.Dimlibs.getPlayerDim() != "overworld")
                {
                    if (Dimlibs.Dimlibs.getPlayerDim() == "solar")
                    {
                        isActive = true;
                    }
                    for (int i = 0; i < 50; i++)
                    {
                        zDistance[i] = Main.rand.NextFloat(0.1f, 0.7f); // get a random 3rd Dimension distance
                        xPos[i] = (Main.rand.NextFloat(0, Main.maxTilesX)) * 16; //makes so that objects dont spawn outside of the world
                        yPos[i] = (Main.rand.NextFloat(50, 51)); //same for Y
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
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(70, 0, 0) * 0.9f);

            for (int i = 0; i < 50; i++)
            {
                spriteBatch.Draw(ModLoader.GetTexture("Terraria/npc_517"),
                new Vector2(Main.screenPosition.X / 2f - xPos[i], (yPos[i] + pillarVelocity / 5)),
                ModLoader.GetTexture("Terraria/npc_517").Bounds,
                Color.White * zDistance[i],
                0,
                new Vector2(0, 0),
                zDistance[i],
                SpriteEffects.None,
                    0f
            );
            }
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
            Main.cloudLimit = 0;

            if (pillarDirection)
            {
                pillarVelocity--;
            }
            else
            {
                pillarVelocity++;
            }

            if (pillarVelocity == 50 || pillarVelocity == -50)
            {
                pillarDirection = !pillarDirection;
            }
        }
    }
}
