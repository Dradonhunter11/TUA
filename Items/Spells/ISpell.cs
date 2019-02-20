using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Items.Spells
{
    internal interface ISpell
    {
        bool GetColor(out Color color);
        bool Cast(Player player);
    }

    internal abstract class Spell : ModItem, ISpell
    {
        private int texStyle;

        public override string Texture => "TUA/Spells/Scroll";

        public virtual bool GetColor(out Color color)
        {
            color = default(Color);
            return false;
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "ItemName" && x.mod == "Terraria");
            if (index != -1 && GetColor(out Color color))
            {
                tooltips[index].overrideColor = color;
            }
        }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 30;
            item.height = 30;
            // texStyle = Main.rand.Next(); // This is for multiple different texStyles
            texStyle = 0;
        }

        public abstract void SafeSetDefaults();

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // Scroll_Mask should actually be "writing" to go on top of Scroll
            var tex2 = mod.GetTexture($"Spells/Scroll_Mask{texStyle}");
            if (GetColor(out Color color))
            {
                spriteBatch.Draw(tex2, frame, color);
            }
            else
            {
                spriteBatch.Draw(tex2, frame, drawColor);
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            // Scroll_Mask should actually be "scripts" to go on top of Scroll
            var tex2 = mod.GetTexture($"Spells/Scroll_Mask{texStyle}");
            if (GetColor(out Color color))
            {
                spriteBatch.Draw(tex2, item.position, null, color,
                    rotation, tex2.Size() / 2, scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(tex2, item.position, null, lightColor,
                    rotation, tex2.Size() / 2, scale, SpriteEffects.None, 0);
            }
        }

        public abstract bool Cast(Player player);

        // Possibly some kind of unique mana system in the future?
        public sealed override bool UseItem(Player player) => Cast(player);
    }

    internal abstract class Godspell : Spell
    {
        public new bool GetColor(out Color color)
        {
            color = Color.DarkMagenta;
            return true;
        }

        public sealed override void SafeSetDefaults()
        {
            SetGodspellDefaults();
            item.accessory = true;
            item.melee = false;
            item.magic = false;
            item.summon = false;
            item.thrown = false;
            item.ranged = false;
            item.damage = 0;
            item.knockBack = 0;
            item.crit = 0;
        }

        public abstract void SetGodspellDefaults();

        public override bool CanUseItem(Player player) => false;

        public override void UpdateAccessory(Player player, bool hideVisual) => Cast(player);
    }
}
