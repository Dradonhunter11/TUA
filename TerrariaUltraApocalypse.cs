using BiomeLibrary.API;
using Dimlibs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;
using TUA.API;
using TUA.API.Dev;
using TUA.API.Experimental;
using TUA.API.Injection;
using TUA.API.LiquidAPI;
using TUA.API.LiquidAPI.Test;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.API.TerraEnergy.MachineRecipe.Forge;
using TUA.Configs;
using TUA.CustomScreenShader;
using TUA.CustomSkies;
using TUA.Dimension.Sky;
using TUA.Items.EoA;
using TUA.Items.Meteoridon.Materials;
using TUA.NPCs;
using TUA.Raids.UI;
using TUA.UIHijack.InGameUI.NPCDialog;
using TUA.UIHijack.MainMenu;
using TUA.UIHijack.MainMenu.TUAOptionMenu;
using TUA.UIHijack.WorldSelection;
using ModExtension = BiomeLibrary.API.ModExtension;

namespace TUA
{
    internal class TUA : Mod
    {
        internal static string version = "0.1 dev";
        internal static String tModLoaderVersion2 = "";
        internal static Version tModLoaderVersion;
        internal static bool devMode = true;

        internal static TUA instance;

        public static bool EoCUltraActivated = false;
        private readonly Type t2d = typeof(Texture2D);
        private readonly Main instance2 = Main.instance;
        private readonly string savePath, worldPath, playerPath;
        private Texture2D logoOriginal;
        public static Texture2D[] originalMoon;

        public static Texture2D SolarFog;

        public static UserInterface machineInterface;
        public static UserInterface CapacitorInterface;
        public static UserInterface raidsInterface;

        private static List<string> quote = new List<string>();
        public static string animate = GetAnimatedTitle();

        private int animationTimer = 25;
        

        private UIWorldSelect originalWorldSelect;
        private readonly MainMenuUI newMainMenu = new MainMenuUI();
        internal static TUASettingMenu setting = new TUASettingMenu();
        internal static RaidsUI raidsUI = new RaidsUI();

        public static readonly string SAVE_PATH = Main.SavePath;

        public const uint SPI_GETMOUSESPEED = 0x0070;

        
        public static CustomTitleMenuConfig custom;
        public bool injectedConfigSetting = false;

