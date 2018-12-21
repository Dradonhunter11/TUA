using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TerrariaUltraApocalypse.UIHijack.MainMenu.MainMenuButton;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.UIHijack.MainMenu
{
    class MainMenuUI : UIState
    {
        private MethodInfo _AddMenuButton;
        private List<MenuButton> _menuButtonsList;
        internal Vector2 currentWindowsSize;

        private UIPanel panel;
        public override void OnInitialize()
        {
            
            _menuButtonsList = new List<MenuButton>();
            _menuButtonsList.Add(new PlayButton(Main.screenWidth / 2 - 250, 250 + 20).setPosition(new Vector2(1, 1)));
            _menuButtonsList.Add(new MultiplayerPlayButton(Main.screenWidth / 2 + 100, 250 + 20).setPosition(new Vector2(0, 1)));
            _menuButtonsList.Add(new OptionMenuButton(Main.screenWidth / 2 - 250, 250 + 70).setPosition(new Vector2(1, 2)));
            _menuButtonsList.Add(new TUAOptionMenuButton(Main.screenWidth / 2 + 100, 250 + 70).setPosition(new Vector2(0, 2)));
            _menuButtonsList.Add(new ModBrowserButton(Main.screenWidth / 2 - 250, 250 + 120).setPosition(new Vector2(1, 3)));
            _menuButtonsList.Add(new ModSourceButton(Main.screenWidth / 2 + 100, 250 + 120).setPosition(new Vector2(0, 3)));
            _menuButtonsList.Add(new ModsButton(Main.screenWidth / 2 + 100, 250 + 170).setPosition(new Vector2(1, 4)));

            _menuButtonsList.Add(new QuitButton(Main.screenWidth / 2 - 25, Main.screenHeight - 50));
            AppendAllButton();

            currentWindowsSize = new Vector2(Main.screenWidth, Main.screenHeight);

        }

        private void randomizeBackground()
        {
            if (!SkyManager.Instance["TerrariaUltraApocalypse:SolarMist"].IsActive())
            {
                Filters.Scene.Activate("TerrariaUltraApocalypse:SolarMist", Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                SkyManager.Instance.Activate("TerrariaUltraApocalypse:SolarMist", Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                Filters.Scene["TerrariaUltraApocalypse:SolarMist"].GetShader().UseIntensity(0f).UseProgress(0f);
                Filters.Scene["TerrariaUltraApocalypse:SolarMist"].GetShader().UseTargetPosition(Vector2.Zero - new Vector2(0f, 10f));
            }   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Recalculate();
            RecalculateChildren();
            Main.menuMode = 888;
            base.Draw(spriteBatch);
            this.Height.Set(Main.screenHeight,0);
            this.Width.Set(Main.screenWidth,0);
            randomizeBackground();

            
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 windowsSize = new Vector2(Main.screenWidth, Main.screenHeight);
            if (currentWindowsSize != new Vector2(Main.screenWidth, Main.screenHeight))
            {
                currentWindowsSize = windowsSize;
                foreach (UIElement element in this.Elements)
                {
                    element.Recalculate();
                }

            }
        }

        private void AppendAllButton()
        {
            foreach (var menuButton in _menuButtonsList)
            {
                Append(menuButton);
            }
        }
    }
}
