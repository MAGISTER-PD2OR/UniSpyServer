﻿using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Entity.Structure;
using UniSpyLib.Logging;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Request;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.NewUser
{
    public class NewUserHandler : PSPCommandHandlerBase
    {
        private Users _user;
        private Profiles _profile;
        private Subprofiles _subProfile;
        protected NewUserRequest _request;
        public NewUserHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new NewUserRequest(recv);
        }

        protected enum _newUserStatus
        {
            CheckAccount,
            AccountNotExist,
            AccountExist,
            CheckProfile,
            ProfileNotExist,
            ProfileExist,
            CheckSubProfile,
            SubProfileNotExist,
            SubProfileExist
        }

        protected override void RequestCheck()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                try
                {
                    DatabaseOperationByType();
                }
                catch (Exception e)
                {
                    LogWriter.ToLog(LogEventLevel.Error, e.ToString());
                    _errorCode = GPError.DatabaseError;
                }

                //update other information
                if (_errorCode == GPError.NoError)
                {
                    UpdateOtherInfo();
                }
            }
        }

        protected override void BuildErrorResponse()
        {
            if (_errorCode >= GPError.NewUser && _errorCode <= GPError.NewUserUniquenickInvalid)
            {
                _sendingBuffer = $@"\nur\{_errorCode}\final\";
            }
            else
            {
                base.BuildErrorResponse();
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            if (ServerManagerBase.ServerName
             == UniSpyServerName.PSP)
            {
                //PSP NewUser
                _sendingBuffer = $@"\nur\\pid\{_subProfile.Profileid}\final\";
            }
            else if (ServerManagerBase.ServerName
                == UniSpyServerName.PCM)
            {
                //PCM NewUser
                _sendingBuffer =
                    $@"\nur\\userid\{_user.Userid}\profileid\{_subProfile.Profileid}\id\{_request.OperationID}\final\";
            }
        }

        private void UpdateOtherInfo()
        {
            using (var db = new retrospyContext())
            {

                if (_request.HasPartnerIDFlag)
                {
                    _subProfile.Partnerid = _request.PartnerID;
                }

                if (_request.HasProductIDFlag)
                {
                    _subProfile.Productid = _request.ProductID;
                }

                if (_request.HasGameNameFlag)
                {
                    _subProfile.Gamename = _request.GameName;
                }

                if (_request.HasGamePortFlag)
                {
                    _subProfile.Port = _request.GamePort;
                }

                if (_request.HasCDKeyEncFlag)
                {
                    _subProfile.Cdkeyenc = _request.CDKeyEnc;
                }

                db.Subprofiles.Update(_subProfile);
                db.SaveChanges();
            }
        }

        private void DatabaseOperationByType()
        {
            using (var db = new retrospyContext())
            {
                switch (_newUserStatus.CheckAccount)
                {
                    case _newUserStatus.CheckAccount:
                        var users = db.Users.Where(u => u.Email == _request.Email)
                                                     .Select(u => u);
                        if (users.Count() == 0)
                        {
                            goto case _newUserStatus.AccountNotExist;
                        }
                        else if (users.Count() == 1)
                        {
                            _user = users.First();
                            goto case _newUserStatus.AccountExist;
                        }
                        else
                        {
                            // double user in database
                            _errorCode = GPError.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in User table!");
                            break;
                        }

                    case _newUserStatus.AccountNotExist:
                        _user = new Users { Email = _request.Email, Password = _request.Password };
                        db.Users.Add(_user);
                        db.SaveChanges();
                        goto case _newUserStatus.CheckProfile;

                    case _newUserStatus.AccountExist:

                        if (_user.Password != _request.Password)
                        {
                            _errorCode = GPError.NewUserBadPasswords;
                            break;
                        }
                        else
                        {
                            goto case _newUserStatus.CheckProfile;
                        }

                    case _newUserStatus.CheckProfile:
                        var profiles = db.Profiles.Where(p => p.Userid == _user.Userid && p.Nick == _request.Nick);
                        if (profiles.Count() == 0)
                        {
                            goto case _newUserStatus.ProfileNotExist;
                        }
                        else if (profiles.Count() == 1)
                        {
                            //same nick name can not register two profiles
                            _profile = profiles.First();
                            goto case _newUserStatus.ProfileExist;
                        }
                        else
                        {
                            //there are two profiles we stop
                            _errorCode = GPError.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in Profile table!");
                            break;
                        }

                    case _newUserStatus.ProfileNotExist:
                        _profile = new Profiles { Userid = _user.Userid, Nick = _request.Nick };
                        db.Profiles.Add(_profile);
                        db.SaveChanges();
                        goto case _newUserStatus.CheckSubProfile;

                    case _newUserStatus.ProfileExist:
                    //we do nothing here

                    case _newUserStatus.CheckSubProfile:
                        var subProfiles = db.Subprofiles
                            .Where(s => s.Profileid == _profile.Profileid
                            && s.Namespaceid == _request.NamespaceID);
                        if (subProfiles.Count() == 0)
                        {
                            goto case _newUserStatus.SubProfileNotExist;
                        }
                        else if (subProfiles.Count() == 1)
                        {
                            _subProfile = subProfiles.First();
                            goto case _newUserStatus.SubProfileExist;
                        }
                        else
                        {
                            _errorCode = GPError.DatabaseError;
                            LogWriter.ToLog(LogEventLevel.Error, "There are two same records in SubProfile table!");
                            break;
                        }

                    case _newUserStatus.SubProfileNotExist:
                        //we create subprofile and return
                        _subProfile = new Subprofiles
                        {
                            Profileid = _profile.Profileid,
                            Uniquenick = _request.Uniquenick,
                            Namespaceid = _request.NamespaceID
                        };

                        db.Subprofiles.Add(_subProfile);
                        db.SaveChanges();
                        break;

                    case _newUserStatus.SubProfileExist:
                        _errorCode = GPError.NewUserUniquenickInUse;
                        break;
                }
            }
        }
    }
}
