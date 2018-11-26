using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton
{
    class TUAOptionMenuButton : MenuButton
    {
        internal static ModSettingsUI settingUI;
        public TUAOptionMenuButton(int xPosition, int yPosition) : base("Mod Settings (W.I.P)", xPosition, yPosition)
        {
            settingUI = new ModSettingsUI();
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.MenuUI.SetState(settingUI);
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }
    }
}
