using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TUA.API;
using TUA.API.TerraEnergy.Block;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;
using TUA.Items.Misc.Spawner;

namespace TUA.Dimension.Block
{
    class MobSpawner : TUABlock
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<MobSpawnerEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }


        public override bool CanPlace(int i, int j)
        {
            if (base.CanPlace(i, j))
            {
                int index = mod.GetTileEntity<MobSpawnerEntity>().Place(i, j);
                MobSpawnerEntity te = (MobSpawnerEntity)TileEntity.ByID[index];
                te.Hook_AfterPlacement(i, j, 0, 0, 0);
                return true;
            } 
            return false;
        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = mod.GetTileEntity<MobSpawnerEntity>().Find(left, top);

            if (index == -1)
            {
                Main.NewText("fail");
                return;
            }

            MobSpawnerEntity mse = (MobSpawnerEntity)TileEntity.ByID[index];
            if (Main.LocalPlayer.HeldItem.modItem is SoulCrystal)
            {
                SoulCrystal sc = Main.LocalPlayer.HeldItem.modItem as SoulCrystal;
                if (sc.isFull())
                {
                    mse.setMob(sc.getMobID());
                }
            }
        }

        public override void readData(int x, int y)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[x, y];

            int left = x - (tile.frameX / 18);
            int top = y - (tile.frameY / 18);

            int index = mod.GetTileEntity<MobSpawnerEntity>().Find(left, top);

            if (index == -1)
            {
                Main.NewText("fail");
                return;
            }

            MobSpawnerEntity mse = (MobSpawnerEntity)TileEntity.ByID[index];
            Main.NewText("Current mob in spawner : " + mse.getCurrentMobName());
            Main.NewText("Time in tick until next spawn : " + mse.getCurrentTimer());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            mod.GetTileEntity<MobSpawnerEntity>().Kill(i, j);
        }
    }

    public class MobSpawnerEntity : ModTileEntity
    {
        private int mobID = -1;
        private int timerUntilNextSpawn = 600;
        private bool deactivated = false;

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("mobID", mobID);
            tag.Add("timer", timerUntilNextSpawn);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            mobID = tag.GetAsInt("mobID");
            timerUntilNextSpawn = tag.GetAsInt("timer");
        }

        public override void Update()
        {
            if (mobID == -1)
            {
                int[] mobChoice = { NPCID.StardustCellBig, NPCID.StardustSpiderBig };
                mobID = Main.rand.Next(mobChoice);
            }


            timerUntilNextSpawn--;
            if (timerUntilNextSpawn == 0)
            {
                List<Point16> mobCoordinateList = new List<Point16>();
                for (int i = Position.X - 10; i < Position.X + 10; i++)
                {
                    for (int j = Position.Y - 5; j < Position.Y + 1; j++)
                    {
                        if (checkValidPosition(i, j))
                        {
                            mobCoordinateList.Add(new Point16(i,j));
                        }

                        if (mobCoordinateList.Count == 4)
                        {
                            goto exitloop;
                        }
                    }
                }
                exitloop:;
                spawnMob(mobCoordinateList);
                timerUntilNextSpawn = Main.rand.Next(300, 600);
            }
        }

        public bool checkValidPosition(int x, int y)
        {
            bool playerDistance = false;
            foreach (Player p in Main.player)
            {
                
                if (Vector2.Distance(p.Center / 16, Position.ToVector2()) < 40)
                {
                    playerDistance = true;
                    Main.NewText("X : " + x + " - Y : " + y + " - Lighting level : " + Lighting.Brightness(x, y));
                    break;
                }
            }
            
            return Main.rand.Next(0, 10) == 1 && playerDistance && Lighting.Brightness(x, y) < 0.50;
        }

        public void spawnMob(List<Point16> list)
        {
            foreach (Point16 point in list)
            {
                Player p = Main.LocalPlayer;
                Point16 i = new Point16((int)p.Center.X / 16, (int)p.Center.Y / 16);

                Dust.NewDust(point.ToVector2() * 16, 50, 50, DustID.Smoke);
                NPC.NewNPC(point.X * 16, point.Y * 16, mobID);
            }
        }

        public void setMob(int newMobId)
        {
            this.mobID = newMobId;
            if (NPCLoader.GetNPC(newMobId) != null)
            {
                Main.NewText("Successfully changed mob to " + NPCLoader.GetNPC(newMobId).DisplayName.GetDefault());
            }
            else
            {
                Main.NewText("Successfully changed mob to " + Lang.GetNPCNameValue(newMobId));
            }
        }

        public string getCurrentMobName()
        {
            if (NPCLoader.GetNPC(mobID) != null)
            {
                return NPCLoader.GetNPC(mobID).DisplayName.GetDefault();
            }
            else
            {
                return Lang.GetNPCNameValue(mobID);
            }
        }

        public int getCurrentTimer()
        {
            return timerUntilNextSpawn;
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && (tile.type == mod.TileType<MobSpawner>()) && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            int[] mobChoice = { NPCID.StardustCellBig, NPCID.StardustSpiderBig};
            mobID = Main.rand.Next(mobChoice);
            return Place(i - 1, j - 1);
        }
    }
}
