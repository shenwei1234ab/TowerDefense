using UnityEngine;
using System.Collections;

//随着时间流逝
public class ProcessBar : MonoBehaviour 
{
    public delegate void TimeComplete(GameObject sender);
    public event TimeComplete m_timeOverEvent;
    //记时器 
    private float m_timer;
    //总时间
    public float m_buildingTime = 3.0f;

    private UISlider m_uiSlider;
	// Use this for initialization
	void Start () 
    {
        m_uiSlider = gameObject.GetComponent<UISlider>();
        m_timer = m_buildingTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(m_timer <= 0)
        {
            //到时间了,通知注册者
             m_timeOverEvent(this.gameObject);
        }
        m_timer -= Time.deltaTime;
        //设置ui
        m_uiSlider.value = m_timer / m_buildingTime;
	}
}
