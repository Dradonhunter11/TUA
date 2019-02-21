using Terraria.ModLoader;

namespace TUA.Items.Armor
{
    abstract class ArmorBase : ModItem
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
    }
}
