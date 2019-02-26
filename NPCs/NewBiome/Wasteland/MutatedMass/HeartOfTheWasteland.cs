using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.NPCs.NewBiome.Wasteland.MutatedMass
{
    [AutoloadBossHead]
    class HeartOfTheWasteland : TUAModNPC
    {
        public bool SleepState { private get; set; }

        private static readonly string HEAD_PATH = "TUA/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head";

        private Texture2D tentacle;

        private Vector2 topBlock;

        public override string BossHeadTexture {
            get { return "TUA/NPCs/NewBiome/Wasteland/MutatedMass/HeartOfTheWasteland_head0"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the wasteland");
            Main.npcFrameCount[npc.type] = 2;
        }


        public override void SetDefaults()
        {
            npc.width = (int) (158 * 1.7f);
            npc.height = (int) (222 * 1.7f);
            npc.lifeMax = 9000;
            npc.damage = 60;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 20, 50, 25);
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.boss = true;
            npc.immortal = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.scale = 2f;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            SleepState = true;

            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }

            tentacle = mod.GetTexture("Texture/NPCs/Heart_Tentacle");
        }

        public override void AI()
        {
            SearchTopBlock();
            if (SleepState)
            {
                npc.dontTakeDamage = true;
                return;
            }

            /*
             * Can someone explain what this is for - Agrair
            npc.boss = true;
            npc.immortal = false;
            */
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.DistanceSQ(npc.position) < 22500) // 150 tiles
                {
                    if (!Main.dedServ) Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAway{Main.rand.Next(4)}", npc.GivenName),
                        new Color(66, 244, 116));
                    player.position = npc.position +=
                        new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(-15, 15));
                    var point = player.position.ToTileCoordinates();
                    if (!Main.dedServ && Main.tile[point.X, point.Y].nactive())
                        Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAwayStuck", npc.GivenName),
                            new Color(66, 244, 116));
                    // Item6 is magic mirror
                    if (!Main.dedServ) Main.PlaySound(SoundID.Item6, npc.position);
                }
            }
        }

        private void SearchTopBlock()
        {
            Tile tile = Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)];
            int y = (int) (npc.Center.Y / 16);
            int x = (int) (npc.Center.X / 16);
            do
            {
                y--;
                tile = Main.tile[x, y];
            } while (!tile.active());
            topBlock = new Vector2(x * 16, y * 16);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void BossHeadSlot(ref int index)
        {
            if (SleepState)
            {
                index = NPCHeadLoader.GetBossHeadSlot(HEAD_PATH + "0");
            }
            else
            {
                index = NPCHeadLoader.GetBossHeadSlot(HEAD_PATH + "1");
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (SleepState)
            {
                npc.frame.Y = frameHeight;
                return;
            } 
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 npcPosition = npc.Center;
            int distanceFromTop = (int) (npc.Center.Y - topBlock.Y);
            int textureHeight = tentacle.Height;
            do
            {
                int distanceToDraw = textureHeight;
                if (distanceFromTop < textureHeight)
                {
                    distanceToDraw = distanceFromTop;
                }

                Vector2 temp = new Vector2(npcPosition.X - (tentacle.Width / 2) - Main.screenPosition.X, npcPosition.Y - distanceFromTop - Main.screenPosition.Y);
                spriteBatch.Draw(mod.GetTexture("Texture/NPCs/Heart_Tentacle"), temp, new Rectangle(0, 0, tentacle.Width, distanceToDraw), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                distanceFromTop -= textureHeight;
            } while (distanceFromTop >  0);

            return true;
        }
    }
}
