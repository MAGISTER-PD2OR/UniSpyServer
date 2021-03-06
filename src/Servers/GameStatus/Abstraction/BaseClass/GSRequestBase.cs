﻿using GameStatus.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    public class GSRequestBase : RequestBase
    {
        protected Dictionary<string, string> _request;
        public uint OperationID { get; protected set; }
        public string CommandName { get; protected set; }
        public GSRequestBase(Dictionary<string, string> request)
        {
            _request = request;
            CommandName = request.Keys.First();
        }

        public virtual GSError Parse()
        {

            if (!_request.ContainsKey("lid") && !_request.ContainsKey("id"))
            {
                return GSError.Parse;
            }

            if (_request.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(_request["lid"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (_request.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_request["id"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            return GSError.NoError;
        }
    }
}
