using UnityEngine;
using System.Collections;



//处理玩家的输入操作
public class InputSystem : MonoBehaviour 
{
    //当前选中的tower
    Tower m_selectTower;
    [HideInInspector]
    //当前要选中的图标
    public UITowerButton m_selectButton = null;
    [HideInInspector]
    //public MyButton m_hoverButton = null;
    enum MousePosition
    {
        HoverInUI,
        HoverInScene,
    }
    MousePosition m_mousePos;


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
    }


    //注册事件
    public void RegistUIEvent(UiButton button)
    {
        //如果是创建TowerButton
        if(button as UITowerButton)
        {
            UIEventListener.Get(button.gameObject).onClick = CreateButtonOnClick;
        }
        else if(button as UINormalButton)
        {
            UINormalButton normalButton = button.GetComponent<UINormalButton>();
            //判断button的类型
            switch (normalButton.m_buttonType)
            {
                case ButtonType.UpdateTower:
                    UIEventListener.Get(button.gameObject).onClick = UpdateButtonOnClick;
                    break;
                case ButtonType.DestoryTower:
                    UIEventListener.Get(button.gameObject).onClick = DestoryButtonOnClick;
                    break;
                case ButtonType.Exit:
                    UIEventListener.Get(button.gameObject).onClick = ExitButtonOnClick;
                    break;
                case ButtonType.NextScene:
                    UIEventListener.Get(button.gameObject).onClick = NextSceneOnClick;
                    break;
                case ButtonType.BackToPrepare:
                    UIEventListener.Get(button.gameObject).onClick = BackToPrepareOnClick;
                    break;
                case ButtonType.BackToChoice:
                    UIEventListener.Get(button.gameObject).onClick = BackToChoiceOnClick;
                    break;
                case ButtonType.PauseGame:
                    UIEventListener.Get(button.gameObject).onClick = PauseGame;
                    break;
                case ButtonType.ResumeGame:
                    UIEventListener.Get(button.gameObject).onClick = ResumeGame;
                    break;
            }
        }
        UIEventListener.Get(button.gameObject).onHover = UIButtonOnHover;
    }




	// Use this for initialization
	void Start () 
    {
       // m_pressStatus = PressStatus.None;
        m_mousePos = MousePosition.HoverInScene;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (m_mousePos == MousePosition.HoverInUI)
            {
                //在ui中
                return;
            }
            else
            {
                if (GameManager.GetInstance().m_gameStatus !=GameStatus.GameStart)
                {
                    return;
                }
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
                            //如果之前点击了UIcreateButton
                            if(m_selectButton)
                            {
                                m_selectButton.NotSelected();
                                m_selectButton = null;
                            }
                            //开启升级,销毁面板
                            m_selectTower = hit.transform.gameObject.GetComponent<Tower>();
                            GameManager.GetInstance().HitTower(m_selectTower, hit.point);
                            return;
                        }
                        //如果点击到了plane
                        else if(tag.CompareTo("Plane") ==0)
                        {
                            if(m_selectTower)
                            {
                                UIManager.Instance().HidPanel();
                                m_selectTower = null;
                            }
                            //不能建造塔 
                            return;
                        }
                        else if(tag.CompareTo("Terrian") == 0)
                        {                          
                            if (m_selectTower )
                            {
                                UIManager.Instance().HidPanel();
                                m_selectTower = null;
                            }
                            if (!m_selectButton)
                            {
                                return;
                            }
                            //选中了就建造塔
                            int hitPointX = (int)hit.point.x;
                            int hitPointY = (int)hit.point.y;
                            int hitPointZ = (int)hit.point.z;
                            Vector3 hitPos = new Vector3(hitPointX, hitPointY, hitPointZ);
                            GameManager.GetInstance().CreateTower(hitPos, m_selectButton.m_towerType);
                            m_selectButton.NotSelected();
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
        if(GameManager.GetInstance().m_gameStatus !=GameStatus.GameStart)
        {
            return;
        }
        //之前已经选中了
        if(m_selectButton)
        {
            m_selectButton.NotSelected();
            m_selectButton = null;
        }
        m_selectButton = button.GetComponent<UITowerButton>();
        m_selectButton.Selected();
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
    void BackToPrepareOnClick(GameObject button)
    {

        Application.LoadLevel("StagePrepare");
    }
    void BackToChoiceOnClick(GameObject button)
    {

        Application.LoadLevel("StageChoice");
    }



    void UIButtonOnHover(GameObject button, bool state)
   {
       if(state)
       {
           m_mousePos = MousePosition.HoverInUI;
       }
       else
       {
           m_mousePos = MousePosition.HoverInScene;
       }
   }





    void NextSceneOnClick(GameObject button)
    {
        //
        Application.LoadLevel("StagePrepare");

    }

    void PauseGame(GameObject button)
    {
        GameManager.GetInstance().PauseGame();
    }


    void ResumeGame(GameObject button)
    {
        GameManager.GetInstance().ResumeGame();
    }
}

