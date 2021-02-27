using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Ionic.Zip;
using log4net;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;
using TUA.API;
using TUA.API.EventManager;
using TUA.API.Injection;
using TUA.API.TerraEnergy.MachineRecipe.Forge;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.CustomScreenShader;
using TUA.CustomSkies;
using TUA.Dimension.Sky;
using TUA.Discord;
using TUA.Enum;
using TUA.Items.Meteoridon.Materials;
using TUA.Raids.UI;
using TUA.Utilities;
using TUA.Void;
using BiomeLibrary;
using MonoMod.Cil;
using SubworldLibrary;
using TUA.Items;
using TUA.Items.Weapons;
using TUA.Patchs;
using TUA.UI.WorldSelection;

namespace TUA
{
    public class TUA : Mod
    {
        public static string version;
        public static string tModLoaderVersion2;
        public static Version tModLoaderVersionBak;
        public static readonly string SAVE_PATH;

        public static TUA instance;

        public static Texture2D SolarFog;

        public UIWorldSelect originalWorldSelect;
        public static RaidsUI raidsUI;

        public static bool devMode;

        private static List<string> quote;
        private static string animate;

        private int animationTimer = 25;

        private int titleTimer = 0;
        private Title currentTitle;

        public static GameTime gameTime;

        static TUA()
        {
            version = "0.1 dev"; ;
            tModLoaderVersion2 = "";
            SAVE_PATH = Main.SavePath;

            raidsUI = new RaidsUI();

#if DEBUG
            devMode = true;
#else
            devMode = false;
#endif

            quote = new List<string>();
            animate = GetAnimatedTitle();

            gameTime = new GameTime();

            ReflManager<Type>.AddItem("TMain", typeof(Main));
            RaidsManager.Fill();
        }

        public TUA()
        {

            animationTimer = 25;

            titleTimer = 0;

            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true
            };

            //AutoloadDependancies();
        }

        private void AutoloadDependancies()
        {
            Lazy<WebClient> client = new Lazy<WebClient>();

            try
            {
                if (DepMissing("BiomeLibrary"))
                {
                    DownloadDep("BiomeLibrary");
                }
                if (DepMissing("Dimlibs"))
                {
                    DownloadDep("Dimlibs");
                }
                if (DepMissing("LiquidAPI"))
                {
                    DownloadDep("LiquidAPI");
                }
                if (client.IsValueCreated) client.Value.Dispose();
            }
            catch (Exception e)
            {
                ILog dep = LogManager.GetLogger("Dependancy manager");
                dep.Info(e.ToString());
                dep.InfoFormat("TUA unable to download dependancies");

                throw new Exception("TUA unable to download dependancies");
            }

            void DownloadDep(string dep)
            {
                client.Value.DownloadFile("https://github.com/Dradonhunter11/TerrariaUltraApocalypse/" +
                    $"files/3075678/{dep}.zip", $".\\TUACache\\Temp\\{dep}.zip");

                ZipFile.Read($".\\TUACache\\Temp\\{dep}.zip").ExtractAll(".\\TUACache\\");

                AutoloadDep(dep);
            }

            bool DepMissing(string dep)
            {
                return !System.IO.File.Exists($".\\TUACache\\{dep}.dll");
            }

            void AutoloadDep(string dep)
            {
                var mod = Assembly.LoadFile($".\\TUACache\\{dep}.dll");

                // ReflManager<Type>.GetItem("TMain").Assembly.GetType("AssemblyManager")
                //     .GetMethod("Instantiate");
            }
        }

        public override void Load()
        {
            instance = this;

            MonoModHooks.RequestNativeAccess();
            
            Environment.SetEnvironmentVariable("MONOMOD_DMD_TYPE", "MethodBuilder");
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", "dmddump");

            Patch.Load();

            //UpdateBiomesInjection.InjectMe();
            //MethodInfo attempt = StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
            //    .GetMethod("PopulateModBrowser", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            //ReflectionUtils.MethodSwap(StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser"), "PopulateModBrowser", typeof(ModBrowserInjection), "PopulateModBrowser");
            CustomMMHooker.Populatebrowser_Hook += ModBrowserInjection.PopulateModBrowser;
            On.Terraria.Cinematics.CinematicManager.Update += (orig, inst, time) =>
            {
                gameTime = time;
                orig.Invoke(inst, gameTime);
            };

            Main.SavePath += "/Tapocalypse";
            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            if (!Main.dedServ)
            {
                /*FieldInfo UIWorldSelectInfo =
                    ReflManager<Type>.GetItem("TMain").GetField("_worldSelectMenu", BindingFlags.Static | BindingFlags.NonPublic);
                originalWorldSelect = (UIWorldSelect)UIWorldSelectInfo.GetValue(null);
                UIWorldSelectInfo.SetValue(null, new NewUIWorldSelect());*/

                SolarFog = GetTexture("CustomScreenShader/HeavyMist");

                Main.OnTick += DRPSystem.Update;
                DRPSystem.Boot();

                UIManager.InitAll();

                AddFilter();
                AddHotWTranslations();
            }
            HookGenLoader();
        }

