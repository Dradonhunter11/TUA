using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.Dev;

namespace TUA.API.LiquidAPI.Swap
{
    class PlayerSwap
    {
        public static void MethodSwap()
        {
            Type Player = typeof(Player);
            Type PlayerSwap = typeof(PlayerExtension);
            ReflectionUtils.MethodSwap(Player, "Update", PlayerSwap, "Update");
        }
    }

    static class PlayerExtension
    {
        public static void Update(this Player self, int i)
        {
            FieldInfo _quickGrappleCooldownInfo =
                typeof(Player).GetField("_quickGrappleCooldown", BindingFlags.Instance | BindingFlags.NonPublic);
            int _quickGrappleCooldown = (int)_quickGrappleCooldownInfo.GetValue(self);


            if (i == Main.myPlayer && Main.netMode != 2)
            {
                LockOnHelper.Update();
            }
            if (self.launcherWait > 0)
            {
                self.launcherWait--;
            }
            self.maxFallSpeed = 10f;
            self.gravity = Player.defaultGravity;
            Player.jumpHeight = 15;
            Player.jumpSpeed = 5.01f;
            self.maxRunSpeed = 3f;
            self.runAcceleration = 0.08f;
            self.runSlowdown = 0.2f;
            self.accRunSpeed = self.maxRunSpeed;
            if (!self.mount.Active || !self.mount.Cart)
            {
                self.onWrongGround = false;
            }
            self.heldProj = -1;
            if (self.PortalPhysicsEnabled)
            {
                self.maxFallSpeed = 35f;
            }
            if (self.wet)
            {
                if (self.honeyWet)
                {
                    self.gravity = 0.1f;
                    self.maxFallSpeed = 3f;
                }
                else if (self.merman)
                {
                    self.gravity = 0.3f;
                    self.maxFallSpeed = 7f;
                }
                else
                {
                    self.gravity = 0.2f;
                    self.maxFallSpeed = 5f;
                    Player.jumpHeight = 30;
                    Player.jumpSpeed = 6.01f;
                }
            }
            if (self.vortexDebuff)
            {
                self.gravity = 0f;
            }
            self.maxFallSpeed += 0.01f;
            bool flag = false;
            if (Main.mapFullscreen)
            {
                self.GamepadEnableGrappleCooldown();
            }
            else if (_quickGrappleCooldown > 0)
            {
                _quickGrappleCooldown--;
            }
            if (Main.myPlayer == i)
            {
                TileObject.objectPreview.Reset();
                if (DD2Event.DownedInvasionAnyDifficulty)
                {
                    self.downedDD2EventAnyDifficulty = true;
                }
            }
            if (self.active)
            {
                if (self.ghostDmg > 0f)
                {
                    self.ghostDmg -= 2.5f;
                }
                if (self.ghostDmg < 0f)
                {
                    self.ghostDmg = 0f;
                }
                if (Main.expertMode)
                {
                    if (self.lifeSteal < 70f)
                    {
                        self.lifeSteal += 0.5f;
                    }
                    if (self.lifeSteal > 70f)
                    {
                        self.lifeSteal = 70f;
                    }
                }
                else
                {
                    if (self.lifeSteal < 80f)
                    {
                        self.lifeSteal += 0.6f;
                    }
                    if (self.lifeSteal > 80f)
                    {
                        self.lifeSteal = 80f;
                    }
                }
                if (self.mount.Active)
                {
                    self.position.Y = self.position.Y + (float)self.height;
                    self.height = 42 + self.mount.HeightBoost;
                    self.position.Y = self.position.Y - (float)self.height;
                    if (self.mount.Type == 0)
                    {
                        int num = (int)(self.position.X + (float)(self.width / 2)) / 16;
                        int j = (int)(self.position.Y + (float)(self.height / 2) - 14f) / 16;
                        Lighting.AddLight(num, j, 0.5f, 0.2f, 0.05f);
                        Lighting.AddLight(num + self.direction, j, 0.5f, 0.2f, 0.05f);
                        Lighting.AddLight(num + self.direction * 2, j, 0.5f, 0.2f, 0.05f);
                    }
                }
                else
                {
                    self.position.Y = self.position.Y + (float)self.height;
                    self.height = 42;
                    self.position.Y = self.position.Y - (float)self.height;
                }
                Main.ActivePlayersCount++;
                self.outOfRange = false;
                if (self.whoAmI != Main.myPlayer)
                {
                    int num2 = (int)(self.position.X + (float)(self.width / 2)) / 16;
                    int num3 = (int)(self.position.Y + (float)(self.height / 2)) / 16;
                    if (!WorldGen.InWorld(num2, num3, 4))
                    {
                        flag = true;
                    }
                    else if (Main.tile[num2, num3] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num2 - 3, num3] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num2 + 3, num3] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num2, num3 - 3] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num2, num3 + 3] == null)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        self.outOfRange = true;
                        self.numMinions = 0;
                        self.slotsMinions = 0f;
                        self.itemAnimation = 0;
                        self.PlayerFrame();
                    }
                }
                if (self.tankPet >= 0)
                {
                    if (!self.tankPetReset)
                    {
                        self.tankPetReset = true;
                    }
                    else
                    {
                        self.tankPet = -1;
                    }
                }
            }
            if (self.chatOverhead.timeLeft > 0)
            {
                self.chatOverhead.timeLeft = self.chatOverhead.timeLeft - 1;
            }
            if (!self.active || flag)
            {
                return;
            }
            PlayerHooks.PreUpdate(self);
            self.miscCounter++;
            if (self.miscCounter >= 300)
            {
                self.miscCounter = 0;
            }
            self.infernoCounter++;
            if (self.infernoCounter >= 180)
            {
                self.infernoCounter = 0;
            }
            float num4 = (float)(Main.maxTilesX / 4200);
            num4 *= num4;
            float num5 = (float)((double)(self.position.Y / 16f - (60f + 10f * num4)) / (Main.worldSurface / 6.0));
            if ((double)num5 < 0.25)
            {
                num5 = 0.25f;
            }
            if (num5 > 1f)
            {
                num5 = 1f;
            }
            self.gravity *= num5;
            self.maxRegenDelay = (1f - (float)self.statMana / (float)self.statManaMax2) * 60f * 4f + 45f;
            self.maxRegenDelay *= 0.7f;
            self.UpdateSocialShadow();
            self.UpdateTeleportVisuals();
            self.whoAmI = i;
            if (self.whoAmI == Main.myPlayer)
            {
                if (!DD2Event.Ongoing)
                {
                    MethodInfo PurgeDD2EnergyCrystals = typeof(Player).GetMethod("PurgeDD2EnergyCrystals",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    PurgeDD2EnergyCrystals.Invoke(self, null);
                }
                self.TryPortalJumping();
            }
            if (self.runSoundDelay > 0)
            {
                self.runSoundDelay--;
            }
            if (self.attackCD > 0)
            {
                self.attackCD--;
            }
            if (self.itemAnimation == 0)
            {
                self.attackCD = 0;
            }
            if (self.potionDelay > 0)
            {
                self.potionDelay--;
            }
            if (i == Main.myPlayer)
            {
                if (self.trashItem.type >= 1522 && self.trashItem.type <= 1527)
                {
                    self.trashItem.SetDefaults(0, false);
                }
                if (self.trashItem.type == 3643)
                {
                    self.trashItem.SetDefaults(0, false);
                }
                self.UpdateBiomes();
                self.UpdateMinionTarget();
            }
            if (self.ghost)
            {
                self.Ghost();
                return;
            }
            if (self.dead)
            {
                self.UpdateDead();
                return;
            }
            if (i == Main.myPlayer)
            {
                self.controlUp = false;
                self.controlLeft = false;
                self.controlDown = false;
                self.controlRight = false;
                self.controlJump = false;
                self.controlUseItem = false;
                self.controlUseTile = false;
                self.controlThrow = false;
                self.controlInv = false;
                self.controlHook = false;
                self.controlTorch = false;
                self.controlSmart = false;
                self.controlMount = false;
                self.controlQuickHeal = false;
                self.controlQuickMana = false;
                self.mapStyle = false;
                self.mapAlphaDown = false;
                self.mapAlphaUp = false;
                self.mapFullScreen = false;
                self.mapZoomIn = false;
                self.mapZoomOut = false;
                if (Main.hasFocus)
                {
                    if (!Main.drawingPlayerChat && !Main.editSign && !Main.editChest && !Main.blockInput)
                    {
                        PlayerInput.Triggers.Current.CopyInto(self);
                        if (Main.mapFullscreen)
                        {
                            if (self.controlUp)
                            {
                                Main.mapFullscreenPos.Y = Main.mapFullscreenPos.Y - 1f * (16f / Main.mapFullscreenScale);
                            }
                            if (self.controlDown)
                            {
                                Main.mapFullscreenPos.Y = Main.mapFullscreenPos.Y + 1f * (16f / Main.mapFullscreenScale);
                            }
                            if (self.controlLeft)
                            {
                                Main.mapFullscreenPos.X = Main.mapFullscreenPos.X - 1f * (16f / Main.mapFullscreenScale);
                            }
                            if (self.controlRight)
                            {
                                Main.mapFullscreenPos.X = Main.mapFullscreenPos.X + 1f * (16f / Main.mapFullscreenScale);
                            }
                            self.controlUp = false;
                            self.controlLeft = false;
                            self.controlDown = false;
                            self.controlRight = false;
                            self.controlJump = false;
                            self.controlUseItem = false;
                            self.controlUseTile = false;
                            self.controlThrow = false;
                            self.controlHook = false;
                            self.controlTorch = false;
                            self.controlSmart = false;
                            self.controlMount = false;
                        }
                        if (self.controlQuickHeal)
                        {
                            if (self.releaseQuickHeal)
                            {
                                self.QuickHeal();
                            }
                            self.releaseQuickHeal = false;
                        }
                        else
                        {
                            self.releaseQuickHeal = true;
                        }
                        if (self.controlQuickMana)
                        {
                            if (self.releaseQuickMana)
                            {
                                self.QuickMana();
                            }
                            self.releaseQuickMana = false;
                        }
                        else
                        {
                            self.releaseQuickMana = true;
                        }
                        if (self.controlLeft && self.controlRight)
                        {
                            self.controlLeft = false;
                            self.controlRight = false;
                        }
                        if (Main.cSmartCursorToggle)
                        {
                            if (self.controlSmart && self.releaseSmart)
                            {
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                Main.SmartCursorEnabled = !Main.SmartCursorEnabled;
                            }
                        }
                        else
                        {
                            if (Main.SmartCursorEnabled != self.controlSmart)
                            {
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            }
                            Main.SmartCursorEnabled = self.controlSmart;
                        }
                        if (self.controlSmart)
                        {
                            self.releaseSmart = false;
                        }
                        else
                        {
                            self.releaseSmart = true;
                        }
                        if (self.controlMount)
                        {
                            if (self.releaseMount)
                            {
                                self.QuickMount();
                            }
                            self.releaseMount = false;
                        }
                        else
                        {
                            self.releaseMount = true;
                        }
                        if (Main.mapFullscreen)
                        {
                            if (self.mapZoomIn)
                            {
                                Main.mapFullscreenScale *= 1.05f;
                            }
                            if (self.mapZoomOut)
                            {
                                Main.mapFullscreenScale *= 0.95f;
                            }
                        }
                        else
                        {
                            if (Main.mapStyle == 1)
                            {
                                if (self.mapZoomIn)
                                {
                                    Main.mapMinimapScale *= 1.025f;
                                }
                                if (self.mapZoomOut)
                                {
                                    Main.mapMinimapScale *= 0.975f;
                                }
                                if (self.mapAlphaUp)
                                {
                                    Main.mapMinimapAlpha += 0.015f;
                                }
                                if (self.mapAlphaDown)
                                {
                                    Main.mapMinimapAlpha -= 0.015f;
                                }
                            }
                            else if (Main.mapStyle == 2)
                            {
                                if (self.mapZoomIn)
                                {
                                    Main.mapOverlayScale *= 1.05f;
                                }
                                if (self.mapZoomOut)
                                {
                                    Main.mapOverlayScale *= 0.95f;
                                }
                                if (self.mapAlphaUp)
                                {
                                    Main.mapOverlayAlpha += 0.015f;
                                }
                                if (self.mapAlphaDown)
                                {
                                    Main.mapOverlayAlpha -= 0.015f;
                                }
                            }
                            if (self.mapStyle)
                            {
                                if (self.releaseMapStyle)
                                {
                                    Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                    Main.mapStyle++;
                                    if (Main.mapStyle > 2)
                                    {
                                        Main.mapStyle = 0;
                                    }
                                }
                                self.releaseMapStyle = false;
                            }
                            else
                            {
                                self.releaseMapStyle = true;
                            }
                        }
                        if (self.mapFullScreen)
                        {
                            if (self.releaseMapFullscreen)
                            {
                                if (Main.mapFullscreen)
                                {
                                    Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                                    Main.mapFullscreen = false;
                                }
                                else if (Main.mapEnabled)
                                {
                                    Main.playerInventory = false;
                                    self.talkNPC = -1;
                                    Main.npcChatCornerItem = 0;
                                    Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                                    float mapFullscreenScale = 2.5f;
                                    Main.mapFullscreenScale = mapFullscreenScale;
                                    Main.mapFullscreen = true;
                                    Main.resetMapFull = true;
                                    Main.buffString = string.Empty;
                                }
                            }
                            self.releaseMapFullscreen = false;
                        }
                        else
                        {
                            self.releaseMapFullscreen = true;
                        }
                    }
                    else if (!PlayerInput.UsingGamepad && !Main.editSign && !Main.editChest && !Main.blockInput)
                    {
                        PlayerInput.Triggers.Current.CopyIntoDuringChat(self);
                    }
                    if (self.confused)
                    {
                        bool flag2 = self.controlLeft;
                        bool flag3 = self.controlUp;
                        self.controlLeft = self.controlRight;
                        self.controlRight = flag2;
                        self.controlUp = self.controlRight;
                        self.controlDown = flag3;
                    }
                    else if (self.cartFlip)
                    {
                        if (self.controlRight || self.controlLeft)
                        {
                            bool flag4 = self.controlLeft;
                            self.controlLeft = self.controlRight;
                            self.controlRight = flag4;
                        }
                        else
                        {
                            self.cartFlip = false;
                        }
                    }
                    for (int k = 0; k < self.doubleTapCardinalTimer.Length; k++)
                    {
                        self.doubleTapCardinalTimer[k]--;
                        if (self.doubleTapCardinalTimer[k] < 0)
                        {
                            self.doubleTapCardinalTimer[k] = 0;
                        }
                    }
                    for (int l = 0; l < 4; l++)
                    {
                        bool flag5 = false;
                        bool flag6 = false;
                        switch (l)
                        {
                            case 0:
                                flag5 = (self.controlDown && self.releaseDown);
                                flag6 = self.controlDown;
                                break;
                            case 1:
                                flag5 = (self.controlUp && self.releaseUp);
                                flag6 = self.controlUp;
                                break;
                            case 2:
                                flag5 = (self.controlRight && self.releaseRight);
                                flag6 = self.controlRight;
                                break;
                            case 3:
                                flag5 = (self.controlLeft && self.releaseLeft);
                                flag6 = self.controlLeft;
                                break;
                        }
                        if (flag5)
                        {
                            if (self.doubleTapCardinalTimer[l] > 0)
                            {
                                self.KeyDoubleTap(l);
                            }
                            else
                            {
                                self.doubleTapCardinalTimer[l] = 15;
                            }
                        }
                        if (flag6)
                        {
                            self.holdDownCardinalTimer[l]++;
                            self.KeyHoldDown(l, self.holdDownCardinalTimer[l]);
                        }
                        else
                        {
                            self.holdDownCardinalTimer[l] = 0;
                        }
                    }
                    PlayerHooks.SetControls(self);
                    if (self.controlInv)
                    {
                        if (self.releaseInventory)
                        {
                            self.ToggleInv();
                        }
                        self.releaseInventory = false;
                    }
                    else
                    {
                        self.releaseInventory = true;
                    }
                    if (self.delayUseItem)
                    {
                        if (!self.controlUseItem)
                        {
                            self.delayUseItem = false;
                        }
                        self.controlUseItem = false;
                    }
                    if (self.itemAnimation == 0 && self.itemTime == 0 && self.reuseDelay == 0)
                    {
                        self.dropItemCheck();
                        int num6 = self.selectedItem;
                        bool flag7 = false;
                        if (!Main.drawingPlayerChat && self.selectedItem != 58 && !Main.editSign && !Main.editChest)
                        {
                            if (PlayerInput.Triggers.Current.Hotbar1)
                            {
                                self.selectedItem = 0;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar2)
                            {
                                self.selectedItem = 1;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar3)
                            {
                                self.selectedItem = 2;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar4)
                            {
                                self.selectedItem = 3;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar5)
                            {
                                self.selectedItem = 4;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar6)
                            {
                                self.selectedItem = 5;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar7)
                            {
                                self.selectedItem = 6;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar8)
                            {
                                self.selectedItem = 7;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar9)
                            {
                                self.selectedItem = 8;
                                flag7 = true;
                            }
                            if (PlayerInput.Triggers.Current.Hotbar10)
                            {
                                self.selectedItem = 9;
                                flag7 = true;
                            }
                            int selectedBinding = self.DpadRadial.SelectedBinding;
                            int selectedBinding2 = self.CircularRadial.SelectedBinding;
                            int arg_F92_0 = self.QuicksRadial.SelectedBinding;
                            self.DpadRadial.Update();
                            self.CircularRadial.Update();
                            self.QuicksRadial.Update();
                            if (self.CircularRadial.SelectedBinding >= 0 && selectedBinding2 != self.CircularRadial.SelectedBinding)
                            {
                                self.DpadRadial.ChangeSelection(-1);
                            }
                            if (self.DpadRadial.SelectedBinding >= 0 && selectedBinding != self.DpadRadial.SelectedBinding)
                            {
                                self.CircularRadial.ChangeSelection(-1);
                            }
                            if (self.QuicksRadial.SelectedBinding != -1 && PlayerInput.Triggers.JustReleased.RadialQuickbar && !PlayerInput.MiscSettingsTEMP.HotbarRadialShouldBeUsed)
                            {
                                switch (self.QuicksRadial.SelectedBinding)
                                {
                                    case 0:
                                        self.QuickHeal();
                                        break;
                                    case 1:
                                        self.QuickBuff();
                                        break;
                                    case 2:
                                        self.QuickMana();
                                        break;
                                }
                            }
                            if (self.controlTorch || flag7)
                            {
                                self.DpadRadial.ChangeSelection(-1);
                                self.CircularRadial.ChangeSelection(-1);
                            }
                            if (self.controlTorch && flag7)
                            {
                                if (self.selectedItem != self.nonTorch)
                                {
                                    Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                }
                                self.nonTorch = self.selectedItem;
                                self.selectedItem = num6;
                                flag7 = false;
                            }
                        }
                        bool flag8 = Main.hairWindow;
                        if (flag8)
                        {
                            int y = Main.screenHeight / 2 + 60;
                            int x = Main.screenWidth / 2 - Main.hairStyleBackTexture.Width / 2;
                            flag8 = new Rectangle(x, y, Main.hairStyleBackTexture.Width, Main.hairStyleBackTexture.Height).Contains(Main.MouseScreen.ToPoint());
                        }
                        if (flag7 && CaptureManager.Instance.Active)
                        {
                            CaptureManager.Instance.Active = false;
                        }
                        if (num6 != self.selectedItem)
                        {
                            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                        }
                        if (Main.mapFullscreen)
                        {
                            float num7 = (float)(PlayerInput.ScrollWheelDelta / 120);
                            if (PlayerInput.UsingGamepad)
                            {
                                num7 += (float)(PlayerInput.Triggers.Current.HotbarPlus.ToInt() - PlayerInput.Triggers.Current.HotbarMinus.ToInt()) * 0.1f;
                            }
                            Main.mapFullscreenScale *= 1f + num7 * 0.3f;
                        }
                        else if (CaptureManager.Instance.Active)
                        {
                            CaptureManager.Instance.Scrolling();
                        }
                        else if (!flag8)
                        {
                            if (!Main.playerInventory)
                            {
                                MethodInfo HandleHotbar = typeof(Player).GetMethod("HandleHotbar",
                                    BindingFlags.NonPublic | BindingFlags.Instance);
                                HandleHotbar.Invoke(self, null);
                            }
                            else
                            {
                                int num8 = PlayerInput.ScrollWheelDelta / 120;
                                bool flag9 = true;
                                if (Main.recBigList)
                                {
                                    int num9 = 42;
                                    int num10 = 340;
                                    int num11 = 310;
                                    PlayerInput.SetZoom_UI();
                                    int num12 = (Main.screenWidth - num11 - 280) / num9;
                                    int num13 = (Main.screenHeight - num10 - 20) / num9;
                                    if (new Rectangle(num11, num10, num12 * num9, num13 * num9).Contains(Main.MouseScreen.ToPoint()))
                                    {
                                        num8 *= -1;
                                        int num14 = Math.Sign(num8);
                                        while (num8 != 0)
                                        {
                                            if (num8 < 0)
                                            {
                                                Main.recStart -= num12;
                                                if (Main.recStart < 0)
                                                {
                                                    Main.recStart = 0;
                                                }
                                            }
                                            else
                                            {
                                                Main.recStart += num12;
                                                if (Main.recStart > Main.numAvailableRecipes - num12)
                                                {
                                                    Main.recStart = Main.numAvailableRecipes - num12;
                                                }
                                            }
                                            num8 -= num14;
                                        }
                                    }
                                    PlayerInput.SetZoom_World();
                                }
                                if (flag9)
                                {
                                    Main.focusRecipe += num8;
                                    if (Main.focusRecipe > Main.numAvailableRecipes - 1)
                                    {
                                        Main.focusRecipe = Main.numAvailableRecipes - 1;
                                    }
                                    if (Main.focusRecipe < 0)
                                    {
                                        Main.focusRecipe = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool flag10 = false;
                        if (!Main.drawingPlayerChat && self.selectedItem != 58 && !Main.editSign && !Main.editChest)
                        {
                            int num15 = -1;
                            if (Main.keyState.IsKeyDown(Keys.D1))
                            {
                                num15 = 0;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D2))
                            {
                                num15 = 1;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D3))
                            {
                                num15 = 2;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D4))
                            {
                                num15 = 3;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D5))
                            {
                                num15 = 4;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D6))
                            {
                                num15 = 5;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D7))
                            {
                                num15 = 6;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D8))
                            {
                                num15 = 7;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D9))
                            {
                                num15 = 8;
                                flag10 = true;
                            }
                            if (Main.keyState.IsKeyDown(Keys.D0))
                            {
                                num15 = 9;
                                flag10 = true;
                            }
                            if (flag10)
                            {
                                if (num15 != self.nonTorch)
                                {
                                    Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                                }
                                self.nonTorch = num15;
                            }
                        }
                    }
                }
                if (self.selectedItem == 58)
                {
                    self.nonTorch = -1;
                }
                else
                {
                    self.SmartSelectLookup();
                }
                if (self.stoned != self.lastStoned)
                {
                    if (self.whoAmI == Main.myPlayer && self.stoned)
                    {
                        int damage = (int)(20.0 * (double)Main.damageMultiplier);
                        self.Hurt(PlayerDeathReason.ByOther(5), damage, 0, false, false, false, -1);
                    }
                    Main.PlaySound(0, (int)self.position.X, (int)self.position.Y, 1, 1f, 0f);
                    for (int m = 0; m < 20; m++)
                    {
                        int num16 = Dust.NewDust(self.position, self.width, self.height, 1, 0f, 0f, 0, default(Color), 1f);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[num16].noGravity = true;
                        }
                    }
                }
                self.lastStoned = self.stoned;
                if (self.frozen || self.webbed || self.stoned)
                {
                    self.controlJump = false;
                    self.controlDown = false;
                    self.controlLeft = false;
                    self.controlRight = false;
                    self.controlUp = false;
                    self.controlUseItem = false;
                    self.controlUseTile = false;
                    self.controlThrow = false;
                    self.gravDir = 1f;
                }
                if (!self.controlThrow)
                {
                    self.releaseThrow = true;
                }
                else
                {
                    self.releaseThrow = false;
                }
                if (Main.netMode == 1)
                {
                    bool flag11 = false;
                    if (self.controlUp != Main.clientPlayer.controlUp)
                    {
                        flag11 = true;
                    }
                    if (self.controlDown != Main.clientPlayer.controlDown)
                    {
                        flag11 = true;
                    }
                    if (self.controlLeft != Main.clientPlayer.controlLeft)
                    {
                        flag11 = true;
                    }
                    if (self.controlRight != Main.clientPlayer.controlRight)
                    {
                        flag11 = true;
                    }
                    if (self.controlJump != Main.clientPlayer.controlJump)
                    {
                        flag11 = true;
                    }
                    if (self.controlUseItem != Main.clientPlayer.controlUseItem)
                    {
                        flag11 = true;
                    }
                    if (self.selectedItem != Main.clientPlayer.selectedItem)
                    {
                        flag11 = true;
                    }
                    if (flag11)
                    {
                        NetMessage.SendData(13, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (Main.playerInventory)
                {
                    self.AdjTiles();
                }
                if (self.chest != -1)
                {
                    if (self.chest != -2)
                    {
                        self.flyingPigChest = -1;
                    }
                    if (self.flyingPigChest >= 0)
                    {
                        if (!Main.projectile[self.flyingPigChest].active || Main.projectile[self.flyingPigChest].type != 525)
                        {
                            Main.PlaySound(SoundID.Item59, -1, -1);
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                        else
                        {
                            int num17 = (int)(((double)self.position.X + (double)self.width * 0.5) / 16.0);
                            int num18 = (int)(((double)self.position.Y + (double)self.height * 0.5) / 16.0);
                            self.chestX = (int)Main.projectile[self.flyingPigChest].Center.X / 16;
                            self.chestY = (int)Main.projectile[self.flyingPigChest].Center.Y / 16;
                            if (num17 < self.chestX - Player.tileRangeX || num17 > self.chestX + Player.tileRangeX + 1 || num18 < self.chestY - Player.tileRangeY || num18 > self.chestY + Player.tileRangeY + 1)
                            {
                                if (self.chest != -1)
                                {
                                    Main.PlaySound(SoundID.Item59, -1, -1);
                                }
                                self.chest = -1;
                                Recipe.FindRecipes();
                            }
                        }
                    }
                    else
                    {
                        int num19 = (int)(((double)self.position.X + (double)self.width * 0.5) / 16.0);
                        int num20 = (int)(((double)self.position.Y + (double)self.height * 0.5) / 16.0);
                        if (Main.tile[self.chestX, self.chestY].type == 463 && num20 < self.chestY)
                        {
                            num20 += 2;
                        }
                        if (num19 < self.chestX - Player.tileRangeX || num19 > self.chestX + Player.tileRangeX + 1 || num20 < self.chestY - Player.tileRangeY || num20 > self.chestY + Player.tileRangeY + 1)
                        {
                            if (self.chest != -1)
                            {
                                Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                            }
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                        else if (!Main.tile[self.chestX, self.chestY].active())
                        {
                            Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                            self.chest = -1;
                            Recipe.FindRecipes();
                        }
                    }
                }
                else
                {
                    self.flyingPigChest = -1;
                }
                if (self.velocity.Y <= 0f)
                {
                    self.fallStart2 = (int)(self.position.Y / 16f);
                }
                if (self.velocity.Y == 0f)
                {
                    int num21 = 25;
                    num21 += self.extraFall;
                    int num22 = (int)(self.position.Y / 16f) - self.fallStart;
                    if (self.mount.CanFly)
                    {
                        num22 = 0;
                    }
                    if (self.mount.Cart && Minecart.OnTrack(self.position, self.width, self.height))
                    {
                        num22 = 0;
                    }
                    if (self.mount.Type == 1)
                    {
                        num22 = 0;
                    }
                    self.mount.FatigueRecovery();
                    if (self.stoned)
                    {
                        int num23 = (int)(((float)num22 * self.gravDir - 2f) * 20f);
                        if (num23 > 0)
                        {
                            self.Hurt(PlayerDeathReason.ByOther(5), num23, 0, false, false, false, -1);
                            self.immune = false;
                        }
                    }
                    else if (((self.gravDir == 1f && num22 > num21) || (self.gravDir == -1f && num22 < -num21)) && !self.noFallDmg && self.wingsLogic == 0)
                    {
                        self.immune = false;
                        int num24 = (int)((float)num22 * self.gravDir - (float)num21) * 10;
                        if (self.mount.Active)
                        {
                            num24 = (int)((float)num24 * self.mount.FallDamage);
                        }
                        self.Hurt(PlayerDeathReason.ByOther(0), num24, 0, false, false, false, -1);
                        if (!self.dead && self.statLife <= self.statLifeMax2 / 10)
                        {
                            AchievementsHelper.HandleSpecialEvent(self, 8);
                        }
                    }
                    self.fallStart = (int)(self.position.Y / 16f);
                }
                if (self.jump > 0 || self.rocketDelay > 0 || self.wet || self.slowFall || (double)num5 < 0.8 || self.tongued)
                {
                    self.fallStart = (int)(self.position.Y / 16f);
                }
            }
            if (Main.netMode != 1)
            {
                if (self.chest == -1 && self.lastChest >= 0 && Main.chest[self.lastChest] != null && Main.chest[self.lastChest] != null)
                {
                    int x2 = Main.chest[self.lastChest].x;
                    int y2 = Main.chest[self.lastChest].y;
                    NPC.BigMimicSummonCheck(x2, y2);
                }
                self.lastChest = self.chest;
            }
            if (self.mouseInterface)
            {
                self.delayUseItem = true;
            }
            Player.tileTargetX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
            Player.tileTargetY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
            if (self.gravDir == -1f)
            {
                Player.tileTargetY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16f);
            }
            if (Player.tileTargetX >= Main.maxTilesX - 5)
            {
                Player.tileTargetX = Main.maxTilesX - 5;
            }
            if (Player.tileTargetY >= Main.maxTilesY - 5)
            {
                Player.tileTargetY = Main.maxTilesY - 5;
            }
            if (Player.tileTargetX < 5)
            {
                Player.tileTargetX = 5;
            }
            if (Player.tileTargetY < 5)
            {
                Player.tileTargetY = 5;
            }
            if (Main.tile[Player.tileTargetX - 1, Player.tileTargetY] == null)
            {
                Main.tile[Player.tileTargetX - 1, Player.tileTargetY] = new Tile();
            }
            if (Main.tile[Player.tileTargetX + 1, Player.tileTargetY] == null)
            {
                Main.tile[Player.tileTargetX + 1, Player.tileTargetY] = new Tile();
            }
            if (Main.tile[Player.tileTargetX, Player.tileTargetY] == null)
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY] = new Tile();
            }
            if (!Main.tile[Player.tileTargetX, Player.tileTargetY].active())
            {
                if (Main.tile[Player.tileTargetX - 1, Player.tileTargetY].active() && Main.tile[Player.tileTargetX - 1, Player.tileTargetY].type == 323)
                {
                    int frameY = (int)Main.tile[Player.tileTargetX - 1, Player.tileTargetY].frameY;
                    if (frameY < -4)
                    {
                        Player.tileTargetX++;
                    }
                    if (frameY > 4)
                    {
                        Player.tileTargetX--;
                    }
                }
                else if (Main.tile[Player.tileTargetX + 1, Player.tileTargetY].active() && Main.tile[Player.tileTargetX + 1, Player.tileTargetY].type == 323)
                {
                    int frameY2 = (int)Main.tile[Player.tileTargetX + 1, Player.tileTargetY].frameY;
                    if (frameY2 < -4)
                    {
                        Player.tileTargetX++;
                    }
                    if (frameY2 > 4)
                    {
                        Player.tileTargetX--;
                    }
                }
            }
            try
            {
                self.SmartCursorLookup();
                self.SmartInteractLookup();
            }
            catch
            {
                Main.SmartCursorEnabled = false;
            }
            self.UpdateImmunity();
            if (self.petalTimer > 0)
            {
                self.petalTimer--;
            }
            if (self.shadowDodgeTimer > 0)
            {
                self.shadowDodgeTimer--;
            }
            if (self.jump > 0 || self.velocity.Y != 0f)
            {
                self.slippy = false;
                self.slippy2 = false;
                self.powerrun = false;
                self.sticky = false;
            }
            self.potionDelayTime = Item.potionDelay;
            self.restorationDelayTime = Item.restorationDelay;
            if (self.pStone)
            {
                self.potionDelayTime = (int)((double)self.potionDelayTime * 0.75);
                self.restorationDelayTime = (int)((double)self.restorationDelayTime * 0.75);
            }
            if (self.yoraiz0rEye > 0)
            {
                self.Yoraiz0rEye();
            }
            self.ResetEffects();
            self.UpdateDyes(i);
            self.meleeCrit += self.inventory[self.selectedItem].crit;
            self.magicCrit += self.inventory[self.selectedItem].crit;
            self.rangedCrit += self.inventory[self.selectedItem].crit;
            self.thrownCrit += self.inventory[self.selectedItem].crit;
            if (self.whoAmI == Main.myPlayer)
            {
                Main.musicBox2 = -1;
                if (Main.waterCandles > 0)
                {
                    self.AddBuff(86, 2, false);
                }
                if (Main.peaceCandles > 0)
                {
                    self.AddBuff(157, 2, false);
                }
                if (Main.campfire)
                {
                    self.AddBuff(87, 2, false);
                }
                if (Main.starInBottle)
                {
                    self.AddBuff(158, 2, false);
                }
                if (Main.heartLantern)
                {
                    self.AddBuff(89, 2, false);
                }
                if (Main.sunflower)
                {
                    self.AddBuff(146, 2, false);
                }
                if (self.hasBanner)
                {
                    self.AddBuff(147, 2, false);
                }
                if (!self.behindBackWall && self.ZoneSandstorm)
                {
                    self.AddBuff(194, 2, false);
                }
            }
            PlayerHooks.PreUpdateBuffs(self);
            for (int num25 = 0; num25 < BuffLoader.BuffCount; num25++)
            {
                self.buffImmune[num25] = false;
            }
            self.UpdateBuffs(i);
            PlayerHooks.PostUpdateBuffs(self);
            if (self.whoAmI == Main.myPlayer)
            {
                if (!self.onFire && !self.poisoned)
                {
                    self.trapDebuffSource = false;
                }
                self.UpdatePet(i);
                self.UpdatePetLight(i);
            }
            bool flag13 = self.wet && !self.lavaWet && (!self.mount.Active || self.mount.Type != 3);
            if (self.accMerman && flag13)
            {
                self.releaseJump = true;
                self.wings = 0;
                self.merman = true;
                self.accFlipper = true;
                self.AddBuff(34, 2, true);
            }
            else
            {
                self.merman = false;
            }
            if (!flag13 && self.forceWerewolf)
            {
                self.forceMerman = false;
            }
            if (self.forceMerman && flag13)
            {
                self.wings = 0;
            }
            self.accMerman = false;
            self.hideMerman = false;
            self.forceMerman = false;
            if (self.wolfAcc && !self.merman && !Main.dayTime && !self.wereWolf)
            {
                self.AddBuff(28, 60, true);
            }
            self.wolfAcc = false;
            self.hideWolf = false;
            self.forceWerewolf = false;
            if (self.whoAmI == Main.myPlayer)
            {
                for (int num26 = 0; num26 < 22; num26++)
                {
                    if (self.buffType[num26] > 0 && self.buffTime[num26] <= 0)
                    {
                        self.DelBuff(num26);
                    }
                }
            }
            self.beetleDefense = false;
            self.beetleOffense = false;
            self.doubleJumpCloud = false;
            self.setSolar = false;
            self.head = self.armor[0].headSlot;
            self.body = self.armor[1].bodySlot;
            self.legs = self.armor[2].legSlot;
            self.handon = -1;
            self.handoff = -1;
            self.back = -1;
            self.front = -1;
            self.shoe = -1;
            self.waist = -1;
            self.shield = -1;
            self.neck = -1;
            self.face = -1;
            self.balloon = -1;
            if (self.MountFishronSpecialCounter > 0f)
            {
                self.MountFishronSpecialCounter -= 1f;
            }
            if (self._portalPhysicsTime > 0)
            {
                self._portalPhysicsTime--;
            }
            self.UpdateEquips(i);
            if (self.velocity.Y == 0f || self.controlJump)
            {
                self.portalPhysicsFlag = false;
            }
            if (self.inventory[self.selectedItem].type == 3384 || self.portalPhysicsFlag)
            {
                self._portalPhysicsTime = 30;
            }
            if (self.mount.Active)
            {
                self.mount.UpdateEffects(self);
            }
            self.gemCount++;
            if (self.gemCount >= 10)
            {
                self.gem = -1;
                self.ownedLargeGems = 0;
                self.gemCount = 0;
                for (int num27 = 0; num27 <= 58; num27++)
                {
                    if (self.inventory[num27].type == 0 || self.inventory[num27].stack == 0)
                    {
                        self.inventory[num27].TurnToAir();
                    }
                    if (self.inventory[num27].type >= 1522 && self.inventory[num27].type <= 1527)
                    {
                        self.gem = self.inventory[num27].type - 1522;
                        self.ownedLargeGems[self.gem] = true;
                    }
                    if (self.inventory[num27].type == 3643)
                    {
                        self.gem = 6;
                        self.ownedLargeGems[self.gem] = true;
                    }
                }
            }

            MethodInfo UpdateArmorLights =
                typeof(Player).GetMethod("UpdateArmorLights", BindingFlags.Instance | BindingFlags.NonPublic);
            UpdateArmorLights.Invoke(self, null);
            self.UpdateArmorSets(i);
            PlayerHooks.PostUpdateEquips(self); // TODO, move down?
            if (self.maxTurretsOld != self.maxTurrets)
            {
                self.UpdateMaxTurrets();
                self.maxTurretsOld = self.maxTurrets;
            }
            if (self.shieldRaised)
            {
                self.statDefense += 20;
            }
            if ((self.merman || self.forceMerman) && flag13)
            {
                self.wings = 0;
            }
            if (self.invis)
            {
                if (self.itemAnimation == 0 && self.aggro > -750)
                {
                    self.aggro = -750;
                }
                else if (self.aggro > -250)
                {
                    self.aggro = -250;
                }
            }
            if (self.inventory[self.selectedItem].type == 3106)
            {
                if (self.itemAnimation > 0)
                {
                    self.stealthTimer = 15;
                    if (self.stealth > 0f)
                    {
                        self.stealth += 0.1f;
                    }
                }
                else if ((double)self.velocity.X > -0.1 && (double)self.velocity.X < 0.1 && (double)self.velocity.Y > -0.1 && (double)self.velocity.Y < 0.1 && !self.mount.Active)
                {
                    if (self.stealthTimer == 0 && self.stealth > 0f)
                    {
                        self.stealth -= 0.02f;
                        if ((double)self.stealth <= 0.0)
                        {
                            self.stealth = 0f;
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(84, -1, -1, null, self.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
                else
                {
                    if (self.stealth > 0f)
                    {
                        self.stealth += 0.1f;
                    }
                    if (self.mount.Active)
                    {
                        self.stealth = 1f;
                    }
                }
                if (self.stealth > 1f)
                {
                    self.stealth = 1f;
                }
                self.meleeDamage += (1f - self.stealth) * 3f;
                self.meleeCrit += (int)((1f - self.stealth) * 30f);
                if (self.meleeCrit > 100)
                {
                    self.meleeCrit = 100;
                }
                self.aggro -= (int)((1f - self.stealth) * 750f);
                if (self.stealthTimer > 0)
                {
                    self.stealthTimer--;
                }
            }
            else if (self.shroomiteStealth)
            {
                if (self.itemAnimation > 0)
                {
                    self.stealthTimer = 5;
                }
                if ((double)self.velocity.X > -0.1 && (double)self.velocity.X < 0.1 && (double)self.velocity.Y > -0.1 && (double)self.velocity.Y < 0.1 && !self.mount.Active)
                {
                    if (self.stealthTimer == 0 && self.stealth > 0f)
                    {
                        self.stealth -= 0.015f;
                        if ((double)self.stealth <= 0.0)
                        {
                            self.stealth = 0f;
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(84, -1, -1, null, self.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
                else
                {
                    float num28 = Math.Abs(self.velocity.X) + Math.Abs(self.velocity.Y);
                    self.stealth += num28 * 0.0075f;
                    if (self.stealth > 1f)
                    {
                        self.stealth = 1f;
                    }
                    if (self.mount.Active)
                    {
                        self.stealth = 1f;
                    }
                }
                self.rangedDamage += (1f - self.stealth) * 0.6f;
                self.rangedCrit += (int)((1f - self.stealth) * 10f);
                self.aggro -= (int)((1f - self.stealth) * 750f);
                if (self.stealthTimer > 0)
                {
                    self.stealthTimer--;
                }
            }
            else if (self.setVortex)
            {
                bool flag14 = false;
                if (self.vortexStealthActive)
                {
                    float num29 = self.stealth;
                    self.stealth -= 0.04f;
                    if (self.stealth < 0f)
                    {
                        self.stealth = 0f;
                    }
                    else
                    {
                        flag14 = true;
                    }
                    if (self.stealth == 0f && num29 != self.stealth && Main.netMode == 1)
                    {
                        NetMessage.SendData(84, -1, -1, null, self.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                    self.rangedDamage += (1f - self.stealth) * 0.8f;
                    self.rangedCrit += (int)((1f - self.stealth) * 20f);
                    self.aggro -= (int)((1f - self.stealth) * 1200f);
                    self.moveSpeed *= 0.3f;
                    if (self.mount.Active)
                    {
                        self.vortexStealthActive = false;
                    }
                }
                else
                {
                    float num30 = self.stealth;
                    self.stealth += 0.04f;
                    if (self.stealth > 1f)
                    {
                        self.stealth = 1f;
                    }
                    else
                    {
                        flag14 = true;
                    }
                    if (self.stealth == 1f && num30 != self.stealth && Main.netMode == 1)
                    {
                        NetMessage.SendData(84, -1, -1, null, self.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (flag14)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust = Main.dust[Dust.NewDust(self.Center - vector * 30f, 0, 0, 229, 0f, 0f, 0, default(Color), 1f)];
                        dust.noGravity = true;
                        dust.position = self.Center - vector * (float)Main.rand.Next(5, 11);
                        dust.velocity = vector.RotatedBy(1.5707963705062866, default(Vector2)) * 4f;
                        dust.scale = 0.5f + Main.rand.NextFloat();
                        dust.fadeIn = 0.5f;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust2 = Main.dust[Dust.NewDust(self.Center - vector2 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
                        dust2.noGravity = true;
                        dust2.position = self.Center - vector2 * 12f;
                        dust2.velocity = vector2.RotatedBy(-1.5707963705062866, default(Vector2)) * 2f;
                        dust2.scale = 0.5f + Main.rand.NextFloat();
                        dust2.fadeIn = 0.5f;
                    }
                }
            }
            else
            {
                self.stealth = 1f;
            }
            if (self.manaSick)
            {
                self.magicDamage *= 1f - self.manaSickReduction;
            }
            if (self.inventory[self.selectedItem].type == 1947)
            {
                self.meleeSpeed = (1f + self.meleeSpeed) / 2f;
            }
            if ((double)self.pickSpeed < 0.3)
            {
                self.pickSpeed = 0.3f;
            }
            if (self.meleeSpeed > 3f)
            {
                self.meleeSpeed = 3f;
            }
            if ((double)self.moveSpeed > 1.6)
            {
                self.moveSpeed = 1.6f;
            }
            if (self.tileSpeed > 3f)
            {
                self.tileSpeed = 3f;
            }
            self.tileSpeed = 1f / self.tileSpeed;
            if (self.wallSpeed > 3f)
            {
                self.wallSpeed = 3f;
            }
            self.wallSpeed = 1f / self.wallSpeed;
            if (self.statManaMax2 > 400)
            {
                self.statManaMax2 = 400;
            }
            if (self.statDefense < 0)
            {
                self.statDefense = 0;
            }
            if (self.slowOgreSpit)
            {
                self.moveSpeed /= 3f;
                if (self.velocity.Y == 0f && Math.Abs(self.velocity.X) > 1f)
                {
                    self.velocity.X = self.velocity.X / 2f;
                }
            }
            else if (self.dazed)
            {
                self.moveSpeed /= 3f;
            }
            else if (self.slow)
            {
                self.moveSpeed /= 2f;
            }
            else if (self.chilled)
            {
                self.moveSpeed *= 0.75f;
            }
            if (self.shieldRaised)
            {
                self.moveSpeed /= 3f;
                if (self.velocity.Y == 0f && Math.Abs(self.velocity.X) > 3f)
                {
                    self.velocity.X = self.velocity.X / 2f;
                }
            }
            if (DD2Event.Ongoing)
            {
                DD2Event.FindArenaHitbox();
                if (DD2Event.ShouldBlockBuilding(self.Center))
                {
                    self.noBuilding = true;
                    self.AddBuff(199, 3, true);
                }
            }
            self.meleeSpeed = 1f / self.meleeSpeed;
            PlayerHooks.PostUpdateMiscEffects(self);
            self.UpdateLifeRegen();
            self.soulDrain = 0;
            self.UpdateManaRegen();
            if (self.manaRegenCount < 0)
            {
                self.manaRegenCount = 0;
            }
            if (self.statMana > self.statManaMax2)
            {
                self.statMana = self.statManaMax2;
            }
            self.runAcceleration *= self.moveSpeed;
            self.maxRunSpeed *= self.moveSpeed;
            self.UpdateJumpHeight();
            for (int num31 = 0; num31 < 22; num31++)
            {
                if (self.buffType[num31] > 0 && self.buffTime[num31] > 0 && self.buffImmune[self.buffType[num31]])
                {
                    self.DelBuff(num31);
                }
            }
            if (self.brokenArmor)
            {
                self.statDefense /= 2;
            }
            if (self.witheredArmor)
            {
                self.statDefense /= 2;
            }
            if (self.witheredWeapon)
            {
                self.meleeDamage *= 0.5f;
                self.rangedDamage *= 0.5f;
                self.magicDamage *= 0.5f;
                self.minionDamage *= 0.5f;
                self.thrownDamage *= 0.5f;
            }
            self.lastTileRangeX = Player.tileRangeX;
            self.lastTileRangeY = Player.tileRangeY;
            if (self.mount.Active && self.mount.BlockExtraJumps)
            {
                self.jumpAgainCloud = false;
                self.jumpAgainSandstorm = false;
                self.jumpAgainBlizzard = false;
                self.jumpAgainFart = false;
                self.jumpAgainSail = false;
                self.jumpAgainUnicorn = false;
            }
            else
            {
                if (!self.doubleJumpCloud)
                {
                    self.jumpAgainCloud = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainCloud = true;
                }
                if (!self.doubleJumpSandstorm)
                {
                    self.jumpAgainSandstorm = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainSandstorm = true;
                }
                if (!self.doubleJumpBlizzard)
                {
                    self.jumpAgainBlizzard = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainBlizzard = true;
                }
                if (!self.doubleJumpFart)
                {
                    self.jumpAgainFart = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainFart = true;
                }
                if (!self.doubleJumpSail)
                {
                    self.jumpAgainSail = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainSail = true;
                }
                if (!self.doubleJumpUnicorn)
                {
                    self.jumpAgainUnicorn = false;
                }
                else if (self.velocity.Y == 0f || self.sliding)
                {
                    self.jumpAgainUnicorn = true;
                }
            }
            if (!self.carpet)
            {
                self.canCarpet = false;
                self.carpetFrame = -1;
            }
            else if (self.velocity.Y == 0f || self.sliding)
            {
                self.canCarpet = true;
                self.carpetTime = 0;
                self.carpetFrame = -1;
                self.carpetFrameCounter = 0f;
            }
            if (self.gravDir == -1f)
            {
                self.canCarpet = false;
            }
            if (self.ropeCount > 0)
            {
                self.ropeCount--;
            }
            if (!self.pulley && !self.frozen && !self.webbed && !self.stoned && !self.controlJump && self.gravDir == 1f && self.ropeCount == 0 && self.grappling[0] == -1 && !self.tongued && !self.mount.Active)
            {
                self.FindPulley();
            }
            if (self.pulley)
            {
                if (self.mount.Active)
                {
                    self.pulley = false;
                }
                self.sandStorm = false;
                self.dJumpEffectCloud = false;
                self.dJumpEffectSandstorm = false;
                self.dJumpEffectBlizzard = false;
                self.dJumpEffectFart = false;
                self.dJumpEffectSail = false;
                self.dJumpEffectUnicorn = false;
                int num32 = (int)(self.position.X + (float)(self.width / 2)) / 16;
                int num33 = (int)(self.position.Y - 8f) / 16;
                bool flag15 = false;
                if (self.pulleyDir == 0)
                {
                    self.pulleyDir = 1;
                }
                if (self.pulleyDir == 1)
                {
                    if (self.direction == -1 && self.controlLeft && (self.releaseLeft || self.leftTimer == 0))
                    {
                        self.pulleyDir = 2;
                        flag15 = true;
                    }
                    else if ((self.direction == 1 && self.controlRight && self.releaseRight) || self.rightTimer == 0)
                    {
                        self.pulleyDir = 2;
                        flag15 = true;
                    }
                    else
                    {
                        if (self.direction == 1 && self.controlLeft)
                        {
                            self.direction = -1;
                            flag15 = true;
                        }
                        if (self.direction == -1 && self.controlRight)
                        {
                            self.direction = 1;
                            flag15 = true;
                        }
                    }
                }
                else if (self.pulleyDir == 2)
                {
                    if (self.direction == 1 && self.controlLeft)
                    {
                        flag15 = true;
                        int num34 = num32 * 16 + 8 - self.width / 2;
                        if (!Collision.SolidCollision(new Vector2((float)num34, self.position.Y), self.width, self.height))
                        {
                            self.pulleyDir = 1;
                            self.direction = -1;
                            flag15 = true;
                        }
                    }
                    if (self.direction == -1 && self.controlRight)
                    {
                        flag15 = true;
                        int num35 = num32 * 16 + 8 - self.width / 2;
                        if (!Collision.SolidCollision(new Vector2((float)num35, self.position.Y), self.width, self.height))
                        {
                            self.pulleyDir = 1;
                            self.direction = 1;
                            flag15 = true;
                        }
                    }
                }
                bool flag16 = false;
                if (!flag15 && ((self.controlLeft && (self.releaseLeft || self.leftTimer == 0)) || (self.controlRight && (self.releaseRight || self.rightTimer == 0))))
                {
                    int num36 = 1;
                    if (self.controlLeft)
                    {
                        num36 = -1;
                    }
                    int num37 = num32 + num36;
                    if (Main.tile[num37, num33].active() && Main.tileRope[(int)Main.tile[num37, num33].type])
                    {
                        self.pulleyDir = 1;
                        self.direction = num36;
                        int num38 = num37 * 16 + 8 - self.width / 2;
                        float num39 = self.position.Y;
                        num39 = (float)(num33 * 16 + 22);
                        if ((!Main.tile[num37, num33 - 1].active() || !Main.tileRope[(int)Main.tile[num37, num33 - 1].type]) && (!Main.tile[num37, num33 + 1].active() || !Main.tileRope[(int)Main.tile[num37, num33 + 1].type]))
                        {
                            num39 = (float)(num33 * 16 + 22);
                        }
                        if (Collision.SolidCollision(new Vector2((float)num38, num39), self.width, self.height))
                        {
                            self.pulleyDir = 2;
                            self.direction = -num36;
                            if (self.direction == 1)
                            {
                                num38 = num37 * 16 + 8 - self.width / 2 + 6;
                            }
                            else
                            {
                                num38 = num37 * 16 + 8 - self.width / 2 + -6;
                            }
                        }
                        if (i == Main.myPlayer)
                        {
                            Main.cameraX = Main.cameraX + self.position.X - (float)num38;
                        }
                        self.position.X = (float)num38;
                        self.gfxOffY = self.position.Y - num39;
                        self.position.Y = num39;
                        flag16 = true;
                    }
                }
                if (!flag16 && !flag15 && !self.controlUp && ((self.controlLeft && self.releaseLeft) || (self.controlRight && self.releaseRight)))
                {
                    self.pulley = false;
                    if (self.controlLeft && self.velocity.X == 0f)
                    {
                        self.velocity.X = -1f;
                    }
                    if (self.controlRight && self.velocity.X == 0f)
                    {
                        self.velocity.X = 1f;
                    }
                }
                if (self.velocity.X != 0f)
                {
                    self.pulley = false;
                }
                if (Main.tile[num32, num33] == null)
                {
                    Main.tile[num32, num33] = new Tile();
                }
                if (!Main.tile[num32, num33].active() || !Main.tileRope[(int)Main.tile[num32, num33].type])
                {
                    self.pulley = false;
                }
                if (self.gravDir != 1f)
                {
                    self.pulley = false;
                }
                if (self.frozen || self.webbed || self.stoned)
                {
                    self.pulley = false;
                }
                if (!self.pulley)
                {
                    self.velocity.Y = self.velocity.Y - self.gravity;
                }
                if (self.controlJump)
                {
                    self.pulley = false;
                    self.jump = Player.jumpHeight;
                    self.velocity.Y = -Player.jumpSpeed;
                }
            }
            if (self.pulley)
            {
                self.fallStart = (int)self.position.Y / 16;
                self.wingFrame = 0;
                if (self.wings == 4)
                {
                    self.wingFrame = 3;
                }
                int num40 = (int)(self.position.X + (float)(self.width / 2)) / 16;
                int num41 = (int)(self.position.Y - 16f) / 16;
                int num42 = (int)(self.position.Y - 8f) / 16;
                bool flag17 = true;
                bool flag18 = false;
                if ((Main.tile[num40, num42 - 1].active() && Main.tileRope[(int)Main.tile[num40, num42 - 1].type]) || (Main.tile[num40, num42 + 1].active() && Main.tileRope[(int)Main.tile[num40, num42 + 1].type]))
                {
                    flag18 = true;
                }
                if (Main.tile[num40, num41] == null)
                {
                    Main.tile[num40, num41] = new Tile();
                }
                if (!Main.tile[num40, num41].active() || !Main.tileRope[(int)Main.tile[num40, num41].type])
                {
                    flag17 = false;
                    if (self.velocity.Y < 0f)
                    {
                        self.velocity.Y = 0f;
                    }
                }
                if (flag18)
                {
                    if (self.controlUp && flag17)
                    {
                        float num43 = self.position.X;
                        float y3 = self.position.Y - Math.Abs(self.velocity.Y) - 2f;
                        if (Collision.SolidCollision(new Vector2(num43, y3), self.width, self.height))
                        {
                            num43 = (float)(num40 * 16 + 8 - self.width / 2 + 6);
                            if (!Collision.SolidCollision(new Vector2(num43, y3), self.width, (int)((float)self.height + Math.Abs(self.velocity.Y) + 2f)))
                            {
                                if (i == Main.myPlayer)
                                {
                                    Main.cameraX = Main.cameraX + self.position.X - num43;
                                }
                                self.pulleyDir = 2;
                                self.direction = 1;
                                self.position.X = num43;
                                self.velocity.X = 0f;
                            }
                            else
                            {
                                num43 = (float)(num40 * 16 + 8 - self.width / 2 + -6);
                                if (!Collision.SolidCollision(new Vector2(num43, y3), self.width, (int)((float)self.height + Math.Abs(self.velocity.Y) + 2f)))
                                {
                                    if (i == Main.myPlayer)
                                    {
                                        Main.cameraX = Main.cameraX + self.position.X - num43;
                                    }
                                    self.pulleyDir = 2;
                                    self.direction = -1;
                                    self.position.X = num43;
                                    self.velocity.X = 0f;
                                }
                            }
                        }
                        if (self.velocity.Y > 0f)
                        {
                            self.velocity.Y = self.velocity.Y * 0.7f;
                        }
                        if (self.velocity.Y > -3f)
                        {
                            self.velocity.Y = self.velocity.Y - 0.2f;
                        }
                        else
                        {
                            self.velocity.Y = self.velocity.Y - 0.02f;
                        }
                        if (self.velocity.Y < -8f)
                        {
                            self.velocity.Y = -8f;
                        }
                    }
                    else if (self.controlDown)
                    {
                        float num44 = self.position.X;
                        float y4 = self.position.Y;
                        if (Collision.SolidCollision(new Vector2(num44, y4), self.width, (int)((float)self.height + Math.Abs(self.velocity.Y) + 2f)))
                        {
                            num44 = (float)(num40 * 16 + 8 - self.width / 2 + 6);
                            if (!Collision.SolidCollision(new Vector2(num44, y4), self.width, (int)((float)self.height + Math.Abs(self.velocity.Y) + 2f)))
                            {
                                if (i == Main.myPlayer)
                                {
                                    Main.cameraX = Main.cameraX + self.position.X - num44;
                                }
                                self.pulleyDir = 2;
                                self.direction = 1;
                                self.position.X = num44;
                                self.velocity.X = 0f;
                            }
                            else
                            {
                                num44 = (float)(num40 * 16 + 8 - self.width / 2 + -6);
                                if (!Collision.SolidCollision(new Vector2(num44, y4), self.width, (int)((float)self.height + Math.Abs(self.velocity.Y) + 2f)))
                                {
                                    if (i == Main.myPlayer)
                                    {
                                        Main.cameraX = Main.cameraX + self.position.X - num44;
                                    }
                                    self.pulleyDir = 2;
                                    self.direction = -1;
                                    self.position.X = num44;
                                    self.velocity.X = 0f;
                                }
                            }
                        }
                        if (self.velocity.Y < 0f)
                        {
                            self.velocity.Y = self.velocity.Y * 0.7f;
                        }
                        if (self.velocity.Y < 3f)
                        {
                            self.velocity.Y = self.velocity.Y + 0.2f;
                        }
                        else
                        {
                            self.velocity.Y = self.velocity.Y + 0.1f;
                        }
                        if (self.velocity.Y > self.maxFallSpeed)
                        {
                            self.velocity.Y = self.maxFallSpeed;
                        }
                    }
                    else
                    {
                        self.velocity.Y = self.velocity.Y * 0.7f;
                        if ((double)self.velocity.Y > -0.1 && (double)self.velocity.Y < 0.1)
                        {
                            self.velocity.Y = 0f;
                        }
                    }
                }
                else if (self.controlDown)
                {
                    self.ropeCount = 10;
                    self.pulley = false;
                    self.velocity.Y = 1f;
                }
                else
                {
                    self.velocity.Y = 0f;
                    self.position.Y = (float)(num41 * 16 + 22);
                }
                float num45 = (float)(num40 * 16 + 8 - self.width / 2);
                if (self.pulleyDir == 1)
                {
                    num45 = (float)(num40 * 16 + 8 - self.width / 2);
                }
                if (self.pulleyDir == 2)
                {
                    num45 = (float)(num40 * 16 + 8 - self.width / 2 + 6 * self.direction);
                }
                if (i == Main.myPlayer)
                {
                    Main.cameraX = Main.cameraX + self.position.X - num45;
                }
                self.position.X = num45;
                self.pulleyFrameCounter += Math.Abs(self.velocity.Y * 0.75f);
                if (self.velocity.Y != 0f)
                {
                    self.pulleyFrameCounter += 0.75f;
                }
                if (self.pulleyFrameCounter > 10f)
                {
                    self.pulleyFrame++;
                    self.pulleyFrameCounter = 0f;
                }
                if (self.pulleyFrame > 1)
                {
                    self.pulleyFrame = 0;
                }
                self.canCarpet = true;
                self.carpetFrame = -1;
                self.wingTime = (float)self.wingTimeMax;
                self.rocketTime = self.rocketTimeMax;
                self.rocketDelay = 0;
                self.rocketFrame = false;
                self.canRocket = false;
                self.rocketRelease = false;
                self.DashMovement();
            }
            else if (self.grappling[0] == -1 && !self.tongued)
            {
                if (self.wingsLogic > 0 && self.velocity.Y != 0f && !self.merman)
                {
                    if (self.wingsLogic == 1 || self.wingsLogic == 2)
                    {
                        self.accRunSpeed = 6.25f;
                    }
                    if (self.wingsLogic == 4)
                    {
                        self.accRunSpeed = 6.5f;
                    }
                    if (self.wingsLogic == 5 || self.wingsLogic == 6 || self.wingsLogic == 13 || self.wingsLogic == 15)
                    {
                        self.accRunSpeed = 6.75f;
                    }
                    if (self.wingsLogic == 7 || self.wingsLogic == 8)
                    {
                        self.accRunSpeed = 7f;
                    }
                    if (self.wingsLogic == 9 || self.wingsLogic == 10 || self.wingsLogic == 11 || self.wingsLogic == 20 || self.wingsLogic == 21 || self.wingsLogic == 23 || self.wingsLogic == 24)
                    {
                        self.accRunSpeed = 7.5f;
                    }
                    if (self.wingsLogic == 22)
                    {
                        if (self.controlDown && self.controlJump && self.wingTime > 0f)
                        {
                            self.accRunSpeed = 10f;
                            self.runAcceleration *= 10f;
                        }
                        else
                        {
                            self.accRunSpeed = 6.25f;
                        }
                    }
                    if (self.wingsLogic == 30 || self.wingsLogic == 31)
                    {
                        if (self.controlDown && self.controlJump && self.wingTime > 0f)
                        {
                            self.accRunSpeed = 12f;
                            self.runAcceleration *= 12f;
                        }
                        else
                        {
                            self.accRunSpeed = 6.5f;
                            self.runAcceleration *= 1.5f;
                        }
                    }
                    if (self.wingsLogic == 26)
                    {
                        self.accRunSpeed = 8f;
                        self.runAcceleration *= 2f;
                    }
                    if (self.wingsLogic == 37)
                    {
                        if (self.controlDown && self.controlJump && self.wingTime > 0f)
                        {
                            self.accRunSpeed = 12f;
                            self.runAcceleration *= 12f;
                        }
                        else
                        {
                            self.accRunSpeed = 6f;
                            self.runAcceleration *= 2.5f;
                        }
                    }
                    if (self.wingsLogic == 29 || self.wingsLogic == 32)
                    {
                        self.accRunSpeed = 9f;
                        self.runAcceleration *= 2.5f;
                    }
                    if (self.wingsLogic == 12)
                    {
                        self.accRunSpeed = 7.75f;
                    }
                    if (self.wingsLogic == 16 || self.wingsLogic == 17 || self.wingsLogic == 18 || self.wingsLogic == 19 || self.wingsLogic == 34 || self.wingsLogic == 3 || self.wingsLogic == 28 || self.wingsLogic == 33 || self.wingsLogic == 34 || self.wingsLogic == 35 || self.wingsLogic == 36)
                    {
                        self.accRunSpeed = 7f;
                    }
                    ItemLoader.HorizontalWingSpeeds(self);
                }
                if (self.sticky)
                {
                    self.maxRunSpeed *= 0.25f;
                    self.runAcceleration *= 0.25f;
                    self.runSlowdown *= 2f;
                    if (self.velocity.X > self.maxRunSpeed)
                    {
                        self.velocity.X = self.maxRunSpeed;
                    }
                    if (self.velocity.X < -self.maxRunSpeed)
                    {
                        self.velocity.X = -self.maxRunSpeed;
                    }
                }
                else if (self.powerrun)
                {
                    self.maxRunSpeed *= 3.5f;
                    self.runAcceleration *= 1f;
                    self.runSlowdown *= 2f;
                }
                else if (self.slippy2)
                {
                    self.runAcceleration *= 0.6f;
                    self.runSlowdown = 0f;
                    if (self.iceSkate)
                    {
                        self.runAcceleration *= 3.5f;
                        self.maxRunSpeed *= 1.25f;
                    }
                }
                else if (self.slippy)
                {
                    self.runAcceleration *= 0.7f;
                    if (self.iceSkate)
                    {
                        self.runAcceleration *= 3.5f;
                        self.maxRunSpeed *= 1.25f;
                    }
                    else
                    {
                        self.runSlowdown *= 0.1f;
                    }
                }
                if (self.sandStorm)
                {
                    self.runAcceleration *= 1.5f;
                    self.maxRunSpeed *= 2f;
                }
                if (self.dJumpEffectBlizzard && self.doubleJumpBlizzard)
                {
                    self.runAcceleration *= 3f;
                    self.maxRunSpeed *= 1.5f;
                }
                if (self.dJumpEffectFart && self.doubleJumpFart)
                {
                    self.runAcceleration *= 3f;
                    self.maxRunSpeed *= 1.75f;
                }
                if (self.dJumpEffectUnicorn && self.doubleJumpUnicorn)
                {
                    self.runAcceleration *= 3f;
                    self.maxRunSpeed *= 1.5f;
                }
                if (self.dJumpEffectSail && self.doubleJumpSail)
                {
                    self.runAcceleration *= 1.5f;
                    self.maxRunSpeed *= 1.25f;
                }
                if (self.carpetFrame != -1)
                {
                    self.runAcceleration *= 1.25f;
                    self.maxRunSpeed *= 1.5f;
                }
                if (self.inventory[self.selectedItem].type == 3106 && self.stealth < 1f)
                {
                    float num46 = self.maxRunSpeed / 2f * (1f - self.stealth);
                    self.maxRunSpeed -= num46;
                    self.accRunSpeed = self.maxRunSpeed;
                }
                if (self.mount.Active)
                {
                    self.rocketBoots = 0;
                    self.wings = 0;
                    self.wingsLogic = 0;
                    self.maxRunSpeed = self.mount.RunSpeed;
                    self.accRunSpeed = self.mount.DashSpeed;
                    self.runAcceleration = self.mount.Acceleration;
                    if (self.mount.Type == 12 && !self.MountFishronSpecial)
                    {
                        self.runAcceleration /= 2f;
                        self.maxRunSpeed /= 2f;
                    }
                    self.mount.AbilityRecovery();
                    if (self.mount.Cart && self.velocity.Y == 0f)
                    {
                        if (!Minecart.OnTrack(self.position, self.width, self.height))
                        {
                            self.fullRotation = 0f;
                            self.onWrongGround = true;
                            self.runSlowdown = 0.2f;
                            if ((self.controlLeft && self.releaseLeft) || (self.controlRight && self.releaseRight))
                            {
                                self.mount.Dismount(self);
                            }
                        }
                        else
                        {
                            self.runSlowdown = self.runAcceleration;
                            self.onWrongGround = false;
                        }
                    }
                    if (self.mount.Type == 8)
                    {
                        self.mount.UpdateDrill(self, self.controlUp, self.controlDown);
                    }
                }
                PlayerHooks.PostUpdateRunSpeeds(self);
                self.HorizontalMovement();
                if (self.gravControl)
                {
                    if (self.controlUp && self.releaseUp)
                    {
                        if (self.gravDir == 1f)
                        {
                            self.gravDir = -1f;
                            self.fallStart = (int)(self.position.Y / 16f);
                            self.jump = 0;
                            Main.PlaySound(SoundID.Item8, self.position);
                        }
                        else
                        {
                            self.gravDir = 1f;
                            self.fallStart = (int)(self.position.Y / 16f);
                            self.jump = 0;
                            Main.PlaySound(SoundID.Item8, self.position);
                        }
                    }
                }
                else if (self.gravControl2)
                {
                    if (self.controlUp && self.releaseUp && self.velocity.Y == 0f)
                    {
                        if (self.gravDir == 1f)
                        {
                            self.gravDir = -1f;
                            self.fallStart = (int)(self.position.Y / 16f);
                            self.jump = 0;
                            Main.PlaySound(SoundID.Item8, self.position);
                        }
                        else
                        {
                            self.gravDir = 1f;
                            self.fallStart = (int)(self.position.Y / 16f);
                            self.jump = 0;
                            Main.PlaySound(SoundID.Item8, self.position);
                        }
                    }
                }
                else
                {
                    self.gravDir = 1f;
                }
                if (self.velocity.Y == 0f && self.mount.Active && self.mount.CanHover && self.controlUp && self.releaseUp)
                {
                    self.velocity.Y = -(self.mount.Acceleration + self.gravity + 0.001f);
                }
                if (self.controlUp)
                {
                    self.releaseUp = false;
                }
                else
                {
                    self.releaseUp = true;
                }
                self.sandStorm = false;
                self.JumpMovement();
                if (self.wingsLogic == 0)
                {
                    self.wingTime = 0f;
                }
                if (self.rocketBoots == 0)
                {
                    self.rocketTime = 0;
                }
                if (self.jump == 0)
                {
                    self.dJumpEffectCloud = false;
                    self.dJumpEffectSandstorm = false;
                    self.dJumpEffectBlizzard = false;
                    self.dJumpEffectFart = false;
                    self.dJumpEffectSail = false;
                    self.dJumpEffectUnicorn = false;
                }
                self.DashMovement();
                self.WallslideMovement();
                self.CarpetMovement();
                self.DoubleJumpVisuals();
                if (self.wings > 0 || self.mount.Active)
                {
                    self.sandStorm = false;
                }
                if (((self.gravDir == 1f && self.velocity.Y > -Player.jumpSpeed) || (self.gravDir == -1f && self.velocity.Y < Player.jumpSpeed)) && self.velocity.Y != 0f)
                {
                    self.canRocket = true;
                }
                bool flag19 = false;
                if (((self.velocity.Y == 0f || self.sliding) && self.releaseJump) || (self.autoJump && self.justJumped))
                {
                    self.mount.ResetFlightTime(self.velocity.X);
                    self.wingTime = (float)self.wingTimeMax;
                }
                if (self.wingsLogic > 0 && self.controlJump && self.wingTime > 0f && !self.jumpAgainCloud && self.jump == 0 && self.velocity.Y != 0f)
                {
                    flag19 = true;
                }
                if ((self.wingsLogic == 22 || self.wingsLogic == 28 || self.wingsLogic == 30 || self.wingsLogic == 32 || self.wingsLogic == 29 || self.wingsLogic == 33 || self.wingsLogic == 35 || self.wingsLogic == 37) && self.controlJump && self.controlDown && self.wingTime > 0f)
                {
                    flag19 = true;
                }
                if (self.frozen || self.webbed || self.stoned)
                {
                    if (self.mount.Active)
                    {
                        self.mount.Dismount(self);
                    }
                    self.velocity.Y = self.velocity.Y + self.gravity;
                    if (self.velocity.Y > self.maxFallSpeed)
                    {
                        self.velocity.Y = self.maxFallSpeed;
                    }
                    self.sandStorm = false;
                    self.dJumpEffectCloud = false;
                    self.dJumpEffectSandstorm = false;
                    self.dJumpEffectBlizzard = false;
                    self.dJumpEffectFart = false;
                    self.dJumpEffectSail = false;
                    self.dJumpEffectUnicorn = false;
                }
                else
                {
                    bool isCustomWings = ItemLoader.WingUpdate(self, flag19);
                    if (flag19)
                    {
                        if (self.wings == 10 && Main.rand.Next(2) == 0)
                        {
                            int num47 = 4;
                            if (self.direction == 1)
                            {
                                num47 = -40;
                            }
                            int num48 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num47, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 76, 0f, 0f, 50, default(Color), 0.6f);
                            Main.dust[num48].fadeIn = 1.1f;
                            Main.dust[num48].noGravity = true;
                            Main.dust[num48].noLight = true;
                            Main.dust[num48].velocity *= 0.3f;
                            Main.dust[num48].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 34 && Main.rand.Next(2) == 0)
                        {
                            int num49 = 4;
                            if (self.direction == 1)
                            {
                                num49 = -40;
                            }
                            int num50 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num49, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 261, 0f, 0f, 50, default(Color), 0.6f);
                            Main.dust[num50].fadeIn = 1.1f;
                            Main.dust[num50].noGravity = true;
                            Main.dust[num50].noLight = true;
                            Main.dust[num50].velocity *= 0.3f;
                            Main.dust[num50].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 9 && Main.rand.Next(2) == 0)
                        {
                            int num51 = 4;
                            if (self.direction == 1)
                            {
                                num51 = -40;
                            }
                            int num52 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num51, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 6, 0f, 0f, 200, default(Color), 2f);
                            Main.dust[num52].noGravity = true;
                            Main.dust[num52].velocity *= 0.3f;
                            Main.dust[num52].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 6 && Main.rand.Next(4) == 0)
                        {
                            int num53 = 4;
                            if (self.direction == 1)
                            {
                                num53 = -40;
                            }
                            int num54 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num53, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 55, 0f, 0f, 200, default(Color), 1f);
                            Main.dust[num54].velocity *= 0.3f;
                            Main.dust[num54].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 5 && Main.rand.Next(3) == 0)
                        {
                            int num55 = 6;
                            if (self.direction == 1)
                            {
                                num55 = -30;
                            }
                            int num56 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num55, self.position.Y), 18, self.height, 58, 0f, 0f, 255, default(Color), 1.2f);
                            Main.dust[num56].velocity *= 0.3f;
                            Main.dust[num56].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 26)
                        {
                            int num57 = 6;
                            if (self.direction == 1)
                            {
                                num57 = -30;
                            }
                            int num58 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num57, self.position.Y), 18, self.height, 217, 0f, 0f, 100, default(Color), 1.4f);
                            Main.dust[num58].noGravity = true;
                            Main.dust[num58].noLight = true;
                            Main.dust[num58].velocity /= 4f;
                            Main.dust[num58].velocity -= self.velocity;
                            Main.dust[num58].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            if (Main.rand.Next(2) == 0)
                            {
                                num57 = -24;
                                if (self.direction == 1)
                                {
                                    num57 = 12;
                                }
                                float num59 = self.position.Y;
                                if (self.gravDir == -1f)
                                {
                                    num59 += (float)(self.height / 2);
                                }
                                num58 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num57, num59), 12, self.height / 2, 217, 0f, 0f, 100, default(Color), 1.4f);
                                Main.dust[num58].noGravity = true;
                                Main.dust[num58].noLight = true;
                                Main.dust[num58].velocity /= 4f;
                                Main.dust[num58].velocity -= self.velocity;
                                Main.dust[num58].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                        }
                        if (self.wings == 37)
                        {
                            int num60 = 6;
                            if (self.direction == 1)
                            {
                                num60 = -30;
                            }
                            Dust dust3 = Dust.NewDustDirect(new Vector2(self.position.X + (float)(self.width / 2) + (float)num60, self.position.Y), 24, self.height, Utils.SelectRandom<int>(Main.rand, new int[]
                                    {
                                        31,
                                        31,
                                        31
                                    }), 0f, 0f, 100, default(Color), 1f);
                            dust3.noGravity = true;
                            dust3.noLight = true;
                            dust3.velocity /= 4f;
                            dust3.velocity -= self.velocity / 2f;
                            dust3.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            if (dust3.type == 55)
                            {
                                dust3.noGravity = true;
                                dust3.velocity *= 2f;
                                dust3.color = Color.Red;
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                num60 = -24;
                                if (self.direction == 1)
                                {
                                    num60 = 12;
                                }
                                float num61 = self.position.Y;
                                if (self.gravDir == -1f)
                                {
                                    num61 += (float)(self.height / 2);
                                }
                                dust3 = Dust.NewDustDirect(new Vector2(self.position.X + (float)(self.width / 2) + (float)num60, num61), 16, self.height / 2, Utils.SelectRandom<int>(Main.rand, new int[]
                                        {
                                            31,
                                            31,
                                            31
                                        }), 0f, 0f, 100, default(Color), 1f);
                                dust3.noGravity = true;
                                dust3.noLight = true;
                                dust3.velocity /= 4f;
                                dust3.velocity -= self.velocity / 2f;
                                dust3.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                if (dust3.type == 55)
                                {
                                    dust3.noGravity = true;
                                    dust3.velocity *= 2f;
                                    dust3.color = Color.Red;
                                }
                            }
                        }
                        if (self.wings == 29 && Main.rand.Next(3) == 0)
                        {
                            int num62 = 4;
                            if (self.direction == 1)
                            {
                                num62 = -40;
                            }
                            int num63 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num62, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 6, 0f, 0f, 100, default(Color), 2.4f);
                            Main.dust[num63].noGravity = true;
                            Main.dust[num63].velocity *= 0.3f;
                            if (Main.rand.Next(10) == 0)
                            {
                                Main.dust[num63].fadeIn = 2f;
                            }
                            Main.dust[num63].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                        if (self.wings == 31)
                        {
                            if (Main.rand.Next(6) == 0)
                            {
                                int num64 = 4;
                                if (self.direction == 1)
                                {
                                    num64 = -40;
                                }
                                Dust dust4 = Main.dust[Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num64, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 86, 0f, 0f, 0, default(Color), 1f)];
                                dust4.noGravity = true;
                                dust4.scale = 1f;
                                dust4.fadeIn = 1.2f;
                                dust4.velocity *= 0.2f;
                                dust4.noLight = true;
                                dust4.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                int num65 = 4;
                                if (self.direction == 1)
                                {
                                    num65 = -40;
                                }
                                Dust dust5 = Main.dust[Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num65, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 240, 0f, 0f, 0, default(Color), 1f)];
                                dust5.noGravity = true;
                                dust5.scale = 1.2f;
                                dust5.velocity *= 0.2f;
                                dust5.alpha = 200;
                                dust5.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                if (Main.rand.Next(6) == 0)
                                {
                                    int num66 = -24;
                                    if (self.direction == 1)
                                    {
                                        num66 = 12;
                                    }
                                    float num67 = self.position.Y;
                                    if (self.gravDir == -1f)
                                    {
                                        num67 += (float)(self.height / 2);
                                    }
                                    Dust dust6 = Main.dust[Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num66, num67), 12, self.height / 2, 86, 0f, 0f, 0, default(Color), 1f)];
                                    dust6.noGravity = true;
                                    dust6.scale = 1f;
                                    dust6.fadeIn = 1.2f;
                                    dust6.velocity *= 0.2f;
                                    dust6.noLight = true;
                                    dust6.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (Main.rand.Next(3) == 0)
                                {
                                    int num66 = -24;
                                    if (self.direction == 1)
                                    {
                                        num66 = 12;
                                    }
                                    float num68 = self.position.Y;
                                    if (self.gravDir == -1f)
                                    {
                                        num68 += (float)(self.height / 2);
                                    }
                                    Dust dust7 = Main.dust[Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num66, num68), 12, self.height / 2, 240, 0f, 0f, 0, default(Color), 1f)];
                                    dust7.noGravity = true;
                                    dust7.scale = 1.2f;
                                    dust7.velocity *= 0.2f;
                                    dust7.alpha = 200;
                                    dust7.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                            }
                        }
                        self.WingMovement();
                    }
                    if (self.wings == 4)
                    {
                        if (flag19 || self.jump > 0)
                        {
                            self.rocketDelay2--;
                            if (self.rocketDelay2 <= 0)
                            {
                                Main.PlaySound(SoundID.Item13, self.position);
                                self.rocketDelay2 = 60;
                            }
                            int num69 = 2;
                            if (self.controlUp)
                            {
                                num69 = 4;
                            }
                            for (int num70 = 0; num70 < num69; num70++)
                            {
                                int type = 6;
                                if (self.head == 41)
                                {
                                    int arg_5A31_0 = self.body;
                                }
                                float scale = 1.75f;
                                int alpha = 100;
                                float x3 = self.position.X + (float)(self.width / 2) + 16f;
                                if (self.direction > 0)
                                {
                                    x3 = self.position.X + (float)(self.width / 2) - 26f;
                                }
                                float num71 = self.position.Y + (float)self.height - 18f;
                                if (num70 == 1 || num70 == 3)
                                {
                                    x3 = self.position.X + (float)(self.width / 2) + 8f;
                                    if (self.direction > 0)
                                    {
                                        x3 = self.position.X + (float)(self.width / 2) - 20f;
                                    }
                                    num71 += 6f;
                                }
                                if (num70 > 1)
                                {
                                    num71 += self.velocity.Y;
                                }
                                int num72 = Dust.NewDust(new Vector2(x3, num71), 8, 8, type, 0f, 0f, alpha, default(Color), scale);
                                Dust expr_5B44_cp_0 = Main.dust[num72];
                                expr_5B44_cp_0.velocity.X = expr_5B44_cp_0.velocity.X * 0.1f;
                                Main.dust[num72].velocity.Y = Main.dust[num72].velocity.Y * 1f + 2f * self.gravDir - self.velocity.Y * 0.3f;
                                Main.dust[num72].noGravity = true;
                                Main.dust[num72].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                if (num69 == 4)
                                {
                                    Dust expr_5BDC_cp_0 = Main.dust[num72];
                                    expr_5BDC_cp_0.velocity.Y = expr_5BDC_cp_0.velocity.Y + 6f;
                                }
                            }
                            self.wingFrameCounter++;
                            if (self.wingFrameCounter > 4)
                            {
                                self.wingFrame++;
                                self.wingFrameCounter = 0;
                                if (self.wingFrame >= 3)
                                {
                                    self.wingFrame = 0;
                                }
                            }
                        }
                        else if (!self.controlJump || self.velocity.Y == 0f)
                        {
                            self.wingFrame = 3;
                        }
                    }
                    else if (self.wings == 28)
                    {
                        if (self.velocity.Y != 0f)
                        {
                            Lighting.AddLight(self.Bottom, 0.3f, 0.1f, 0.4f);
                        }
                    }
                    else if (self.wings == 22)
                    {
                        if (!self.controlJump)
                        {
                            self.wingFrame = 0;
                            self.wingFrameCounter = 0;
                        }
                        else if (self.wingTime > 0f)
                        {
                            if (self.controlDown)
                            {
                                if (self.velocity.X != 0f)
                                {
                                    self.wingFrameCounter++;
                                    int num73 = 2;
                                    if (self.wingFrameCounter < num73)
                                    {
                                        self.wingFrame = 1;
                                    }
                                    else if (self.wingFrameCounter < num73 * 2)
                                    {
                                        self.wingFrame = 2;
                                    }
                                    else if (self.wingFrameCounter < num73 * 3)
                                    {
                                        self.wingFrame = 3;
                                    }
                                    else if (self.wingFrameCounter < num73 * 4 - 1)
                                    {
                                        self.wingFrame = 2;
                                    }
                                    else
                                    {
                                        self.wingFrame = 2;
                                        self.wingFrameCounter = 0;
                                    }
                                }
                                else
                                {
                                    self.wingFrameCounter++;
                                    int num74 = 6;
                                    if (self.wingFrameCounter < num74)
                                    {
                                        self.wingFrame = 4;
                                    }
                                    else if (self.wingFrameCounter < num74 * 2)
                                    {
                                        self.wingFrame = 5;
                                    }
                                    else if (self.wingFrameCounter < num74 * 3 - 1)
                                    {
                                        self.wingFrame = 4;
                                    }
                                    else
                                    {
                                        self.wingFrame = 4;
                                        self.wingFrameCounter = 0;
                                    }
                                }
                            }
                            else
                            {
                                self.wingFrameCounter++;
                                int num75 = 2;
                                if (self.wingFrameCounter < num75)
                                {
                                    self.wingFrame = 4;
                                }
                                else if (self.wingFrameCounter < num75 * 2)
                                {
                                    self.wingFrame = 5;
                                }
                                else if (self.wingFrameCounter < num75 * 3)
                                {
                                    self.wingFrame = 6;
                                }
                                else if (self.wingFrameCounter < num75 * 4 - 1)
                                {
                                    self.wingFrame = 5;
                                }
                                else
                                {
                                    self.wingFrame = 5;
                                    self.wingFrameCounter = 0;
                                }
                            }
                        }
                        else
                        {
                            self.wingFrameCounter++;
                            int num76 = 6;
                            if (self.wingFrameCounter < num76)
                            {
                                self.wingFrame = 4;
                            }
                            else if (self.wingFrameCounter < num76 * 2)
                            {
                                self.wingFrame = 5;
                            }
                            else if (self.wingFrameCounter < num76 * 3 - 1)
                            {
                                self.wingFrame = 4;
                            }
                            else
                            {
                                self.wingFrame = 4;
                                self.wingFrameCounter = 0;
                            }
                        }
                    }
                    else if (self.wings == 12)
                    {
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            int num77 = 5;
                            if (self.wingFrameCounter < num77)
                            {
                                self.wingFrame = 1;
                            }
                            else if (self.wingFrameCounter < num77 * 2)
                            {
                                self.wingFrame = 2;
                            }
                            else if (self.wingFrameCounter < num77 * 3)
                            {
                                self.wingFrame = 3;
                            }
                            else if (self.wingFrameCounter < num77 * 4 - 1)
                            {
                                self.wingFrame = 2;
                            }
                            else
                            {
                                self.wingFrame = 2;
                                self.wingFrameCounter = 0;
                            }
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            self.wingFrame = 2;
                        }
                        else
                        {
                            self.wingFrame = 0;
                        }
                    }
                    else if (self.wings == 24)
                    {
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            int num78 = 1;
                            if (self.wingFrameCounter < num78)
                            {
                                self.wingFrame = 1;
                            }
                            else if (self.wingFrameCounter < num78 * 2)
                            {
                                self.wingFrame = 2;
                            }
                            else if (self.wingFrameCounter < num78 * 3)
                            {
                                self.wingFrame = 3;
                            }
                            else
                            {
                                self.wingFrame = 2;
                                if (self.wingFrameCounter >= num78 * 4 - 1)
                                {
                                    self.wingFrameCounter = 0;
                                }
                            }
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            if (self.controlJump)
                            {
                                self.wingFrameCounter++;
                                int num79 = 3;
                                if (self.wingFrameCounter < num79)
                                {
                                    self.wingFrame = 1;
                                }
                                else if (self.wingFrameCounter < num79 * 2)
                                {
                                    self.wingFrame = 2;
                                }
                                else if (self.wingFrameCounter < num79 * 3)
                                {
                                    self.wingFrame = 3;
                                }
                                else
                                {
                                    self.wingFrame = 2;
                                    if (self.wingFrameCounter >= num79 * 4 - 1)
                                    {
                                        self.wingFrameCounter = 0;
                                    }
                                }
                            }
                            else if (self.wingTime == 0f)
                            {
                                self.wingFrame = 0;
                            }
                            else
                            {
                                self.wingFrame = 1;
                            }
                        }
                        else
                        {
                            self.wingFrame = 0;
                        }
                    }
                    else if (self.wings == 30)
                    {
                        bool flag20 = false;
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            int num80 = 2;
                            if (self.wingFrameCounter >= num80 * 3)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = 1 + self.wingFrameCounter / num80;
                            flag20 = true;
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            if (self.controlJump)
                            {
                                self.wingFrameCounter++;
                                int num81 = 2;
                                if (self.wingFrameCounter >= num81 * 3)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = 1 + self.wingFrameCounter / num81;
                                flag20 = true;
                            }
                            else if (self.wingTime == 0f)
                            {
                                self.wingFrame = 0;
                            }
                            else
                            {
                                self.wingFrame = 0;
                            }
                        }
                        else
                        {
                            self.wingFrame = 0;
                        }
                        if (flag20)
                        {
                            for (int num82 = 0; num82 < 4; num82++)
                            {
                                if (Main.rand.Next(4) == 0)
                                {
                                    Vector2 value = (-0.745398164f + 0.3926991f * (float)num82 + 0.03f * (float)num82).ToRotationVector2() * new Vector2((float)(-(float)self.direction * 20), 20f);
                                    Dust dust8 = Main.dust[Dust.NewDust(self.Center, 0, 0, 229, 0f, 0f, 100, Color.White, 0.8f)];
                                    dust8.noGravity = true;
                                    dust8.position = self.Center + value;
                                    dust8.velocity = self.DirectionTo(dust8.position) * 2f;
                                    if (Main.rand.Next(10) != 0)
                                    {
                                        dust8.customData = self;
                                    }
                                    else
                                    {
                                        dust8.fadeIn = 0.5f;
                                    }
                                    dust8.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                            }
                            for (int num83 = 0; num83 < 4; num83++)
                            {
                                if (Main.rand.Next(8) == 0)
                                {
                                    Vector2 value2 = (-0.7053982f + 0.3926991f * (float)num83 + 0.03f * (float)num83).ToRotationVector2() * new Vector2((float)(self.direction * 20), 24f) + new Vector2((float)(-(float)self.direction) * 16f, 0f);
                                    Dust dust9 = Main.dust[Dust.NewDust(self.Center, 0, 0, 229, 0f, 0f, 100, Color.White, 0.5f)];
                                    dust9.noGravity = true;
                                    dust9.position = self.Center + value2;
                                    dust9.velocity = Vector2.Normalize(dust9.position - self.Center - new Vector2((float)(-(float)self.direction) * 16f, 0f)) * 2f;
                                    dust9.position += dust9.velocity * 5f;
                                    if (Main.rand.Next(10) != 0)
                                    {
                                        dust9.customData = self;
                                    }
                                    else
                                    {
                                        dust9.fadeIn = 0.5f;
                                    }
                                    dust9.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                            }
                        }
                    }
                    else if (self.wings == 34)
                    {
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            int num84 = 4;
                            if (self.wingFrameCounter >= num84 * 6)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = self.wingFrameCounter / num84;
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            if (self.controlJump)
                            {
                                self.wingFrameCounter++;
                                int num85 = 9;
                                if (self.wingFrameCounter >= num85 * 6)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = self.wingFrameCounter / num85;
                            }
                            else
                            {
                                self.wingFrameCounter++;
                                int num86 = 6;
                                if (self.wingFrameCounter >= num86 * 6)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = self.wingFrameCounter / num86;
                            }
                        }
                        else
                        {
                            self.wingFrameCounter++;
                            int num87 = 4;
                            if (self.wingFrameCounter >= num87 * 6)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = self.wingFrameCounter / num87;
                        }
                    }
                    else if (self.wings == 39)
                    {
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            int num88 = 4;
                            if (self.wingFrameCounter >= num88 * 6)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = self.wingFrameCounter / num88;
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            if (self.controlJump)
                            {
                                self.wingFrameCounter++;
                                int num89 = 9;
                                if (self.wingFrameCounter >= num89 * 6)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = self.wingFrameCounter / num89;
                            }
                            else
                            {
                                self.wingFrameCounter++;
                                int num90 = 6;
                                if (self.wingFrameCounter >= num90 * 6)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = self.wingFrameCounter / num90;
                            }
                        }
                        else
                        {
                            self.wingFrameCounter++;
                            int num91 = 4;
                            if (self.wingFrameCounter >= num91 * 6)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = self.wingFrameCounter / num91;
                        }
                        int num92 = 1;
                        if (self.wingFrame == 3)
                        {
                            num92 = 5;
                        }
                        if (self.velocity.Y == 0f)
                        {
                            num92 = 0;
                        }
                        Rectangle r = Utils.CenteredRectangle((self.gravDir == 1f) ? (self.Bottom + new Vector2(0f, -10f)) : (self.Top + new Vector2(0f, 10f)), new Vector2(50f, 20f));
                        for (int num93 = 0; num93 < num92; num93++)
                        {
                            Dust dust10 = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, 31, 0f, 0f, 0, Color.Black, 1f);
                            dust10.scale = 0.7f;
                            dust10.velocity *= 0.4f;
                            Dust expr_675B_cp_0 = dust10;
                            expr_675B_cp_0.velocity.Y = expr_675B_cp_0.velocity.Y + self.gravDir * 0.5f;
                            dust10.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                        }
                    }
                    else if (self.wings == 33)
                    {
                        bool flag21 = false;
                        if (flag19 || self.jump > 0)
                        {
                            flag21 = true;
                        }
                        else if (self.velocity.Y != 0f && self.controlJump)
                        {
                            flag21 = true;
                        }
                        if (flag21)
                        {
                            Color newColor = Main.hslToRgb(Main.rgbToHsl(self.eyeColor).X, 1f, 0.5f);
                            int num94 = (self.direction == 1) ? 0 : -4;
                            int num95 = (self.gravDir == 1f) ? self.height : 0;
                            for (int num96 = 0; num96 < 2; num96++)
                            {
                                Dust dust11 = Main.dust[Dust.NewDust(self.position, self.width, self.height, 182, self.velocity.X, self.velocity.Y, 127, newColor, 1f)];
                                dust11.noGravity = true;
                                dust11.fadeIn = 1f;
                                dust11.scale = 1f;
                                dust11.noLight = true;
                                if (num96 == 0)
                                {
                                    dust11.position = new Vector2(self.position.X + (float)num94, self.position.Y + (float)num95);
                                    dust11.velocity.X = dust11.velocity.X * 1f - 2f - self.velocity.X * 0.3f;
                                    dust11.velocity.Y = dust11.velocity.Y * 1f + 2f * self.gravDir - self.velocity.Y * 0.3f;
                                }
                                else if (num96 == 1)
                                {
                                    dust11.position = new Vector2(self.position.X + (float)self.width + (float)num94, self.position.Y + (float)num95);
                                    dust11.velocity.X = dust11.velocity.X * 1f + 2f - self.velocity.X * 0.3f;
                                    dust11.velocity.Y = dust11.velocity.Y * 1f + 2f * self.gravDir - self.velocity.Y * 0.3f;
                                }
                                Dust dust12 = Dust.CloneDust(dust11);
                                dust12.scale *= 0.65f;
                                dust12.fadeIn *= 0.65f;
                                dust12.color = new Color(255, 255, 255, 255);
                                dust11.noLight = true;
                                dust11.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                        }
                    }
                    else if (self.wings == 38)
                    {
                        bool flag22 = false;
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            if (self.wingFrameCounter >= 32)
                            {
                                self.wingFrameCounter = 0;
                            }
                            self.wingFrame = 1 + self.wingFrameCounter / 8;
                            if (self.wingFrame == 4)
                            {
                                self.wingFrame = 2;
                            }
                            flag22 = true;
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            if (self.controlJump)
                            {
                                self.wingFrameCounter++;
                                if (self.wingFrameCounter >= 32)
                                {
                                    self.wingFrameCounter = 0;
                                }
                                self.wingFrame = 1 + self.wingFrameCounter / 8;
                                if (self.wingFrame == 4)
                                {
                                    self.wingFrame = 2;
                                }
                                flag22 = true;
                            }
                            else
                            {
                                self.wingFrame = 0;
                            }
                        }
                        else
                        {
                            self.wingFrame = 0;
                        }
                        if (flag22)
                        {
                            Vector2 value3 = new Vector2((float)self.direction, self.gravDir);
                            Vector2 value4 = self.velocity * 0.5f;
                            int type2 = 267;
                            int num97 = self.miscCounter * self.direction;
                            for (int num98 = 0; num98 < 3; num98++)
                            {
                                Vector2 value5 = Vector2.Zero;
                                switch (num98)
                                {
                                    case 1:
                                        value5 = self.velocity * -0.33f;
                                        break;
                                    case 2:
                                        value5 = self.velocity * -0.66f;
                                        break;
                                }
                                Vector2 value6 = new Vector2(-39f, 6f) * value3 + new Vector2(2f, 0f).RotatedBy((double)((float)num97 / -15f * 6.28318548f), default(Vector2));
                                Dust dust13 = Dust.NewDustPerfect(self.Center + value6 + value5, type2, new Vector2?(value4), 0, self.underShirtColor, 1f);
                                dust13.noGravity = true;
                                dust13.noLight = true;
                                dust13.scale = 0.47f;
                                dust13.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                value6 = new Vector2(-23f, 2f) * value3 + new Vector2(2f, 0f).RotatedBy((double)((float)num97 / -15f * 6.28318548f), default(Vector2));
                                dust13 = Dust.NewDustPerfect(self.Center + value6 + value5, type2, new Vector2?(value4), 0, self.underShirtColor, 1f);
                                dust13.noGravity = true;
                                dust13.noLight = true;
                                dust13.scale = 0.35f;
                                dust13.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                value6 = new Vector2(-31f, -6f) * value3 + new Vector2(2f, 0f).RotatedBy((double)((float)num97 / -20f * 6.28318548f), default(Vector2));
                                dust13 = Dust.NewDustPerfect(self.Center + value6 + value5, type2, new Vector2?(value4), 0, self.underShirtColor, 1f);
                                dust13.noGravity = true;
                                dust13.noLight = true;
                                dust13.scale = 0.49f;
                                dust13.shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                        }
                    }
                    else if (!isCustomWings)
                    {
                        int num99 = 4;
                        if (self.wings == 32)
                        {
                            num99 = 3;
                        }
                        if (flag19 || self.jump > 0)
                        {
                            self.wingFrameCounter++;
                            if (self.wingFrameCounter > num99)
                            {
                                self.wingFrame++;
                                self.wingFrameCounter = 0;
                                if (self.wingFrame >= 4)
                                {
                                    self.wingFrame = 0;
                                }
                            }
                        }
                        else if (self.velocity.Y != 0f)
                        {
                            self.wingFrame = 1;
                            if (self.wings == 32)
                            {
                                self.wingFrame = 3;
                            }
                            if (self.wings == 29 && Main.rand.Next(5) == 0)
                            {
                                int num100 = 4;
                                if (self.direction == 1)
                                {
                                    num100 = -40;
                                }
                                int num101 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num100, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 6, 0f, 0f, 100, default(Color), 2.4f);
                                Main.dust[num101].noGravity = true;
                                Main.dust[num101].velocity *= 0.3f;
                                if (Main.rand.Next(10) == 0)
                                {
                                    Main.dust[num101].fadeIn = 2f;
                                }
                                Main.dust[num101].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                            }
                        }
                        else
                        {
                            self.wingFrame = 0;
                        }
                    }
                    if (self.wingsLogic > 0 && self.rocketBoots > 0 && self.velocity.Y != 0f)
                    {
                        int num102 = 6;
                        self.wingTime += (float)(self.rocketTime * num102);
                        if (self.wingTime > (float)(self.wingTimeMax + self.rocketTimeMax * num102))
                        {
                            self.wingTime = (float)(self.wingTimeMax + self.rocketTimeMax * num102);
                        }
                        self.rocketTime = 0;
                    }
                    if (flag19 && self.wings != 4 && self.wings != 22 && self.wings != 0 && self.wings != 24 && self.wings != 28 && self.wings != 30 && self.wings != 33 && !isCustomWings)
                    {
                        if (self.wingFrame == 3)
                        {
                            if (!self.flapSound)
                            {
                                Main.PlaySound(SoundID.Item32, self.position);
                            }
                            self.flapSound = true;
                        }
                        else
                        {
                            self.flapSound = false;
                        }
                    }
                    if (self.velocity.Y == 0f || self.sliding || (self.autoJump && self.justJumped))
                    {
                        self.rocketTime = self.rocketTimeMax;
                    }
                    if ((self.wingTime == 0f || self.wingsLogic == 0) && self.rocketBoots > 0 && self.controlJump && self.rocketDelay == 0 && self.canRocket && self.rocketRelease && !self.jumpAgainCloud)
                    {
                        if (self.rocketTime > 0)
                        {
                            self.rocketTime--;
                            self.rocketDelay = 10;
                            if (self.rocketDelay2 <= 0)
                            {
                                if (self.rocketBoots == 1)
                                {
                                    Main.PlaySound(SoundID.Item13, self.position);
                                    self.rocketDelay2 = 30;
                                }
                                else if (self.rocketBoots == 2 || self.rocketBoots == 3)
                                {
                                    Main.PlaySound(SoundID.Item24, self.position);
                                    self.rocketDelay2 = 15;
                                }
                            }
                        }
                        else
                        {
                            self.canRocket = false;
                        }
                    }
                    if (self.rocketDelay2 > 0)
                    {
                        self.rocketDelay2--;
                    }
                    if (self.rocketDelay == 0)
                    {
                        self.rocketFrame = false;
                    }
                    if (self.rocketDelay > 0)
                    {
                        int num103 = self.height;
                        if (self.gravDir == -1f)
                        {
                            num103 = 4;
                        }
                        self.rocketFrame = true;
                        for (int num104 = 0; num104 < 2; num104++)
                        {
                            int type3 = 6;
                            float scale2 = 2.5f;
                            int alpha2 = 100;
                            if (self.rocketBoots == 2)
                            {
                                type3 = 16;
                                scale2 = 1.5f;
                                alpha2 = 20;
                            }
                            else if (self.rocketBoots == 3)
                            {
                                type3 = 76;
                                scale2 = 1f;
                                alpha2 = 20;
                            }
                            else if (self.socialShadowRocketBoots)
                            {
                                type3 = 27;
                                scale2 = 1.5f;
                            }
                            if (num104 == 0)
                            {
                                int num105 = Dust.NewDust(new Vector2(self.position.X - 4f, self.position.Y + (float)num103 - 10f), 8, 8, type3, 0f, 0f, alpha2, default(Color), scale2);
                                Main.dust[num105].shader = GameShaders.Armor.GetSecondaryShader(self.cShoe, self);
                                if (self.rocketBoots == 1)
                                {
                                    Main.dust[num105].noGravity = true;
                                }
                                Main.dust[num105].velocity.X = Main.dust[num105].velocity.X * 1f - 2f - self.velocity.X * 0.3f;
                                Main.dust[num105].velocity.Y = Main.dust[num105].velocity.Y * 1f + 2f * self.gravDir - self.velocity.Y * 0.3f;
                                if (self.rocketBoots == 2)
                                {
                                    Main.dust[num105].velocity *= 0.1f;
                                }
                                if (self.rocketBoots == 3)
                                {
                                    Main.dust[num105].velocity *= 0.05f;
                                    Dust expr_73BE_cp_0 = Main.dust[num105];
                                    expr_73BE_cp_0.velocity.Y = expr_73BE_cp_0.velocity.Y + 0.15f;
                                    Main.dust[num105].noLight = true;
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        Main.dust[num105].noGravity = true;
                                        Main.dust[num105].scale = 1.75f;
                                    }
                                }
                            }
                            else
                            {
                                int num106 = Dust.NewDust(new Vector2(self.position.X + (float)self.width - 4f, self.position.Y + (float)num103 - 10f), 8, 8, type3, 0f, 0f, alpha2, default(Color), scale2);
                                Main.dust[num106].shader = GameShaders.Armor.GetSecondaryShader(self.cShoe, self);
                                if (self.rocketBoots == 1)
                                {
                                    Main.dust[num106].noGravity = true;
                                }
                                Main.dust[num106].velocity.X = Main.dust[num106].velocity.X * 1f + 2f - self.velocity.X * 0.3f;
                                Main.dust[num106].velocity.Y = Main.dust[num106].velocity.Y * 1f + 2f * self.gravDir - self.velocity.Y * 0.3f;
                                if (self.rocketBoots == 2)
                                {
                                    Main.dust[num106].velocity *= 0.1f;
                                }
                                if (self.rocketBoots == 3)
                                {
                                    Main.dust[num106].velocity *= 0.05f;
                                    Dust expr_7585_cp_0 = Main.dust[num106];
                                    expr_7585_cp_0.velocity.Y = expr_7585_cp_0.velocity.Y + 0.15f;
                                    Main.dust[num106].noLight = true;
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        Main.dust[num106].noGravity = true;
                                        Main.dust[num106].scale = 1.75f;
                                    }
                                }
                            }
                        }
                        if (self.rocketDelay == 0)
                        {
                            self.releaseJump = true;
                        }
                        self.rocketDelay--;
                        self.velocity.Y = self.velocity.Y - 0.1f * self.gravDir;
                        if (self.gravDir == 1f)
                        {
                            if (self.velocity.Y > 0f)
                            {
                                self.velocity.Y = self.velocity.Y - 0.5f;
                            }
                            else if ((double)self.velocity.Y > (double)(-(double)Player.jumpSpeed) * 0.5)
                            {
                                self.velocity.Y = self.velocity.Y - 0.1f;
                            }
                            if (self.velocity.Y < -Player.jumpSpeed * 1.5f)
                            {
                                self.velocity.Y = -Player.jumpSpeed * 1.5f;
                            }
                        }
                        else
                        {
                            if (self.velocity.Y < 0f)
                            {
                                self.velocity.Y = self.velocity.Y + 0.5f;
                            }
                            else if ((double)self.velocity.Y < (double)Player.jumpSpeed * 0.5)
                            {
                                self.velocity.Y = self.velocity.Y + 0.1f;
                            }
                            if (self.velocity.Y > Player.jumpSpeed * 1.5f)
                            {
                                self.velocity.Y = Player.jumpSpeed * 1.5f;
                            }
                        }
                    }
                    else if (!flag19)
                    {
                        if (self.mount.CanHover)
                        {
                            self.mount.Hover(self);
                        }
                        else if (self.mount.CanFly && self.controlJump && self.jump == 0)
                        {
                            if (self.mount.Flight())
                            {
                                if (self.controlDown)
                                {
                                    self.velocity.Y = self.velocity.Y * 0.9f;
                                    if (self.velocity.Y > -1f && (double)self.velocity.Y < 0.5)
                                    {
                                        self.velocity.Y = 1E-05f;
                                    }
                                }
                                else
                                {
                                    if (self.velocity.Y > 0f)
                                    {
                                        self.velocity.Y = self.velocity.Y - 0.5f;
                                    }
                                    else if ((double)self.velocity.Y > (double)(-(double)Player.jumpSpeed) * 1.5)
                                    {
                                        self.velocity.Y = self.velocity.Y - 0.1f;
                                    }
                                    if (self.velocity.Y < -Player.jumpSpeed * 1.5f)
                                    {
                                        self.velocity.Y = -Player.jumpSpeed * 1.5f;
                                    }
                                }
                            }
                            else
                            {
                                self.velocity.Y = self.velocity.Y + self.gravity / 3f * self.gravDir;
                                if (self.gravDir == 1f)
                                {
                                    if (self.velocity.Y > self.maxFallSpeed / 3f && !self.controlDown)
                                    {
                                        self.velocity.Y = self.maxFallSpeed / 3f;
                                    }
                                }
                                else if (self.velocity.Y < -self.maxFallSpeed / 3f && !self.controlUp)
                                {
                                    self.velocity.Y = -self.maxFallSpeed / 3f;
                                }
                            }
                        }
                        else if (self.slowFall && ((!self.controlDown && self.gravDir == 1f) || (!self.controlDown && self.gravDir == -1f)))
                        {
                            if ((self.controlUp && self.gravDir == 1f) || (self.controlUp && self.gravDir == -1f))
                            {
                                self.gravity = self.gravity / 10f * self.gravDir;
                            }
                            else
                            {
                                self.gravity = self.gravity / 3f * self.gravDir;
                            }
                            self.velocity.Y = self.velocity.Y + self.gravity;
                        }
                        else if (self.wingsLogic > 0 && self.controlJump && self.velocity.Y > 0f)
                        {
                            self.fallStart = (int)(self.position.Y / 16f);
                            if (self.velocity.Y > 0f)
                            {
                                if (self.wings == 10 && Main.rand.Next(3) == 0)
                                {
                                    int num107 = 4;
                                    if (self.direction == 1)
                                    {
                                        num107 = -40;
                                    }
                                    int num108 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num107, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 76, 0f, 0f, 50, default(Color), 0.6f);
                                    Main.dust[num108].fadeIn = 1.1f;
                                    Main.dust[num108].noGravity = true;
                                    Main.dust[num108].noLight = true;
                                    Main.dust[num108].velocity *= 0.3f;
                                    Main.dust[num108].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (self.wings == 34 && Main.rand.Next(3) == 0)
                                {
                                    int num109 = 4;
                                    if (self.direction == 1)
                                    {
                                        num109 = -40;
                                    }
                                    int num110 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num109, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 261, 0f, 0f, 50, default(Color), 0.6f);
                                    Main.dust[num110].fadeIn = 1.1f;
                                    Main.dust[num110].noGravity = true;
                                    Main.dust[num110].noLight = true;
                                    Main.dust[num110].velocity *= 0.3f;
                                    Main.dust[num110].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (self.wings == 9 && Main.rand.Next(3) == 0)
                                {
                                    int num111 = 8;
                                    if (self.direction == 1)
                                    {
                                        num111 = -40;
                                    }
                                    int num112 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num111, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 6, 0f, 0f, 200, default(Color), 2f);
                                    Main.dust[num112].noGravity = true;
                                    Main.dust[num112].velocity *= 0.3f;
                                    Main.dust[num112].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (self.wings == 29 && Main.rand.Next(3) == 0)
                                {
                                    int num113 = 8;
                                    if (self.direction == 1)
                                    {
                                        num113 = -40;
                                    }
                                    int num114 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num113, self.position.Y + (float)(self.height / 2) - 15f), 30, 30, 6, 0f, 0f, 100, default(Color), 2.4f);
                                    Main.dust[num114].noGravity = true;
                                    Main.dust[num114].velocity *= 0.3f;
                                    if (Main.rand.Next(10) == 0)
                                    {
                                        Main.dust[num114].fadeIn = 2f;
                                    }
                                    Main.dust[num114].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (self.wings == 6)
                                {
                                    if (Main.rand.Next(10) == 0)
                                    {
                                        int num115 = 4;
                                        if (self.direction == 1)
                                        {
                                            num115 = -40;
                                        }
                                        int num116 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num115, self.position.Y + (float)(self.height / 2) - 12f), 30, 20, 55, 0f, 0f, 200, default(Color), 1f);
                                        Main.dust[num116].velocity *= 0.3f;
                                        Main.dust[num116].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                    }
                                }
                                else if (self.wings == 5 && Main.rand.Next(6) == 0)
                                {
                                    int num117 = 6;
                                    if (self.direction == 1)
                                    {
                                        num117 = -30;
                                    }
                                    int num118 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num117, self.position.Y), 18, self.height, 58, 0f, 0f, 255, default(Color), 1.2f);
                                    Main.dust[num118].velocity *= 0.3f;
                                    Main.dust[num118].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                }
                                if (self.wings == 4)
                                {
                                    self.rocketDelay2--;
                                    if (self.rocketDelay2 <= 0)
                                    {
                                        Main.PlaySound(SoundID.Item13, self.position);
                                        self.rocketDelay2 = 60;
                                    }
                                    int type4 = 6;
                                    float scale3 = 1.5f;
                                    int alpha3 = 100;
                                    float x4 = self.position.X + (float)(self.width / 2) + 16f;
                                    if (self.direction > 0)
                                    {
                                        x4 = self.position.X + (float)(self.width / 2) - 26f;
                                    }
                                    float num119 = self.position.Y + (float)self.height - 18f;
                                    if (Main.rand.Next(2) == 1)
                                    {
                                        x4 = self.position.X + (float)(self.width / 2) + 8f;
                                        if (self.direction > 0)
                                        {
                                            x4 = self.position.X + (float)(self.width / 2) - 20f;
                                        }
                                        num119 += 6f;
                                    }
                                    int num120 = Dust.NewDust(new Vector2(x4, num119), 8, 8, type4, 0f, 0f, alpha3, default(Color), scale3);
                                    Dust expr_811F_cp_0 = Main.dust[num120];
                                    expr_811F_cp_0.velocity.X = expr_811F_cp_0.velocity.X * 0.3f;
                                    Dust expr_813D_cp_0 = Main.dust[num120];
                                    expr_813D_cp_0.velocity.Y = expr_813D_cp_0.velocity.Y + 10f;
                                    Main.dust[num120].noGravity = true;
                                    Main.dust[num120].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                    self.wingFrameCounter++;
                                    if (self.wingFrameCounter > 4)
                                    {
                                        self.wingFrame++;
                                        self.wingFrameCounter = 0;
                                        if (self.wingFrame >= 3)
                                        {
                                            self.wingFrame = 0;
                                        }
                                    }
                                }
                                else if (self.wings != 22 && self.wings != 28 && !isCustomWings)
                                {
                                    if (self.wings == 30)
                                    {
                                        self.wingFrameCounter++;
                                        int num121 = 5;
                                        if (self.wingFrameCounter >= num121 * 3)
                                        {
                                            self.wingFrameCounter = 0;
                                        }
                                        self.wingFrame = 1 + self.wingFrameCounter / num121;
                                    }
                                    else if (self.wings == 34)
                                    {
                                        self.wingFrameCounter++;
                                        int num122 = 7;
                                        if (self.wingFrameCounter >= num122 * 6)
                                        {
                                            self.wingFrameCounter = 0;
                                        }
                                        self.wingFrame = self.wingFrameCounter / num122;
                                    }
                                    else if (self.wings == 39)
                                    {
                                        self.wingFrameCounter++;
                                        int num123 = 12;
                                        if (self.wingFrameCounter >= num123 * 6)
                                        {
                                            self.wingFrameCounter = 0;
                                        }
                                        self.wingFrame = self.wingFrameCounter / num123;
                                    }
                                    else if (self.wings == 26)
                                    {
                                        int num124 = 6;
                                        if (self.direction == 1)
                                        {
                                            num124 = -30;
                                        }
                                        int num125 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num124, self.position.Y), 18, self.height, 217, 0f, 0f, 100, default(Color), 1.4f);
                                        Main.dust[num125].noGravity = true;
                                        Main.dust[num125].noLight = true;
                                        Main.dust[num125].velocity /= 4f;
                                        Main.dust[num125].velocity -= self.velocity;
                                        Main.dust[num125].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            num124 = -24;
                                            if (self.direction == 1)
                                            {
                                                num124 = 12;
                                            }
                                            float num126 = self.position.Y;
                                            if (self.gravDir == -1f)
                                            {
                                                num126 += (float)(self.height / 2);
                                            }
                                            num125 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num124, num126), 12, self.height / 2, 217, 0f, 0f, 100, default(Color), 1.4f);
                                            Main.dust[num125].noGravity = true;
                                            Main.dust[num125].noLight = true;
                                            Main.dust[num125].velocity /= 4f;
                                            Main.dust[num125].velocity -= self.velocity;
                                            Main.dust[num125].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                        }
                                        self.wingFrame = 2;
                                    }
                                    else if (self.wings == 37)
                                    {
                                        Color color = Color.Lerp(Color.Black, Color.White, Main.rand.NextFloat());
                                        int num127 = 6;
                                        if (self.direction == 1)
                                        {
                                            num127 = -30;
                                        }
                                        int num128 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num127, self.position.Y), 24, self.height, Utils.SelectRandom<int>(Main.rand, new int[]
                                                {
                                                    31,
                                                    31,
                                                    31
                                                }), 0f, 0f, 100, default(Color), 0.7f);
                                        Main.dust[num128].noGravity = true;
                                        Main.dust[num128].noLight = true;
                                        Main.dust[num128].velocity /= 4f;
                                        Main.dust[num128].velocity -= self.velocity;
                                        Main.dust[num128].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                        if (Main.dust[num128].type == 55)
                                        {
                                            Main.dust[num128].color = color;
                                        }
                                        if (Main.rand.Next(3) == 0)
                                        {
                                            num127 = -24;
                                            if (self.direction == 1)
                                            {
                                                num127 = 12;
                                            }
                                            float num129 = self.position.Y;
                                            if (self.gravDir == -1f)
                                            {
                                                num129 += (float)(self.height / 2);
                                            }
                                            num128 = Dust.NewDust(new Vector2(self.position.X + (float)(self.width / 2) + (float)num127, num129), 12, self.height / 2, Utils.SelectRandom<int>(Main.rand, new int[]
                                                    {
                                                        31,
                                                        31,
                                                        31
                                                    }), 0f, 0f, 140, default(Color), 0.7f);
                                            Main.dust[num128].noGravity = true;
                                            Main.dust[num128].noLight = true;
                                            Main.dust[num128].velocity /= 4f;
                                            Main.dust[num128].velocity -= self.velocity;
                                            Main.dust[num128].shader = GameShaders.Armor.GetSecondaryShader(self.cWings, self);
                                            if (Main.dust[num128].type == 55)
                                            {
                                                Main.dust[num128].color = color;
                                            }
                                        }
                                        self.wingFrame = 2;
                                    }
                                    else if (self.wings != 24)
                                    {
                                        if (self.wings == 12)
                                        {
                                            self.wingFrame = 3;
                                        }
                                        else
                                        {
                                            self.wingFrame = 2;
                                        }
                                    }
                                }
                            }
                            self.velocity.Y = self.velocity.Y + self.gravity / 3f * self.gravDir;
                            if (self.gravDir == 1f)
                            {
                                if (self.velocity.Y > self.maxFallSpeed / 3f && !self.controlDown)
                                {
                                    self.velocity.Y = self.maxFallSpeed / 3f;
                                }
                            }
                            else if (self.velocity.Y < -self.maxFallSpeed / 3f && !self.controlUp)
                            {
                                self.velocity.Y = -self.maxFallSpeed / 3f;
                            }
                        }
                        else if (self.cartRampTime <= 0)
                        {
                            self.velocity.Y = self.velocity.Y + self.gravity * self.gravDir;
                        }
                        else
                        {
                            self.cartRampTime--;
                        }
                    }
                    if (!self.mount.Active || self.mount.Type != 5)
                    {
                        if (self.gravDir == 1f)
                        {
                            if (self.velocity.Y > self.maxFallSpeed)
                            {
                                self.velocity.Y = self.maxFallSpeed;
                            }
                            if (self.slowFall && self.velocity.Y > self.maxFallSpeed / 3f && !self.controlDown)
                            {
                                self.velocity.Y = self.maxFallSpeed / 3f;
                            }
                            if (self.slowFall && self.velocity.Y > self.maxFallSpeed / 5f && self.controlUp)
                            {
                                self.velocity.Y = self.maxFallSpeed / 10f;
                            }
                        }
                        else
                        {
                            if (self.velocity.Y < -self.maxFallSpeed)
                            {
                                self.velocity.Y = -self.maxFallSpeed;
                            }
                            if (self.slowFall && self.velocity.Y < -self.maxFallSpeed / 3f && !self.controlDown)
                            {
                                self.velocity.Y = -self.maxFallSpeed / 3f;
                            }
                            if (self.slowFall && self.velocity.Y < -self.maxFallSpeed / 5f && self.controlUp)
                            {
                                self.velocity.Y = -self.maxFallSpeed / 10f;
                            }
                        }
                    }
                }
            }
            if (self.mount.Active)
            {
                self.wingFrame = 0;
            }
            if ((self.wingsLogic == 22 || self.wingsLogic == 28 || self.wingsLogic == 30 || self.wingsLogic == 31 || self.wingsLogic == 33 || self.wingsLogic == 35 || self.wingsLogic == 37) && self.controlDown && self.controlJump && self.wingTime > 0f && !self.merman)
            {
                self.velocity.Y = self.velocity.Y * 0.9f;
                if (self.velocity.Y > -2f && self.velocity.Y < 1f)
                {
                    self.velocity.Y = 1E-05f;
                }
            }
            if (self.wingsLogic == 37 && self.controlDown && self.controlJump && self.wingTime > 0f && !self.merman)
            {
                self.velocity.Y = self.velocity.Y * 0.92f;
                if (self.velocity.Y > -2f && self.velocity.Y < 1f)
                {
                    self.velocity.Y = 1E-05f;
                }
            }

            MethodInfo GrabItems =
                typeof(Player).GetMethod("GrabItems", BindingFlags.Instance | BindingFlags.NonPublic);

            Object[] obj = {i};


            GrabItems.Invoke(self, obj);
            MethodInfo LookForTileInteractions = typeof(Player).GetMethod("LookForTileInteractions", BindingFlags.Instance | BindingFlags.NonPublic);
            LookForTileInteractions.Invoke(self, null);
            if (self.tongued)
            {
                bool flag23 = false;
                if (Main.wof >= 0)
                {
                    float num130 = Main.npc[Main.wof].position.X + (float)(Main.npc[Main.wof].width / 2);
                    num130 += (float)(Main.npc[Main.wof].direction * 200);
                    float num131 = Main.npc[Main.wof].position.Y + (float)(Main.npc[Main.wof].height / 2);
                    Vector2 center = self.Center;
                    float num132 = num130 - center.X;
                    float num133 = num131 - center.Y;
                    float num134 = (float)Math.Sqrt((double)(num132 * num132 + num133 * num133));
                    float num135 = 11f;
                    float num136;
                    if (num134 > num135)
                    {
                        num136 = num135 / num134;
                    }
                    else
                    {
                        num136 = 1f;
                        flag23 = true;
                    }
                    num132 *= num136;
                    num133 *= num136;
                    self.velocity.X = num132;
                    self.velocity.Y = num133;
                }
                else
                {
                    flag23 = true;
                }
                if (flag23 && Main.myPlayer == self.whoAmI)
                {
                    for (int num137 = 0; num137 < 22; num137++)
                    {
                        if (self.buffType[num137] == 38)
                        {
                            self.DelBuff(num137);
                        }
                    }
                }
            }
            if (Main.myPlayer == self.whoAmI)
            {
                self.WOFTongue();
                if (self.controlHook)
                {
                    if (self.releaseHook)
                    {
                        self.QuickGrapple();
                    }
                    self.releaseHook = false;
                }
                else
                {
                    self.releaseHook = true;
                }
                if (self.talkNPC >= 0)
                {
                    Rectangle rectangle = new Rectangle((int)(self.position.X + (float)(self.width / 2) - (float)(Player.tileRangeX * 16)), (int)(self.position.Y + (float)(self.height / 2) - (float)(Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
                    Rectangle value7 = new Rectangle((int)Main.npc[self.talkNPC].position.X, (int)Main.npc[self.talkNPC].position.Y, Main.npc[self.talkNPC].width, Main.npc[self.talkNPC].height);
                    if (!rectangle.Intersects(value7) || self.chest != -1 || !Main.npc[self.talkNPC].active)
                    {
                        if (self.chest == -1)
                        {
                            Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                        }
                        self.talkNPC = -1;
                        Main.npcChatCornerItem = 0;
                        Main.npcChatText = "";
                    }
                }
                if (self.sign >= 0)
                {
                    Rectangle value8 = new Rectangle((int)(self.position.X + (float)(self.width / 2) - (float)(Player.tileRangeX * 16)), (int)(self.position.Y + (float)(self.height / 2) - (float)(Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
                    try
                    {
                        bool flag24 = false;
                        if (Main.sign[self.sign] == null)
                        {
                            flag24 = true;
                        }
                        if (!flag24 && !new Rectangle(Main.sign[self.sign].x * 16, Main.sign[self.sign].y * 16, 32, 32).Intersects(value8))
                        {
                            flag24 = true;
                        }
                        if (flag24)
                        {
                            Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                            self.sign = -1;
                            Main.editSign = false;
                            Main.npcChatText = "";
                        }
                    }
                    catch
                    {
                        Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                        self.sign = -1;
                        Main.editSign = false;
                        Main.npcChatText = "";
                    }
                }
                if (Main.editSign)
                {
                    if (self.sign == -1)
                    {
                        Main.editSign = false;
                    }
                    else
                    {
                        Main.InputTextSign();
                    }
                }
                else if (Main.editChest)
                {
                    Main.InputTextChest();
                    if (Main.player[Main.myPlayer].chest == -1)
                    {
                        Main.editChest = false;
                    }
                }
                if (self.mount.Active && self.mount.Cart && Math.Abs(self.velocity.X) > 4f)
                {
                    Rectangle rectangle2 = new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height);
                    for (int num138 = 0; num138 < 200; num138++)
                    {
                        if (Main.npc[num138].active && !Main.npc[num138].dontTakeDamage && !Main.npc[num138].friendly && Main.npc[num138].immune[i] == 0 && rectangle2.Intersects(new Rectangle((int)Main.npc[num138].position.X, (int)Main.npc[num138].position.Y, Main.npc[num138].width, Main.npc[num138].height)))
                        {
                            float num139 = (float)self.meleeCrit;
                            if (num139 < (float)self.rangedCrit)
                            {
                                num139 = (float)self.rangedCrit;
                            }
                            if (num139 < (float)self.magicCrit)
                            {
                                num139 = (float)self.magicCrit;
                            }
                            bool crit = false;
                            if ((float)Main.rand.Next(1, 101) <= num139)
                            {
                                crit = true;
                            }
                            float num140 = Math.Abs(self.velocity.X) / self.maxRunSpeed;
                            int damage2 = Main.DamageVar(25f + 55f * num140);
                            if (self.mount.Type == 11)
                            {
                                damage2 = Main.DamageVar(50f + 100f * num140);
                            }
                            if (self.mount.Type == 13)
                            {
                                damage2 = Main.DamageVar(15f + 30f * num140);
                            }
                            float knockback = 5f + 25f * num140;
                            int direction = 1;
                            if (self.velocity.X < 0f)
                            {
                                direction = -1;
                            }
                            if (self.whoAmI == Main.myPlayer)
                            {
                                self.ApplyDamageToNPC(Main.npc[num138], damage2, knockback, direction, crit);
                            }
                            Main.npc[num138].immune[i] = 30;
                            if (!Main.npc[num138].active)
                            {
                                AchievementsHelper.HandleSpecialEvent(self, 9);
                            }
                        }
                    }
                }

                MethodInfo Update_NPCCollision = typeof(Player).GetMethod("Update_NPCCollision",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Update_NPCCollision.Invoke(self, null);

                Vector2 vector3;
                if (!self.mount.Active || !self.mount.Cart)
                {
                    vector3 = Collision.HurtTiles(self.position, self.velocity, self.width, self.height, self.fireWalk);
                }
                else
                {
                    vector3 = Collision.HurtTiles(self.position, self.velocity, self.width, self.height - 16, self.fireWalk);
                }
                if (vector3.Y == 0f && !self.fireWalk)
                {
                    for (int i1 = 0; i1 < self.TouchedTiles.Count; i1++)
                    {
                        Point current = self.TouchedTiles[i1];
                        Tile tile = Main.tile[current.X, current.Y];
                        if (tile != null && tile.active() && tile.nactive() && !self.fireWalk && TileID.Sets.TouchDamageHot[(int)tile.type] != 0)
                        {
                            vector3.Y = (float)TileID.Sets.TouchDamageHot[(int)tile.type];
                            vector3.X = (float)((self.Center.X / 16f < (float)current.X + 0.5f) ? -1 : 1);
                            break;
                        }
                    }
                }
                if (vector3.Y == 20f)
                {
                    self.AddBuff(67, 20, true);
                }
                else if (vector3.Y == 15f)
                {
                    if (self.suffocateDelay < 5)
                    {
                        self.suffocateDelay += 1;
                    }
                    else
                    {
                        self.AddBuff(68, 1, true);
                    }
                }
                else if (vector3.Y != 0f)
                {
                    int damage3 = Main.DamageVar(vector3.Y);
                    self.Hurt(PlayerDeathReason.ByOther(3), damage3, 0, false, false, false, 0);
                }
                else
                {
                    self.suffocateDelay = 0;
                }
            }
            if (self.controlRight)
            {
                self.releaseRight = false;
            }
            else
            {
                self.releaseRight = true;
                self.rightTimer = 7;
            }
            if (self.controlLeft)
            {
                self.releaseLeft = false;
            }
            else
            {
                self.releaseLeft = true;
                self.leftTimer = 7;
            }
            self.releaseDown = !self.controlDown;
            if (self.rightTimer > 0)
            {
                self.rightTimer--;
            }
            else if (self.controlRight)
            {
                self.rightTimer = 7;
            }
            if (self.leftTimer > 0)
            {
                self.leftTimer--;
            }
            else if (self.controlLeft)
            {
                self.leftTimer = 7;
            }
            self.GrappleMovement();
            self.StickyMovement();
            self.CheckDrowning();
            if (self.gravDir == -1f)
            {
                self.waterWalk = false;
                self.waterWalk2 = false;
            }
            int num141 = self.height;
            if (self.waterWalk)
            {
                num141 -= 6;
            }
            bool flag25 = Collision.LavaCollision(self.position, self.width, num141);
            if (flag25)
            {
                if (!self.lavaImmune && Main.myPlayer == i && !self.immune)
                {
                    if (self.lavaTime > 0)
                    {
                        self.lavaTime--;
                    }
                    else if (self.lavaRose)
                    {
                        self.Hurt(PlayerDeathReason.ByOther(2), 50, 0, false, false, false, -1);
                        self.AddBuff(24, 210, true);
                    }
                    else
                    {
                        self.Hurt(PlayerDeathReason.ByOther(2), 80, 0, false, false, false, -1);
                        self.AddBuff(24, 420, true);
                    }
                }
                self.lavaWet = true;
            }
            else
            {
                self.lavaWet = false;
                if (self.lavaTime < self.lavaMax)
                {
                    self.lavaTime++;
                }
            }
            if (self.lavaTime > self.lavaMax)
            {
                self.lavaTime = self.lavaMax;
            }
            if (self.waterWalk2 && !self.waterWalk)
            {
                num141 -= 6;
            }
            bool flag26 = Collision.WetCollision(self.position, self.width, self.height);
            bool flag27 = Collision.honey;
            
            //Interaction Hook
            if (flag27)
            {
                self.AddBuff(48, 1800, true);
                self.honeyWet = true;
            }
            if (flag26)
            {

                
                if (self.onFire && !self.lavaWet)
                {
                    for (int num142 = 0; num142 < 22; num142++)
                    {
                        if (self.buffType[num142] == 24)
                        {
                            self.DelBuff(num142);
                        }
                    }
                }
                if (!self.wet)
                {
                    if (self.wetCount == 0)
                    {
                        self.wetCount = 10;
                        if (!flag25)
                        {
                            if (self.honeyWet)
                            {
                                for (int num143 = 0; num143 < 20; num143++)
                                {
                                    int num144 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2) - 8f), self.width + 12, 24, 152, 0f, 0f, 0, default(Color), 1f);
                                    Dust expr_967B_cp_0 = Main.dust[num144];
                                    expr_967B_cp_0.velocity.Y = expr_967B_cp_0.velocity.Y - 1f;
                                    Dust expr_9699_cp_0 = Main.dust[num144];
                                    expr_9699_cp_0.velocity.X = expr_9699_cp_0.velocity.X * 2.5f;
                                    Main.dust[num144].scale = 1.3f;
                                    Main.dust[num144].alpha = 100;
                                    Main.dust[num144].noGravity = true;
                                }
                                Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 1, 1f, 0f);
                            }
                            else
                            {
                                for (int num145 = 0; num145 < 50; num145++)
                                {
                                    int num146 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2) - 8f), self.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
                                    Dust expr_9793_cp_0 = Main.dust[num146];
                                    expr_9793_cp_0.velocity.Y = expr_9793_cp_0.velocity.Y - 3f;
                                    Dust expr_97B1_cp_0 = Main.dust[num146];
                                    expr_97B1_cp_0.velocity.X = expr_97B1_cp_0.velocity.X * 2.5f;
                                    Main.dust[num146].scale = 0.8f;
                                    Main.dust[num146].alpha = 100;
                                    Main.dust[num146].noGravity = true;
                                }
                                Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 0, 1f, 0f);
                            }
                        }
                        else
                        {
                            for (int num147 = 0; num147 < 20; num147++)
                            {
                                int num148 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2) - 8f), self.width + 12, 24, 35, 0f, 0f, 0, default(Color), 1f);
                                Dust expr_98A8_cp_0 = Main.dust[num148];
                                expr_98A8_cp_0.velocity.Y = expr_98A8_cp_0.velocity.Y - 1.5f;
                                Dust expr_98C6_cp_0 = Main.dust[num148];
                                expr_98C6_cp_0.velocity.X = expr_98C6_cp_0.velocity.X * 2.5f;
                                Main.dust[num148].scale = 1.3f;
                                Main.dust[num148].alpha = 100;
                                Main.dust[num148].noGravity = true;
                            }
                            Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 1, 1f, 0f);
                        }
                    }
                    self.wet = true;
                }
            }
            else if (self.wet)
            {
                self.wet = false;
                if (self.jump > Player.jumpHeight / 5 && self.wetSlime == 0)
                {
                    self.jump = Player.jumpHeight / 5;
                }
                if (self.wetCount == 0)
                {
                    self.wetCount = 10;
                    if (!self.lavaWet)
                    {
                        if (self.honeyWet)
                        {
                            for (int num149 = 0; num149 < 20; num149++)
                            {
                                int num150 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2) - 8f), self.width + 12, 24, 152, 0f, 0f, 0, default(Color), 1f);
                                Dust expr_9A26_cp_0 = Main.dust[num150];
                                expr_9A26_cp_0.velocity.Y = expr_9A26_cp_0.velocity.Y - 1f;
                                Dust expr_9A44_cp_0 = Main.dust[num150];
                                expr_9A44_cp_0.velocity.X = expr_9A44_cp_0.velocity.X * 2.5f;
                                Main.dust[num150].scale = 1.3f;
                                Main.dust[num150].alpha = 100;
                                Main.dust[num150].noGravity = true;
                            }
                            Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 1, 1f, 0f);
                        }
                        else
                        {
                            for (int num151 = 0; num151 < 50; num151++)
                            {
                                int num152 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2)), self.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
                                Dust expr_9B38_cp_0 = Main.dust[num152];
                                expr_9B38_cp_0.velocity.Y = expr_9B38_cp_0.velocity.Y - 4f;
                                Dust expr_9B56_cp_0 = Main.dust[num152];
                                expr_9B56_cp_0.velocity.X = expr_9B56_cp_0.velocity.X * 2.5f;
                                Main.dust[num152].scale = 0.8f;
                                Main.dust[num152].alpha = 100;
                                Main.dust[num152].noGravity = true;
                            }
                            Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 0, 1f, 0f);
                        }
                    }
                    else
                    {
                        for (int num153 = 0; num153 < 20; num153++)
                        {
                            int num154 = Dust.NewDust(new Vector2(self.position.X - 6f, self.position.Y + (float)(self.height / 2) - 8f), self.width + 12, 24, 35, 0f, 0f, 0, default(Color), 1f);
                            Dust expr_9C4D_cp_0 = Main.dust[num154];
                            expr_9C4D_cp_0.velocity.Y = expr_9C4D_cp_0.velocity.Y - 1.5f;
                            Dust expr_9C6B_cp_0 = Main.dust[num154];
                            expr_9C6B_cp_0.velocity.X = expr_9C6B_cp_0.velocity.X * 2.5f;
                            Main.dust[num154].scale = 1.3f;
                            Main.dust[num154].alpha = 100;
                            Main.dust[num154].noGravity = true;
                        }
                        Main.PlaySound(19, (int)self.position.X, (int)self.position.Y, 1, 1f, 0f);
                    }
                }
            }
            if (!flag27)
            {
                self.honeyWet = false;
            }
            if (!self.wet)
            {
                self.lavaWet = false;
                self.honeyWet = false;
            }
            if (self.wetCount > 0)
            {
                self.wetCount -= 1;
            }
            if (self.wetSlime > 0)
            {
                self.wetSlime -= 1;
            }
            if (self.wet && self.mount.Active)
            {
                switch (self.mount.Type)
                {
                    case 3:
                        self.wetSlime = 30;
                        if (self.velocity.Y > 2f)
                        {
                            self.velocity.Y = self.velocity.Y * 0.9f;
                        }
                        self.velocity.Y = self.velocity.Y - 0.5f;
                        if (self.velocity.Y < -4f)
                        {
                            self.velocity.Y = -4f;
                        }
                        break;
                    case 5:
                    case 7:
                        if (self.whoAmI == Main.myPlayer)
                        {
                            self.mount.Dismount(self);
                        }
                        break;
                }
            }
            if (Main.expertMode && self.ZoneSnow && self.wet && !self.lavaWet && !self.honeyWet && !self.arcticDivingGear)
            {
                self.AddBuff(46, 150, true);
            }
            float num155 = 1f + Math.Abs(self.velocity.X) / 3f;
            if (self.gfxOffY > 0f)
            {
                self.gfxOffY -= num155 * self.stepSpeed;
                if (self.gfxOffY < 0f)
                {
                    self.gfxOffY = 0f;
                }
            }
            else if (self.gfxOffY < 0f)
            {
                self.gfxOffY += num155 * self.stepSpeed;
                if (self.gfxOffY > 0f)
                {
                    self.gfxOffY = 0f;
                }
            }
            if (self.gfxOffY > 32f)
            {
                self.gfxOffY = 32f;
            }
            if (self.gfxOffY < -32f)
            {
                self.gfxOffY = -32f;
            }
            if (Main.myPlayer == i && !self.iceSkate)
            {
                self.CheckIceBreak();
            }
            self.SlopeDownMovement();
            bool flag28 = self.mount.Type == 7 || self.mount.Type == 8 || self.mount.Type == 12;
            if (self.velocity.Y == self.gravity && (!self.mount.Active || (!self.mount.Cart && !flag28)))
            {
                Collision.StepDown(ref self.position, ref self.velocity, self.width, self.height, ref self.stepSpeed, ref self.gfxOffY, (int)self.gravDir, self.waterWalk || self.waterWalk2);
            }
            if (self.gravDir == -1f)
            {
                if ((self.carpetFrame != -1 || self.velocity.Y <= self.gravity) && !self.controlUp)
                {
                    Collision.StepUp(ref self.position, ref self.velocity, self.width, self.height, ref self.stepSpeed, ref self.gfxOffY, (int)self.gravDir, self.controlUp, 0);
                }
            }
            else if (flag28 || ((self.carpetFrame != -1 || self.velocity.Y >= self.gravity) && !self.controlDown && !self.mount.Cart))
            {
                Collision.StepUp(ref self.position, ref self.velocity, self.width, self.height, ref self.stepSpeed, ref self.gfxOffY, (int)self.gravDir, self.controlUp, 0);
            }
            self.oldPosition = self.position;
            self.oldDirection = self.direction;
            bool falling = false;
            if (self.velocity.Y > self.gravity)
            {
                falling = true;
            }
            if (self.velocity.Y < -self.gravity)
            {
                falling = true;
            }
            Vector2 velocity = self.velocity;
            self.slideDir = 0;
            bool ignorePlats = false;
            bool fallThrough = self.controlDown;
            if (self.gravDir == -1f || (self.mount.Active && self.mount.Cart) || self.GoingDownWithGrapple)
            {
                ignorePlats = true;
                fallThrough = true;
            }
            self.onTrack = false;
            bool flag29 = false;
            if (self.mount.Active && self.mount.Cart)
            {
                float num156;
                if (!self.ignoreWater && !self.merman)
                {
                    if (self.honeyWet)
                    {
                        num156 = 0.25f;
                    }
                    else if (self.wet)
                    {
                        num156 = 0.5f;
                    }
                    else
                    {
                        num156 = 1f;
                    }
                }
                else
                {
                    num156 = 1f;
                }
                self.velocity *= num156;
                DelegateMethods.Minecart.rotation = self.fullRotation;
                DelegateMethods.Minecart.rotationOrigin = self.fullRotationOrigin;
                BitsByte bitsByte = Minecart.TrackCollision(ref self.position, ref self.velocity, ref self.lastBoost, self.width, self.height, self.controlDown, self.controlUp, self.fallStart2, false, self.mount.MinecartDust);
                if (bitsByte[0])
                {
                    self.onTrack = true;
                    self.gfxOffY = Minecart.TrackRotation(ref self.fullRotation, self.position + self.velocity, self.width, self.height, self.controlDown, self.controlUp, self.mount.MinecartDust);
                    self.fullRotationOrigin = new Vector2((float)(self.width / 2), (float)self.height);
                }
                if (bitsByte[1])
                {
                    if (self.controlLeft || self.controlRight)
                    {
                        if (self.cartFlip)
                        {
                            self.cartFlip = false;
                        }
                        else
                        {
                            self.cartFlip = true;
                        }
                    }
                    if (self.velocity.X > 0f)
                    {
                        self.direction = 1;
                    }
                    else if (self.velocity.X < 0f)
                    {
                        self.direction = -1;
                    }
                    Main.PlaySound(SoundID.Item56, (int)self.position.X + self.width / 2, (int)self.position.Y + self.height / 2);
                }
                self.velocity /= num156;
                if (bitsByte[3] && self.whoAmI == Main.myPlayer)
                {
                    flag29 = true;
                }
                if (bitsByte[2])
                {
                    self.cartRampTime = (int)(Math.Abs(self.velocity.X) / self.mount.RunSpeed * 20f);
                }
                if (bitsByte[4])
                {
                    self.trackBoost -= 4f;
                }
                if (bitsByte[5])
                {
                    self.trackBoost += 4f;
                }
            }
            bool flag30 = self.whoAmI == Main.myPlayer && !self.mount.Active;
            Vector2 position = self.position;
            if (self.vortexDebuff)
            {
                self.velocity.Y = self.velocity.Y * 0.8f + (float)Math.Cos((double)(self.Center.X % 120f / 120f * 6.28318548f)) * 5f * 0.2f;
            }
            PlayerHooks.PreUpdateMovement(self);
            if (self.tongued)
            {
                self.position += self.velocity;
                flag30 = false;
            }
            else if (self.honeyWet && !self.ignoreWater)
            {
                self.HoneyCollision(fallThrough, ignorePlats);
            }
            else if (self.wet && !self.merman && !self.ignoreWater)
            {
                self.WaterCollision(fallThrough, ignorePlats);
            }
            else
            {
                self.DryCollision(fallThrough, ignorePlats);
                if (self.mount.Active && self.mount.Type == 3 && self.velocity.Y != 0f && !self.SlimeDontHyperJump)
                {
                    Vector2 velocity2 = self.velocity;
                    self.velocity.X = 0f;
                    self.DryCollision(fallThrough, ignorePlats);
                    self.velocity.X = velocity2.X;
                }
            }
            self.UpdateTouchingTiles();

            MethodInfo TryBouncingBlocks =
                typeof(Player).GetMethod("TryBouncingBlocks", BindingFlags.Instance | BindingFlags.NonPublic);
            TryBouncingBlocks.Invoke(self, new Object[]{falling});

            MethodInfo TryLandingOnDetonator =
                typeof(Player).GetMethod("TryLandingOnDetonator", BindingFlags.Instance | BindingFlags.NonPublic);
            TryLandingOnDetonator.Invoke(self, null);
            self.SlopingCollision(fallThrough);
            Collision.StepConveyorBelt(self, self.gravDir);
            if (flag30 && self.velocity.Y == 0f)
            {
                AchievementsHelper.HandleRunning(Math.Abs(self.position.X - position.X));
            }
            if (flag29)
            {
                NetMessage.SendData(13, -1, -1, null, self.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                Minecart.HitTrackSwitch(new Vector2(self.position.X, self.position.Y), self.width, self.height);
            }
            if (velocity.X != self.velocity.X)
            {
                if (velocity.X < 0f)
                {
                    self.slideDir = -1;
                }
                else if (velocity.X > 0f)
                {
                    self.slideDir = 1;
                }
            }
            if (self.gravDir == 1f && Collision.up)
            {
                self.velocity.Y = 0.01f;
                if (!self.merman)
                {
                    self.jump = 0;
                }
            }
            else if (self.gravDir == -1f && Collision.down)
            {
                self.velocity.Y = -0.01f;
                if (!self.merman)
                {
                    self.jump = 0;
                }
            }
            if (self.velocity.Y == 0f && self.grappling[0] == -1)
            {
                self.FloorVisuals(falling);
            }
            if (self.whoAmI == Main.myPlayer)
            {
                Collision.SwitchTiles(self.position, self.width, self.height, self.oldPosition, 1);
            }
            PressurePlateHelper.UpdatePlayerPosition(self);
            self.BordersMovement();
            self.numMinions = 0;
            self.slotsMinions = 0f;
            self.ItemCheck_ManageRightClickFeatures();
            MethodInfo ItemCheckWrapped =
                typeof(Player).GetMethod("ItemCheckWrapped", BindingFlags.NonPublic | BindingFlags.Instance);
            Object[] args = {
                i
            }
            ;
            ItemCheckWrapped.Invoke(self, args);
            self.PlayerFrame();
            if (self.mount.Type == 8)
            {
                self.mount.UseDrill(self);
            }
            if (self.statLife > self.statLifeMax2)
            {
                self.statLife = self.statLifeMax2;
            }
            if (self.statMana > self.statManaMax2)
            {
                self.statMana = self.statManaMax2;
            }
            self.grappling[0] = -1;
            self.grapCount = 0;
            self.releaseUseTile = !self.tileInteractAttempted;
            PlayerHooks.PostUpdate(self);
            _quickGrappleCooldownInfo.SetValue(self, _quickGrappleCooldown);
        }

    }
}
