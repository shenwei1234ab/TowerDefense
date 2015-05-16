using UnityEngine;
using System.Collections;
public enum ButtonType
{
    CreateTower,
    UpdateTower,
    DestoryTower,
    Exit,
}

public class MyButton : MonoBehaviour 
{
    //对应tower的类型 
    public TowerType m_towerType;
    public ButtonType m_buttonType;
    // Use this for initialization

    public GameObject m_highLightBorder;

    void Awake()
    {

    }


    void Start()
    {
       
    }
    void Update()
    {

    }


    public void Selected()
    {
        m_highLightBorder.SetActive(true);
    }


    public void NotSelected()
    {
        m_highLightBorder.SetActive(false);
    }
 
}
