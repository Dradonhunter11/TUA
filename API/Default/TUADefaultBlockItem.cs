namespace TUA.API.Default
{
    abstract class TUADefaultBlockItem : TUAModItem
    {
        public abstract string TUAName { get; }
        public abstract int TUAValue { get; }
        public abstract int BlockToPlace { get; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(TUAName);
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.value = TUAValue;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = BlockToPlace;
        }
    }
}
