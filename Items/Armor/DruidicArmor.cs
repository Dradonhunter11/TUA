using Terraria;
using Terraria.ModLoader;

namespace TUA.Items.Armor
{
    // Serves as an example, may or may not implement this later
    abstract class DruidicArmor : ArmorBase
    {
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("DruidicHelmet")
                && body.type == mod.ItemType("DruidicBreastplate")
                && legs.type == mod.ItemType("DruidicLeggings");
        }
    }

    [AutoloadEquip(EquipType.Head)]
    class DruidicHelmet : DruidicArmor { }
    [AutoloadEquip(EquipType.Body)]
    class DruidicBreastplate : DruidicArmor { }
    [AutoloadEquip(EquipType.Legs)]
    class DruidicLeggings : DruidicArmor { }
}
