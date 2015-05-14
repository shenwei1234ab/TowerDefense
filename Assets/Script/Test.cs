using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
    
	// Use this for initialization
	void Start () 
    {
	     UIEventListener.Get(this.gameObject).onClick = OnClick;
        UIEventListener.Get(this.gameObject).onHover = OnButtonHoverOn;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnClick(GameObject obj)
    {
        Debug.Log("OnClick");
    }

    void OnButtonHoverOn(GameObject obj, bool state)
    {
        Debug.Log("Hover" + obj);
        Debug.Log("state" + state);
    }
}
