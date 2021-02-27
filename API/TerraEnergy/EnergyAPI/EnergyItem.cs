using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TUA.API.TerraEnergy.EnergyAPI
{
    public abstract class EnergyItem : TUAModItem
    {
        public override bool CloneNewInstances
        {
            get { return true; }
        }

        private int maxEnergyStorage;
        private EnergyCore _energyCore;
        public int energy = 0;

        public int MaxEnergyStorage
        {
            get { return maxEnergyStorage; }
            protected set { maxEnergyStorage = value; }
        }

        public int CurrentEnergy
        {
            get => _energyCore.getCurrentEnergyLevel(); 
        }

        public sealed override void SetDefaults()
        {
            SafeSetDefault(ref maxEnergyStorage);
            _energyCore = new EnergyCore(maxEnergyStorage);
        }

        public virtual void SafeSetDefault(ref int maxEnergy)
        {

        }

        public sealed override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Set("CurrentEnergy", _energyCore.getCurrentEnergyLevel());
            NewSave(ref tag);
            return tag;
        }

        public virtual void NewSave(ref TagCompound tag)
        {

        }

        public sealed override void Load(TagCompound tag)
        {
            _energyCore.addEnergy(tag.GetAsInt("currentEnergy"));
            NewLoad(tag);
        }

        public virtual void NewLoad(TagCompound tag)
        {

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine energyLine = new TooltipLine(mod, "energy", _energyCore.getCurrentEnergyLevel() + " / " + _energyCore.getMaxEnergyLevel() + " TE");
            tooltips.Add(energyLine);
        }

        

        public void AddEnergy(int energy)
        {
            _energyCore.addEnergy(energy);
        }

        public bool isFull()
        {
            return _energyCore.isFull();
        }
    }
}
