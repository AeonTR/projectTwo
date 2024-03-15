using Cysharp.Text;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class UIEquipmentEnchanting : UIBaseEquipmentBonus<UIEquipmentEnchantingData>
    {
        [Header("String Formats")]
        [Tooltip("Format => {0} = {Set Title}, {1} = {List Of Effect}")]
        public UILocaleKeySetting formatKeySet = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_EQUIPMENT_SET);
        [Tooltip("Format => {0} = {Equip Amount}, {1} = {List Of Bonus}")]
        public UILocaleKeySetting formatKeyAppliedEffect = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_EQUIPMENT_SET_APPLIED_EFFECT);
        [Tooltip("Format => {0} = {Equip Amount}, {1} = {List Of Bonus}")]
        public UILocaleKeySetting formatKeyUnappliedEffect = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_EQUIPMENT_SET_UNAPPLIED_EFFECT);

        protected override void UpdateData()
        {
            using (Utf16ValueStringBuilder allBonusText = ZString.CreateStringBuilder(false))
            {
                int effectCount = 1;
                string tempText;

                tempText = GetEquipmentBonusText(Data.equipmentBonus);
                if (!string.IsNullOrEmpty(tempText))
                {
                    if (allBonusText.Length > 0)
                        allBonusText.Append('\n');
                    allBonusText.AppendFormat(
                        effectCount <= Data.equippedCount ?
                            LanguageManager.GetText(formatKeyAppliedEffect) :
                            LanguageManager.GetText(formatKeyUnappliedEffect),
                        effectCount.ToString("N0"),
                        tempText);
                }
                ++effectCount;

                if (uiTextAllBonus != null)
                {
                    uiTextAllBonus.SetGameObjectActive(allBonusText.Length > 0);
                    uiTextAllBonus.text = ZString.Format(
                        LanguageManager.GetText(formatKeySet),
                        "",
                        allBonusText.ToString());
                }
            }
        }
    }
}
