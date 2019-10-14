﻿using PresenceConnectionManager;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.InviteTo
{
    public class InviteToHandler
    {
        //public static GPCMDBQuery DBQuery = null;
        public static void AddFriends(GPCMSession session, Dictionary<string, string> recv)
        {
           GPErrorCode error =  IsContainAllKeys(recv);
            if (error != GPErrorCode.NoError)
            {
                GameSpyLib.Common.GameSpyUtils.SendGPError(session, error, "Parsing error in request");
            }


        }
        public static GPErrorCode IsContainAllKeys(Dictionary<string,string> recv)
        {
            if (!recv.ContainsKey("products") || !recv.ContainsKey("sesskey"))
                return GPErrorCode.Parse;

            if (!recv.ContainsKey("sesskey"))
                return GPErrorCode.Parse ;

            return GPErrorCode.NoError;
           
        }
    }
}