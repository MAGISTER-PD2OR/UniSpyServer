﻿using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.Message
{
    public class UTMReply
    {
        public static string BuildUTMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ChatReplyCode.UTM, name, message);
        }

    }
}
