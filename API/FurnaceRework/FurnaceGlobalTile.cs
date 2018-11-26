using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TerrariaUltraApocalypse.API.FurnaceRework.TileEntity;
using TerrariaUltraApocalypse.API.TerraEnergy.Block.FunctionnalBlock;
using TerrariaUltraApocalypse.API.TerraEnergy.Items;
using TerrariaUltraApocalypse.API.TerraEnergy.TileEntities;

namespace TerrariaUltraApocalypse.API.FurnaceRework
{
    class FurnaceGlobalTile : GlobalTile
    {
        public override void SetDefaults()
        {
            TileObjectData.GetTileData(TileID.Furnaces, 0).HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<RegularFurnaceEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.GetTileData(TileID.Hellforge, 0).HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<HellForgeEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.GetTileData(TileID.AdamantiteForge, 0).HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<AdamantiteTitaniumForgeEntity>().Hook_AfterPlacement, -1, 0, false);
        }

        public override void RightClick(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            BaseFurnaceEntity furnace = GetFurnaceEntity(type);
            if (furnace != null)
            {
                int left = i - (tile.frameX / 18);
                int top = j - (tile.frameY / 18);

                
                int index = furnace.Find(left, top);

                Main.NewText("X " + i + " Y " + j);

                if (index == -1)
                {
                    Main.NewText("false");
                    return;
                }

                BaseFurnaceEntity tfe = (BaseFurnaceEntity)Terraria.DataStructures.TileEntity.ByID[index];
                tfe.Activate();
            }
        }

        private BaseFurnaceEntity GetFurnaceEntity(int type)
        {
            switch (type)
            {
                case TileID.AdamantiteForge:
                    return mod.GetTileEntity<AdamantiteTitaniumForgeEntity>();
                case TileID.Hellforge:
                    return mod.GetTileEntity<HellForgeEntity>();
                case TileID.Furnaces:
                    return mod.GetTileEntity<RegularFurnaceEntity>();
                default:
                    return null;
            }
        }
    }
}
