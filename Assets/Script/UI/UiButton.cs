using UnityEngine;
using System.Collections;

public class UiButton : MonoBehaviour 
{
    public GameObject m_highLightBorder;
	// Use this for initialization
	protected void Start() 
    {
        
	}
	
	// Update is called once per frame
	protected void Update () 
    {
	
	}

    public void Selected()
    {
        if(m_highLightBorder)
        {
            m_highLightBorder.SetActive(true);
        }
       
    }


    public void NotSelected()
    {
        if(m_highLightBorder)
        {
            m_highLightBorder.SetActive(false);
        }
        
    }
}
