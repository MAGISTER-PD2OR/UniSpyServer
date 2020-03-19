﻿using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.RegisterCDKey
{
    public class RegisterCDKeyHandler : CommandHandlerBase
    {
        public RegisterCDKeyHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("cdkeyenc"))
            {
                _errorCode = Enumerator.GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                var result = db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid
                && s.Namespaceid == session.UserInfo.NamespaceID
                && s.Productid == session.UserInfo.productID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    _errorCode = Enumerator.GPErrorCode.DatabaseError;
                }

                db.Subprofiles.Where(s => s.Profileid == session.UserInfo.Profileid
            && s.Namespaceid == session.UserInfo.NamespaceID
            && s.Productid == session.UserInfo.productID).FirstOrDefault()
            .Cdkeyenc = recv["cdkeyenc"];
                db.SaveChanges();
            }
        }

        protected override void ConstructResponse(GPCMSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = @"\rc\final\";
        }
    }
}
