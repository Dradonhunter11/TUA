using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Dimlibs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.Dev;

namespace TUA.API.Injection
{
    class UpdateBiomesInjection
    {

        public static void InjectMe()
        {
            ReflectionUtils.MethodSwap(typeof(Player), "UpdateBiomes", typeof(UpdateBiomesInjection), "UpdateBiome");
        }

        public void UpdateBiome()
        {
            Player self = Main.LocalPlayer;

            FieldInfo _strongBlizzardSoundInfo = typeof(Player).GetField("_strongBlizzardSound",
                BindingFlags.Static | BindingFlags.NonPublic);
            SlotId _strongBlizzardSound = (SlotId)_strongBlizzardSoundInfo.GetValue(null);

            FieldInfo _weakBlizzardSoundInfo = typeof(Player).GetField("_weakBlizzardSound",
                BindingFlags.Static | BindingFlags.NonPublic);
            SlotId _weakBlizzardSound = (SlotId)_weakBlizzardSoundInfo.GetValue(null);

            FieldInfo _insideBlizzardSoundInfo = typeof(Player).GetField("_insideBlizzardSound",
                BindingFlags.Static | BindingFlags.NonPublic);
            SlotId _insideBlizzardSound = (SlotId)_insideBlizzardSoundInfo.GetValue(null);

            FieldInfo _blizzardSoundVolumeInfo = typeof(Player).GetField("_blizzardSoundVolume",
                BindingFlags.Static | BindingFlags.NonPublic);
            float _blizzardSoundVolume = (float)_blizzardSoundVolumeInfo.GetValue(null);

            FieldInfo _shaderObstructionInternalValueInfo = typeof(Player).GetField("_shaderObstructionInternalValue",
                BindingFlags.Instance | BindingFlags.NonPublic);
            float _shaderObstructionInternalValue = (float)_shaderObstructionInternalValueInfo.GetValue(self);

            FieldInfo _stormShaderObstructionInfo = typeof(Player).GetField("_stormShaderObstruction",
                BindingFlags.Instance | BindingFlags.NonPublic);
            float _stormShaderObstruction = (float)_stormShaderObstructionInfo.GetValue(self);


            Point point = self.Center.ToTileCoordinates();
            self.ZoneDungeon = false;
            if (Main.dungeonTiles >= 250 && (double)self.Center.Y > Main.worldSurface * 16.0)
            {
                int num = (int)self.Center.X / 16;
                int num2 = (int)self.Center.Y / 16;
                if (Main.wallDungeon[(int)Main.tile[num, num2].wall])
                {
                    self.ZoneDungeon = true;
                }
            }
            //Tile tileSafely = Framing.GetTileSafely(self.Center);
            /*if (tileSafely != null)
            {
                self.behindBackWall = (tileSafely.wall > 0);
            }
            if (Main.sandTiles > 1000 && self.Center.Y > 3200f)
            {
                if (WallID.Sets.Conversion.Sandstone[(int)tileSafely.wall] || WallID.Sets.Conversion.HardenedSand[(int)tileSafely.wall])
                {
                    self.ZoneUndergroundDesert = true;
                }
            }
            else
            {
                self.ZoneUndergroundDesert = false;
            }*/
            self.ZoneCorrupt = (Main.evilTiles >= 200);
            self.ZoneHoly = (Main.holyTiles >= 100);
            self.ZoneMeteor = (Main.meteorTiles >= 50);
            self.ZoneJungle = (Main.jungleTiles >= 80);
            self.ZoneSnow = (Main.snowTiles >= 300);
            self.ZoneCrimson = (Main.bloodTiles >= 200);
            self.ZoneWaterCandle = (Main.waterCandles > 0);
            self.ZonePeaceCandle = (Main.peaceCandles > 0);
            self.ZoneDesert = (Main.sandTiles > 1000);
            self.ZoneGlowshroom = (Main.shroomTiles > 100);
            if (Dimlibs.Dimlibs.getPlayerDim() != "overworld")
            {
                self.ZoneUnderworldHeight = false;
                self.ZoneSkyHeight = false;
                self.ZoneBeach = false;
            }
            else
            {
                /*
                self.ZoneUnderworldHeight = (point.Y > Main.maxTilesY - 200);
                self.ZoneSkyHeight = ((double)point.Y <= Main.worldSurface * 0.34999999403953552);
                self.ZoneBeach = (self.ZoneOverworldHeight && (point.X < 380 || point.X > Main.maxTilesX - 380));*/

                self.ZoneUnderworldHeight = false;
                self.ZoneSkyHeight = false;
                self.ZoneBeach = false;
            }
            //Main.NewText("Update biome Dim was succesfully injected");
            //self.ZoneUnderworldHeight = (point.Y > Main.maxTilesY - 200);
            self.ZoneRockLayerHeight = (point.Y <= Main.maxTilesY - 200 && (double)point.Y > Main.rockLayer);
            self.ZoneDirtLayerHeight = ((double)point.Y <= Main.rockLayer && (double)point.Y > Main.worldSurface);
            self.ZoneOverworldHeight = ((double)point.Y <= Main.worldSurface && (double)point.Y > Main.worldSurface * 0.34999999403953552);
            
            self.ZoneRain = (Main.raining && (double)point.Y <= Main.worldSurface);
            self.ZoneSandstorm = ((double)point.Y <= Main.worldSurface && self.ZoneDesert && !self.ZoneBeach && Sandstorm.Happening);
            self.ZoneTowerSolar = (self.ZoneTowerVortex = (self.ZoneTowerNebula = (self.ZoneTowerStardust = false)));
            self.ZoneOldOneArmy = false;
            Vector2 value = Vector2.Zero;
            Vector2 value2 = Vector2.Zero;
            Vector2 value3 = Vector2.Zero;
            Vector2 value4 = Vector2.Zero;
            Vector2 arg_32B_0 = Vector2.Zero;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active)
                {
                    if (Main.npc[i].type == 493)
                    {
                        if (self.Distance(Main.npc[i].Center) <= 4000f)
                        {
                            self.ZoneTowerStardust = true;
                            value4 = Main.npc[i].Center;
                        }
                    }
                    else if (Main.npc[i].type == 507)
                    {
                        if (self.Distance(Main.npc[i].Center) <= 4000f)
                        {
                            self.ZoneTowerNebula = true;
                            value3 = Main.npc[i].Center;
                        }
                    }
                    else if (Main.npc[i].type == 422)
                    {
                        if (self.Distance(Main.npc[i].Center) <= 4000f)
                        {
                            self.ZoneTowerVortex = true;
                            value2 = Main.npc[i].Center;
                        }
                    }
                    else if (Main.npc[i].type == 517)
                    {
                        if (self.Distance(Main.npc[i].Center) <= 4000f)
                        {
                            self.ZoneTowerSolar = true;
                            value = Main.npc[i].Center;
                        }
                    }
                    else if (Main.npc[i].type == 549 && self.Distance(Main.npc[i].Center) <= 4000f)
                    {
                        self.ZoneOldOneArmy = true;
                        value = Main.npc[i].Center;
                    }
                }
            }
            bool flag = self.ZoneRain && self.ZoneSnow;
            bool flag2 = point.Y > Main.maxTilesY - 320;
            bool flag3 = self.ZoneOverworldHeight && (point.X < 380 || point.X > Main.maxTilesX - 380);
            // TODO, are these flags a problem?
            PlayerHooks.UpdateBiomes(self);
            self.ManageSpecialBiomeVisuals("Stardust", self.ZoneTowerStardust, value4 - new Vector2(0f, 10f));
            self.ManageSpecialBiomeVisuals("Nebula", self.ZoneTowerNebula, value3 - new Vector2(0f, 10f));
            self.ManageSpecialBiomeVisuals("Vortex", self.ZoneTowerVortex, value2 - new Vector2(0f, 10f));
            self.ManageSpecialBiomeVisuals("Solar", self.ZoneTowerSolar, value - new Vector2(0f, 10f));
            self.ManageSpecialBiomeVisuals("MoonLord", NPC.AnyNPCs(398), default(Vector2));
            self.ManageSpecialBiomeVisuals("BloodMoon", Main.bloodMoon, default(Vector2));
            self.ManageSpecialBiomeVisuals("Blizzard", Main.UseStormEffects && flag, default(Vector2));
            self.ManageSpecialBiomeVisuals("HeatDistortion", Main.UseHeatDistortion && (flag2 || ((double)point.Y < Main.worldSurface && self.ZoneDesert && !flag3 && !Main.raining && !Filters.Scene["Sandstorm"].IsActive())), default(Vector2));
            if (!Filters.Scene["WaterDistortion"].IsActive() && Main.WaveQuality > 0)
            {
                Filters.Scene.Activate("WaterDistortion", default(Vector2), new object[0]);
            }
            else if (Filters.Scene["WaterDistortion"].IsActive() && Main.WaveQuality == 0)
            {
                Filters.Scene.Deactivate("WaterDistortion", new object[0]);
            }
            if (Filters.Scene["WaterDistortion"].IsActive())
            {
                float num3 = (float)Main.maxTilesX * 0.5f - Math.Abs((float)point.X - (float)Main.maxTilesX * 0.5f);
                float num4 = 1f;
                float num5 = Math.Abs(Main.windSpeed);
                num4 += num5 * 1f;
                float num6 = MathHelper.Clamp(Main.maxRaining, 0f, 1f);
                num4 += num6 * 1.5f;
                float num7 = -(MathHelper.Clamp((num3 - 380f) / 100f, 0f, 1f) * 0.5f - 0.25f);
                num4 += num7;
                float num8 = 1f - MathHelper.Clamp(3f * ((float)((double)point.Y - Main.worldSurface) / (float)(Main.rockLayer - Main.worldSurface)), 0f, 1f);
                num4 *= num8;
                float num9 = 0.9f - MathHelper.Clamp((float)(Main.maxTilesY - point.Y - 200) / 300f, 0f, 1f) * 0.9f;
                num4 += num9;
                num4 += (1f - num8) * 0.75f;
                num4 = MathHelper.Clamp(num4, 0f, 2.5f);
                Filters.Scene["WaterDistortion"].GetShader().UseIntensity(num4);
            }
            if (flag2)
            {
                float num10 = (float)(point.Y - (Main.maxTilesY - 320)) / 120f;
                num10 = Math.Min(1f, num10) * 2f;
                Filters.Scene["HeatDistortion"].GetShader().UseIntensity(num10);
            }

            

