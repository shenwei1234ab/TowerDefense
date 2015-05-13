using UnityEngine;
using System.Collections;


//使用Ngui hudtext显示错误信息
public class UiWarning : MonoBehaviour 
{
    //要显示的内容
    public string m_showMessage;
    //淡入淡出的时间
    public float m_showTime = 1f;
    private HUDText m_hudText;
	// Use this for initialization
	void Start () 
    {
        m_hudText = gameObject.GetComponent<HUDText>();
        //m_showMessage = "金钱不够";
       
      
        m_hudText.Add(m_showMessage, Color.red, m_showTime);
        //开启协程
        StartCoroutine("Destory");
	}


    IEnumerator Destory()
    {
        //当过了showTime+淡出时间就删除
        yield return new WaitForSeconds(m_showTime+2.0f);
        Destroy(this.gameObject);
    }

	// Update is called once per frame
	void Update () 
    {
        
	}
}
