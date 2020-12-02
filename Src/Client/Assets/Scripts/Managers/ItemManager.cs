using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);

                Debug.LogFormat("ItemManage:Init[{0}]", item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        public ItemDefine GetItem(int itemId)
        {
            return null;
        }

        private bool OnItemNotify(NStatus status)
        {
            if (status.Action==StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }

        private void AddItem(int id, int value)
        {
            Item item = null;
            if (this.Items.TryGetValue(id,out item))
            {
                item.Count += value;
            }
            else
            {
                item = new Item(id, value);
                this.Items.Add(id, item);
            }
            BagManager.Instance.AddItem(id, value);
        }

        private void RemoveItem(int id, int value)
        {
            if (!this.Items.ContainsKey(id))
            {
                return;
            }
            Item item = this.Items[id];
            if (item.Count<value)
            {
                return;
            }
            item.Count -= value;
            BagManager.Instance.RemoveItem(id, value);
        }
    }
}