        public TUA()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true

            };
        }

        public override void AddRecipes()
        {
            RecipeManager.removeRecipe(ItemID.RegenerationPotion);
            RecipeManager.removeRecipe(ItemID.IronskinPotion);
            //RecipeManager.addRecipe(this, "Regeneration Potion", 1, new RecipeForma(new string[] { "Amethyst", "Bottled Water", "Day Bloom" }, new int[] { 3, 1, 1}));
            //RecipeManager.addRecipe(this, "Ironskin Potion", 1, new RecipeForma(new string[] { "Iron Bar", "Bottled Water", "Day Bloom", "Iron Ore" }, new int[]{ 3, 1, 1, 3 }));


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

            //Pre hardmode ore
            addFurnaceRecipe(ItemID.CopperOre, ItemID.CopperBar);
            addFurnaceRecipe(ItemID.TinOre, ItemID.TinBar, 10);
            addFurnaceRecipe(ItemID.IronOre, ItemID.IronBar, 30);
            addFurnaceRecipe(ItemID.LeadOre, ItemID.LeadBar);
            addFurnaceRecipe(ItemID.SilverOre, ItemID.SilverBar);
            addFurnaceRecipe(ItemID.TungstenOre, ItemID.TungstenBar);
            addFurnaceRecipe(ItemID.GoldOre, ItemID.GoldBar, 30);
            addFurnaceRecipe(ItemID.PlatinumOre, ItemID.PlatinumBar, 40);
            addFurnaceRecipe(ItemID.CrimtaneOre, ItemID.CrimtaneBar);
            addFurnaceRecipe(ItemID.DemoniteOre, ItemID.DemoniteBar);
            addFurnaceRecipe(ItemID.Meteorite, ItemID.MeteoriteBar, 30);

            //Hardmode ore 
            addFurnaceRecipe(ItemID.CobaltOre, ItemID.CobaltBar, 25);
            addFurnaceRecipe(ItemID.PalladiumOre, ItemID.PalladiumBar, 25);
            addFurnaceRecipe(ItemID.MythrilOre, ItemID.MythrilBar, 40);
            addFurnaceRecipe(ItemID.OrichalcumOre, ItemID.OrichalcumBar, 50);
            addFurnaceRecipe(ItemID.AdamantiteOre, ItemID.AdamantiteBar, 70);
            addFurnaceRecipe(ItemID.TitaniumOre, ItemID.TitaniumBar, 80);
            addFurnaceRecipe(ItemID.ChlorophyteOre, ItemID.ChlorophyteBar, 120);
            addFurnaceRecipe(ItemID.LunarOre, ItemID.LunarBar, 240);

            //Non ore recipe for furnace
            addFurnaceRecipe(ItemID.SandBlock, ItemID.Glass, 20);

            //Smetler recipe recipe
            addInductionSmelterRecipe(ItemID.Hellstone, ItemID.Obsidian, ItemID.HellstoneBar, 1, 2, 1, 90);
            addInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.Ectoplasm, ItemID.SpectreBar, 4, 1, 3);
            addInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.GlowingMushroom, ItemID.ShroomiteBar, 1, 5, 1, 180);

            RecipeManager.GetAllRecipeByIngredientAndReplace(ItemID.PixieDust, ItemType<MeteorideScale>());
        }

        public void addFurnaceRecipe(int itemID, int itemResult, int timer = 20)
        {
            FurnaceRecipe r1 = FurnaceRecipeManager.CreateRecipe(this);
            r1.addIngredient(itemID, 1);
            r1.setResult(itemResult, 1);
            r1.setCostAndCookTime(timer);
            r1.addRecipe();
        }

        public void addInductionSmelterRecipe(int itemID1, int itemID2, int itemResult, int quantityItem1 = 1, int quantityItem2 = 1, int resultQuantity = 1, int timer = 120)
        {
            ForgeRecipe fr1 = ForgeRecipeManager.CreateRecipe(this);
            fr1.addCatalyserIngredient(itemID1, quantityItem1);
            fr1.addIngredient(itemID2, quantityItem2);
            fr1.setResult(itemResult, resultQuantity);
            fr1.addRecipe();
        }

        public void convertTileToChunk(ArrayChunk<Tile> tiles)
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    tiles[i, j] = Main.tile[i, j];
                }
            }
        }

        public override void Load()
        {
            
            LoadModContent(mod =>
            {
                Autoload(mod);
            });
            ArrayChunk<Tile> tile = new ArrayChunk<Tile>();
            convertTileToChunk(tile);
            newMainMenu.load();
            

            instance = this;
            UpdateBiomesInjection.inject();
            LiquidRegistery.MassMethodSwap();
            Console.Write("AM I NULL? " + typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser"));
            MethodInfo attempt = typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetMethod("PopulateModBrowser", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            ReflectionExtension.MethodSwap(typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser"), "PopulateModBrowser", typeof(ModBrowserInjection), "PopulateModBrowser");

            Main.SavePath += "/Tapocalypse";
            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";
            Main.musicVolume = 0.5f;

            if (Main.menuMode == 0)
            {
                //Main.spriteBatch.Draw(test, new Vector2((float)400, (float)500), Microsoft.Xna.Framework.Color.White);

            }

            machineInterface = new UserInterface();
            CapacitorInterface = new UserInterface();
            raidsInterface = new UserInterface();

            Filters.Scene["TUA:TUAPlayer"] = new Filter(new Terraria.Graphics.Shaders.ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f), EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:TUAPlayer"] = new TUACustomSky();
            Filters.Scene["TUA:StardustPillar"] = new Filter(new Terraria.Graphics.Shaders.ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f), EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:StardustPillar"] = new StardustCustomSky();
            Filters.Scene["TUA:SolarMist"] = new Filter(new MeteoridonScreenShader().UseColor(0.4f, 0, 0).UseOpacity(0.7f), EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:SolarMist"] = new HeavyMistSky();


            if (Main.netMode == 0)
            {
                FieldInfo UIWorldSelectInfo =
                    typeof(Main).GetField("_worldSelectMenu", BindingFlags.Static | BindingFlags.NonPublic);
                originalWorldSelect = (UIWorldSelect)UIWorldSelectInfo.GetValue(null);
                UIWorldSelectInfo.SetValue(null, new NewUIWorldSelect());


                SteamID64Checker.getInstance().CopyIDToClipboard();

                logoOriginal = Main.logo2Texture;
                originalMoon = Main.moonTexture;
                SolarFog = GetTexture("CustomScreenShader/HeavyMist");

                DRPSystem.Init();
                Main.OnTick += DRPSystem.Update;
            }

            if (IntPtr.Size == 8)
            {
                AllowGignaticWorld();
            }


            DRPSystem.Init();
            Main.OnTick += DRPSystem.Update;
        }

        private static void LoadModContent(Action<Mod> loadAction)
        {
            //Object o = new OverworldHandler();
            int num = 0;
            foreach (var mod in ModLoader.Mods)
            {
                try
                {
                    loadAction(mod);
                }
                catch (Exception e)
                {
                }
            }
        }

        public void AllowGignaticWorld()
        {
            Main.chest = new Chest[10000];
            Main.tile = new Tile[25200, 9600];
            Main.maxTilesX = 25200;
            Main.maxTilesY = 9600;

        }

        public static String GetAnimatedTitle()
        {
            Random r = new Random();
            lazyWaytoShowTitle();

            tModLoaderVersion2 = "tModLoader v" + ModLoader.version;
            tModLoaderVersion = ModLoader.version;

            return tModLoaderVersion2 + $" - TUA v{version} - {quote[r.Next(quote.Count - 1)]}";
        }

        public static void lazyWaytoShowTitle()
        {
            quote = new List<string>();
            quote.Add("now with 100% less life insurance! ");
            quote.Add("make sure to give EoA my best . . . or my worst depending on how you look at it ");
            quote.Add("I failed to help Heather with a door edition ");
            quote.Add("you have beings have only 3 dimensions? pffft ");
            quote.Add("you want me to die? Not if I kill myself first! ");
            quote.Add("the nurse may need a few more doctorate degrees . . . and a better hairdo ");
            quote.Add("our mod will create an exodus from the others! ");
            quote.Add("build a wall? Pffft, we have more important things to do ");
            quote.Add("old age should burn and rave at close of day . . . yes, that means you, Jof ");
            quote.Add("now with a bunch of stupid title version like this one! ");
            quote.Add("I dont feel so good... ");
            quote.Add("should have gone for the cpu ");
            quote.Add("I'll go grab some pizza ");
            quote.Add("Don't forget to change this with sonething, ok? ");
        }



        public override void Unload()
        {
            //DrawMapInjection.revert();
            UpdateBiomesInjection.inject();
            LiquidRegistery.MassMethodSwap();

            Main.logo2Texture = logoOriginal;

            Main.SavePath = SAVE_PATH;

            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            instance = null;
            quote.Clear();
            FieldInfo info2 = typeof(ModLoader).GetField("versionedName",
                BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            info2.SetValue(null, string.Format("tModLoader v{0}", (object)Terraria.ModLoader.ModLoader.version) + (Terraria.ModLoader.ModLoader.branchName.Length == 0 ? "" : " " + Terraria.ModLoader.ModLoader.branchName) + (Terraria.ModLoader.ModLoader.beta == 0 ? "" : string.Format(" Beta {0}", (object)Terraria.ModLoader.ModLoader.beta)));

            if (!Main.dedServ)
            {
                DRPSystem.Kill();
                Main.OnTick -= DRPSystem.Update;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (machineInterface != null && machineInterface.IsVisible)
            {
                machineInterface.Update(gameTime);
            }

            if (raidsInterface != null && raidsInterface.IsVisible)
            {
                raidsInterface.Update(gameTime);
            }
        }

        public override void UpdateMusic(ref int music, ref MusicPriority musicPriority)
        {
            //string s = Dimlibs.Dimlibs.getPlayerDim();
            //DrawIngameOptionInjection.SwapMethod();
            if (Main.gameMenu)
            {
                Main.instance.newMusic = MusicID.Dungeon;
                music = MusicID.Dungeon;
            }



            if (Main.myPlayer != -1 && Main.gameMenu && Main.LocalPlayer.name != "")
            {
                DimPlayer p = Main.player[Main.myPlayer].GetModPlayer<DimPlayer>();
                FieldInfo info = typeof(LanguageManager).GetField("_localizedTexts", BindingFlags.Instance | BindingFlags.NonPublic);
                Dictionary<string, LocalizedText> dictionary = info.GetValue(LanguageManager.Instance) as Dictionary<string, LocalizedText>;

                FieldInfo textInfo = typeof(LocalizedText).GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
                resetMenu(dictionary, textInfo);
            }
            if (Main.myPlayer != -1 && !Main.gameMenu && Main.LocalPlayer.active)
            {


                //DimPlayer p = Main.LocalPlayer.GetModPlayer<DimPlayer>(this);
                if (this.GetBiome("Meteoridon").InBiome())
                {
                    music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/Stars_Lament_Loop");
                }
                else if (this.GetBiome("Plagues").InBiome())
                {
                    music = MusicID.LunarBoss;
                }
                else if (TUAWorld.apocalypseMoon)
                {
                    music = MusicID.LunarBoss;
                }
                else if (Dimlibs.Dimlibs.getPlayerDim() == "solar")
                {
                    music = MusicID.TheTowers;
                    Main.musicBox = 36;
                    musicPriority = MusicPriority.Environment;

                }
            }

            if (Main.menuMode == 0 && custom.customMenu)
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(newMainMenu);
            }
            AnimateVersion();
            if(Main.gameMenu && Main.menuMode == 0 || (Main.menuMode == 888 && TUA.custom.customMenu))
                TUA.SetTheme();

        }

        

        private void AnimateVersion()
        {
            if (animationTimer == 0)
            {
                char last = animate[animate.Length - 1];
                animate = animate.Remove(animate.Length - 1, 1);
                animate = last + animate;
                FieldInfo info2 = typeof(ModLoader).GetField("versionedName",
                    BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                info2.SetValue(null, animate);
                animationTimer = 10;
            }

            animationTimer--;

        }


        private static void resetMenu(Dictionary<string, LocalizedText> dictionary, FieldInfo textInfo)
        {
            if (Main.menuMode == 1)
            {
                textInfo.SetValue(dictionary["UI.New"], "New");
                textInfo.SetValue(dictionary["UI.SelectWorld"], "Select World");
            }
        }

        internal void Autoload(Mod mod)
        {

            if (mod.Code == null)
            {
                return;
            }

            foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
            {
                /*if (type.IsAbstract || type.GetConstructor(new Type[0]) == null)//don't autoload things with no default constructor
                {
                    continue;
                }*/
                if (type.IsSubclassOf(typeof(ModLiquid)))
                {
                    AutoloadLiquid(type);
                }
                if (type.IsSubclassOf(typeof(TUAGlobalNPC)))
                {
                    AutoloadTUAGlobalNPC(type);
                }

            }
        }

        private void AutoloadLiquid(Type type)
        {
            ModLiquid liquid = (ModLiquid)Activator.CreateInstance(type);
            LiquidRegistery.getInstance().addNewModLiquid(liquid);
        }

        private void AutoloadTUAGlobalNPC(Type type)
        {
            TUAGlobalNPC globalNPC = (TUAGlobalNPC)Activator.CreateInstance(type);
            TUANPCLoader.addTUAGlobalNPC(globalNPC);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            int setting = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            int npcDialog = layers.FindIndex(layer => layer.Name.Equals("Vanilla: NPC / Sign Dialog"));


            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : Furnace GUI",
                    delegate
                    {
                        if (machineInterface.IsVisible && Main.playerInventory)
                        {
                            machineInterface.CurrentState.Draw(Main.spriteBatch); ;
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : Raids GUI", delegate
                    {
                        if (raidsInterface.IsVisible)
                        {
                            raidsInterface.CurrentState.Draw(Main.spriteBatch);
                        }

                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            if (npcDialog != -1)
            {
                layers[npcDialog] = new LegacyGameInterfaceLayer("Vanilla: NPC / Sign Dialog", delegate
                {
                    NewNPCChatDraw.GUIChatDraw();
                    return true;
                }, InterfaceScaleType.UI);
            }
        }

        public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBossWithInfo", "Eye of cthulhu (Ultra Version)", 16.0f, (Func<bool>)(() => TUAWorld.EoCDeath >= 1), "Use a [i:" + ItemID.SuspiciousLookingEye + "] at night after Moon lord has been defeated");
                bossChecklist.Call("AddBossWithInfo", "Eye of EoADowned - God of destruction", 16.1f, (Func<bool>)(() => TUAWorld.UltraMode), "Use a [i:" + ItemType("Spawner") + "] after --1sing Ay. 0F C1^lh> in ^1tra and murder it, if you can...");
            }

            Mod achievementLibs = ModLoader.GetMod("AchievementLibs");
            if (achievementLibs != null)
            {
                Func<bool> c = () => TUAWorld.EoADowned;
                achievementLibs.Call("AddAchievementWithoutReward", this, "Once there was an eye that was a god...", "Kill the eye of apocalypse - god of destruction!", "Achievement/EoAKill", c);
                achievementLibs.Call("AddAchievementWithoutReward", this, "The terrible moon tried to kill us", "Use [i:" + ItemType<Spawner>() + "] and succesfully survive all 8 wave of the apocalypse moon", "Achievement/EoAKill", (Func<bool>)(() => TUAWorld.ApoMoonDowned));
                achievementLibs.Call("AddAchievementWithoutAction", this,
                    "Once there was the eye of cthulhu... the ultra one", "Kill the ultra EoC succesfully.",
                    "Achievement/UltraEoC", new int[] { ItemType<Spawner>() }, new int[] { 1 },
                    (Func<bool>)(() => TUAWorld.EoCDeath >= 1));
            }

            Mod st = ModLoader.GetMod("SacredTool");
            if (st != null)
            {
                bool ultraMode = (bool)st.Call("TrueMode");
            }

            RecipeManager.setAllFurnaceRecipeSystem();
            LiquidRegistery.getInstance().addNewModLiquid(new PlutonicWaste());
            LiquidRegistery.getInstance().addNewModLiquid(new WeirdLiquid());

            
        }

        public override object Call(params object[] args)
        {
            string command = args[0] as string;
            if (args[0] == "UltraMode")
            {
                return TUAWorld.UltraMode;
            }
            return base.Call(args);
        }

        public static void SetTheme()
        {
            Dictionary<String, CustomSky> temp2 = (Dictionary<string, CustomSky>)typeof(SkyManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SkyManager.Instance);
            Dictionary<String, Filter> temp = (Dictionary<string, Filter>) typeof(FilterManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Filters.Scene);
            List<String> allKey = temp.Keys.ToList();
            foreach (var  key in allKey)
            {
                if(key != TUA.custom.newMainMenuTheme)
                    Filters.Scene[key].Deactivate();
            }

            
            allKey = temp2.Keys.ToList();
            foreach (var key in allKey)
            {
                if (key != TUA.custom.newMainMenuTheme)
                    SkyManager.Instance.Deactivate(key);
            }

            Main.worldSurface = 565;
            if (TUA.custom.newMainMenuTheme != "Vanilla" && !SkyManager.Instance[TUA.custom.newMainMenuTheme].IsActive())
            {
                switch (TUA.custom.newMainMenuTheme)
                {
                    case "Vanilla":
                        return;
                    default:
                        if(Filters.Scene[TUA.custom.newMainMenuTheme] != null)
                            Filters.Scene.Activate(TUA.custom.newMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        if (SkyManager.Instance[TUA.custom.newMainMenuTheme] != null)
                            SkyManager.Instance.Activate(TUA.custom.newMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        if (Overlays.Scene[TUA.custom.newMainMenuTheme] != null)
                            Overlays.Scene.Activate(TUA.custom.newMainMenuTheme,
                            Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                        Filters.Scene[TUA.custom.newMainMenuTheme].GetShader().UseTargetPosition(new Vector2(2556.793f, 4500f));
                        break;
                }
            }
        }
    }

}
