using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public Image icon;
    public Text name;
    public Text level;
    public Text cls;
    public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NFriendInfo info;

    bool isEquiped = false;

    public void SetFriendInfo(NFriendInfo item)
    {
        this.info = item;
        if(this.name != null) this.name.text = this.info.friendInfo.Name;
        if(this.cls != null) this.cls.text = this.info.friendInfo.Class.ToString();
        if(this.level != null) this.level.text = this.info.friendInfo.Level.ToString();
        if (this.status != null) this.status.text = this.info.Status == 1 ? "在线" : "离线";

    }




}
