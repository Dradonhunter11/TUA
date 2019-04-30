using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TUA.API;
using TUA.API.Dev;

namespace TUA.Items.Armor
{
    abstract class ArmorBase : TUAModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            if (CustomValue(out int value, out byte rare))
            {
                item.value = value;
                item.rare = rare;
            }
            else
            {
                item.value = 10000;
                item.rare = 2;
            }
        }

        protected virtual bool CustomValue(out int value, out byte rare)
        {
            value = 0;
            rare = 0;
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            if (DevSet(out var _) && !SteamID64Checker.Instance.VerifyDevID())
            {
                DevSetPenalty(player);
            }
        }

        private bool DevSet(out string dev)
        {
            dev = "";
            return false;
        }

        protected virtual void DevSetPenalty(Player plr) { plr.statLife--; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (DevSet(out string dev))
            {
                tooltips.Add(new TooltipLine(mod, "DevSet", $"Thanks for supporting Terraria Ultra Apocalypse! - {dev}"));
            }
        }

        public sealed override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool flag = IsArmorSetSafe(head, body, legs) && DevSet(out var _) && SteamID64Checker.Instance.VerifyDevID();
            if (DevSet(out var _))
            {
                flag = SteamID64Checker.Instance.VerifyDevID();
            }
            return flag;
        }

        public virtual bool IsArmorSetSafe(Item head, Item body, Item legs) { return false; }
    }
}
