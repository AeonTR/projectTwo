namespace MultiplayerARPG
{
    public struct UIEquipmentEnchantingData
    {
        public EquipmentBonus equipmentBonus;
        public int equippedCount;
        public UIEquipmentEnchantingData(EquipmentBonus equipmentBonus, int equippedCount)
        {
            this.equipmentBonus = equipmentBonus;
            this.equippedCount = equippedCount;
        }
    }
}
