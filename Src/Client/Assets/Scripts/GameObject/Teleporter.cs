using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;
using UnityEditor;
using Common.Data;
using Services;

public class Teleporter : MonoBehaviour 
{

	public int ID;
	Mesh Mesh = null;
	// Use this for initialization
	void Start () {
		this.Mesh = this.GetComponent<MeshFilter>().sharedMesh;
	}
	

#if UNITY_EDITOR
	void OnDrawGizmos()
    {
		Gizmos.color = Color.green;
        if (this.Mesh!=null)
        {
			Gizmos.DrawWireMesh(this.Mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, this.transform.rotation, this.transform.localScale);
        }
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif

	void OnTriggerEnter(Collider other)
    {
		PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if (playerInputController != null && playerInputController.isActiveAndEnabled)
        {
			TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td==null)
            {
				Debug.LogErrorFormat("TeleporterObject:Character[{0}] enter teleporter [{1}],but teleporterDefine not existed", playerInputController.character.Info.Name, this.ID);
				return;
            }
			Debug.LogFormat("TeleporterObject: Character[{0}] enter teleporter[{1}：{2}]", playerInputController.character.Info.Name, td.ID, td.Name);
            if (td.LinkTo>0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleporter(this.ID);
                }
                else
                {
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkTo {1} Error!", td.ID, td.LinkTo);
                }
            }
        }
    }
}
