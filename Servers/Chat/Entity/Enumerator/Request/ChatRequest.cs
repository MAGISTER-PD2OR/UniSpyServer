﻿using System;
namespace Chat.Entity.Structure.Enumerator.Request
{
    public enum ChatRequestType
    {
        CRYPT,
        USRIP,
        NICK,
        USER,
        LOGINPREAUTH,
        LOGIN,
        REGISTERNICK,
        QUIT,
        MODE,
        CDKEY,
        LIST,
        LISTLIMIT,
        JOIN,
        PART,
        PRIVMSG,
        NOTICE,
        UTM,
        ATM,
        TOPIC,
        SETGROUP,
        NAMES,
        WHOIS,
        WHO,
        INVITE,
        KICK,
        GETUDPRELAY,
        SETCHANKEY,
        SETCKEY,
        SETKEY,
        PONG,
    }
}