using UnityEngine;
using System.Collections;

//ChoiceScene的ui控制
public class ChoiceSceneUI : MonoBehaviour 
{

    public UILabel m_uiLevel;
    public UILabel m_uiSpeed;
    public UILabel m_uiLife;
    public UILabel m_uiDenfense;
    public UILabel m_uiCoin;
    public UILabel m_uiDescript;



    private static ChoiceSceneUI m_instance;
    public static ChoiceSceneUI Instance()
    {
        return m_instance;
    }



    //MonsterIcon的最初的位置
    public GameObject m_uiIconPreb;


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



    //添加Monster图标到滚动条中
    //添加敌人的图标
    public void AddEnemyIconToScrollView(Monster monster,Vector3 pos)
    {
            //Vector3 pos = m_initPos + new Vector3((index - 1) * m_gridInterval, 0, 0);
            GameObject newObj = (GameObject)GameObject.Instantiate(m_uiIconPreb);
            newObj.transform.parent = m_uiIconPreb.transform.parent; newObj.transform.localPosition = pos;
            newObj.SetActive(true);
            newObj.transform.localScale = new Vector3(1, 1, 1);
           
            //设置图标的名字和Sprite
            newObj.SendMessage("SetText", monster.enemyName);
            newObj.SendMessage("SetSprite", monster.enemyIcon);
            newObj.SendMessage("SetMonster", monster);
    }



    void Awake()
    {
        m_instance = this;
    }
	// Use this for initialization
	void Start () 
    {
        m_initPos = m_uiIconPreb.transform.localPosition;
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
}
