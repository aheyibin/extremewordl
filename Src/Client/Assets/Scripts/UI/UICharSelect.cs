using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharSelect : MonoBehaviour {

	public GameObject[] roles;
	public Image[] titles;

	public Text mDescription;
	// Use this for initialization
	private int currentIndex = 1;
	public InputField roleName;

	public GameObject panelCreate;
	public GameObject panelSelect;

	public GameObject uiCharPrefab;
	public Transform uiCharList;
	public List<GameObject> uiChars;

	public int roleIndex;
	public int CurrentIndex
	{
        get { return currentIndex; }
        set { currentIndex = value; UpdateUI(); }
	}

	void Start () {
		//DataManager.Instance.Load();
		//UpdateUI();
		UserService.Instance.OnRoleCreate += OnRoleCreate;
		InitCharacterSelect(true);
		OnCLickSelectCharacter(roleIndex, User.Instance.Info.Player.Characters[roleIndex].Class);
	}

	private void UpdateUI()
    {
        for (int i = 0; i < 3; i++)
        {
			roles[i].SetActive( i == currentIndex-1);
			titles[i].gameObject.SetActive(i == currentIndex-1);
		}
		mDescription.text = DataManager.Instance.Characters[currentIndex].Description;
	}

	public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(roleName.text))
        {
			MessageBox.Show("请输入角色名");
			return;
        }
		UserService.Instance.SendCreateRole(roleName.text, (CharacterClass)currentIndex);
    }
	void OnRoleCreate(Result result, string messsage)
    {
		if (result == Result.Success)
		{
			Debug.Log("角色创建成功");
			InitCharacterSelect(true);
			roleIndex = User.Instance.Info.Player.Characters.Count - 1;
			OnCLickSelectCharacter(roleIndex, User.Instance.Info.Player.Characters[roleIndex].Class);
		}
		else
		{
			MessageBox.Show(messsage, "错误", MessageBoxType.Error);
		}
	}

	public void InitCharacterSelect(bool init)
    {
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);
		if (init)
        {
            foreach (var item in uiChars)
            {
				Destroy(item);
            }
			uiChars.Clear();
            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
				GameObject go = Instantiate(uiCharPrefab, this.uiCharList);
				UICharInfo charInfo = go.GetComponent<UICharInfo>();
				charInfo.info = User.Instance.Info.Player.Characters[i];
				int idx = i;
				Button btn = go.GetComponentInChildren<Button>();
				btn.onClick.AddListener(() => { OnCLickSelectCharacter(idx, User.Instance.Info.Player.Characters[idx].Class); });
				//print(idx.ToString()+ User.Instance.Info.Player.Characters[idx].Class.ToString());
				uiChars.Add(go);
				go.SetActive(true);
			}
        }
	}

	public void OnClickNewUser()
    {
		panelCreate.SetActive(true);
		panelSelect.SetActive(false);
	}

	public void OnCLickSelectCharacter(int idx,CharacterClass cls)
    {

		for (int i = 0; i < uiChars.Count; i++)
        {
			if (idx == i)
			{
				uiChars[i].GetComponentInChildren<Image>().color = new Color(229, 163, 27, 100);
			}
			else
			{
				uiChars[i].GetComponentInChildren<Image>().color = new Color(229, 163, 27, 0);
			}
		}
		CurrentIndex = (int)cls;
		roleIndex = idx;
	}

	public void OnClickEnter()
    {
		UserService.Instance.SendUserGameEnter(roleIndex);
    }
	public void ReturnLogin()
    {
		SceneManager.Instance.LoadScene("Loading");
	}
}
