﻿using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Error;
using Serilog.Events;
using System.Collections.Generic;
using System.Diagnostics;

namespace PresenceConnectionManager.Handler
{
    public class PCMCommandHandlerBase:CommandHandlerBase
    {
        protected GPErrorCode _errorCode;
        protected string _sendingBuffer;
        protected ushort _operationID;
        protected uint _namespaceid;
        protected uint _profileid;
        protected uint _productid;
        protected Dictionary<string, string> _recv;
        protected GPCMSession _session;
        public PCMCommandHandlerBase(IClient client, Dictionary<string, string> recv):base(client)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _operationID = 1;
            _namespaceid = 0;
            _session = (GPCMSession)client.GetInstance();
        }

        public override void Handle()
        {
            base.Handle();

            CheckRequest();

            if (_errorCode != GPErrorCode.NoError)
            {
                //TODO
                ErrorMsg.SendGPCMError(_client, _errorCode, _operationID);
                return;
            }

            DataOperation();

            if (_errorCode == GPErrorCode.DatabaseError)
            {
                //TODO
                ErrorMsg.SendGPCMError(_client, _errorCode, _operationID);
                return;
            }

            ConstructResponse();

            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPCMError(_client, _errorCode, _operationID);
                return;
            }

            Response();
        }

        protected virtual void CheckRequest()
        {
            if (_recv.ContainsKey("id"))
            {
                if (!ushort.TryParse(_recv["id"], out _operationID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(_recv["namespaceid"], out _namespaceid))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected virtual void DataOperation() { }

        protected virtual void ConstructResponse() { }

        protected virtual void Response()
        {
            if (_sendingBuffer == null)
            {
                return;
            }

            _client.SendAsync(_sendingBuffer);
        }
    }
}