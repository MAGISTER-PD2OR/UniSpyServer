﻿using UniSpyLib.Abstraction.Interface;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Handler.SystemHandler.Manager;

namespace NATNegotiation.Handler.CommandHandler
{
    public class InitHandler : NNCommandHandlerBase
    {
        protected new InitRequest _request;
        public InitHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (InitRequest)request;
        }

        protected override void CheckRequest()
        {
            _request.Parse();

            string key = _session.RemoteEndPoint.ToString() + "-" + _request.PortType.ToString();

            if (!NNManager.SessionPool.TryGetValue(key, out _))
            {
                NNManager.SessionPool.TryAdd(key, _session);
            }
        }

        protected override void DataOperation()
        {
            _session.UserInfo.Parse(_request);
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = new InitResponse(_request, _session.RemoteEndPoint).BuildResponse();

                _request
                .SetPacketType(NatPacketType.InitAck)
                .BuildResponse();
        }


        protected override void Response()
        {
            base.Response();
            NNManager
                .Negotiate(
                _request.PortType,
                _request.Version,
                _request.Cookie);
        }
    }
}
