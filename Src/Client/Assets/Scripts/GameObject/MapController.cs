using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class MapController : MonoBehaviour {

	public Collider minimapBoundBox;
	// Use this for initialization
	void Start () {
		MiniMapManager.Instance.UpdateMiniMap(minimapBoundBox);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
