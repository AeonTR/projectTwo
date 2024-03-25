using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiplayerARPG
{
    public partial class BaseItem
    {
        public static bool GenerateRandomAttrRightHandItem(IPlayerCharacterData character, int paperId, out UITextKeys gameMessage)
        {
            return GenerateRandomAttr(character, character.EquipWeapons.rightHand, paperId, (enhancedSocketItem) =>
            {
                EquipWeapons equipWeapon = character.EquipWeapons;
                equipWeapon.rightHand = enhancedSocketItem;
                character.EquipWeapons = equipWeapon;
            }, out gameMessage);
        }

        public static bool GenerateRandomAttrLeftHandItem(IPlayerCharacterData character, int paperId, out UITextKeys gameMessage)
        {
            return GenerateRandomAttr(character, character.EquipWeapons.leftHand, paperId, (enhancedSocketItem) =>
            {
                EquipWeapons equipWeapon = character.EquipWeapons;
                equipWeapon.leftHand = enhancedSocketItem;
                character.EquipWeapons = equipWeapon;
            }, out gameMessage);
        }

        public static bool GenerateRandomAttrEquipItem(IPlayerCharacterData character, int index, int paperId, out UITextKeys gameMessage)
        {
            return GenerateRandomAttrByList(character, character.EquipItems, index, paperId, out gameMessage);
        }

        public static bool GenerateRandomAttrNonEquipItem(IPlayerCharacterData character, int index, int paperId, out UITextKeys gameMessage)
        {
            return GenerateRandomAttrByList(character, character.NonEquipItems, index, paperId, out gameMessage);
        }

        private static bool GenerateRandomAttrByList(IPlayerCharacterData character, IList<CharacterItem> list, int index, int paperId, out UITextKeys gameMessage)
        {
            return GenerateRandomAttr(character, list[index], paperId, (enhancedSocketItem) =>
            {
                list[index] = enhancedSocketItem;
            }, out gameMessage);
        }

        private static bool GenerateRandomAttr(IPlayerCharacterData character, CharacterItem enchantingItem, int paperId, System.Action<CharacterItem> onEnhanceSocket, out UITextKeys gameMessage)
        {
            gameMessage = UITextKeys.NONE;
            if (enchantingItem.IsEmptySlot())
            {
                // Cannot enhance socket because character item is empty
                gameMessage = UITextKeys.UI_ERROR_ITEM_NOT_FOUND;
                return false;
            }
            IEquipmentItem equipmentItem = enchantingItem.GetEquipmentItem();
            if (equipmentItem == null)
            {
                // Cannot enhance socket because it's not equipment item
                gameMessage = UITextKeys.UI_ERROR_ITEM_NOT_EQUIPMENT;
                return false;
            }

            BaseItem enhancerItem;
            if (!GameInstance.Items.TryGetValue(paperId, out enhancerItem) || !enhancerItem.IsEnchantingPaper())
            {
                // Cannot enhance socket because enhancer id is invalid
                gameMessage = UITextKeys.UI_ERROR_CANNOT_ENHANCE_SOCKET;
                return false;
            }

            if (equipmentItem.EnchantingAttributes.damages == null)
            {
                equipmentItem.EnchantingAttributes.damages = new DamageAmount[1];
            }

            if (equipmentItem.EnchantingAttributes.damages.Length >= 4)
            {
                gameMessage = UITextKeys.UI_ERROR_CANNOT_ENCHANTING_SOCKET;
                return false;
            }

            EnchantingPaperItem paperItem = enhancerItem as EnchantingPaperItem;

            character.DecreaseItems(paperId, 1);
            character.FillEmptySlots();

            Random random = new Random();
            int v = random.Next(1, 100);

            if (v < 50)
            {
                gameMessage = UITextKeys.UI_ERROR_CANNOT_ENCHANTING_FAIL;
                return false;
            }

            int dataId = GetRandomDamageElement(equipmentItem.EnchantingAttributes.damages, equipmentItem.ItemType);

            int count = equipmentItem.EnchantingAttributes.damages.Length;
            Array.Resize(ref equipmentItem.EnchantingAttributes.damages, count + 1);

            equipmentItem.EnchantingAttributes.damages[count] = new DamageAmount();
            var damageElement = GameInstance.DamageElements[dataId];
            equipmentItem.EnchantingAttributes.damages[count].damageElement = damageElement;
            equipmentItem.EnchantingAttributes.damages[count].amount = new MinMaxFloat() { max = 2, min = 1 };

            EnchantingData data = new EnchantingData()
            {
                dataId = dataId,
                min = 1,
                max = 2
            };
            enchantingItem.EnchantingDatas.Add(data);

            onEnhanceSocket.Invoke(enchantingItem);

            GameInstance.ServerLogHandlers.LogEnhanceSocketItem(character, enchantingItem, enhancerItem);

            return true;
        }

        public static int GetRandomDamageElement(DamageAmount[] filter, ItemType itemType)
        {
            Dictionary<int, DamageElement> filterDamage = new Dictionary<int, DamageElement>();
            foreach (var damage in GameInstance.DamageElements)
            {
                if (itemType == damage.Value.AdapterType)
                {
                    filterDamage.Add(damage.Key, damage.Value);
                }
            }

            List<DamageElement> list = new List<DamageElement>();
            for (int i = 0; i < filterDamage.Count; i++)
            {
                int dataId = filterDamage.ElementAt(i).Key;
                bool isFilter = false;
                for (int j = 0; j < filter.Length; j++)
                {
                    if (filter[j].damageElement != null && filter[j].damageElement.DataId == dataId)
                    {
                        isFilter = true;
                    }
                }

                if (isFilter == false)
                {
                    list.Add(filterDamage.ElementAt(i).Value);
                }
            }

            int len = list.Count;
            Random random = new Random();
            int index = random.Next(0, len);

            return list[index].DataId;
        }

        public static bool ChangeRandomAttrRightHandItem(IPlayerCharacterData character, int paperId, out UITextKeys gameMessage)
        {
            return ChangeRandomAttr(character, character.EquipWeapons.rightHand, paperId, (enhancedSocketItem) =>
            {
                EquipWeapons equipWeapon = character.EquipWeapons;
                equipWeapon.rightHand = enhancedSocketItem;
                character.EquipWeapons = equipWeapon;
            }, out gameMessage);
        }

        public static bool ChangeRandomAttrLeftHandItem(IPlayerCharacterData character, int paperId, out UITextKeys gameMessage)
        {
            return ChangeRandomAttr(character, character.EquipWeapons.leftHand, paperId, (enhancedSocketItem) =>
            {
                EquipWeapons equipWeapon = character.EquipWeapons;
                equipWeapon.leftHand = enhancedSocketItem;
                character.EquipWeapons = equipWeapon;
            }, out gameMessage);
        }

        public static bool ChangeRandomAttrEquipItem(IPlayerCharacterData character, int index, int paperId, out UITextKeys gameMessage)
        {
            return ChangeRandomAttrByList(character, character.EquipItems, index, paperId, out gameMessage);
        }

        public static bool ChangeRandomAttrNonEquipItem(IPlayerCharacterData character, int index, int paperId, out UITextKeys gameMessage)
        {
            return ChangeRandomAttrByList(character, character.NonEquipItems, index, paperId, out gameMessage);
        }

        private static bool ChangeRandomAttrByList(IPlayerCharacterData character, IList<CharacterItem> list, int index, int paperId, out UITextKeys gameMessage)
        {
            return ChangeRandomAttr(character, list[index], paperId, (enhancedSocketItem) =>
            {
                list[index] = enhancedSocketItem;
            }, out gameMessage);
        }

        private static bool ChangeRandomAttr(IPlayerCharacterData character, CharacterItem enchantingItem, int costId, System.Action<CharacterItem> onEnhanceSocket, out UITextKeys gameMessage)
        {
            gameMessage = UITextKeys.NONE;
            if (enchantingItem.IsEmptySlot())
            {
                // Cannot enhance socket because character item is empty
                gameMessage = UITextKeys.UI_ERROR_ITEM_NOT_FOUND;
                return false;
            }
            IEquipmentItem equipmentItem = enchantingItem.GetEquipmentItem();
            if (equipmentItem == null)
            {
                // Cannot enhance socket because it's not equipment item
                gameMessage = UITextKeys.UI_ERROR_ITEM_NOT_EQUIPMENT;
                return false;
            }

            BaseItem enhancerItem;
            if (!GameInstance.Items.TryGetValue(costId, out enhancerItem) || !enhancerItem.IsEnchantingCost())
            {
                // Cannot enhance socket because enhancer id is invalid
                gameMessage = UITextKeys.UI_ERROR_CANNOT_ENHANCE_SOCKET;
                return false;
            }

            character.DecreaseItems(costId, 1);
            character.FillEmptySlots();

            if (equipmentItem.EnchantingAttributes == null)
            {
                // Cannot enhance socket because enhancer id is invalid
                gameMessage = UITextKeys.UI_ERROR_CANNOT_ENHANCE_SOCKET;
                return false;
            }

            Dictionary<int, DamageElement> filterDamage = new Dictionary<int, DamageElement>();
            foreach(var damage in GameInstance.DamageElements)
            {
                if (equipmentItem.ItemType == damage.Value.AdapterType)
                {
                    filterDamage.Add(damage.Key, damage.Value);
                }
            }

            if (filterDamage.Count == 0)
            {
                gameMessage = UITextKeys.UI_ERROR_NO_ADAPTER_ENCHANTING_BONUS;
                return false;
            }

            List<int> list = new List<int>();
            for (int i = 0; i < filterDamage.Count; i++)
            {
                list.Add(i);
            }

            FisherYatesShuffle(ref list);

            var newList = new List<EnchantingData>();

            DamageAmount[] damageAmount = equipmentItem.EnchantingAttributes.damages;
            for (int j = 0; j < damageAmount.Length; j++)
            {
                int randomIndex = list[j];

                DamageElement newDamage = filterDamage.ElementAt(randomIndex).Value;
                damageAmount[j].damageElement = newDamage;
                damageAmount[j].amount = new MinMaxFloat() { max = newDamage.DamageRange.max, min = newDamage.DamageRange.min };

                var data = enchantingItem.EnchantingDatas[j];
                data.dataId = newDamage.DataId;

                Random random = new Random();
                int v = random.Next(newDamage.DamageRange.min, newDamage.DamageRange.max);

                data.min = v;
                data.max = v;

                newList.Add(data);
            }

            enchantingItem.EnchantingDatas = newList;

            onEnhanceSocket.Invoke(enchantingItem);

            GameInstance.ServerLogHandlers.LogEnhanceSocketItem(character, enchantingItem, enhancerItem);

            return true;
        }

        public static void FisherYatesShuffle<T>(ref List<T> list)
        {
            List<T> cache = new List<T>();
            int currentIndex;
            while (list.Count > 0)
            {
                Random random = new Random();
                currentIndex = random.Next(0, list.Count);
                cache.Add(list[currentIndex]);
                list.RemoveAt(currentIndex);
            }
            for (int i = 0; i < cache.Count; i++)
            {
                list.Add(cache[i]);
            }
        }
    }
}