            _shaderObstructionInternalValue = Utils.Clamp<float>(_shaderObstructionInternalValue + (float)self.behindBackWall.ToDirectionInt() * -0.005f, -0.1f, 1.1f);
            _stormShaderObstruction = Utils.Clamp<float>(_shaderObstructionInternalValue, 0f, 1f);

            _shaderObstructionInternalValueInfo.SetValue(self, _shaderObstructionInternalValue);
            _stormShaderObstructionInfo.SetValue(self, _stormShaderObstruction);

            if (Filters.Scene["Sandstorm"].IsActive())
            {
                Filters.Scene["Sandstorm"].GetShader().UseIntensity(_stormShaderObstruction * 0.4f * Math.Min(1f, Sandstorm.Severity));
                Filters.Scene["Sandstorm"].GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * _stormShaderObstruction);
                ((SimpleOverlay)Overlays.Scene["Sandstorm"]).GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * (1f - _stormShaderObstruction));
            }
            else if (self.ZoneDesert && !flag3 && !Main.raining && !flag2)
            {
                Vector3 vector = Main.tileColor.ToVector3();
                float num11 = (vector.X + vector.Y + vector.Z) / 3f;
                float num12 = _stormShaderObstruction * 4f * Math.Max(0f, 0.5f - Main.cloudAlpha) * num11;
                Filters.Scene["HeatDistortion"].GetShader().UseIntensity(num12);
                if (num12 <= 0f)
                {
                    Filters.Scene["HeatDistortion"].IsHidden = true;
                }
                else
                {
                    Filters.Scene["HeatDistortion"].IsHidden = false;
                }
            }
            

            if (flag)
            {
                


                ActiveSound activeSound = Main.GetActiveSound(_strongBlizzardSound);
                ActiveSound activeSound2 = Main.GetActiveSound(_insideBlizzardSound);
                if (activeSound == null)
                {
                    _strongBlizzardSound = Main.PlayTrackedSound(SoundID.BlizzardStrongLoop);
                }
                if (activeSound2 == null)
                {
                    _insideBlizzardSound = Main.PlayTrackedSound(SoundID.BlizzardInsideBuildingLoop);
                }
                activeSound = Main.GetActiveSound(_strongBlizzardSound);
                activeSound2 = Main.GetActiveSound(_insideBlizzardSound);
                float num13 = Math.Min(1f, Main.cloudAlpha * 2f) * _stormShaderObstruction;
                Filters.Scene["Blizzard"].GetShader().UseIntensity(_stormShaderObstruction * 0.4f * Math.Min(1f, Main.cloudAlpha * 2f) * 0.9f + 0.1f);
                Filters.Scene["Blizzard"].GetShader().UseOpacity(num13);
                ((SimpleOverlay)Overlays.Scene["Blizzard"]).GetShader().UseOpacity(1f - num13);
            }
            if (flag)
            {
                _blizzardSoundVolume = Math.Min(_blizzardSoundVolume + 0.01f, 1f);
            }
            else
            {
                _blizzardSoundVolume = Math.Max(_blizzardSoundVolume - 0.01f, 0f);
            }
            float num14 = Math.Min(1f, Main.cloudAlpha * 2f) * _stormShaderObstruction;
            ActiveSound activeSound3 = Main.GetActiveSound(_strongBlizzardSound);
            ActiveSound activeSound4 = Main.GetActiveSound(_insideBlizzardSound);
            if (_blizzardSoundVolume > 0f)
            {
                if (activeSound3 == null)
                {
                    _strongBlizzardSound = Main.PlayTrackedSound(SoundID.BlizzardStrongLoop);
                    activeSound3 = Main.GetActiveSound(_strongBlizzardSound);
                }
                activeSound3.Volume = num14 * _blizzardSoundVolume;
                if (activeSound4 == null)
                {
                    _insideBlizzardSound = Main.PlayTrackedSound(SoundID.BlizzardInsideBuildingLoop);
                    activeSound4 = Main.GetActiveSound(_insideBlizzardSound);
                }
                activeSound4.Volume = (1f - num14) * _blizzardSoundVolume;
            }
            else
            {
                if (activeSound3 != null)
                {
                    activeSound3.Volume = 0f;
                }
                else
                {
                    _strongBlizzardSound = SlotId.Invalid;
                }
                if (activeSound4 != null)
                {
                    activeSound4.Volume = 0f;
                }
                else
                {
                    _insideBlizzardSound = SlotId.Invalid;
                }
            }
            PlayerHooks.UpdateBiomeVisuals(self);
            if (!self.dead)
            {
                Point point2 = self.Center.ToTileCoordinates();
                if (WorldGen.InWorld(point2.X, point2.Y, 1))
                {
                    int num15 = 0;
                    if (Main.tile[point2.X, point2.Y] != null)
                    {
                        num15 = (int)Main.tile[point2.X, point2.Y].wall;
                    }
                    int num16 = num15;
                    if (num16 != 62)
                    {
                        if (num16 == 86)
                        {
                            AchievementsHelper.HandleSpecialEvent(self, 12);
                        }
                    }
                    else
                    {
                        AchievementsHelper.HandleSpecialEvent(self, 13);
                    }
                }
                if (self._funkytownCheckCD > 0)
                {
                    self._funkytownCheckCD--;
                }
                if (self.position.Y / 16f > (float)(Main.maxTilesY - 200))
                {
                    AchievementsHelper.HandleSpecialEvent(self, 14);
                    return;
                }
                if (self._funkytownCheckCD == 0 && (double)(self.position.Y / 16f) < Main.worldSurface && Main.shroomTiles >= 200)
                {
                    AchievementsHelper.HandleSpecialEvent(self, 15);
                    return;
                }
            }
            else
            {
                self._funkytownCheckCD = 100;
            }

            _weakBlizzardSoundInfo.SetValue(null, _weakBlizzardSound);
            _strongBlizzardSoundInfo.SetValue(null, _strongBlizzardSound);
            _insideBlizzardSoundInfo.SetValue(null, _insideBlizzardSound);
            //_blizzardSoundVolumeInfo.SetValue(null, _insideBlizzardSound);

            Mod mod = ModLoader.GetMod("TUA");

            

        }
    }
}
