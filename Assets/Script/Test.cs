using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
    BoxCollider m_box;
	// Use this for initialization
	void Start () 
    {
        m_box = gameObject.GetComponent<BoxCollider>();
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetMouseButtonDown(0))
        {
            m_box.enabled = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            m_box.enabled = true;
        }
	}
   
}
