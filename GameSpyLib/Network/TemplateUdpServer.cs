﻿using System;
using System.Text;
using NetCoreServer;
using GameSpyLib.Logging;
using System.Net;
using System.Net.Sockets;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a UDP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateUdpServer : UdpServer
    {
        /// <summary>
        /// The name of the server that is started, used primary in logging functions.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// Initialize UDP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateUdpServer(string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerName = serverName;
        }

        /// <summary>
        /// Initialize UDP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateUdpServer(string serverName, IPAddress address, int port) : base(address, port)
        {
            ServerName = serverName;
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.Log.Write(LogLevel.Error, "[{0}] Error: {1}", ServerName, Enum.GetName(typeof(SocketError), error));
        }

        /// <summary>
        /// Send datagram to the given endpoint (asynchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.SendAsync" in your overrided function.
        /// </remarks>
        public override bool SendAsync(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Send] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            return base.SendAsync(endpoint, buffer, offset, size);
        }

        /// <summary>
        /// Send datagram to the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>Size of sent datagram</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.Send" in your overrided function.
        /// </remarks>
        public override long Send(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Send] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            return base.Send(endpoint, buffer, offset, size);
        }

        /// <summary>
        /// Handle datagram received notification
        /// </summary>
        /// <param name="endpoint">Received endpoint</param>
        /// <param name="buffer">Received datagram buffer</param>
        /// <param name="offset">Received datagram buffer offset</param>
        /// <param name="size">Received datagram buffer size</param>
        /// <remarks>
        /// Notification is called when another datagram was received from some endpoint
        /// We override this method in order to let it print the data it transmits, please call "base.OnReceived" in your overrided function
        /// </remarks>
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Recv] UDP data: {1}", ServerName, Encoding.UTF8.GetString(buffer, 0, (int)size));

        }
    }
}