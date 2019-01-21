using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TUA.UIHijack.MainMenu.MainMenuButton;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace TUA.UIHijack.MainMenu
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
            if (!SkyManager.Instance["TUA:SolarMist"].IsActive())
            {
                Filters.Scene.Activate("TUA:SolarMist", Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                SkyManager.Instance.Activate("TUA:SolarMist", Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                Filters.Scene["TUA:SolarMist"].GetShader().UseIntensity(0f).UseProgress(0f);
                Filters.Scene["TUA:SolarMist"].GetShader().UseTargetPosition(Vector2.Zero - new Vector2(0f, 10f));
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
            // randomizeBackground();
            

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

        public void load()
        {
            IL.Terraria.Main.DrawMenu += HookMenu;
        }

        private void HookMenu(HookIL il)
        {
            ILog logger = LogManager.GetLogger("Injection logger");

            Instruction ins;
            //We opened terraria with dnSpy to find the IL code offset and the type of call, then we get the instruction from it
            if(IntPtr.Size != 8)
                ins = il.Body.Instructions.Single(i => i.OpCode.Code == Code.Stsfld && i.ToString().Contains("IL_1754"));
            else
                ins = il.Body.Instructions.Single(i => i.OpCode.Code == Code.Stsfld && i.ToString().Contains("IL_178f"));

            logger.Info("Found the first instruction");
            //Same thing as above, with a little bit of research we found out that Terraria.Graphics.Effects.SkyManager.DeactivateAll() was only called once, which mean we can easily use it's compiled counter like this Terraria.Graphics.Effects.SkyManager::DeactivateAll() 
            Instruction ins2 = il.Body.Instructions.Single(i => i.OpCode.Code == Code.Callvirt && i.ToString().Contains("Terraria.Graphics.Effects.SkyManager::DeactivateAll()"));
            logger.Info("Found the second instruction");
            //Then we get the Il processor of the method we are changing, this will allow us to write in the method on runtime
            var processor = il.Body.GetILProcessor();
            //Finally we write after the instruction after the first we found a br (which is a jump) to the the second instruction that we found.
            processor.InsertBefore(ins.Next, processor.Create(OpCodes.Br, ins2.Next));
            logger.Info("Wrote the assembly");
            //In here, all we did is skipping the whole deactivation phase in the game menu for screen shader and custom pillar background  
        }
    }
}
