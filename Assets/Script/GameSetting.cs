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

	void Start () 
    {
        m_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
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

    //start界面单击了选项
    public void OnOptionClick()
    {
        m_StartTween.PlayForward();
        m_OptionTween.PlayForward();
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
