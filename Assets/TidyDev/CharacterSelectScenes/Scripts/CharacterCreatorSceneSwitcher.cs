using System.Collections.Generic;
using UnityEngine;
using MultiplayerARPG;
using MultiplayerARPG.GameData.Model.Playables;

namespace TidyDev
{
    public class CharacterCreatorSceneSwitcher : MonoBehaviour
    {
        public List<PlayerCharacterScene> playerCharacterScenes = new List<PlayerCharacterScene>();

        [SerializeField] private UICharacterList _uiCharacterList;
        [SerializeField] private UICharacterCreate _uiCharacterCreate;

        private GameObject _tmpCharacter;

        private void OnValidate()
        {
            _uiCharacterList = FindObjectOfType<UICharacterList>(true);
            _uiCharacterCreate = FindObjectOfType<UICharacterCreate>(true);
        }

        private void OnEnable()
        {
            if (!_uiCharacterCreate || !_uiCharacterList) return;

            _uiCharacterCreate.eventOnSelectCharacterClass.AddListener(ClassChange);
        }

        private void LateUpdate()
        {
            if(!_uiCharacterCreate.IsVisible())
            {
                foreach (PlayerCharacterScene pcs in playerCharacterScenes)
                {
                    if (_tmpCharacter) Destroy(_tmpCharacter);
                    pcs.sceneObject.SetActive(false);
                }
            }
        }

        private void ClassChange(BaseCharacter baseCharacter)
        {
            PlayerCharacter selectedPlayerCharacter = baseCharacter as PlayerCharacter;

            if (selectedPlayerCharacter != null)
            {
                Debug.Log("Class Changed");
                if (!_uiCharacterCreate.SelectedModel) return;
                _tmpCharacter = _uiCharacterCreate.SelectedModel.gameObject;

                PlayableCharacterModel characterModel = _uiCharacterCreate.SelectedModel.GetComponent<PlayableCharacterModel>();
                foreach (PlayerCharacterScene pcs in playerCharacterScenes)
                {
                    if(pcs.sceneObject) pcs.sceneObject.SetActive(false);
                    if(pcs.playerCharacter == selectedPlayerCharacter)
                    {
                        if(pcs.sceneObject) pcs.sceneObject.SetActive(true);
                        if (pcs.playerCenter) _uiCharacterCreate.SelectedModel.transform.SetParent(pcs.playerCenter, false);
                        if (pcs.animationClip) 
                        {
                            if(characterModel)
                            {
                                MultiplayerARPG.GameData.Model.Playables.ActionAnimation _anim = new MultiplayerARPG.GameData.Model.Playables.ActionAnimation();

                                _anim.state.clip = pcs.animationClip;
                                _anim.state.animSpeedRate = 1;
                                characterModel.PlayNpcActionAnimationDirectly(_anim);
                            }
                        }
                    }
                }
            }
        }
    }
}