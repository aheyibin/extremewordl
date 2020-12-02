using Common.Data;
using System;
using System.Collections.Generic;

namespace Managers
{
	public class NpcManager : Singleton<NpcManager>
	{

		public delegate bool NpcActionHandler(NpcDefine npc);
		Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

		public void RegisterNpcEvent(NpcFunction function,NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
				eventMap[function] = action;
            }
            else
            {
				eventMap[function] += action;
            }
        }

		public NpcDefine GetNpcDefine(int npcid)
		{
			NpcDefine npc = null;
			DataManager.Instance.NPCs.TryGetValue(npcid, out npc);
			return npc;
		}

		public bool Interactive(int npcid)
        {
            if (DataManager.Instance.NPCs.ContainsKey(npcid))
            {
				var npc = DataManager.Instance.NPCs[npcid];
				return Interactive(npc);
			}
			return false;
        }

		public bool Interactive(NpcDefine npc)
        {
            if (npc.Type==NpcType.Task)
            {
				return DoTaskInteractive(npc);
            }
            else if (npc.Type==NpcType.Functional)
            {
				return DoFunctionInteractive(npc);
            }
            return false;
        }

        private bool DoTaskInteractive(NpcDefine npc)
        {
            MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
            return true;
        }
        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (npc.Type!=NpcType.Functional)
            {
                return false;
            }
            if (!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }
            return eventMap[npc.Function](npc);
        }

    }
}