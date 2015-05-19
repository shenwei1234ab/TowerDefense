using UnityEngine;
using System.Collections;

public class ChoiceSceneInputSystem : MonoBehaviour 
{
    MonsterUIButton m_selectMonster;
    public static  ChoiceSceneInputSystem Instance()
    {
        return m_instance;
    }

    static ChoiceSceneInputSystem m_instance;
	// Use this for initialization

    void Awake()
    {
        m_instance = this;
        m_selectMonster = null;
    }
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void RegistUIEvent(UiButton button)
    {
        //判断button的类型
        //MonsterUIButton monsterButton = button as MonsterUIButton;
        if (button as MonsterUIButton)
        {
            UIEventListener.Get(button.gameObject).onClick = MonsterButtonClickOn;          
        }
        else if(button as NormalUIButton)
        {
            UIEventListener.Get(button.gameObject).onClick = NormalButtonClickOn;       
        }
    }

    //点击到了某个怪兽的图标
    void MonsterButtonClickOn(GameObject button)
    {
        if(m_selectMonster !=null)
        {
            m_selectMonster.NotSelected();
        }
        MonsterUIButton monsterButton = button.GetComponent<MonsterUIButton>();
        ChoiceSceneUI.Instance().ShowMonsterInfo(monsterButton.m_monster);
        m_selectMonster = monsterButton;
        m_selectMonster.Selected();
    }

    //点击的普通UI图标
    void NormalButtonClickOn(GameObject button)
    {
        NormalUIButton uiButton = button.GetComponent<NormalUIButton>();
        switch(uiButton.m_uiButtonType)
        {
            case NormalUIButtonType.OK:
                Application.LoadLevel("Scene1");
                break;
            case NormalUIButtonType.BackToMain:
                Debug.Log("BackToMain");
                break;
        }
    }


    void HeroButtonClickOn(GameObject button)
    {

    }

    void TowerButtonClickOn(GameObject button)
    {

    }
}
