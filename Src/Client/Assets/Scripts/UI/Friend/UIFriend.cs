﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using System;
using Models;
using Managers;
using UnityEngine.UI;

public class UIFriend : UIWindow {

    public GameObject addBox;
    public Text text;

    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;
	// Use this for initialization
	void Start ()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
    }

    private void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        addBox.SetActive(true);
    }

    public void OnClickFriendAdd_Send()
    {
        string tips = "";
        OnFriendAddSubmit(text.text,out tips);
        addBox.SetActive(false);
        MessageBox.Show(tips);
    }

    private bool OnFriendAddSubmit(string input,out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input,out friendId))
        {
            friendName = input;
        }
        if (friendId==User.Instance.CurrentCharacter.Id||friendName==User.Instance.CurrentCharacter.Name)
        {
            tips = "开玩笑吗？不能添加自己哦";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    {
        MessageBox.Show("暂未开放");
    }

    public void OnClickFriendRemove()
    {
        if (selectedItem==null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友【{0}】吗", selectedItem.info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(selectedItem.info.Id, this.selectedItem.info.friendInfo.Id);
        };
    }

    void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    void InitFriendItems()
    {
        foreach (var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }
}