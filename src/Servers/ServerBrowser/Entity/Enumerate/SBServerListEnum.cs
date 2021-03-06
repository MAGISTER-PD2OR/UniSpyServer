﻿namespace ServerBrowser.Entity.Enumerate
{
    public enum SBClientRequestType
    {
        ServerListRequest,
        ServerInfoRequest,
        SendMessageRequest,
        KeepAliveReply,
        MapLoopRequest,
        PlayerSearchRequest,
    }

    public enum SBServerResponseType
    {
        PushKeysMessage = 1,
        PushServerMessage,
        KeepAliveMessage,
        DeleteServerMessage,
        MapLoopMessage,
        PlayerSearchMessage
    }

    public enum SBProtocolVersion
    {
        ListProtocolVersion1 = 0,
        ListEncodingVersion = 3
    }

    public enum SBServerListUpdateOption
    {
        /// <summary>
        /// This is used to tell server browser client want main server list (keys and values)
        /// </summary>
        GeneralRequest = 0,
        SendFieldForAll = 1,
        /// <summary>
        /// this is used to check the connection to server browser
        /// </summary>
        NoServerList = 2,
        PushUpdates = 4,
        AlternateSourceIP = 8,
        SendGroups = 32,
        NoListCache = 64,
        LimitResultCount = 128
    }

    public enum GameServerFlags
    {
        /// <summary>
        /// game can directly send request to dedicate server
        /// </summary>
        UnsolicitedUDPFlag = 1,
        /// <summary>
        /// private ip exist
        /// </summary>
        PrivateIPFlag = 2,
        /// <summary>
        /// connect with nat neg
        /// </summary>
        ConnectNegotiateFlag = 4,
        /// <summary>
        /// server has icmp
        /// </summary>
        ICMPIPFlag = 8,
        /// <summary>
        /// non standard query port
        /// </summary>
        NonStandardPort = 16,

        /// <summary>
        /// nonstandard private port
        /// </summary>
        NonStandardPrivatePortFlag = 32,
        /// <summary>
        /// has standard keys
        /// </summary>
        HasKeysFlag = 64,
        /// <summary>
        /// has full rules keys
        /// </summary>
        HasFullRulesFlag = 128
    }
}
