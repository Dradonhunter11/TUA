﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using TerrariaUltraApocalypse.API.LiquidAPI.Data;

namespace TerrariaUltraApocalypse.API.LiquidAPI.LiquidMod
{

    //The following code is a modified version of DataCore from Project_Logic 0.5.0.1 provided by Rartrin
    class LiquidCore : ModWorld
    {
        private const string extension = "liquid";//Should work without the leading period
        private const byte MODE = 0;//Extra data
        private const byte FORM = 3;//Saving format

        public static LiquidCore grid = new LiquidCore();

        public static Bit[,] liquidGrid;//MAKE SURE YOU DEREFERENCE THIS ON UNLOAD

        public override bool Autoload(ref string name) { return true; }

        public override void Initialize() { liquidGrid = new Bit[Main.maxTilesX, Main.maxTilesY]; }

        public LiquidRef this[int x, int y]
        {
            get
            {
                return new LiquidRef(x, y);
            }
        }

        public override TagCompound Save()
        {
            try
            {
                string path = Path.ChangeExtension(Main.ActiveWorldFileData.Path, extension); //Change current world path to the custom save one
                if (FileUtilities.Exists(path, false)) { FileUtilities.Copy(path, path + ".bak", false, true); } //also make a backup
                Queue<byte> data = new Queue<byte>();
                data.Enqueue(MODE);
                data.Enqueue(FORM);//Point Storage
                for (ushort y = 0; y < Main.maxTilesY; y++)
                {
                    for (ushort x = 0; x < Main.maxTilesX; x++)
                    {
                        if (liquidGrid[x, y] != 0)
                        {
                            data.Enqueue((byte)(x >> 8)); data.Enqueue((byte)x);
                            data.Enqueue((byte)(y >> 8)); data.Enqueue((byte)y);
                            data.Enqueue(liquidGrid[x, y]);
                        }
                    }
                }
                FileUtilities.WriteAllBytes(path, data.ToArray(), false);
                return new TagCompound();
            }
            catch { return null; }
        }

        public override void Load(TagCompound tag)
        {
            try
            {
                string path = Path.ChangeExtension(Main.ActiveWorldFileData.Path, extension);
                if (!FileUtilities.Exists(path, false)) { return; }
                Queue<byte> data = new Queue<byte>(FileUtilities.ReadAllBytes(path, false));
                byte mode = data.Dequeue();
                byte form = data.Dequeue();
                if (form == 3)//Point Storage
                {
                    while (data.Count > 0)
                    {
                        liquidGrid[(data.Dequeue() << 8) + data.Dequeue(), (data.Dequeue() << 8) + data.Dequeue()] = data.Dequeue();
                    }
                }
            }
            catch { }
        }
    }
}