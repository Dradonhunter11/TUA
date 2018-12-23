using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TerrariaUltraApocalypse.API.CustomInventory;
using TerrariaUltraApocalypse.API.CustomInventory.UI;
using TerrariaUltraApocalypse.API.FurnaceRework;
using TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock;
using TerrariaUltraApocalypse.API.UI;

namespace TerrariaUltraApocalypse.API.TerraEnergy.UI
{
    class FurnaceUI : UIState
    {
        public UIPanel furnaceUI;
        public UIPanelTrigger upgradeUI;
        public static bool visible = false;

        private readonly InputOutputSlot _input;
        private readonly InputOutputSlot _output;
        private readonly FuelSlot _fuel;
        private readonly UIEnergyBar _energyBar;
        private string _furnaceName = "";

        public FurnaceUI(ExtraSlot input, ExtraSlot output, Core core, string furnaceName)
        {
            this._input = new InputOutputSlot(input, Main.inventoryBack10Texture);
            this._output = new InputOutputSlot(output, Main.inventoryBack10Texture);
            this._furnaceName = furnaceName;

            _energyBar = new UIEnergyBar(core);
            
        }

        public FurnaceUI(ExtraSlot input, ExtraSlot output, ExtraSlot fuel, BudgetCore core, string furnaceName)
        {
            this._input = new InputOutputSlot(input, Main.inventoryBack10Texture);
            this._output = new InputOutputSlot(output, Main.inventoryBack10Texture);

            if (core != null)
            {
                _energyBar = new UIEnergyBar(core);
            }

            this._fuel = new FuelSlot(fuel, Main.inventoryBack10Texture, core);
            this._furnaceName = furnaceName;
        }

        public override void OnInitialize()
        {
            furnaceUI = new UIPanel();
            furnaceUI.SetPadding(0);
            furnaceUI.Width.Set(400, 0f);
            furnaceUI.Height.Set(200, 0f);
            furnaceUI.Top.Set(Main.screenHeight / 2 - 100, 0f);
            furnaceUI.Left.Set(Main.screenWidth / 2 - 200, 0f);

            upgradeUI = new UIPanelTrigger();
            upgradeUI.SetPadding(0);
            upgradeUI.Width.Set(200, 0f);
            upgradeUI.Height.Set(150, 0f);
            upgradeUI.Top.Set(Main.screenHeight / 2 - 100, 0f);
            upgradeUI.Left.Set(Main.screenWidth / 2 + 215, 0f);

            //furnaceUI.BackgroundColor = new Color(73, 94, 171);
            _output.Top.Set(60, 0f);
            _output.Left.Set(300, 0f);
            
            if (_fuel != null)
            {
                _input.Top.Set(30, 0f);
                _input.Left.Set(50, 0f);

                _fuel.Top.Set(90, 0f);
                _fuel.Left.Set(50, 0f);
                furnaceUI.Append(_fuel);
            }
            else
            {
                _input.Top.Set(60, 0f);
                _input.Left.Set(50, 0f);
            }


            furnaceUI.Append(_input);
            furnaceUI.Append(_output);

            Texture2D buttonDeleteTexture = ModLoader.GetTexture("Terraria/UI/ButtonDelete");
            UIImageButton closeButton = new UIImageButton(buttonDeleteTexture);
            closeButton.Left.Set(400 - 35, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            furnaceUI.Append(closeButton);

            
            _energyBar.Top.Set(180f, 0);
            _energyBar.Left.Set(10f, 0);
            _energyBar.Height.Set(14f, 0);
            _energyBar.Width.Set(386f, 0);
            furnaceUI.Append(_energyBar);
            
            Append(furnaceUI);
            Append(upgradeUI);
        }


        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            TerrariaUltraApocalypse.machineInterface.IsVisible = false;
            Main.playerInventory = false;
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            Vector2 nameDrawingPosition = new Vector2(Main.screenWidth / 2 - 60, Main.screenHeight / 2 - 95);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, _furnaceName,
                nameDrawingPosition, Color.White, 0f, Vector2.Zero,
                Vector2.One);
            upgradeUI.isVisible = false;
            if (_furnaceName.Equals("Adamantite Forge") || _furnaceName.Equals("Titanium Forge"))
            {

                upgradeUI.isVisible = true;
                Vector2 UpgradeDrawingPosition = new Vector2(Main.screenWidth / 2 + 230, Main.screenHeight / 2 - 95);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, "Upgrade",
                    UpgradeDrawingPosition, Color.White, 0f, Vector2.Zero,
                    Vector2.One);
            }
        }
    }
}
