﻿using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using UniSpyLib.UniSpyConfig;
using Serilog.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpClient = NetCoreServer.TcpClient;
using System.Collections.Generic;

namespace UniSpyLib.Network
{

    public abstract class TCPClientBase : TcpClient
    {
        private EndPoint _endPoint;
        /// <summary>
        /// We automatic connect to remote server address
        /// </summary>
        public TCPClientBase() : base
            (
                ConfigManager.Config.Servers.Where(s => s.Name == ServerManagerBase.ServerName).First().RemoteAddress
               , ConfigManager.Config.Servers.Where(s => s.Name == ServerManagerBase.ServerName).First().RemotePort
            )
        {
        }

        protected override void OnConnected()
        {
            _endPoint = Socket.RemoteEndPoint;
            LogWriter.ToLog(LogEventLevel.Information, $"[Proxy] [Conn] IRC server: {_endPoint}");
        }

        protected override void OnDisconnected()
        {
            LogWriter.ToLog(LogEventLevel.Information, $"[Proxy] [Disc] IRC server: {_endPoint}");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                 $"[Proxy] [Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            byte[] tempBuffer = new byte[size];
            Array.Copy(buffer, 0, tempBuffer, 0, size);
            OnReceived(tempBuffer);
        }

        protected virtual void OnReceived(byte[] buffer)
        {
            OnReceived(Encoding.ASCII.GetString(buffer));
        }
        protected virtual void OnReceived(string buffer)
        {
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.LogNetworkTraffic("Proxy", "Send", StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size));
            return base.SendAsync(buffer, offset, size);
        }

        public override long Send(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Proxy] [Send] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");
            return base.Send(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }
    }
}
