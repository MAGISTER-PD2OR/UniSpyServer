﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Channel;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response;
using Chat.Entity.Structure.Response.General;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    public class MODEHandler : ChatLogedInHandlerBase
    {
        new MODERequest _request;
        ChatChannel _channel;
        ChatChannelUser _user;
        public MODEHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (MODERequest)request;
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            switch (_request.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                case ModeRequestType.DisableUserQuietFlag:
                    //we do not need to find user and its channel here
                    break;
                default:
                    GetChannelAndUser();
                    break;
            }
        }
        private void GetChannelAndUser()
        {
            if (_session.UserInfo.JoinedChannels.Count == 0)
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatError.IRCError;
                _sendingBuffer = ChatIRCError.BuildNoSuchChannelError(_request.ChannelName);
                return;
            }

            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatError.Parse;
                _sendingBuffer = ChatIRCError.BuildNoSuchNickError();
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            switch (_request.RequestType)
            {
                case ModeRequestType.EnableUserQuietFlag:
                    _session.UserInfo.SetQuietModeFlag(true);
                    break;
                case ModeRequestType.DisableUserQuietFlag:
                    _session.UserInfo.SetQuietModeFlag(false);
                    break;
                default:
                    ProcessOtherModeRequest();
                    break;
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                string modes = _channel.Property.ChannelMode.GetChannelMode();
                _sendingBuffer = MODEReply.BuildModeReply(
                     _channel.Property.ChannelName, modes);
            }
        }

        private void ProcessOtherModeRequest()
        {
            //we check if the user is operator in channel
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatError.DataOperation;
                return;
            }
            _channel.Property.SetProperties(_user, _request);
        }
    }
}
