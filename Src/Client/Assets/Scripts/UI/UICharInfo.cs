using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {

    public SkillBridge.Message.NCharacterInfo info;

    public Text charLevel;
    public Text charName;
    public int tid;
    //public Text charClass;
    // Use this for initialization
    void Start()
    {
        if (info != null)
        {
            //this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
            this.charLevel.text = this.info.Level.ToString();
            tid = this.info.Tid;
        }
    }
}
