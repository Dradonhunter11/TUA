using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TUA.API.TerraEnergy.EnergyAPI;
using TUA.API.TerraEnergy.MachineRecipe.Forge;

namespace TUA.API.TerraEnergy.Block.FunctionnalBlock
{
    class TerraForge : TUATile
    {
        private int currentFrame = 1;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Origin = new Point16(4, 2);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TerraForgeEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);

            animationFrameHeight = 54;

        }

        


        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 15)
            {
                currentFrame++;
                if (currentFrame == 0)
                {
                    frame = 0;
                }
                else if (currentFrame == 1 || currentFrame == 3)
                {
                    frame = 1;
                }
                else if (currentFrame == 2)
                {
                    frame = 2;
                }

                if (currentFrame == 3)
                {
                    currentFrame = -1;
                }
                frameCounter = 0;
            }

        }

        public override void NewRightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = ModContent.GetInstance<TerraForgeEntity>().Find(left, top);

            if (index == -1)
            {
                Main.NewText("false");
                return;
            }

            TerraForgeEntity tfe = (TerraForgeEntity)TileEntity.ByID[index];
        }


        //obselete
        /*
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Player player = Main.player[Main.myPlayer];
            Item currentSelectedItem = player.inventory[player.selectedItem];

            Tile tile = Main.tile[i, j];

            int left = i - (tile.frameX / 18);
            int top = j - (tile.frameY / 18);

            int index = ModContent.GetInstance<TerraForgeEntity>().Find(left, top);

            if (index == -1) {
                Main.NewText("false");
                return;
            }

            TerraForgeEntity tfe = (TerraForgeEntity)TileEntity.ByID[index];

            
            if(tfe.getAnimateState())
            {
                tfe.substractTimer();
                if (tfe.getTimer() == 0)
                {
                    tfe.setCurrentFrame(tfe.getCurrentFrame() + 1);
                    if (tfe.getCurrentFrame() == 1)
                    {
                        frameYOffset = 54 * 1;
                        
                    }
                    else if (tfe.getCurrentFrame() == 2 || tfe.getCurrentFrame() == 4)
                    {
                        frameYOffset = 54 * 2;
                    }
                    else if (tfe.getCurrentFrame() == 3)
                    {
                        frameYOffset = 54 * 3;
                    }

                    if (tfe.getCurrentFrame() == 4)
                    {
                        tfe.setCurrentFrame(1);
                    }
                    tfe.resetTimer();
                }
                frameXOffset = 0;
            }
            
        }*/

        /*public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            for (int i = 0; i < TileEntity.ByID.Count; i++) {
                TileEntity tileEntity = TileEntity.ByID[i];
                if (tileEntity.type == mod.TileEntityType("TerraForgeEntity")) {
                    TerraForgeEntity tfe = tileEntity as TerraForgeEntity;
                    if (tfe.getAnimateState())
                    {
                        tfe.substractTimer();
                        if (tfe.getTimer() == 0)
                        {
                            tfe.setCurrentFrame(tfe.getCurrentFrame() + 1);
                            if (tfe.getCurrentFrame() == 1)
                            {
                                frame = 0;

                            }
                            else if (tfe.getCurrentFrame() == 2 || tfe.getCurrentFrame() == 4)
                            {
                                frame = 1;
                            }
                            else if (tfe.getCurrentFrame() == 3)
                            {
                                frame = 2;
                            }

                            if (tfe.getCurrentFrame() == 4)
                            {

                                tfe.setCurrentFrame(1);
                            }
                            tfe.resetTimer();
                        }
                    }
                }
                
            }
        }*/
    }

    class TerraForgeEntity : StorageEntity
    {
        private Item slot1 = new Item();
        private Item slot2 = new Item();
        private Item resultSlot = new Item();

        private ForgeRecipe currentRecipe;

        public Item[] getItemSlot()
        {
            Item[] itemSlot = new Item[2];
            itemSlot[0] = slot1;
            itemSlot[1] = slot2;
            return itemSlot;
        }

        public void setItemSlot(Item[] itemSlot)
        {
            slot1 = itemSlot[0];
            slot2 = itemSlot[1];
        }

        public Item getResultSlot()
        {
            return resultSlot;
        }

        public void setResultSlot(Item result)
        {
            resultSlot = result;
        }

        public override void Update()
        {
            base.Update();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            return Place(i - 4, j - 2);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            Main.NewText("here");
            Main.NewText((tile.active() && tile.type == ModContent.TileType<TerraForge>() && tile.frameX == 0 && tile.frameY == 0));
            return tile.active() && (tile.type == ModContent.TileType<TerraForge>()) && tile.frameX == 0 && tile.frameY == 0;
        }
    }
}
