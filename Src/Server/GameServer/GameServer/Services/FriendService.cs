using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using SkillBridge.Message;
using Network;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class FriendService:Singleton<FriendService>
    {
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveRequest);
        }

        public void Init()
        {

        }

        void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);

            if (request.ToId==0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name==request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }

            NetConnection<NetSession> friend = null;
            if (request.ToId>0)
            {
                if (character.FriendManager.GetFriendInfo(request.ToId)!=null)
                {
                    sender.Session.Response.friendAddResponse = new FriendAddResponse();
                    sender.Session.Response.friendAddResponse.Result = Result.Failed;
                    sender.Session.Response.friendAddResponse.Errormsg = "我们已经是好友了";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(request.ToId);
            }
            if (friend==null)
            {
                sender.Session.Response.friendAddResponse = new FriendAddResponse();
                sender.Session.Response.friendAddResponse.Result = Result.Failed;
                sender.Session.Response.friendAddResponse.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardRequest:: FromId:{0} FromName:{1} ToId:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            friend.Session.Response.friendAddRequest = request;
            friend.SendResponse();
        }

        void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse::character:{0} Result:{1} FromID:{2} ToID:{3}", character.Id, response.Result, response.Requet.FromId, response.Requet.ToId);
            sender.Session.Response.friendAddResponse = response;
            if (response.Result==Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(response.Requet.FromId);
                if (requester==null)
                {
                    sender.Session.Response.friendAddResponse.Result = Result.Failed;
                    sender.Session.Response.friendAddResponse.Errormsg = "请求者已下线";
                }
                else
                {
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddResponse = response;
                    requester.Session.Response.friendAddResponse.Result = Result.Success;
                    requester.Session.Response.friendAddResponse.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        void OnFriendRemoveRequest(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemoveRequest::character:{0} FriendRelationID:{1}", character.Id, request.Id);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = request.Id;
            if (character.FriendManager.RemoveFriendByID(request.Id))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(request.friendId);
                if (friend!=null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendID(character.Id);
                }
                else
                {
                    this.RemoveFriend(request.friendId, character.Id);
                }
            }
            else
            {
                sender.Session.Response.friendRemove.Result = Result.Failed;
            }
            DBService.Instance.Save();
            sender.SendResponse();

        }

        void RemoveFriend(int characterId,int friendId)
        {
            var removeItem = DBService.Instance.Entities.TCharacterFriends.FirstOrDefault(v => v.CharacterID == characterId && v.FriendID == friendId);
            if (removeItem!=null)
            {
                DBService.Instance.Entities.TCharacterFriends.Remove(removeItem);
            }
        }
    }
}