        public override void Unload()
        {
            //DrawMapInjection.revert();
            UpdateBiomesInjection.InjectMe();

            Main.SavePath = SAVE_PATH;
            Main.PlayerPath = Main.SavePath + "/Player";
            Main.WorldPath = Main.SavePath + "/World";

            instance = null;
            quote.Clear();
            FieldInfo info2 = typeof(ModLoader).GetField("versionedName",
                BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            info2.SetValue(null, string.Format("tModLoader v{0}", (object)ModLoader.version)
                + (ModLoader.branchName.Length == 0 ? "" : " " + ModLoader.branchName)
                + (ModLoader.beta == 0 ? "" : string.Format(" Beta {0}", (object)ModLoader.beta)));

            MoonEventManagerWorld.moonEventList.Clear();

            if (!Main.dedServ)
            {
                DRPSystem.Kill();
                Main.OnTick -= DRPSystem.Update;
            }

            ReflManager<Type>.Clear();
            RaidsManager.Clear();
        }

        private static void HookGenLoader()
        {

            ILCursor c;
            /*IL.Terraria.Main.GUIChatDrawInner += il =>
            {
                
                c = il.At(0);

                // Let's go to the next SetChatButtons call.
                // From the start of the method, it's the first one.
                // Assuming that SetChatButtons is a static method in NPCLoader...
                if (c.TryGotoNext(i => i.MatchCall(typeof(NPCLoader), "SetChatButtons")))
                {

                    c.Index++;
                    c.EmitDelegate<RaidsGlobalNPC.SetChatButtonsCustomDelegate>(RaidsGlobalNPC.SetChatButtonsCustom);
                }
            };*/

            IL.Terraria.Main.UpdateAudio += il =>
            {

                c = il.At(0);
                if (c.TryGotoNext(i =>
                    i.MatchLdarg(out int empty),
                    i => i.MatchLdfld(out FieldReference reference),
                    i => i.MatchStsfld(out FieldReference reference2),
                    i => i.MatchLdcR4(out float anotherUseless),
                    i => i.MatchStloc(out int empty2)))
                {
                    c.Index += 5;
                    c.EmitDelegate<Action>(() =>
                    {
                        if (Main.gameMenu)
                        {
                            Main.curMusic = instance.GetSoundSlot(SoundType.Music, "Sounds/Music/ArmageddonsAnthem");
                        }
                    });
                }
            };
        }

        private static void AddFilter()
        {
            Filters.Scene["TUA:TUAPlayer"] =
                new Filter(
                    new ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                    EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:TUAPlayer"] = new TUACustomSky();
            Filters.Scene["TUA:StardustPillar"] =
                new Filter(
                    new ScreenShaderData("FilterMoonLord").UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                    EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:StardustPillar"] = new StardustCustomSky();
            Filters.Scene["TUA:SolarMist"] = new Filter(new MeteoridonScreenShader().UseColor(0.4f, 0, 0).UseOpacity(0.7f),
                EffectPriority.VeryHigh);
            SkyManager.Instance["TUA:SolarMist"] = new HeavyMistSky();
        }

        private void AddHotWTranslations()
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
            var r = new UnifiedRandom();
            InitializeQuoteList();

            tModLoaderVersion2 = "tModLoader v" + ModLoader.version;
            tModLoaderVersionBak = ModLoader.version;

            return $"{tModLoaderVersion2} - TUA v{version} - {quote[r.Next(quote.Count)]}";
        }

        public override void AddRecipes()
        {
            AddAllPreHardmodeRecipeFurnace();
            AddAllHardmodeRecipeFurnace();
            AddFurnaceRecipe(ItemID.SandBlock, ItemID.Glass, 20);

            //Smetler recipe recipe
            AddInductionSmelterRecipe(ItemID.Hellstone, ItemID.Obsidian, ItemID.HellstoneBar, 1, 2, 1, 90);
            AddInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.Ectoplasm, ItemID.SpectreBar, 4, 1, 3);
            AddInductionSmelterRecipe(ItemID.ChlorophyteBar, ItemID.GlowingMushroom, ItemID.ShroomiteBar, 1, 5, 1, 180);

            RecipeUtils.GetAllRecipeByIngredientAndReplace(ItemID.PixieDust, ModContent.ItemType<MeteorideScale>());

            var recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.LightsBane);
            recipe.AddIngredient(ItemID.BladeofGrass);
            recipe.AddIngredient(ItemID.Muramasa);
            recipe.AddIngredient(ModContent.ItemType<VenomousGreatBlade>());
            recipe.SetResult(ItemID.NightsEdge);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.BloodButcherer);
            recipe.AddIngredient(ItemID.BladeofGrass);
            recipe.AddIngredient(ItemID.Muramasa);
            recipe.AddIngredient(ModContent.ItemType<VenomousGreatBlade>());
            recipe.SetResult(ItemID.NightsEdge);
            recipe.AddRecipe();
        }

        public void AddFurnaceRecipe(int itemID, int itemResult, int timer = 20)
        {
            FurnaceRecipe r1 = FurnaceRecipeManager.CreateRecipe(this);
            r1.AddIngredient(itemID, 1);
            r1.SetResult(itemResult, 1);
            r1.SetCostAndCookTime(timer);
            r1.AddRecipe();
        }

        public void AddInductionSmelterRecipe(int catalyst, int reactant, 
            int product, int catalystQuantity = 1, int reactantQuantity = 1, 
            int productQuantity = 1, int timer = 120)
        {
            ForgeRecipe fr1 = ForgeRecipeManager.CreateRecipe(this);
            fr1.AddCatalyst(catalyst, catalystQuantity);
            fr1.AddIngredient(reactant, reactantQuantity);
            fr1.SetResult(product, productQuantity);
            fr1.AddRecipe();
        }

        public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBossWithInfo", "Ultra Eye of Cthulhu", 16.0f, (Func<bool>)(() => TUAWorld.EoCDeathCount >= 1), "Use a [i:" + ItemID.SuspiciousLookingEye + "] at night after Moon lord has been defeated");
                bossChecklist.Call("AddBossWithInfo", "Eye of EoADowned - God of Destruction", 16.1f, (Func<bool>)(() => ModContent.GetInstance<TUAWorld>().UltraMode), "Use a [i:" + ItemType("Spawner") + "] after --1sing Ay. 0F C1^lh> in ^1tra and murder it, if you can...");
            }

            Mod achievementLibs = ModLoader.GetMod("AchievementLibs");
            if (achievementLibs != null)
            {
                Func<bool> c = () => TUAWorld.EoADowned;
                achievementLibs.Call("AddAchievementWithoutReward", this, "Once there was an eye that was a god...", "Kill the Eye of Apocalypse - God of Destruction!", "Achievement/EoAKill", c);
                achievementLibs.Call("AddAchievementWithoutReward", this, "The terrible moon tried to kill us", "Use [i:" + ModContent.ItemType<EyeOfPerception>() + "] and succesfully survive all 8 wave of the Apocalypse Moon", "Achievement/EoAKill", (Func<bool>)(() => TUAWorld.ApoMoonDowned));
                achievementLibs.Call("AddAchievementWithoutAction", this,
                    "Once there was the Eye of Cthulhu... the ultra one", "Kill the Ultra EoC succesfully.",
                    "Achievement/UltraEoC", new int[] { ModContent.ItemType<EyeOfPerception>() }, new int[] { 1 },
                    (Func<bool>)(() => TUAWorld.EoCDeathCount >= 1));
            }

            RecipeUtils.SetAllFurnaceRecipeSystem();

            ReflManager<Type>.RemoveItem("TMain");
            ReflManager<Type>.AddItem("TMain", typeof(Main));
        }

        public override void UpdateUI(GameTime gameTime) => UIManager.UpdateUI(gameTime);

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
                // DimPlayer p = Main.player[Main.myPlayer].GetModPlayer<DimPlayer>();
                FieldInfo info = typeof(LanguageManager).GetField("_localizedTexts", BindingFlags.Instance | BindingFlags.NonPublic);
                Dictionary<string, LocalizedText> dictionary = info.GetValue(LanguageManager.Instance) as Dictionary<string, LocalizedText>;

                FieldInfo textInfo = typeof(LocalizedText).GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
                ResetMenu(dictionary, textInfo);
            }
            if (Main.myPlayer != -1 && !Main.gameMenu && Main.LocalPlayer.active)
            {
                UpdateInGameMusic(ref music, ref musicPriority);
            }
            AnimateVersion();
        }

        private void UpdateInGameMusic(ref int music, ref MusicPriority musicPriority)
        {
            if (this.GetBiome("Meteoridon").InBiome(Main.LocalPlayer))
            {
                music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/Stars_Lament_Loop");
            }
            else if (MoonEventManagerWorld.IsActive("Apocalypse Moon"))
            {
                music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/Terminal_Inception");
                musicPriority = MusicPriority.BossHigh;
            }
            else if (Subworld.IsActive<SolarSubworld>())
            {
                music = MusicID.TheTowers;
                Main.musicBox = 36;
                musicPriority = MusicPriority.Environment;
            }
            else if (Main.LocalPlayer.position.Y / 16 > Main.maxTilesY - 200 &&
                     Main.ActiveWorldFileData.HasCrimson)
            {
                music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/Exclusion_Zone");
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
                        UIManager.DrawFurnace();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                //TODO 1.1 : Anvil interface
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : Raids GUI", delegate
                    {
                        UIManager.DrawRaids();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : LORE IN CAPS", delegate
                    {
                        UIManager.DrawLore();
                        return true;
                    }, InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "TUA : Void GUI", delegate
                    {
                        UIManager.OpenVoidUI();
                        UIManager.DrawVoid();
                        return true;
                    }, InterfaceScaleType.UI)
                );
            }
        }


        private void AddAllPreHardmodeRecipeFurnace()
        {
            AddFurnaceRecipe(ItemID.CopperOre, ItemID.CopperBar);
            AddFurnaceRecipe(ItemID.TinOre, ItemID.TinBar, 10);
            AddFurnaceRecipe(ItemID.IronOre, ItemID.IronBar, 30);
            AddFurnaceRecipe(ItemID.LeadOre, ItemID.LeadBar);
            AddFurnaceRecipe(ItemID.SilverOre, ItemID.SilverBar);
            AddFurnaceRecipe(ItemID.TungstenOre, ItemID.TungstenBar);
            AddFurnaceRecipe(ItemID.GoldOre, ItemID.GoldBar, 30);
            AddFurnaceRecipe(ItemID.PlatinumOre, ItemID.PlatinumBar, 40);
            AddFurnaceRecipe(ItemID.CrimtaneOre, ItemID.CrimtaneBar);
            AddFurnaceRecipe(ItemID.DemoniteOre, ItemID.DemoniteBar);
            AddFurnaceRecipe(ItemID.Meteorite, ItemID.MeteoriteBar, 30);
        }

        private void AddAllHardmodeRecipeFurnace()
        {
            AddFurnaceRecipe(ItemID.CobaltOre, ItemID.CobaltBar, 25);
            AddFurnaceRecipe(ItemID.PalladiumOre, ItemID.PalladiumBar, 25);
            AddFurnaceRecipe(ItemID.MythrilOre, ItemID.MythrilBar, 40);
            AddFurnaceRecipe(ItemID.OrichalcumOre, ItemID.OrichalcumBar, 50);
            AddFurnaceRecipe(ItemID.AdamantiteOre, ItemID.AdamantiteBar, 70);
            AddFurnaceRecipe(ItemID.TitaniumOre, ItemID.TitaniumBar, 80);
            AddFurnaceRecipe(ItemID.ChlorophyteOre, ItemID.ChlorophyteBar, 120);
            AddFurnaceRecipe(ItemID.LunarOre, ItemID.LunarBar, 240);
        }

        private static void InitializeQuoteList()
        {
            quote = new List<string>
            {
                "Now with 100% less life insurance! ",
                "Make sure to give EoA my best . . . or my worst depending on how you look at it ",
                "I failed to help Heather with a door edition ",
                "You have only been to 3 dimensions? Pffft ",
                "You want me to die? Not if I kill myself first! ",
                "The nurse may need a few more doctorate degrees . . . and a better hairdo ",
                "Our mod will create an exodus from the others! ",
                "Build a wall? Hah, we have more important things to do ",
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
                "WHERE IS MAH SPAGHET CODE - Red ",
                "This is Minecraft, wait no it's evil ",
                "The fairy land, it's great... and evil... but great! ",
                "Lots of spaghetti! ",
                "Terraria: A game you're currently playing ",
                "Also available in 64bit ",
                "Too many toasters! ",
                "2738 times, now that's dedication! ",
                "Now 100% clean code free ",
                "All to the pickle train ",
                "You beat the Moon Lord? HA, YEAH RIGHT, A PUNY KID LIKE YOU!? ",
                "Ya know, Ningishu just released a 45 minute Moon Lord speedrun ",
                "Nvidia Turing was a complete disappointment "
            };
            if (IntPtr.Size == 8)
            {
                quote.Add("HAH, YOU GOT 64BIT? JUST DOWNLOAD MORE RAM, NOOB (that was a joke btw)");
            }
        }

        private void ResetMenu(Dictionary<string, LocalizedText> dictionary, FieldInfo textInfo)
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

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            TUANetMessage message = (TUANetMessage)reader.ReadByte();
            switch (message)
            {
                case TUANetMessage.VoidPlayerSync:
                    VoidPlayer player = Main.player[reader.ReadByte()].GetModPlayer<VoidPlayer>();
                    player.voidAffinity = reader.ReadInt32();
                    player.maxVoidAffinity = reader.ReadInt32();
                    player.voidTier2Unlocked = reader.ReadBoolean();
                    player.voidTier3Unlocked = reader.ReadBoolean();
                    player.voidTier4Unlocked = reader.ReadBoolean();
                    break;
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (currentTitle.active)
            {
                titleTimer--;
                currentTitle.Draw(spriteBatch, titleTimer);
            }
        }

        public void SetTitle(string text, string subText, Color textColor, Color subTextColor, DynamicSpriteFont font, int timer = 30, float baseOpacity = 1f, bool fadeEffect = false)
        {
            titleTimer = timer;
            currentTitle = new Title(text, subText, textColor, subTextColor, font, timer, baseOpacity, fadeEffect);


        }

        // TO DO : Move this to a proper utils file
        public static void BroadcastMessage(string text, Color color)
        {
            switch (Main.netMode)
            {
                case NetmodeID.Server:
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), color, -1);
                    return;
                case NetmodeID.SinglePlayer:
                    Main.NewText(text, color, true);
                    return;
            }
        }

        public static void BroadcastMessage(string text)
        {
            BroadcastMessage(text, Color.White);
        }

        protected struct Title
        {
            public string text, subText;
            public Color textColor, subTextColor;
            public DynamicSpriteFont font;
            public float baseOpacity, currentOpacity;
            public bool fadeEffect;
            public int maxTimer;
            public bool active;

            public Title(string text, string subText, Color textColor, Color subTextColor, DynamicSpriteFont font, int maxTimer = 30, float baseOpacity = 1, bool fadeEffect = true)
            {
                this.text = text;
                this.subText = subText;
                this.textColor = textColor;
                this.subTextColor = subTextColor;
                this.font = font;
                this.baseOpacity = baseOpacity;
                this.fadeEffect = fadeEffect;
                this.maxTimer = maxTimer;
                this.currentOpacity = 0;
                this.active = true;
            }

            public void Draw(SpriteBatch sb, int currentTimer)
            {

                Vector2 textSize = font.MeasureString(text) * 1.5f;
                Vector2 subTextSize = font.MeasureString(subText) * 0.9f;

                float top = Main.screenHeight / 2 - 150;
                float bottom = Main.screenHeight / 2 - 50;
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                float subTextPositionLeft = Main.screenWidth / 2 - subTextSize.X / 2;

                if (fadeEffect)
                {
                    if (currentTimer > maxTimer - 10 && currentOpacity < baseOpacity)
                    {
                        currentOpacity += 0.1f;
                    }
                    else if (currentTimer < 10)
                    {
                        currentOpacity -= 0.1f;
                    }
                }
                else
                {
                    currentOpacity = baseOpacity;
                }

                if (currentTimer == 0)
                {
                    active = false;
                }

                Utils.DrawBorderStringFourWay(sb, font, text, textPositionLeft, top, textColor * currentOpacity, Color.Gray * currentOpacity, Vector2.Zero, 1.5f);
                Utils.DrawBorderStringFourWay(sb, font, subText, subTextPositionLeft, bottom, subTextColor * currentOpacity, Color.Gray * currentOpacity, Vector2.Zero, 0.9f);
            }
        }
    }
}
