using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TUA.UIHijack.MainMenu.MainMenuButton
{
    class QuitButton : MenuButton
    {
        public QuitButton(int xPosition, int yPosition) : base("Quit", xPosition, yPosition)
        {
        }

        public override void ExecuteButton(UIMouseEvent evt, UIElement element)
        {
            MethodInfo quitGame = typeof(Main).GetMethod("QuitGame", BindingFlags.NonPublic | BindingFlags.Instance);
            quitGame.Invoke(Main.instance, new object[] { });
        }

        public override void Recalculate()
        {
            Top.Set(Main.screenHeight - 50, 0);
            Left.Set(Main.screenWidth / 2 - 25, 0);
            xPosition = Main.screenWidth / 2 - 25;
            yPosition = Main.screenHeight - 50;
            PostRecalculate();
        }
    }
}
