﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NATNegotiation.Entity.Structure;

namespace NatNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {

        public static List<ClientInfo> ClientList = new List<ClientInfo>();
        private System.Timers.Timer _CheckTimer = new System.Timers.Timer { Enabled = true, Interval = 10000, AutoReset = true };


        public NatNegServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            
            _CheckTimer.Start();
            _CheckTimer.Elapsed+=CheckClientTimeOut;
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            //check and add client into clientList
            if (ClientList.Where(c => c.EndPoint == endPoint).Count() == 0)
            {
                ClientList.Add(new ClientInfo { EndPoint = endPoint });
            }
            CommandSwitcher.Switch(this, endPoint, message);
        }

        private void CheckClientTimeOut(object sender,System.Timers.ElapsedEventArgs e)
        {
            ToLog("Check timeout excuted!");
            foreach (var c in ClientList)
            {
                if (DateTime.Now.Second - c.LastPacketTime.Second > 120)
                {
                    ClientList.Remove(c);
                    ToLog("deleted client");
                }
            }
            foreach (var c in ClientList)
            {
                if (c.Connected)
                    if (c.GotConnectAck)
                        if (DateTime.Now.Second - c.SentConnectPacketTime.Second > 5)
                        {
                            //send connect packet to c it self
                            ConnectHandler.SendConnectPacket(this, c, null);
                        }
            }
            foreach (var c in ClientList)
            {
                if (DateTime.Now.Second - c.ConnectTime.Second > 30)
                    if (!c.Connected)
                    {
                        //send DeadHeatBeat notice
                        ConnectHandler.SendDeadHeartBeatNotice(this, c);
                    }
            }
        }
    }
}
