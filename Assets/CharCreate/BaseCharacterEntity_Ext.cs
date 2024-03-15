using LiteNetLibManager;
using MultiplayerARPG.MMO;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class BaseCharacterEntity
    {
        [Category(5, "Character Settings")]
        public CharacterGender gender;
        public CharacterGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }
    }
}
