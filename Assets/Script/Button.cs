using UnityEngine;
using System.Collections;


public enum ButtonType
{
    CreateTower,
    UpdateTower,
    DestoryTower,
    Exit,
}
public class Button : MonoBehaviour 
{
    public ButtonType m_buttonType;
    //对应tower的名字
    public string m_TowerName;
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Debug.Log(gameObject.transform.localScale);
	}
    
    public void SetScale()
    {
        //UISprite colorSprite = button.GetComponent<UISprite>();
        //Debug.Log(colorSprite.GetComponent<UIWidget>().width);
    }

    
}
