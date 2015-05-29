using UnityEngine;
using System.Collections;

//创建塔的uiButton
public class UITowerButton : UiButton
{
    //public GameObject m_anchorObj;
    //对应tower的类型 
    public TowerType m_towerType;
    

    // Use this for initialization


    void Awake()
     {
         
     }
    void Start()
    {
        //像inputSystem注册事件
        InputSystem.Instance().RegistUIEvent(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //设置uitowerbutton的类型并设置相应的sprite
    public TowerType TowerType
    {
        get
        {
            return m_towerType;
        }

        set
        {
            //设置towertype并且设置sprite
            m_towerType = value;
           gameObject.GetComponent<UISprite>().spriteName = m_towerType.ToString() + "1";
        }
    }


}
