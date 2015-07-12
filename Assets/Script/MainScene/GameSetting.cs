using UnityEngine;
using System.Collections;

//游戏难度
public enum GameDifficulty
{
    EASY,
    HARD,
    HELL
}
 //全局控制
public class GameSetting : MonoBehaviour {

	//游戏选项
    public bool m_IfWindows = true;
    public GameDifficulty m_Difficulty = GameDifficulty.EASY;
    public float m_fVolumn = 1;


    //动画脚本
    public TweenPosition m_StartTween;
    public TweenPosition m_OptionTween;

    //记录当前操作的模块
    GameObject m_CurrenModules;

    public GameObject m_Buttons;
    
    //输入模块
    public GameObject m_LoginModule;
    
    //注册模块
    public GameObject m_RegisterModule;



	void Start () 
    {
        m_instance = this;
        m_CurrenModules = m_Buttons;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    private static GameSetting m_instance;
    public static GameSetting Instance
    {
        get
        {
            return m_instance;
        }
    }


    public void OnVolumnChanged()
    {
        m_fVolumn = UISlider.current.value;
    }

    public void OnDifficultyChanged()
    {
        switch(UIPopupList.current.value.Trim())
        {
            case "简单":
                m_Difficulty = GameDifficulty.EASY;
                break;
            case "困难":
                m_Difficulty = GameDifficulty.HARD;
                break;
            case "地狱":
                m_Difficulty = GameDifficulty.HELL;
                break;
        }
    }

    public void OnWindowsChanged()
    {
        m_IfWindows = UIToggle.current.value;
    }



    public void OnLoginClick()
    {
        m_CurrenModules.SetActive(false);
        //出现输入用户名和密码的输入框
        m_LoginModule.SetActive(true);
        m_CurrenModules = m_LoginModule;
    }


    //提交登陆信息
    public void PostLogin()
    {
        //判断判断用户名和密码是否正确
        Protocal.HTTP_RequestLogin reqLogin = new Protocal.HTTP_RequestLogin("shenwei", "645127861");
        Protocal.HTTP_ResponseLogin resLogin = new Protocal.HTTP_ResponseLogin();
        //设置request对象和response对象和得到http请求的回调函数
        NetWorkManager.GetInstance().Login(reqLogin, resLogin, this.LoginHandler);
    }

    void LoginHandler(object getDate)
    {
        Protocal.HTTP_ResponseLogin resDate = (Protocal.HTTP_ResponseLogin)getDate;
        Debug.Log(resDate.StateCode);
        Debug.Log(resDate.LogMessage);
        Debug.Log(resDate.UserID);
    }

    public void LoginCancel()
    {
        m_CurrenModules.SetActive(false);
        m_Buttons.SetActive(true);
        m_CurrenModules = m_Buttons;
    }


    //start界面单击了选项
    public void OnOptionClick()
    {
        m_StartTween.PlayForward();
        m_OptionTween.PlayForward();
    }

    /// <summary>
    /// 点击注册用户
    /// </summary>
    public void OnRegisterClick()
    {
        m_CurrenModules.SetActive(false);
        m_RegisterModule.SetActive(true);
        m_CurrenModules = m_RegisterModule;
    }

    /// <summary>
    /// 提交注册用户
    /// </summary>
    public void PostRegister()
    {
        //两次输入是否一致


    }

    public void RegisterCancel()
    {
        m_CurrenModules.SetActive(false);
        m_LoginModule.SetActive(true);
        m_CurrenModules = m_LoginModule;
    }





    public void OnExitClick()
    {
        Debug.Log("离开游戏");
    }



    //option界面完成了选项的调整
    public void OnFinishOption()
    {
        m_OptionTween.PlayReverse();
        m_StartTween.PlayReverse();
    }





    //开始游戏
    public void StartGame()
    {
        Application.LoadLevel("StageChoice");
    }

    
}
