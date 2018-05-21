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

            if (itemUseCooldown == 0)
            {
                WorldFile.saveWorld(false, true);
                if (p.currentDimension != "solar")
                {
                    p.currentDimension = "solar";
                    Main.NewText("You are entering into the solar dimension...", Color.Orange);




                    if (!File.Exists(Main.SavePath + "/World/Solar/" + Main.worldName + ".wld"))
                    {
                        info.SetValue(Main.ActiveWorldFileData, Main.SavePath + "/World/Solar/" + Main.worldName + ".wld");
                        generateDimension();
                        p.player.Spawn();
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

        private void generateDimension()
        {
            WorldFile.saveWorld(false, true);
            WorldGen.clearWorld();

	        SolarWorldGen.GenerateSolarWorld(-1);
            itemUseCooldown = 500;

            //TUAWorld.solarWorldGen(mod);
            WorldGen.EveryTileFrame();
        }

        public override void UpdateInventory(Player player)
        {
            itemUseCooldown--;
            if (itemUseCooldown < 0)
            {
                itemUseCooldown = 0;
            }
        }


    }
}
