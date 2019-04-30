namespace TUA.API.Default
{
    abstract class TUADefaultBlockItem : TUAModItem
    {
        public abstract string name { get; }
        public abstract int value { get; }
        public abstract int blockToPlace { get; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.value = value;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = blockToPlace;
        }
    }
}
