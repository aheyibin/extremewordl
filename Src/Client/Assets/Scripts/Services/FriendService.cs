using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Network;
using SkillBridge.Message;

public class FriendService : Singleton<FriendService>,IDisposable
{
    public UnityAction OnFriendUpdate;
   
    public void Init()
    {

    }
    public FriendService()
    {
        MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
        MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
        MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
    }
    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
        MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
        MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
    }

    public void SendFriendAddRequest(int friendId, string friendName)
    {
        Debug.Log("SendFriendAddRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddRequest = new FriendAddRequest();
        message.Request.friendAddRequest.FromId = Models.User.Instance.CurrentCharacter.Id;
        message.Request.friendAddRequest.FromName = Models.User.Instance.CurrentCharacter.Name;
        message.Request.friendAddRequest.ToId = friendId;
        message.Request.friendAddRequest.ToName = friendName;
        NetClient.Instance.SendMessage(message);
    }
    private void OnFriendAddRequest(object sender, FriendAddRequest request)
    {
        UIMessageBox confirm = MessageBox.Show(string.Format("{0} 请求加你为好友", request.FromName), "好友请求", MessageBoxType.Confirm,"接受","拒绝");
        confirm.OnYes = () =>
        {
            this.SendFriendAddResponse(true, request);
        };
        confirm.OnNo = () =>
        {
            this.SendFriendAddResponse(false, request);
        };
    }

    public void SendFriendAddResponse(bool accept, FriendAddRequest request)
    {
        Debug.Log("SendFriendAddResponse");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddResponse = new FriendAddResponse();
        message.Request.friendAddResponse.Result = accept ? Result.Success : Result.Failed;
        message.Request.friendAddResponse.Errormsg = accept ? "对方同意" : "对方拒绝了你的请求";
        message.Request.friendAddResponse.Requet = request;
        NetClient.Instance.SendMessage(message);
    }
    private void OnFriendAddResponse(object sender, FriendAddResponse response)
    {
        if (response.Result==Result.Success)
        {
            MessageBox.Show(response.Requet.ToName + " 接受了您的请求", "添加好友成功");
        }
        else
        {
            MessageBox.Show(response.Errormsg, "添加好友失败");
        }
    }

    private void OnFriendList(object sender, FriendListResponse response)
    {
        Debug.Log("OnFriendList");
        FriendManager.Instance.allFriends = response.Friends;
        if (this.OnFriendUpdate!=null)
        {
            this.OnFriendUpdate();
        }
    }

    public void SendFriendRemoveRequest(int id, int friendId)
    {
        Debug.Log("SendFriendRemoveRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendRemove = new FriendRemoveRequest();
        message.Request.friendRemove.Id = id;
        message.Request.friendRemove.friendId = friendId;
        NetClient.Instance.SendMessage(message);
    }
    private void OnFriendRemove(object sender, FriendRemoveResponse response)
    {
        if (response.Result==Result.Success)
        {
            MessageBox.Show("删除成功","删除好友");
        }
        else
        {
            MessageBox.Show("删除失败", "删除好友",MessageBoxType.Error);
        }
    }
}
