using Terraria.ModLoader;

namespace TUA.API.VoidClass
{
    public class VoidDamagePlayer : ModPlayer
    {
        public float voidDmg = 1f;
        public float voidKb = 0f;
        public int voidCrit = 100;

        public int voidCatalyst = 0;

        public override void Initialize()
        {
            voidDmg = 1f;
            voidKb = 0f;
            voidCrit = 100;
        }

        public override void ResetEffects()
        {
            Initialize();
        }

        public override void UpdateDead()
        {
            Initialize();
        }

        /*
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.modItem is VoidDamageItem)
            {
                VoidDamageItem voidItem = item.modItem as VoidDamageItem;
            }
        }
        */
    }
}
