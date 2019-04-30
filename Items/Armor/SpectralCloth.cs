using Terraria;
using Terraria.ModLoader;

namespace TUA.Items.Armor
{
    abstract class SpectralCloth : ArmorBase
    {
        public override bool IsArmorSetSafe(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("SpectralClothHelmet")
                && body.type == mod.ItemType("SpectralClothBreastplate")
                && legs.type == mod.ItemType("SpectralClothLeggings");
        }
    }
    [AutoloadEquip(EquipType.Head)]
    abstract class SpectralClothHelmet : SpectralCloth { }
    [AutoloadEquip(EquipType.Body)]
    abstract class SpectralClothBreastplate : SpectralCloth { }
    [AutoloadEquip(EquipType.Legs)]
    abstract class SpectralClothLeggings : SpectralCloth { }
}
