﻿using System;

namespace ServerBrowser.Entity.Enumerator
{
    public enum SBClientRequestType
    {
        ServerListRequest,
        ServerInfoRequest,
        SendMessageRequest,
        KeepAliveReply,
        MapLoopRequest,
        PlayerSearchRequest
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
        SendFieldForAll = 1,
        NoServerList = 2,
        AlternateSourceIP=8,
        PushUpdates = 4,
        SendGroups = 32,
        NoListCache = 64,
        LimitResultCount = 128
    }

    public enum GameServerFlags
    {
        ServerEnd = 0,
        UnsolicitedUDPFlag = 1,
        PrivateIPFlag = 2,
        ConnectNegotiateFlag = 4,
        ICMPIPFlag = 8,
        NonStandardPort = 16,
        NonStandardPrivatePortFlag = 32,
        HasKeysFlag = 64,
        HasFullRulesFlag = 128
    }
}