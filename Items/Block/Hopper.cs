using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace TUA.Items.Block
{
    class Hopper : ModTileEntity
    {
        private Item[] list;



        public override void Update()
        {
            if (list == null)
            {
                list = new Item[5];
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                Item t = Main.item[i];
                if (t.position.X == Position.X && t.position.Y == Position.Y + 1)
                {
                    absorb(t);
                }
            }
        }

        private void absorb(Item item)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == null)
                {
                    list[i] = item;
                    item.type = 0;
                    return;
                }
            }
        }

        public override bool ValidTile(int i, int j)
        {

            throw new NotImplementedException();
        }

        public override TagCompound Save()
        {
            TagCompound taglist = new TagCompound();
            taglist.Add("ItemList", list);
            taglist.Get<Item>("ItemList");
            return taglist;
        }
    }

    public class HopperTile : ModTile
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class HopperUI : UIState
    {
        public UIPanel itemPanel;

        public override void OnInitialize()
        {
            itemPanel = new UIPanel();
            itemPanel.SetPadding(0);
            itemPanel.Left.Set(5f, 0f);
            itemPanel.Top.Set(400f, 0f);
            itemPanel.Width.Set(200f, 0f);
            itemPanel.Height.Set(100f, 0f);

        }
    }
}
