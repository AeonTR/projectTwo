using UnityEngine;

namespace MultiplayerARPG
{
    [CreateAssetMenu(fileName = GameDataMenuConsts.SOCKET_ENCHANTING_COST_FILE, menuName = GameDataMenuConsts.SOCKET_ENCHANTING_COST_MENU, order = GameDataMenuConsts.SOCKET_ENCHANTING_COST_ORDER)]
    public partial class EnchantingCostItem : BaseItem, IEnchantingCostItem
    {
        public override string TypeTitle
        {
            get { return LanguageManager.GetText(UIItemTypeKeys.UI_ITEM_TYPE_SOCKET_ENHANCER.ToString()); }
        }

        public override ItemType ItemType
        {
            get { return ItemType.EnchantingCost; }
        }

        public override void PrepareRelatesData()
        {
            base.PrepareRelatesData();
        }
    }
}
