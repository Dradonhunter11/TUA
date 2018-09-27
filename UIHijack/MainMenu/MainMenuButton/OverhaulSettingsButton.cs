using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton
{
    class OverhaulSettingsButton : MenuButton
    {
        public OverhaulSettingsButton(int xPosition, int yPosition) : base("Overhaul Settings", xPosition, yPosition)
        {
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 43300;
            Main.PlaySound(10, -1, -1, 1);
        }
    }
}
