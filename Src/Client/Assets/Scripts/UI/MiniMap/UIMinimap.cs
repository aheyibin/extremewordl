﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour {

    public Text miniMapName;
    public Image minimap;
    public Image arrow;
    public Collider minimapBoundingBox;

    private Transform playerTransform;

    void Start()
    {
        this.InitMap();
    }

    void InitMap ()
    {
        this.miniMapName.text = User.Instance.CurrentMapData.Name;
        if (this.minimap.overrideSprite==null)
        {
            this.minimap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        }
        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;
        //this.playerTransform = User.Instance.CurrentCharacterObject.transform;
    }   

    void Update()
    {
        if (playerTransform==null)
        {
            playerTransform = MiniMapManager.Instance.PlayerTransform;
        }
        if (minimapBoundingBox==null||playerTransform==null)
        {
            return;
        }
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}
