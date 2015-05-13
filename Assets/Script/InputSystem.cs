using UnityEngine;
using System.Collections;


public enum MousePosition
{
    InUI,
    InGame
}
//处理玩家的输入操作
public class InputSystem : MonoBehaviour 
{

    private static InputSystem m_instance;
    public static InputSystem Instance()
    {
        return m_instance;
    }
    MousePosition m_curMousePos;
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
           
        }
    }
	// Use this for initialization
	void Start () 
    {
        m_curMousePos = MousePosition.InGame;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //获取玩家输入
        if(Input.GetMouseButtonDown(0))
        {
            if(m_curMousePos == MousePosition.InGame)
            {

            }
            else
            {

            }
            
        }
	
	}

    void CreateButtonOnClick(GameObject gameobj)
    {

    }

    void UpdateButtonOnClick(GameObject gameobj)
    {

    }
  
   void DestoryButtonOnClick(GameObject gameobj)
    {

    }


   void  ExitButtonOnClick(GameObject gameobj)
   {

   }
}
