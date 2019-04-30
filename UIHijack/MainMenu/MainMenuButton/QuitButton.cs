using Terraria;

namespace TUA.UIHijack.MainMenu.MainMenuButton
{
    class QuitButton : MenuButton
    {
        public QuitButton(int xPosition, int yPosition) : base("Quit", xPosition, yPosition)
        {
        }

        public override void Recalculate()
        {
            Top.Set(Main.screenHeight - 50, 0);
            Left.Set(Main.screenWidth / 2 - 25, 0);
            xPosition = Main.screenWidth / 2 - 25;
            yPosition = Main.screenHeight - 50;
        }
    }
}
