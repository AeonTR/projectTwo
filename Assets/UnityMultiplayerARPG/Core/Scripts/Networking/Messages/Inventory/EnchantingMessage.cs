using LiteNetLib.Utils;

namespace MultiplayerARPG
{
    public struct RequestUseEnchantingPaperMessage : INetSerializable
    {
        public InventoryType inventoryType;
        public int index;
        public int paperId;

        public void Deserialize(NetDataReader reader)
        {
            inventoryType = (InventoryType)reader.GetByte();
            index = reader.GetPackedInt();
            paperId = reader.GetPackedInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)inventoryType);
            writer.PutPackedInt(index);
            writer.PutPackedInt(paperId);
        }
    }

    public struct ResponseUseEnchantingPaperMessage : INetSerializable
    {
        public UITextKeys message;

        public void Deserialize(NetDataReader reader)
        {
            message = (UITextKeys)reader.GetPackedUShort();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.PutPackedUShort((ushort)message);
        }
    }

    public struct RequestUseEnchantingCostMessage : INetSerializable
    {
        public InventoryType inventoryType;
        public int index;
        public int costId;

        public void Deserialize(NetDataReader reader)
        {
            inventoryType = (InventoryType)reader.GetByte();
            index = reader.GetPackedInt();
            costId = reader.GetPackedInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)inventoryType);
            writer.PutPackedInt(index);
            writer.PutPackedInt(costId);
        }
    }

    public struct ResponseUseEnchantingCostMessage : INetSerializable
    {
        public UITextKeys message;

        public void Deserialize(NetDataReader reader)
        {
            message = (UITextKeys)reader.GetPackedUShort();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.PutPackedUShort((ushort)message);
        }
    }
}
