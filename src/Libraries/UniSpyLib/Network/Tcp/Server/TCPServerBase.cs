﻿using UniSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;
using System.Net;
using System.Net.Sockets;

namespace UniSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public abstract class TCPServerBase : TcpServer
    {
        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TCPServerBase(IPEndPoint endpoint) : base(endpoint)
        {
        }

        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TCPServerBase(IPAddress address, int port) : base(address, port)
        {
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.ToLog(LogEventLevel.Error, error.ToString());
        }
    }
}
