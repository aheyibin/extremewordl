using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager> {

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
        return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.CurrentMapData.FileName);
    }

}
