﻿using LiteNetLib.Utils;
using System.Collections.Generic;

namespace MultiplayerARPG
{
    public struct ResponseGetGuildRequestsMessage : INetSerializable
    {
        public UITextKeys message;
        public List<SocialCharacterData> guildRequests;

        public void Deserialize(NetDataReader reader)
        {
            message = (UITextKeys)reader.GetPackedUShort();
            if (message == UITextKeys.NONE)
                guildRequests = reader.GetList<SocialCharacterData>();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.PutPackedUShort((ushort)message);
            if (message == UITextKeys.NONE)
                writer.PutList(guildRequests);
        }
    }
}
