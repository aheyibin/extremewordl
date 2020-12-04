using Models;
using Services;
using UnityEngine.UI;
using UnityEngine;

class UIMain : MonoSingleton<UIMain> {

	public Text avatarName;
	public Text avatarLevel;

    // Use this for initialization
    protected override void OnStart()
    {
		this.UpdateAvatar();
	}

	void UpdateAvatar()
	{
		this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
		this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}

	public void ReturnRoleSelect()
	{
		SceneManager.Instance.LoadScene("CharSelect");
		UserService.Instance.SendUserGameLeave();
    }

	public void OnClickTest()
    {
		UITest test = UIManager.Instance.Show<UITest>();
		test.SetTitle("这是一个测试的UI标题");
        test.Onclose += Test_Onclose;
    }

    private void Test_Onclose(UIWindow sender, UIWindow.WindowResult result)
    {
		MessageBox.Show("点击了对话框的：" + result, "对话框响应结果", MessageBoxType.Information);
    }

    public void OnclickBag()
    {
        var bag = UIManager.Instance.Show<UIBag>();
        bag.transform.SetParent(this.transform);
        bag.transform.localPosition = Vector3.zero;
        bag.OnReset();
    }

    public void OnclickFriend()
    {
        var friend = UIManager.Instance.Show<UIFriend>();
        friend.transform.SetParent(this.transform);
        friend.transform.localPosition = Vector3.zero;
    }
}
