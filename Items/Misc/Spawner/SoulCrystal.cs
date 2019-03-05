using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API;

namespace TUA.Items.Misc.Spawner
{
    class SoulCrystal : TUAModItem
    {
        private int mobID;

        private int r = 255;
        private readonly int g = 255;
        private readonly int b = 255;

        private int maxSoul = 0;
        private int currentSoul = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Crystal");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 28;
            item.rare = -13;

        }

        public int getMobID()
        {
            return mobID;
        }

        internal void setID(int mobID)
        {
            this.mobID = mobID;
        }

        internal void setMaxCap(int maxCap)
        {
            this.maxSoul = maxCap;
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("mobId", mobID);
            tag.Add("soulCount", currentSoul);
            tag.Add("maxSoulCount", maxSoul);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            mobID = tag.GetAsInt("mobId");
            currentSoul = tag.GetAsInt("soulCount");
            if (tag.ContainsKey("maxSoulCount"))
            {
                maxSoul = tag.GetAsInt("maxSoulCount");
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            TooltipLine mobName;
            TooltipLine mobMod;
            TooltipLine soulCharge = new TooltipLine(mod, "soulCharge", "Current soul charge : " + currentSoul + " / " + maxSoul);
            if (NPCLoader.GetNPC(mobID) == null)
            {
                mobName = new TooltipLine(mod, "mobName", "Creature : " + Lang.GetNPCNameValue(mobID));
                mobMod = new TooltipLine(mod, "mobMod", "Source : Vanilla");
            }
            else
            {
                mobName = new TooltipLine(mod, "mobName", "Creature : " + NPCLoader.GetNPC(mobID).DisplayName.GetDefault());
                mobMod = new TooltipLine(mod, "mobMod", "Source : " + NPCLoader.GetNPC(mobID).mod.DisplayName);
            }
            tooltips.Add(mobName);
            tooltips.Add(mobMod);
            tooltips.Add(soulCharge);
        }

        public override void UpdateInventory(Player player)
        {

        }

        public override bool OnPickup(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item items = player.inventory[i];
                if (items.modItem is SoulCrystal)
                {
                    SoulCrystal sc = items.modItem as SoulCrystal;
                    if (Equals(sc) && !sc.isFull())
                    {
                        sc.currentSoul++;
                        item.TurnToAir();
                        return false;
                    }
                }
            }
            return true;
        }

        private bool Equals(SoulCrystal sc)
        {
            return mobID == sc.mobID;
        }

        public bool isFull()
        {
            return currentSoul >= maxSoul;
        }
    }
}
