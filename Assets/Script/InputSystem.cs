using UnityEngine;
using System.Collections;



//处理玩家的输入操作
public class InputSystem : MonoBehaviour 
{
   
   [HideInInspector]
    Tower m_selectTower;
    //当前要选中的图标
    public MyButton m_selectButton = null;
    [HideInInspector]
    public MyButton m_hoverButton = null;

    private static InputSystem m_instance;
    public static InputSystem Instance()
    {
        return m_instance;
    }
    int nUiMask;
    int nTowerMask;
    int nTerrianMask;
    int nPlaneMask;

    void Awake()
    {
        m_instance = this;
        ////注册所有ui事件
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {     
            //判断button的类型
            switch (button.GetComponent<MyButton>().m_buttonType)
            {
                case ButtonType.CreateTower:
                    UIEventListener.Get(button).onClick = CreateButtonOnClick;
                    break;
                case ButtonType.UpdateTower:
                    UIEventListener.Get(button).onClick = UpdateButtonOnClick;
                    break;
                case ButtonType.DestoryTower:
                    UIEventListener.Get(button).onClick = DestoryButtonOnClick;
                    break;
                case ButtonType.Exit:
                    UIEventListener.Get(button).onClick = ExitButtonOnClick;
                    //什么事都不做
                    break;
            }
            UIEventListener.Get(button).onHover = UIButtonOnHover;
        }

    }
	// Use this for initialization
	void Start () 
    {
      
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (m_hoverButton)
            {
                //在ui中
                return;
            }
            else
            {
                //如果点击事件在场景中
                Vector3 mousePos = Input.mousePosition;
                    //创建一条从摄像机射出的射线
                    Ray ray = Camera.main.ScreenPointToRay(mousePos);
                    //计算射线与地面的碰撞
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        string tag = hit.collider.gameObject.tag;
                        //如果点击到了tower
                        if(tag.CompareTo("Tower") == 0)
                        {
                            if(m_selectButton != null)
                            {
                                m_selectButton = null;
                            }
                            m_selectTower = hit.transform.gameObject.GetComponent<Tower>();
                            GameManager.GetInstance().HitTower(m_selectTower, hit.point);
                            return;
                        }
                        //如果点击到了plane
                        else if(tag.CompareTo("Plane") ==0)
                        {
                            //不能建造塔 
                            return;
                        }
                        else if(tag.CompareTo("Terrian") == 0)
                        {
                            if(!m_selectButton)
                            {
                                //没有选中createbutton
                                return;
                            }
                            //选中了就建造塔
                            int hitPointX = (int)hit.point.x;
                            int hitPointY = (int)hit.point.y;
                            int hitPointZ = (int)hit.point.z;
                            Vector3 hitPos = new Vector3(hitPointX, hitPointY, hitPointZ);
                            GameManager.GetInstance().CreateTower(hitPos, m_selectButton.m_towerType);
                            m_selectButton = null;
                            return;
                        }
                        else
                        {
                            Debug.Log("hit other");
                        }
                    }
                    
            }
        }
	}



    //点击到了ui
    void CreateButtonOnClick(GameObject button)
    {
        m_selectButton = button.GetComponent<MyButton>();
    }

    void UpdateButtonOnClick(GameObject button)
    {
        //升级塔 
        if (!m_selectTower)
        {
            return;
        }
        GameManager.GetInstance().UpdateTower(m_selectTower);
    }


    void DestoryButtonOnClick(GameObject button)
    {
        if (!m_selectTower)
        {
            return;
        }
        GameManager.GetInstance().DestoryTower(m_selectTower);
        m_selectTower = null;
    }


    void ExitButtonOnClick(GameObject button)
    {
        //ui回到初始位置
        UIManager.Instance().HidPanel();
    }


    void UIButtonOnHover(GameObject button, bool state)
   {
       if(state)
       {
           m_hoverButton = button.GetComponent<MyButton>();     
       }
       else
       {
           m_hoverButton = null;
       }
   }
}
