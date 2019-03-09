using Terraria.ModLoader;
using TUA.API.Dev;

namespace TUA.Commands
{
    class ToggleTime : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
<<<<<<< Updated upstream
            if (SteamID64Checker.Instance.VerifyDevID())
=======
            if (SteamID64Checker.VerifyID())
>>>>>>> Stashed changes
            {
                TUAWorld.RealisticTimeMode = !TUAWorld.RealisticTimeMode;
            }
        }

        public override string Command => "ToggleRealisticTime";
        public override CommandType Type => CommandType.World;
    }
}
