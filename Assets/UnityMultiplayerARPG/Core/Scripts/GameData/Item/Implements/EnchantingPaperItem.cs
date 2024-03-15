using UnityEngine;

namespace MultiplayerARPG
{
    [CreateAssetMenu(fileName = GameDataMenuConsts.SOCKET_ENCHANTING_PAPER_FILE, menuName = GameDataMenuConsts.SOCKET_ENCHANTING_PAPER_MENU, order = GameDataMenuConsts.SOCKET_ENCHANTING_PAPER_ORDER)]
    public partial class EnchantingPaperItem : BaseItem, IEnchantingPaperItem
    {
        public override string TypeTitle
        {
            get { return LanguageManager.GetText(UIItemTypeKeys.UI_ITEM_TYPE_SOCKET_ENHANCER.ToString()); }
        }

        public override ItemType ItemType
        {
            get { return ItemType.EnchantingPaper; }
        }

        [Category(3, "Random Attr Settings")]
        [SerializeField]
        private DamageElement[] enchatingAttr = default;
        public DamageElement[] EnchatingAttr
        {
            get { return enchatingAttr; }
        }

        [Category(3, "Random Attr Count")]
        [SerializeField]
        private int enchatingAttrCount = default;
        public int EnchatingAttrCount
        {
            get { return enchatingAttrCount; }
        }

        public override void PrepareRelatesData()
        {
            base.PrepareRelatesData();
        }
    }
}
