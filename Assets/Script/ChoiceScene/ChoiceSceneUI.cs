using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//ChoiceScene的ui控制
public class ChoiceSceneUI : MonoBehaviour
{ 
    //MonsterIcon的最初的模板
    public GameObject m_uiMonsterIconPreb;
    
    //TowerIcon的最初的位置
    public GameObject m_uiTowerIconPreb;

    //当前选择的Tower的集合
    public List<SelectTowerUIButton> m_selectedTowerButtons;
    
    
    public UILabel m_uiLevel;
    public UILabel m_uiSpeed;
    public UILabel m_uiLife;
    public UILabel m_uiDenfense;
    public UILabel m_uiCoin;
    public UILabel m_uiDescript;


    MonsterUIButton m_selectMonster;
    TowerUIButton m_selectTower;

    private static ChoiceSceneUI m_instance;
    public static ChoiceSceneUI Instance()
    {
        return m_instance;
    }



   


    Vector3 m_initPos;
    public Vector3 IconInitPos
    {
        get
        {
            return m_initPos;
        }
        
    }
    //MonsterIcon的间隔
    public float m_gridInterval = 150.0f;


    //在指定位置clone一个Preb
    GameObject ClonePreb(GameObject preb, Vector3 pos)
    {
        GameObject newObj = (GameObject)GameObject.Instantiate(preb);
        newObj.transform.parent = preb.transform.parent;
        newObj.transform.localPosition = pos;
        newObj.SetActive(true);
        newObj.transform.localScale = new Vector3(1, 1, 1);
        return newObj;
    }


    //添加Monster图标到滚动条中
    public void AddEnemyIconToScrollView(Monster monster,Vector3 pos)
    {
        GameObject newObj = ClonePreb(m_uiMonsterIconPreb, pos);
        //设置图标的名字和Sprite
        newObj.SendMessage("SetText", monster.enemyName);
        newObj.SendMessage("SetSprite", monster.enemyIcon);
        newObj.SendMessage("SetMonster", monster);
    }


     //添加Tower图标到滚动条中
    void AddTowerIconToScrollView(GameObject button)
    {
        //GameObject newObj = ClonePreb(button,)
    }




    void Awake()
    {
        m_instance = this;
        m_selectMonster = null;
    }
	// Use this for initialization
	void Start () 
    {
        m_initPos = m_uiMonsterIconPreb.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ShowMonsterInfo(Monster monster)
    {
        m_uiCoin.text = monster.enemyCoin.ToString();
        m_uiDenfense.text = monster.enemyDefense.ToString();
        m_uiDescript.text = monster.enemyDesciption;
        m_uiLevel.text = monster.enemyLevel.ToString();
        m_uiLife.text = monster.enemyLife.ToString();
        m_uiSpeed.text = monster.enemySpeed.ToString();
    }


    public void RegistUIEvent(UiButton button)
    {
        //判断button的类型
        //MonsterUIButton monsterButton = button as MonsterUIButton;
        if (button as MonsterUIButton)
        {
            UIEventListener.Get(button.gameObject).onClick = MonsterButtonClickOn;
        }
        else if (button as NormalUIButton)
        {
            UIEventListener.Get(button.gameObject).onClick = NormalButtonClickOn;
        }
        //else if (button as TowerUIButton)
        //{
        //    UIEventListener.Get(button.gameObject).onClick = TowerButtonClickOn;
        //}
    }

    //点击到了某个怪兽的图标
    void MonsterButtonClickOn(GameObject button)
    {
        if (m_selectMonster != null)
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
        switch (uiButton.m_uiButtonType)
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
