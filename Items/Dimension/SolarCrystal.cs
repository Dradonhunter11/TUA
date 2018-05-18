using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using TerrariaUltraApocalypse.API;

namespace TerrariaUltraApocalypse.Items.Dimension
{
    class SolarCrystal : TUAModItem
    {
        public int itemUseCooldown = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar crystal");
            Tooltip.SetDefault("Allow you to travel in a new universe\nUltra mode");
        }

        public override void SetDefaults()
        {
            ultra = true;
            item.width = 32;
            item.height = 32;
            item.useStyle = 4;
            item.useTime = 5;
            item.useAnimation = 20;
        }

        public override bool UseItem(Player player)
        {
            TUAPlayer p = player.GetModPlayer<TUAPlayer>(mod);

            FieldInfo info = typeof(FileData).GetField("_path", BindingFlags.Instance | BindingFlags.NonPublic);
            string get = (string)info.GetValue(Main.ActiveWorldFileData);

            if (itemUseCooldown == 0) {
                WorldFile.saveWorld(false, true);
                if (p.currentDimension != "solar") {
                    p.currentDimension = "solar";
                    Main.NewText("You are entering into the solar dimension...", Color.Orange);


                    

                    if (!File.Exists(Main.SavePath + "/World/Solar/" + Main.worldName + ".wld")) {
                        info.SetValue(Main.ActiveWorldFileData, Main.SavePath + "/World/Solar/" + Main.worldName + ".wld");
                        WorldFile.saveWorld(false, true);
                        WorldGen.clearWorld();

                        WorldGen.generateWorld(-1);
                        itemUseCooldown = 500;


                        WorldGen.EveryTileFrame();
                        ModifyWorld();
                        return true;
                    }

                    info.SetValue(Main.ActiveWorldFileData, Main.SavePath + "/World/Solar/" + Main.worldName + ".wld");
                    itemUseCooldown = 500;
                    WorldGen.EveryTileFrame();
                    WorldGen.playWorld();
                    return true;
                }
                info.SetValue(Main.ActiveWorldFileData, Main.SavePath + "/World/" + Main.worldName + ".wld");
                p.currentDimension = "overworld";
                itemUseCooldown = 500;
                WorldGen.playWorld();
                return true;
            }
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            itemUseCooldown--;
            if (itemUseCooldown < 0) {
                itemUseCooldown = 0;
            }
        }

        private void ModifyWorld()
        {
            NPC.downedBoss1 = true;
            NPC.downedBoss2 = true;
            NPC.downedBoss3 = true;
            NPC.downedGoblins = true;
            NPC.downedFrost = true;
            Main.hardMode = true;
            NPC.downedChristmasIceQueen = true;
            NPC.downedChristmasSantank = true;
            NPC.downedChristmasTree = true;
            NPC.downedClown = true;
            NPC.downedGolemBoss = true;
            
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile T = Main.tile[i, j];
                    if (T == null)
                        T = new Tile();
                    if (T.active())
                    {
                        if (T.type == 0 || T.type == TileID.Sand || T.type == TileID.SnowBlock  || T.type == TileID.Mud || T.type == TileID.ClayBlock || T.type == TileID.Grass || T.type == TileID.JungleGrass || T.type == TileID.CorruptGrass || T.type == TileID.FleshGrass || T.type == TileID.MushroomGrass) {
                            T.type = (ushort)mod.TileType("SolarDirt");
                        }
                        if (T.type == TileID.Stone || T.type == TileID.Crimstone || T.type == TileID.Ebonstone || T.type == TileID.IceBlock || T.type == TileID.CorruptIce || T.type == TileID.FleshIce || T.type == TileID.HallowedIce || T.type == TileID.Sandstone) {
                            T.type = (ushort)mod.TileType("SolarRock");
                        }
                        
                    }
                }
            }
            WorldGen.EveryTileFrame();
        }
    }
}
