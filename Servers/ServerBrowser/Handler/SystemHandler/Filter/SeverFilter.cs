﻿using QueryReport.Entity.Structure;
using System.Collections.Generic;
using System.Net;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter
{
    public class ServerFilter
    {
        public static IEnumerable<KeyValuePair<EndPoint, DedicatedGameServer>> GetFilteredServer(IEnumerable<KeyValuePair<EndPoint, DedicatedGameServer>> rawServer, string filter)
        {
            //TODO
            //We filter server for next step
            return rawServer;
        }
    }
}