namespace MultiplayerARPG
{
    public partial interface IEnchantingPaperItem : IItem
    {
        /// <summary>
        /// Stats whichc will be increased to item which put this item into it
        /// </summary>
        DamageElement[] EnchatingAttr { get; }
    }

    public partial interface IEnchantingCostItem : IItem
    {
    }
}
