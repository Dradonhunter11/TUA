using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using TUA.API.FurnaceRework;
using TUA.API.Inventory;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.Interface;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.API.TerraEnergy.TileEntities;
using TUA.API.TerraEnergy.UI;
using TUA.Items;
using TUA.Utilities;

namespace TUA.API.TerraEnergy.Block.FunctionnalBlock
{
    class TerraFurnace : TUATile
    {
        public override void SetDefaults()
        {

            //TileObjectData.newTile.CoordinateHeights = new int[] { 60, 60, 42 };
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Origin = new Point16(3, 2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TerraFurnaceEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void HitWire(int i, int j)
        {

        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];


            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = ModContent.GetInstance<TerraFurnaceEntity>().Find(left, top);

            Main.NewText("X " + i + " Y " + j);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }

            StorageEntity se = (StorageEntity)TileEntity.ByID[index];
            if (currentSelectedItem.type == mod.ItemType("TerraMeter"))
            {
                se = (StorageEntity)TileEntity.ByID[index];
                Main.NewText(se.GetEnergy().getCurrentEnergyLevel() + " / " + se.GetEnergy().getMaxEnergyLevel() + " TE");
                return;
            }

            if (currentSelectedItem.type == mod.ItemType("RodOfLinking"))
            {
                RodOfLinking it = currentSelectedItem.modItem as RodOfLinking;
                se = (StorageEntity)TileEntity.ByID[index];
                it.SaveLinkableEntityLocation(se);
                Main.NewText("Terra Furnace succesfully linked, now right click on a capacitor to unlink");
                return;
            }

            TerraFurnaceEntity tfe = (TerraFurnaceEntity)TileEntity.ByID[index];
            tfe.Activate();


        }

    }

    class TerraFurnaceEntity : StorageEntity, ITECapacitorLinkable
    {
        private FurnaceUI furnaceUi;

        private CapacitorEntity boundCapacitor;

        private Ref<Item> _inputItem = new Ref<Item>();
        private Ref<Item> _outputItem = new Ref<Item>();

        public ref Item inputItem => ref _inputItem.Value;
        public ref Item outputItem => ref _outputItem.Value;

        private int checkTimer = 20; //Maybe will reduce lag
        private int progression = 0;

        private FurnaceRecipe currentRecipe;

        public TerraFurnaceEntity()
        {
            _inputItem = new Ref<Item>(new Item());
            _outputItem = new Ref<Item>(new Item());
            inputItem.TurnToAir();
            outputItem.TurnToAir();

            energy = new EnergyCore(50000);
        }

        public void Activate()
        {
            Main.playerInventory = true;
            UIManager.OpenMachineUI(new FurnaceUI(_inputItem, _outputItem, energy, "Terra Furnace"));
        }
        
        

        public override void LoadEntity(TagCompound tag)
        {
            inputItem = new Item();
            outputItem = new Item();

            Item temporaryInputItem = tag.Get<Item>("inputSlot");
            Item temporaryOutputItem = tag.Get<Item>("outputSlot");

            SetAir(ref temporaryInputItem);
            SetAir(ref temporaryOutputItem);

            inputItem = temporaryInputItem;
            outputItem = temporaryOutputItem;
        }

        public void SetAir(ref Item item)
        {
            if (item.Name == "Unloaded Item")
            {
                item.TurnToAir();
            }
        }

        public override void SaveEntity(TagCompound tag)
        {
            tag.Add("inputSlot", inputItem);
            tag.Add("outputSlot", outputItem);
        }

        public void LinkToCapacitor(CapacitorEntity capacitor) => boundCapacitor = capacitor;

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            energy = new EnergyCore(50000);
            return Place(i - 3, j - 2);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            Main.NewText((tile.active() && tile.type == ModContent.TileType<TerraFurnace>() && tile.frameX == 0 && tile.frameY == 0));
            return tile.active() && (tile.type == ModContent.TileType<TerraFurnace>()) && tile.frameX == 0 && tile.frameY == 0;
        }

        public override void Update()
        {
            if (currentRecipe == null && checkTimer <= 0)
            {
                FurnaceRecipe recipe = GetRecipe();
                if (recipe != null &&
                    (outputItem.IsAir || outputItem.type == recipe.GetResult().type))
                {
                    currentRecipe = recipe;
                }

                checkTimer = 20;
            }

            if (currentRecipe != null)
            {
                updateItem();
                progression++;

            }

            if (boundCapacitor != null)
            {
                energy.addEnergy(boundCapacitor.energy.ConsumeEnergy(boundCapacitor.maxTransferRate));
            }
            checkTimer--;

        }

        private FurnaceRecipe GetRecipe()
        {
            if (!inputItem.IsAir)
            {
                if (FurnaceRecipeManager.Instance.IsValid(inputItem))
                {
                    return FurnaceRecipeManager.Instance.Recipe;
                }
            }
            return null;
        }

        private void updateItem()
        {
            if (progression >= currentRecipe.GetCookTime() && energy.ConsumeEnergy(50) == 50)
            {
                inputItem.stack -= currentRecipe.GetIngredientStack();

                Item result = currentRecipe.GetResult().Clone();

                if (outputItem.IsAir)
                {
                    outputItem = result;
                }
                else
                {
                    outputItem.stack++;
                }

                currentRecipe = null;
                progression = 0;
            }
        }
    }
}
