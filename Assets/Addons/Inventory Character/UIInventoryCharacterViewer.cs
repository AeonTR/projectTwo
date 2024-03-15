using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MultiplayerARPG
{
    public class UIInventoryCharacterViewer : UIBase
    {
        [Header("UI")]
        public Transform modelContainer;
        private BaseCharacterModel SelectedModel;
        private List<CharacterItem> EquipItems = new List<CharacterItem>();
        protected readonly Dictionary<int, BaseCharacterModel> CharacterModelByEntityId = new Dictionary<int, BaseCharacterModel>();
        private void OnEnable()
        {
            modelContainer.RemoveChildren();
            Setup();
        }

        private void OnDisable()
        {
            modelContainer.RemoveChildren();
            GameInstance.PlayingCharacterEntity.onSelectableWeaponSetsOperation -= UpdateCharacter;
            GameInstance.PlayingCharacterEntity.onEquipItemsOperation -= UpdateCharacter;
        }

        private void UpdateCharacterSetup()
        {
            var character = GameInstance.PlayingCharacter;
            var characterEntity = GameInstance.PlayingCharacterEntity;
            if (character == null)
                return;

            if (SelectedModel == null)
                return;

            SelectedModel.SetEquipItems(character.EquipItems, character.SelectableWeaponSets, character.EquipWeaponSet, characterEntity.IsWeaponsSheathed);
        }

        private void Update()
        {
            if (SelectedModel != null)
                SelectedModel.UpdateAnimation(Time.deltaTime);
        }

        void Setup()
        {
            EquipItems.Clear();
            var character = GameInstance.PlayingCharacter;
            if (character == null)
                return;
            var characterEntity = GameInstance.PlayingCharacterEntity;

            GameInstance.PlayingCharacterEntity.onSelectableWeaponSetsOperation += UpdateCharacter;
            GameInstance.PlayingCharacterEntity.onEquipItemsOperation += UpdateCharacter;
            EquipItems.AddRange(character.EquipItems);
            BaseCharacterModel Model = character.InstantiateModel(modelContainer);
            CharacterModelByEntityId[character.EntityId] = Model;
            CharacterModelByEntityId.TryGetValue(character.EntityId, out SelectedModel);
            PlayerCharacterBodyPartComponent[] comps = Model.transform.root.GetComponentsInChildren<PlayerCharacterBodyPartComponent>();
            for (int i = 0; i < comps.Length; ++i)
            {
                comps[i].SetupCharacterModelEvents(Model);
                comps[i].ApplyModelAndColorBySavedData(character.PublicInts);
            }
            SelectedModel.SetEquipItems(character.EquipItems, character.SelectableWeaponSets, character.EquipWeaponSet, characterEntity.IsWeaponsSheathed);
        }

        public void UpdateCharacter(LiteNetLibManager.LiteNetLibSyncList.Operation operation, int index)
        {
            UpdateCharacterSetup();
        }
    }
}
