using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API;
using TUA.Void;

namespace TUA.Items.Unused
{
    abstract class VoidArtefact : TUAModItem
    {
        public abstract string Name { get; }
        public abstract int Tier { get; }

        public abstract void AddVoidRecipe(ref VoidRecipe recipe);

        public virtual void SetDimension(ref int width, ref int height)
        {
            return;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Name);
            Tooltip.SetDefault("A relic from the void, contains an intense dark energy");
        }

        public sealed override void SetDefaults()
        {
            item.maxStack = 1;
            SetDimension(ref item.width, ref item.height);
            item.value = 0; 
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "TUA : Void Tier", $"[c/4B0082:-- Relic tier {Tier} --]"));
        }

        public sealed override void AddRecipes()
        {
            VoidRecipe recipe = new VoidRecipe(mod);
            AddVoidRecipe(ref recipe);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }

    internal class VoidArtefactT1 : VoidArtefact
    {
        public override string Name => "Void Artefact Alpha";
        public override int Tier => 1;

        public override void SetDimension(ref int width, ref int height)
        {
            width = 24;
            height = 24;
        }

        public override void AddVoidRecipe(ref VoidRecipe recipe)
        {
            recipe.SetAmountOfRequiredVoidAffinity(100);
            recipe.AddIngredient(ItemID.StoneBlock, 200);
        }
    }

    internal class VoidArtefactT2 : VoidArtefact
    {
        public override string Name => "Void Artefact Beta";
        public override int Tier => 2;

        public override void SetDimension(ref int width, ref int height)
        {
            width = 32;
            height = 36;
        }

        public override void AddVoidRecipe(ref VoidRecipe recipe)
        {
            recipe.SetAmountOfRequiredVoidAffinity(200);
            recipe.AddIngredient(ItemID.StoneBlock, 1800); //will be replaced with 200 Void fragment
        }
    }

    internal class VoidArtefactT3 : VoidArtefact
    {
        public override string Name => "Void Artefact Gamma";
        public override int Tier => 3;

        public override void SetDimension(ref int width, ref int height)
        {
            width = 36;
            height = 40;
        }

        public override void AddVoidRecipe(ref VoidRecipe recipe)
        {
            recipe.SetAmountOfRequiredVoidAffinity(500);
            recipe.AddIngredient(ItemID.LunarOre, 200);
        }
    }
}
