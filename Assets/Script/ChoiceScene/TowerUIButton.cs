using UnityEngine;
using System.Collections;

//tower图标拖动
public class TowerUIButton : UiButton 
{
    [HideInInspector]
    public bool m_ifSelected = false;
    public TowerType m_towerType;
	// Use this for initialization
    protected void Start()
    {
        //向ChoiceScene注册事件
        ChoiceSceneUI.Instance().RegistUIEvent(this);
    }
}
