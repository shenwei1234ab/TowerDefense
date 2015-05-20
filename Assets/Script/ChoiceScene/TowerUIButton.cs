using UnityEngine;
using System.Collections;

//tower图标拖动
public class TowerUIButton : UIDragDropItem 
{
    [HideInInspector]
    //public bool m_ifSelected = false;
    //对应tower的类型 
    public TowerType m_towerType;
	// Use this for initialization
	


    //拖动
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        Debug.Log(surface);
    }
}
