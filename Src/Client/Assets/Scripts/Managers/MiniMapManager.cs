using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{ 
    class MiniMapManager : Singleton<MiniMapManager> {

        public UIMinimap minimap;
        private Collider minimapBoundBox;
        public Collider MinimapBoundBox
        {
            get { return minimapBoundBox; }
        }
        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.CurrentMapData.Resource);
        }

        public void UpdateMiniMap(Collider Minimapboundbox)
        {
            this.minimapBoundBox = Minimapboundbox;
            if (this.minimap!=null)
            {
                this.minimap.UpdateMap();
            }
        }
    }
}