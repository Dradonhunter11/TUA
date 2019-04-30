using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.TerraEnergy.EnergyAPI
{
    abstract class EnergyWeapon : EnergyItem
    {
        private int _energyConsumedPerShot;


        public int EnergyConsumedPerShot {
            get { return _energyConsumedPerShot; }
            set { _energyConsumedPerShot = value; }
        }

        public sealed override bool UseItem(Player player)
        {
            if (CurrentEnergy < EnergyConsumedPerShot)
            {
                Main.NewText("Not enough energy", Color.Red);
                return false;
            }

            return NewUseItem(player);
        }

        public virtual bool NewUseItem(Player player)
        {
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            base.ModifyTooltips(tooltips);
        }

        public virtual void NewModifyTooltips(List<TooltipLine> tooltips)
        {

        }

        
    }
}
