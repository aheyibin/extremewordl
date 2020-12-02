using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Common.Data;

public class MapTool : MonoBehaviour {

	[MenuItem("Map Tools/Export Teleporters")]
	// Use this for initialization
	public static void ExportTeleporters()
    {
		DataManager.Instance.Load();

		Scene current = EditorSceneManager.GetActiveScene();
		string currentScene = current.name;
        if (current.isDirty)
        {
			EditorUtility.DisplayDialog("提示", "请先保存场景", "确定");
			return;
        }
		List<Teleporter> allteleporters = new List<Teleporter>();

		foreach (var map in DataManager.Instance.Maps)
        {
			string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
				Debug.LogWarningFormat("Scene {0} not existed", sceneFile);
				continue;
            }
			EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);
			Teleporter[] teleporters = GameObject.FindObjectsOfType<Teleporter>();
            foreach (var item in teleporters)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(item.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0}中配置的Teleporter:[{1}] 中不存在",map.Value.Resource,item.ID), "确定");
                    return;
                }
                TeleporterDefine def = DataManager.Instance.Teleporters[item.ID];
                if (def.MapID!=map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0}中配置的Teleporter:[{1}] MapID:{2} 错误", map.Value.Resource, item.ID,def.MapID), "确定");
                    return;
                }
                def.Position = GameObjectTool.WorldToLogicN(item.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(item.transform.forward);
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }
}
