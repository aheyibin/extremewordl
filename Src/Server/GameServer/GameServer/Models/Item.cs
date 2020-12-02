﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        TCharacterItem dbItem;

        public int ItemID;
        public int Count;

        public Item(TCharacterItem item)
        {
            this.dbItem = item;
            this.ItemID = item.ItemID;
            this.Count = item.ItemCount;
        }

        public void Add(int count)
        {
            this.Count += count;
            dbItem.ItemCount += count;
        }

        public void Remove(int count)
        {
            this.Count -= count;
            dbItem.ItemCount -= count;
        }

        public bool Use(int count=1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", this.ItemID, this.Count);
        }
    }
}
