using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.API.Dev;

namespace TerrariaUltraApocalypse.Commands
{
    class ToggleTime : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (SteamID64Checker.getInstance().verifyID())
            {
                TUAWorld.RealisticTimeMode = !TUAWorld.RealisticTimeMode;
            }
        }

        public override string Command => "ToggleRealisticTime";
        public override CommandType Type => CommandType.World;
    }
}
