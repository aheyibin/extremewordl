using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Network;

namespace GameServer.Entities
{
    /// <summary>
    /// Character
    /// 玩家角色类
    /// </summary>
    class Character : CharacterBase,IPostResponser
    {
       
        public TCharacter Data;

        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public FriendManager FriendManager;


        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Id = cha.ID;
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 1;//cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);

            this.Info.Bag = new NbagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Firends);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold==value)
                {
                    return;

                }
                else
                {
                    this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                    this.Data.Gold = value;
                }
            }
        }

        public void PostProcess(NetMessageResponse response)
        {
            this.FriendManager.PostProcess(response);
            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(response);
            }
        }

        public void Clear()
        {
            this.FriendManager.UpdateFriendInfo(this.Info, 0);
        }
    }
}
