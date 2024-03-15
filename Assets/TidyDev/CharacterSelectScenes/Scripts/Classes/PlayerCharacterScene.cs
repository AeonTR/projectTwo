using UnityEngine;
using MultiplayerARPG;

namespace TidyDev
{
    [System.Serializable]
    public class PlayerCharacterScene
    {
        public PlayerCharacter playerCharacter;
        public GameObject sceneObject;
        public Transform playerCenter;
        public AnimationClip animationClip;
    }
}