using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace MultiplayerARPG
{
    public class UIEnchantingItem : UIBaseOwningCharacterItem
    {
        public IEquipmentItem EquipmentItem { get { return CharacterItem != null ? CharacterItem.GetEquipmentItem() : null; } }

        public int SelectedPaperId
        {
            get
            {
                if (uiSocketEnhancerItems.CacheSelectionManager.SelectedUI != null &&
                    uiSocketEnhancerItems.CacheSelectionManager.SelectedUI.EnchantingPaperItem != null)
                    return uiSocketEnhancerItems.CacheSelectionManager.SelectedUI.EnchantingPaperItem.DataId;
                return 0;
            }
        }

        public int SelectedCostId
        {
            get
            {
                if (uiAppliedSocketEnhancerItems.CacheSelectionManager.SelectedUI != null &&
                    uiAppliedSocketEnhancerItems.CacheSelectionManager.SelectedUI.EnchantingCostItem != null)
                    return uiAppliedSocketEnhancerItems.CacheSelectionManager.SelectedUI.EnchantingCostItem.DataId;
                return 0;
            }
        }

        [Header("String Formats")]
        [FormerlySerializedAs("formatKeyRemoveRequireGold")]
        [Tooltip("Format => {0} = {Current Gold Amount}, {1} = {Target Amount}")]
        public UILocaleKeySetting formatKeyRequireGold = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_REQUIRE_GOLD);
        [FormerlySerializedAs("formatKeyRemoveRequireGoldNotEnough")]
        [Tooltip("Format => {0} = {Current Gold Amount}, {1} = {Target Amount}")]
        public UILocaleKeySetting formatKeyRequireGoldNotEnough = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_REQUIRE_GOLD_NOT_ENOUGH);
        [Tooltip("Format => {0} = {Target Amount}")]
        public UILocaleKeySetting formatKeySimpleRequireGold = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_SIMPLE);

        [Header("UI Elements for UI Enhance Socket Item")]
        public UINonEquipItems uiSocketEnhancerItems;
        public UINonEquipItems uiAppliedSocketEnhancerItems;
        public UIItemAmounts uiRequireItemAmounts;
        public UICurrencyAmounts uiRequireCurrencyAmounts;
        [FormerlySerializedAs("uiTextRemoveRequireGold")]
        public TextWrapper uiTextRequireGold;
        public TextWrapper uiTextSimpleRequireGold;

        protected bool activated;
        protected string activeItemId;

        protected override void Update()
        {
            base.Update();

            if (uiRequireItemAmounts != null)
            {
                if (SelectedPaperId == 0 || GameInstance.Singleton.enhancerRemoval.RequireItems == null || GameInstance.Singleton.enhancerRemoval.RequireItems.Length == 0)
                {
                    uiRequireItemAmounts.Hide();
                }
                else
                {
                    uiRequireItemAmounts.displayType = UIItemAmounts.DisplayType.Requirement;
                    uiRequireItemAmounts.Show();
                    uiRequireItemAmounts.Data = GameDataHelpers.CombineItems(GameInstance.Singleton.enhancerRemoval.RequireItems, null);
                }
            }

            if (uiRequireCurrencyAmounts != null)
            {
                if (SelectedPaperId == 0 || GameInstance.Singleton.enhancerRemoval.RequireCurrencies == null || GameInstance.Singleton.enhancerRemoval.RequireCurrencies.Length == 0)
                {
                    uiRequireCurrencyAmounts.Hide();
                }
                else
                {
                    uiRequireCurrencyAmounts.displayType = UICurrencyAmounts.DisplayType.Requirement;
                    uiRequireCurrencyAmounts.Show();
                    uiRequireCurrencyAmounts.Data = GameDataHelpers.CombineCurrencies(GameInstance.Singleton.enhancerRemoval.RequireCurrencies, null);
                }
            }

            if (uiTextRequireGold != null)
            {
                if (SelectedPaperId == 0)
                {
                    uiTextRequireGold.text = ZString.Format(
                        LanguageManager.GetText(formatKeyRequireGold),
                        "0",
                        "0");
                }
                else
                {
                    uiTextRequireGold.text = ZString.Format(
                        GameInstance.PlayingCharacter.Gold >= GameInstance.Singleton.enhancerRemoval.RequireGold ?
                            LanguageManager.GetText(formatKeyRequireGold) :
                            LanguageManager.GetText(formatKeyRequireGoldNotEnough),
                        GameInstance.PlayingCharacter.Gold.ToString("N0"),
                        GameInstance.Singleton.enhancerRemoval.RequireGold.ToString("N0"));
                }
            }

            if (uiTextSimpleRequireGold != null)
                uiTextSimpleRequireGold.text = string.Format(LanguageManager.GetText(formatKeySimpleRequireGold), SelectedPaperId == 0 ? "0" : GameInstance.Singleton.enhancerRemoval.RequireGold.ToString("N0"));
        }

        public override void OnUpdateCharacterItems()
        {
            if (!IsVisible())
                return;

            // Store data to variable so it won't lookup for data from property again
            CharacterItem characterItem = CharacterItem;

            if (activated && (characterItem.IsEmptySlot() || !characterItem.id.Equals(activeItemId)))
            {
                // Item's ID is difference to active item ID, so the item may be destroyed
                // So clear data
                Data = new UIOwningCharacterItemData(InventoryType.NonEquipItems, -1);
                return;
            }

            if (uiCharacterItem != null)
            {
                if (characterItem.IsEmptySlot())
                {
                    uiCharacterItem.Hide();
                }
                else
                {

                    uiCharacterItem.Setup(new UICharacterItemData(characterItem, InventoryType), GameInstance.PlayingCharacter, IndexOfData);
                    uiCharacterItem.Show();
                }
            }

            if (uiSocketEnhancerItems != null)
            {
                uiSocketEnhancerItems.filterItemTypes = new List<ItemType>() { ItemType.EnchantingPaper };
                uiSocketEnhancerItems.CacheSelectionManager.selectionMode = UISelectionMode.SelectSingle;
                uiSocketEnhancerItems.UpdateData(GameInstance.PlayingCharacter);
            }

            if (uiAppliedSocketEnhancerItems != null)
            {
                uiAppliedSocketEnhancerItems.filterItemTypes = new List<ItemType>() { ItemType.EnchantingCost };
                uiAppliedSocketEnhancerItems.CacheSelectionManager.selectionMode = UISelectionMode.SelectSingle;
                uiAppliedSocketEnhancerItems.UpdateData(GameInstance.PlayingCharacter);
            }
        }

        public override void Show()
        {
            base.Show();
            activated = false;
            OnUpdateCharacterItems();
        }

        public override void Hide()
        {
            base.Hide();
            Data = new UIOwningCharacterItemData(InventoryType.NonEquipItems, -1);
        }

        public void OnClickEnhanceSocket()
        {
            if (CharacterItem.IsEmptySlot() || SelectedPaperId == 0)
                return;
            activated = true;
            activeItemId = CharacterItem.id;
            GameInstance.ClientInventoryHandlers.RequestUseEnchantingPaper(new RequestUseEnchantingPaperMessage()
            {
                inventoryType = InventoryType,
                index = IndexOfData,
                paperId = SelectedPaperId,
            }, ClientInventoryActions.ResponseUseEnchantingPaper);
        }

        public void OnClickRemoveEnhancer()
        {
            if (CharacterItem.IsEmptySlot() || SelectedCostId == 0)
                return;
            activated = true;
            activeItemId = CharacterItem.id;
            GameInstance.ClientInventoryHandlers.RequestUseEnchantingCost(new RequestUseEnchantingCostMessage()
            {
                inventoryType = InventoryType,
                index = IndexOfData,
                costId = SelectedCostId,
            }, ClientInventoryActions.ResponseUseEnchantingCost);
        }
    }
}
