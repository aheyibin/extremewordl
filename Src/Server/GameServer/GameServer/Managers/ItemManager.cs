using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;

        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        
        public ItemManager(Character character)
        {
            this.Owner = character;
            foreach (var item in character.Data.Items)
            {
                this.Items.Add(item.ItemID, new Item(item));
            }
        }

        public bool UseItem(int itemId,int count = 1)
        {
            Log.InfoFormat("[0] useitem [{1}:{2}]", this.Owner.Data.ID, itemId, count);
            Item item = null;
            if (this.Items.TryGetValue(itemId,out item))
            {
                if (item.Count<count)
                {
                    return false;
                }
                //to do useitem code
                item.Remove(count);
                return true;
            }
            return false;
        }

        public bool HasItem(int itemId)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId,out item))
            {
                return item.Count > 0;
            }
            return false;
        }

        public Item GetItem(int itemid)
        {
            Item item = null;
            this.Items.TryGetValue(itemid,out item);
            Log.InfoFormat("[{0}] GetItem [{1}:{2}]", this.Owner.Data.ID, itemid, item);
            return item;
        }

        public bool AddItem(int itemId,int count)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.CharacterID = Owner.Data.ID;
                dbItem.Owner = Owner.Data;
                dbItem.ItemID = itemId;
                dbItem.ItemCount = count;
                Owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                this.Items.Add(itemId, item);
            }
            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Add);
            Log.InfoFormat("[{0}] AddItem [{1}] Count:{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        public bool RemoveItem(int itemId,int count)
        {
            if (!this.Items.ContainsKey(itemId))
            {
                return false;
            }
            Item item = this.Items[itemId];
            if (item.Count<count)
            {
                return false;
            }
            item.Remove(count);
            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Delete);
            Log.InfoFormat("[{0}] RemoveItem [{1}] Count:{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in this.Items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.Count });
            }
        }
    }
}
