using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace TUA.UIHijack.MainMenu.MainMenuButton
{
    class PlayButton : MenuButton
    {
        public PlayButton(int xPosition, int yPosition) : base("Single Player", xPosition, yPosition)
        {

        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            Main.menuMode = 1;
            Main.PlaySound(10, -1, -1, 1, 1f, 0f);
        }
    }
}
