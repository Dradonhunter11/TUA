using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.Commands
{
    class ToggleDifficulty : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args[0])
            {
                case "normal":
                case "0":
                    Main.expertMode = false;
                    ModContent.GetInstance<TUAWorld>().UltraMode = false;
                    break;
                case "expert":
                case "1":
                    Main.expertMode = true;
                    ModContent.GetInstance<TUAWorld>().UltraMode = false;
                    break;
                case "Ultra":
                case "2":
                    Main.expertMode = true;

                    ModContent.GetInstance<TUAWorld>().UltraMode = true;
                    break;
            }
        }

        public override string Command => "toggleDifficulty";
        public override CommandType Type => CommandType.World;
    }
}
