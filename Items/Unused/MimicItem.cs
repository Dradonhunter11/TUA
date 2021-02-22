using Terraria;
using Terraria.ModLoader;

namespace TUA.Items.Unused
{
    class MimicItem : ModItem
    {
        public override string Texture => "MimicItemReal";

        private bool activated;
        public int mimic;

        public override bool Autoload(ref string name)
        {
            //mod.AddItem("MimicItem_Meowmere", new MimicItem(ItemID.Meowmere));
            return false;
        }

        public MimicItem(int type)
        {
            mimic = type;
            activated = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mimic");
            Tooltip.SetDefault("HAHA, GOTEM!");
        }

        public override void SetDefaults()
        {
            item.netDefaults(mimic);
        }

        public override void UpdateInventory(Player player)
        {
            if (activated)
            {
                activated = false;
                item.netDefaults(mod.ItemType("MimicItemReal"));
            }
        }
    }

    abstract class MimicItemReal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mimic");
            Tooltip.SetDefault("HAHA, GOTEM!");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = 2;
            item.value = Item.buyPrice(100, 100, 100, 100);
        }
    }

    public partial class TUAPlayer : ModPlayer
    {
        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            item.value = 0;
            Main.NewText("RIP, DID YOU REALLY THINK IT WAS WORTH THAT MUCH?");
            return true;
        }
    }
}
