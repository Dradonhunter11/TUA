using BiomeLibrary.API;
using Dimlibs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
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

namespace TUA
{
    internal class TerrariaUltraApocalypse : Mod
    {
        internal static string version = "0.1 dev";
        internal static String tModLoaderVersion2 = "";
        internal static Version tModLoaderVersion;
        internal static readonly string SAVE_PATH = Main.SavePath;

        internal static TerrariaUltraApocalypse instance;


        internal static Texture2D SolarFog;

        internal static UserInterface machineInterface;
        internal static UserInterface CapacitorInterface;
        internal static UserInterface raidsInterface;

        internal UIWorldSelect originalWorldSelect;
        internal readonly MainMenuUI newMainMenu = new MainMenuUI();
        internal static RaidsUI raidsUI = new RaidsUI();

        internal static CustomTitleMenuConfig custom;

        private static List<string> quote = new List<string>();
        private static string animate = GetAnimatedTitle();

        private int animationTimer = 25;

        public TerrariaUltraApocalypse()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true

            };
        }

        public override void Load()
        {
            LoadModContent(mod =>
            {
                Autoload(mod);
            });
            newMainMenu.load();
            

            instance = this;
            UpdateBiomesInjection.inject();
            LiquidRegistery.MassMethodSwap();
            Console.Write("AM I NULL? " + typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser"));
            MethodInfo attempt = typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetMethod("PopulateModBrowser", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            ReflectionUtils.MethodSwap(typeof(Main).Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser"), "PopulateModBrowser", typeof(ModBrowserInjection), "PopulateModBrowser");

            Main.SavePath += "/Tapocalypse";
            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            if (!Main.dedServ)
            {
                FieldInfo UIWorldSelectInfo =
                    typeof(Main).GetField("_worldSelectMenu", BindingFlags.Static | BindingFlags.NonPublic);
                originalWorldSelect = (UIWorldSelect)UIWorldSelectInfo.GetValue(null);
                UIWorldSelectInfo.SetValue(null, new NewUIWorldSelect());

                SteamID64Checker.getInstance().CopyIDToClipboard();

                SolarFog = GetTexture("CustomScreenShader/HeavyMist");

                //DRPSystem.Init();
                //Main.OnTick += DRPSystem.Update;
                

                machineInterface = new UserInterface();
                CapacitorInterface = new UserInterface();
                raidsInterface = new UserInterface();

                AddFilter();
                CreateTranslation();
            }


            //DRPSystem.Init();
            //Main.OnTick += DRPSystem.Update;
        }

        private static void AddFilter()
        {
            Filters.Scene["TUA:TUAPlayer"] =
                new Filter(
                    new Terraria.Graphics.Shaders.ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                    EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:TUAPlayer"] = new TUACustomSky();
            Filters.Scene["TUA:StardustPillar"] =
                new Filter(
                    new Terraria.Graphics.Shaders.ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                    EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:StardustPillar"] = new StardustCustomSky();
            Filters.Scene["TUA:SolarMist"] = new Filter(new MeteoridonScreenShader().UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:SolarMist"] = new HeavyMistSky();
        }

        private void CreateTranslation()
        {
            var text = CreateTranslation("HotWFarAway0");
            text.SetDefault("<{0}> Where do you think you're going??");
            AddTranslation(text);

            text = CreateTranslation("HotWFarAway1");
            text.SetDefault("<{0}> No, no, no, we can't have that, come back here.");
            AddTranslation(text);

            text = CreateTranslation("HotWFarAway2");
            text.SetDefault("<{0}> Come back here! You awakened me, so I will make you pay!");
            AddTranslation(text);

            text = CreateTranslation("HotWFarAway3");
            text.SetDefault("<{0}> Face me, cowardly Terrarian!");
            AddTranslation(text);

            text = CreateTranslation("HotWFarAwayStuck");
            text.SetDefault("<{0}> Welp, your fault for trying to run away.");
        }

        public static string GetAnimatedTitle()
        {
            Random r = new Random();
            InitializeQuoteList();

            tModLoaderVersion2 = "tModLoader v" + ModLoader.version;
            tModLoaderVersion = ModLoader.version;

            return tModLoaderVersion2 + $" - TUA v{version} - {quote[r.Next(quote.Count - 1)]}";
        }



        public override void Unload()
        {
            //DrawMapInjection.revert();
            UpdateBiomesInjection.inject();
            LiquidRegistery.MassMethodSwap();

            Main.SavePath = SAVE_PATH;

            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            instance = null;
            quote.Clear();
            FieldInfo info2 = typeof(ModLoader).GetField("versionedName",
                BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            info2.SetValue(null, string.Format("tModLoader v{0}", (object)Terraria.ModLoader.ModLoader.version) + (Terraria.ModLoader.ModLoader.branchName.Length == 0 ? "" : " " + Terraria.ModLoader.ModLoader.branchName) + (Terraria.ModLoader.ModLoader.beta == 0 ? "" : string.Format(" Beta {0}", (object)Terraria.ModLoader.ModLoader.beta)));

            //Remember to re enable it once it's fixed
            if (!Main.dedServ)
            {
                //DRPSystem.Kill();
                //Main.OnTick -= DRPSystem.Update;
            }
        }

        public override void AddRecipes()
        {
            AddAllPreHardmodeRecipeFurnace();
            AddAllHardmodeRecipeFurnace();
            addFurnaceRecipe(ItemID.SandBlock, ItemID.Glass, 20);

            //Smetler recipe recipe
            AddInductionSmelterRecipe(ItemID.Hellstone, ItemID.Obsidian, ItemID.HellstoneBar, 1, 2, 1, 90);
            AddInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.Ectoplasm, ItemID.SpectreBar, 4, 1, 3);
            AddInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.GlowingMushroom, ItemID.ShroomiteBar, 1, 5, 1, 180);

            RecipeUtils.GetAllRecipeByIngredientAndReplace(ItemID.PixieDust, ItemType<MeteorideScale>());
        }

        public void addFurnaceRecipe(int itemID, int itemResult, int timer = 20)
        {
            FurnaceRecipe r1 = FurnaceRecipeManager.CreateRecipe(this);
            r1.addIngredient(itemID, 1);
            r1.setResult(itemResult, 1);
            r1.setCostAndCookTime(timer);
            r1.addRecipe();
        }

        public void AddInductionSmelterRecipe(int itemID1, int itemID2, int itemResult, int quantityItem1 = 1, int quantityItem2 = 1, int resultQuantity = 1, int timer = 120)
        {
            ForgeRecipe fr1 = ForgeRecipeManager.CreateRecipe(this);
            fr1.addCatalyserIngredient(itemID1, quantityItem1);
            fr1.addIngredient(itemID2, quantityItem2);
            fr1.setResult(itemResult, resultQuantity);
            fr1.addRecipe();
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

            RecipeUtils.setAllFurnaceRecipeSystem();
            LiquidRegistery.getInstance().addNewModLiquid(new PlutonicWaste());
            LiquidRegistery.getInstance().addNewModLiquid(new WeirdLiquid());
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
                UpdateInGameMusic(ref music, ref musicPriority);
            }

            if (Main.menuMode == 0 && custom.customMenu)
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(newMainMenu);
            }
            AnimateVersion();
            if (Main.gameMenu && Main.menuMode == 0 || (Main.menuMode == 888 && TerrariaUltraApocalypse.custom.customMenu))
                TerrariaUltraApocalypse.SetTheme();

        }

        private void UpdateInGameMusic(ref int music, ref MusicPriority musicPriority)
        {
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

        public override object Call(params object[] args)
        {
            string command = args[0] as string;
            if (args[0] == "UltraMode")
            {
                return TUAWorld.UltraMode;
            }
            return base.Call(args);
        }

        

        private void AddAllPreHardmodeRecipeFurnace()
        {
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
        }

        private void AddAllHardmodeRecipeFurnace()
        {
            addFurnaceRecipe(ItemID.CobaltOre, ItemID.CobaltBar, 25);
            addFurnaceRecipe(ItemID.PalladiumOre, ItemID.PalladiumBar, 25);
            addFurnaceRecipe(ItemID.MythrilOre, ItemID.MythrilBar, 40);
            addFurnaceRecipe(ItemID.OrichalcumOre, ItemID.OrichalcumBar, 50);
            addFurnaceRecipe(ItemID.AdamantiteOre, ItemID.AdamantiteBar, 70);
            addFurnaceRecipe(ItemID.TitaniumOre, ItemID.TitaniumBar, 80);
            addFurnaceRecipe(ItemID.ChlorophyteOre, ItemID.ChlorophyteBar, 120);
            addFurnaceRecipe(ItemID.LunarOre, ItemID.LunarBar, 240);
        }

        private static void InitializeQuoteList()
        {
            quote = new List<string>
            {
                "Now with 100% less life insurance! ",
                "Make sure to give EoA my best . . . or my worst depending on how you look at it ",
                "I failed to help Heather with a door edition ",
                "You only have 3 dimensions? pffft ",
                "You want me to die? Not if I kill myself first! ",
                "The nurse may need a few more doctorate degrees . . . and a better hairdo ",
                "Our mod will create an exodus from the others! ",
                "Build a wall? Pffft, we have more important things to do ",
                "Old age should burn and rave at the close of day . . . yes, that means you, Jof ",
                "Now with a bunch of stupid title version like this one! ",
                "I dont feel so good... ",
                "Should have gone for the cpu ",
                "I'll go grab some pizza ",
                "Don't forget to change this with something, ok? ",
                "REEEEEEEEEEEEEEEEEEEE! ",
                "Just remember boys and girls, no anime :smile: ",
                "This mod does not exist ",
                "I heard from a guy a weird story abouts blocks ",
                "Marble it down! ",
                "Minecraft world loading screen? Don't be dumb ",
                "I heard that forknife was a good kitchen utensil game, try it! ",
                "So you heard that? No, welp it's you! ",
                "Rumor has it that club penguin is evil ",
                "I hate it when I code and a velociraptor throw a fridge at my grandma ",
                "Meme? NO MEME ALLOWED HERE ",
                "Dimension is a very simple thing, it's basically a bunch of non-sense my child ",
                "Terraria is not a game, it's a mess! ",
                "It's not a mess, it's red code",
                "Somebody touch my spaghet code - Red ",
                "This is minecraft, wait no it's evil ",
                "The fairy land, it's great... and evil... but great! ",
                "Lots of spaghetti! ",
                "Terraria: A game you're currently playing ",
                "Also available in 64bit ",
                "Too many toasters! ",
                "2738 times, now that's dedication! ",
                "Now 100% clean code free ",
                "All to the pickle train "
            };
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

        private void resetMenu(Dictionary<string, LocalizedText> dictionary, FieldInfo textInfo)
        {
            if (Main.menuMode == 1)
            {
                textInfo.SetValue(dictionary["UI.New"], "New");
                textInfo.SetValue(dictionary["UI.SelectWorld"], "Select World");
            }
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

        private static void LoadModContent(Action<Mod> loadAction)
        {
            for (int i = 0; i < ModLoader.Mods.Length; i++)
            {
                Mod mod = ModLoader.Mods[i];
                try
                {
                    loadAction(mod);
                }
                catch { }
            }
        }

        private void Autoload(Mod mod)
        {

            if (mod.Code == null)
            {
                return;
            }

            foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
            {
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

        private static void SetTheme()
        {
            Dictionary<String, CustomSky> temp2 = (Dictionary<string, CustomSky>)typeof(SkyManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SkyManager.Instance);
            Dictionary<String, Filter> temp = (Dictionary<string, Filter>)typeof(FilterManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Filters.Scene);
            List<String> allKey = temp.Keys.ToList();
            foreach (var key in allKey)
            {
                if (key != TerrariaUltraApocalypse.custom.newMainMenuTheme)
                    Filters.Scene[key].Deactivate();
            }


            allKey = temp2.Keys.ToList();
            foreach (var key in allKey)
            {
                if (key != TerrariaUltraApocalypse.custom.newMainMenuTheme)
                    SkyManager.Instance.Deactivate(key);
            }

            Main.worldSurface = 565;
            if (TerrariaUltraApocalypse.custom.newMainMenuTheme != "Vanilla" && !SkyManager.Instance[TerrariaUltraApocalypse.custom.newMainMenuTheme].IsActive())
            {
                switch (TerrariaUltraApocalypse.custom.newMainMenuTheme)
                {
                    case "Vanilla":
                        return;
                    default:
                        if (Filters.Scene[TerrariaUltraApocalypse.custom.newMainMenuTheme] != null)
                            Filters.Scene.Activate(TerrariaUltraApocalypse.custom.newMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        if (SkyManager.Instance[TerrariaUltraApocalypse.custom.newMainMenuTheme] != null)
                            SkyManager.Instance.Activate(TerrariaUltraApocalypse.custom.newMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        if (Overlays.Scene[TerrariaUltraApocalypse.custom.newMainMenuTheme] != null)
                            Overlays.Scene.Activate(TerrariaUltraApocalypse.custom.newMainMenuTheme,
                                Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                        break;
                }
            }
        }

        /// <summary>
        /// Move to dimlibs or implement trough the new Save File API
        /// </summary>
        private void AllowGignaticWorld()
        {
            /*
            Main.chest = new Chest[10000];
            Main.tile = new Tile[25200, 9600];
            Main.maxTilesX = 25200;
            Main.maxTilesY = 9600;*/
        }
    }
}
