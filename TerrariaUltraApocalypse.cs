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
using System.Reflection;
using Terraria.Graphics.Effects;
using TerrariaUltraApocalypse.Dimension.Sky;

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

            addIngotRecipe(ItemID.CopperOre, ItemID.CopperBar);
            addIngotRecipe(ItemID.TinOre, ItemID.TinBar);
            addIngotRecipe(ItemID.IronOre, ItemID.IronBar);
            addIngotRecipe(ItemID.LeadOre, ItemID.LeadBar);
            addIngotRecipe(ItemID.SilverOre, ItemID.SilverBar);
            addIngotRecipe(ItemID.TungstenOre, ItemID.TungstenBar);
            addIngotRecipe(ItemID.GoldOre, ItemID.GoldBar);
            addIngotRecipe(ItemID.PlatinumOre, ItemID.PlatinumBar);
            addIngotRecipe(ItemID.CrimtaneOre, ItemID.CrimtaneBar);
            addIngotRecipe(ItemID.DemoniteOre, ItemID.DemoniteBar);
        }

        public void addIngotRecipe(int itemID, int itemResult, int timer = 20) {
            FurnaceRecipe r1 = FurnaceRecipeManager.CreateRecipe(this);
            r1.addIngredient(itemID, 1);
            r1.setResult(itemResult, 1);
            r1.setCookTime(timer);
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

            Filters.Scene["TerrariaUltraApocalypse:TUAPlayer"] = new Filter(new Terraria.Graphics.Shaders.ScreenShaderData("FilterMoonLord").UseColor(25,0,0).UseOpacity(0.7f), EffectPriority.VeryHigh);
            SkyManager.Instance["TerrariaUltraApocalypse:TUAPlayer"] = new TUACustomSky();

            //FieldInfo info2 = typeof(Main).GetField("Windows", BindingFlags.Instance | BindingFlags.NonPublic);
            //info2.SetValue(Main.instance.Window, "Terraria in terraria in terraria in terraria in terraria in terraria in terraria in terraria in terraria in terraria ");


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
            if (Main.myPlayer != -1 && Main.gameMenu && Main.LocalPlayer.name != "") {
                TUAPlayer p = Main.player[Main.myPlayer].GetModPlayer<TUAPlayer>();
                if (p != null) {
                    if (p.currentDimension == "solar")
                    {
                        Main.WorldPath = Main.SavePath + "/World/solar";

                    } else
                    if (p.currentDimension == "overworld")
                    {
                        Main.WorldPath = Main.SavePath + "/World";
                    }
                }
            }
            if (Main.myPlayer != -1 && !Main.gameMenu && Main.LocalPlayer.active)
            {
                TUAPlayer p = Main.LocalPlayer.GetModPlayer<TUAPlayer>(this);
                if (BiomeLibs.InBiome("Meteoridon"))
                {
                    music = MusicID.TheHallow;
                }
                else if (BiomeLibs.InBiome("Plagues"))
                {
                    music = MusicID.LunarBoss;
                }
                else if (TUAWorld.apocalypseMoon)
                {
                    music = MusicID.LunarBoss;
                }
                else if (p.currentDimension == "solar") {
                    music = MusicID.TheTowers;
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
