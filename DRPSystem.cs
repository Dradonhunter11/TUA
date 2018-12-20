using drpc;
using Terraria;

namespace TerrariaUltraApocalypse
{
    // Almost all of this is from Overhaul.
    public static class DRPSystem
    {
        private const string AppID = "525122574695399425";

        private static DiscordRP.RichPresence presence;

        private static DiscordRP.EventHandlers handlers;

        public static void Init()
        {
            handlers = default(DiscordRP.EventHandlers);
            DiscordRP.Initialize(AppID, ref handlers, true, (string)null);
            Reset(true);
            Update();
        }

        public static void Update()
        {
            if (Main.gameMenu & Main.rand.NextBool(100))
            {
                Reset(false);
                return;
            }
            var p = Main.LocalPlayer;
            presence.largeImageText = (Main.netMode == 0)
                ? $"Playing Singleplayer as {p.name}"
                : $"Playing Multiplayer ({Main.ActivePlayersCount} / {Main.maxNetPlayers})";
            presence.state = $"HP: {p.statLife} MP: {p.statMana} DEF: {p.statDefense}";
            // https://discordapp.com/developers/docs/rich-presence/how-to#updating-presence-update-presence-payload-fields
            if (!NPC.downedSlimeKing | !NPC.downedBoss1)
            {

            }
            if (NPC.downedBoss1 & NPC.downedSlimeKing)
            {

            }
            if (NPC.downedBoss2)
            {

            }
            if (NPC.downedBoss3)
            {

            }
            if (Main.hardMode)
            {

            }
            if (NPC.downedPlantBoss)
            {

            }
        }

        public static void Kill()
        {
            DiscordRP.Shutdown();
        }

        private static void Reset(bool timestamp)
        {
            DiscordRP.UpdatePresence(ref presence);
        }
    }
}
