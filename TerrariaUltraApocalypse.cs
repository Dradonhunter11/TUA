using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaUltraApocalypse.NPCs;
using BiomeLibrary;
using TerrariaUltraApocalypse.API.TerraEnergy.UI;
using Terraria.UI;
using System.Collections.Generic;
using TerrariaUltraApocalypse.API.TerraEnergy.MachineRecipe.Furnace;

namespace TerrariaUltraApocalypse
{



    class TerrariaUltraApocalypse : Mod
	{

        
        public static int EoCDeath;

        public static bool EoCUltraActivated = false;
        private Type t2d = typeof(Texture2D);
        private Main instance2 = Main.instance;
        private String savePath, worldPath, playerPath;
        private Texture2D logoOriginal;
	    public static Texture2D[] originalMoon;
        public static FurnaceUI furnaceUI;
        public UserInterface furnaceInterface;

        public TerrariaUltraApocalypse()
		{            
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
        }

        public override void AddRecipes()
        {
            RecipeManager.removeRecipe(ItemID.RegenerationPotion);
            RecipeManager.removeRecipe(ItemID.IronskinPotion);
            //RecipeManager.addRecipe(this, "Regeneration Potion", 1, new RecipeForma(new String[] { "Amethyst", "Bottled Water", "Day Bloom" }, new int[] { 3, 1, 1}));
            //RecipeManager.addRecipe(this, "Ironskin Potion", 1, new RecipeForma(new String[] { "Iron Bar", "Bottled Water", "Day Bloom", "Iron Ore" }, new int[]{ 3, 1, 1, 3 }));
            
            
            ModRecipe r = new ModRecipe(this);
            r.AddIngredient(ItemID.Amethyst, 3);
            r.AddIngredient(ItemID.BottledWater, 1);
            r.AddIngredient(ItemID.Daybloom, 1);
            r.SetResult(ItemID.RegenerationPotion);
            r.AddRecipe();

            ModRecipe r2 = RecipeManager.createModRecipe(this);
            r2.AddIngredient(ItemID.IronBar, 3);
            r2.AddIngredient(ItemID.BottledWater, 1);
            r2.AddIngredient(ItemID.Daybloom, 1);
            r2.AddIngredient(ItemID.IronOre, 3);
            
            r2.SetResult(ItemID.IronskinPotion, 1);
            r2.AddRecipe();

            FurnaceRecipe r1 = FurnaceRecipeManager.CreateRecipe(this);
            r1.addIngredient(ItemID.IronOre, 1);
            r1.setResult(ItemID.IronskinPotion, 5);
            r1.addRecipe();
        }

        public override void Load()
        {
            savePath = Main.SavePath;
            worldPath = Main.WorldPath;
            playerPath = Main.PlayerPath;
            logoOriginal = Main.logo2Texture;
            originalMoon = Main.moonTexture;
            Main.SavePath = Main.SavePath + "/Tapocalypse";
            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            //Main.logo2Texture = GetTexture("Texture/Logo2");

            //Main.noWindowBorder = true;
            Main.musicVolume = 0.5f;

            if (Main.menuMode == 0)
            {
                //Main.spriteBatch.Draw(test, new Vector2((float)400, (float)500), Microsoft.Xna.Framework.Color.White);

            }

            furnaceUI = new FurnaceUI();
            furnaceUI.Activate();
            furnaceInterface = new UserInterface();
            furnaceInterface.SetState(furnaceUI);
        }

        public override void Unload()
        {
            Main.logo2Texture = logoOriginal;
            Main.PlayerPath = playerPath;
            Main.WorldPath = worldPath;
            Main.SavePath = savePath;
            
            
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            if (furnaceInterface != null && FurnaceUI.visible) {
                furnaceInterface.Update(gameTime);
            }
        }

        public override void UpdateMusic(ref int music)
        {

            if (Main.myPlayer != -1 && !Main.gameMenu && Main.LocalPlayer.active)
            {
                if (BiomeLibs.InBiome("Meteoridon"))
                {
                    music = MusicID.TheHallow;
                }
                else if (BiomeLibs.InBiome("Plagues"))
                {
                    music = MusicID.LunarBoss;
                }
                else if (TUAWorld.apocalypseMoon) {
                    music = MusicID.LunarBoss;
                }
            }


        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : Furnace GUI",
                    delegate
                    {
                        if (FurnaceUI.visible)
                        {
                            furnaceUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostSetupContent()
	    {
	        Mod bossChecklist = ModLoader.GetMod("BossChecklist");
	        if (bossChecklist != null)
	        {
	            bossChecklist.Call("AddBossWithInfo", "Eye of cthulhu (Ultra Version)", 16.0f, (Func<bool>)(() => TerrariaUltraApocalypse.EoCDeath >= 1), "Use a [i:" + ItemID.SuspiciousLookingEye + "] at night after Moon lord has been defeated");
	            bossChecklist.Call("AddBossWithInfo", "Eye of Apocalypse", 16.1f, (Func<bool>)(() => TUAWorld.UltraMode), "Use a [i:" + ItemType("Spawner") + "] after --1sing Ay. 0F C1^lh> in ^1tra and murder it, if you can...");
            }

        }
	}
}
