﻿using CDKey.Handler.CommandHandler.SKey;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDKey.Handler.CommandSwitcher
{
    public class CDKeyCommandSwitcher
    {
        public static void Switch(ISession client, string message)
        {
            message.Replace(@"\r\n", "").Replace("\0", "");
            string[] keyValueArray = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(keyValueArray);

            try
            {
                switch (recv.Keys.First())
                {
                    //keep client alive request, we skip this
                    case "ka":
                        Console.WriteLine("Received keep alive command");
                        break;
                    case "auth":
                        break;
                    case "resp":
                        break;
                    case "skey":
                        SKeyHandler.IsCDKeyValid(client, recv);
                        break;
                    case "disc"://disconnect from server
                        break;
                    case "uon":
                        break;

                    default:
                        LogWriter.UnknownDataRecieved(message);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }
        }
    }
}
