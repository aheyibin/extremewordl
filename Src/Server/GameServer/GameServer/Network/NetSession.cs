﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using SkillBridge.Message;
using GameServer.Services;

namespace Network
{
    class NetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }

        public void Disconected()
        {
            if (this.Character!=null)
            {
                UserService.Instance.CharacterLeave(this.Character);
            }
        }
    }
}
