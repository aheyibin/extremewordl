using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour {


	public InputField username;
	public InputField password;
	public InputField again;
	// Use this for initialization
	public GameObject uiLogin;
	void Start () {
		UserService.Instance.OnRegister += OnRegister;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(username.text))
        {
			MessageBox.Show("请输入用户名");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
			MessageBox.Show("请输入密码");
		}
		else if (string.IsNullOrEmpty(again.text))
		{
			MessageBox.Show("请输入确认密码");
		}
		else if (password.text!=again.text)
		{
			MessageBox.Show("密码不一致");
		}
        else
        {
			UserService.Instance.SendRegister(username.text, password.text);
        }
	}
	void OnRegister(Result result,string message)
	{
        if (result==Result.Success)
        {
			MessageBox.Show("注册成功,请登录", "提示", MessageBoxType.Information).OnYes = () => {
				this.gameObject.SetActive(false);
				uiLogin.SetActive(true);
			};
		}
		else
			MessageBox.Show(message, "错误", MessageBoxType.Error);
	}
}
