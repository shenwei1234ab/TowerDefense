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


    void Awake()
    {

    }


    void Start()
    {
       
    }
    void Update()
    {

    }


 
}
