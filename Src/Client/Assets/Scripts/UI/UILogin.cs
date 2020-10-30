using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

	public InputField username;
	public InputField password;
	// Use this for initialization
	void Start () {
		UserService.Instance.OnLogin += OnLogin;
	}
	

	public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(username.text))
        {
			MessageBox.Show("请输入用户名");
        }
        else if (string.IsNullOrEmpty(password.text))
        {
			MessageBox.Show("请输入密码");
		}
        else
        {
			UserService.Instance.SendLogin(username.text, password.text);
        }
    }

	void OnLogin(Result result,string messsage)
    {
        if (result==Result.Success)
        {
            Debug.Log("登录成功");
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(messsage,"错误",MessageBoxType.Error);
        }
    }
}
