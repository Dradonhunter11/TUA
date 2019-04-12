using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace TUA.Utilities
{
    public static class UIManager
    {
        private static UserInterface machineInterface;
        // private static UserInterface CapacitorInterface;
        private static UserInterface raidsInterface;
        private static UserInterface loreInterface;

        private static LoreBook.UI.LoreUI loreUI;
        private static Raids.UI.RaidsUI raidsUI;

        internal static LoreBook.UI.LoreUI GetLoreInstance() => loreUI;

        public static void InitAll()
        {
            machineInterface = new UserInterface();
            // CapacitorInterface = new UserInterface();
            raidsInterface = new UserInterface();
            loreInterface = new UserInterface();

            loreUI = new LoreBook.UI.LoreUI();
            raidsUI = new Raids.UI.RaidsUI();
        }

        public static void UpdateUI(GameTime gameTime)
        {
            if (machineInterface != null && machineInterface.IsVisible)
            {
                machineInterface.Update(gameTime);
            }

            if (raidsInterface != null && raidsInterface.IsVisible)
            {
                raidsInterface.Update(gameTime);
            }

            if (loreInterface != null && loreInterface.IsVisible)
            {
                loreInterface.Update(gameTime);
            }
        }

        public static void DrawFurnace()
        {
            if (machineInterface.IsVisible && Main.playerInventory)
            {
                machineInterface.CurrentState.Draw(Main.spriteBatch); ;
            }
        }

        public static void DrawRaids()
        {
            if (raidsInterface.IsVisible)
            {
                raidsInterface.CurrentState.Draw(Main.spriteBatch);
            }
        }

        public static void DrawLore()
        {
            if (loreInterface.IsVisible)
            {
                loreUI.Draw(Main.spriteBatch);
            }
        }

        public static void OpenLoreUI(Player plr)
        {
            loreUI.InitLoreUI(plr.GetModPlayer<LoreBook.LorePlayer>());
            loreInterface.SetState(loreUI);
            loreInterface.IsVisible = true;
        }

        public static void CloseLoreUI()
        {
            loreInterface.IsVisible = false;
            loreInterface.SetState(null);
        }

        public static void OpenRaidsUI()
        {
            raidsInterface.SetState(raidsUI);
            raidsInterface.IsVisible = true;
        }

        public static void CloseRaidsUI()
        {
            raidsInterface.IsVisible = false;
            Main.npcChatText = "I'll be able to help you in your future raids! After all, I'm the guide."
                + (Main.rand.NextBool() ? " :smile:" : "");
        }

        public static void OpenMachineUI(UIState state)
        {
            machineInterface.SetState(state);
            machineInterface.IsVisible = true;
        }

        public static void CloseMachineUI()
        {
            machineInterface.SetState(null);
            machineInterface.IsVisible = false;
        }
    }
}
