using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerrariaUltraApocalypse.API.TerraEnergy.EnergyAPI
{
    public abstract class EnergyItem : TUAModItem
    {
        public override bool CloneNewInstances
        {
            get { return true; }
        }

        private int maxEnergyStorage;
        private Core core;
        public int energy = 0;

        public int MaxEnergyStorage
        {
            get { return maxEnergyStorage; }
            protected set { maxEnergyStorage = value; }
        }

        public int CurrentEnergy
        {
            get => core.getCurrentEnergyLevel(); 
        }

        public sealed override void SetDefaults()
        {
            SafeSetDefault(ref maxEnergyStorage);
            core = new Core(maxEnergyStorage);
        }

        public virtual void SafeSetDefault(ref int maxEnergy)
        {

        }

        public sealed override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Set("CurrentEnergy", core.getCurrentEnergyLevel());
            NewSave(ref tag);
            return tag;
        }

        public virtual void NewSave(ref TagCompound tag)
        {

        }

        public sealed override void Load(TagCompound tag)
        {
            core.addEnergy(tag.GetAsInt("currentEnergy"));
            NewLoad(tag);
        }

        public virtual void NewLoad(TagCompound tag)
        {

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine energyLine = new TooltipLine(mod, "energy", core.getCurrentEnergyLevel() + " / " + core.getMaxEnergyLevel() + " TE");
            tooltips.Add(energyLine);
        }

        

        public void AddEnergy(int energy)
        {
            core.addEnergy(energy);
        }

        public bool isFull()
        {
            return core.isFull();
        }
    }
}
