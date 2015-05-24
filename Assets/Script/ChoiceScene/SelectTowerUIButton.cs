using UnityEngine;
using System.Collections;

public class SelectTowerUIButton : TowerUIButton 
{
    public GameObject m_spriteObj;

    public bool Selected
    {
        get
        {
            return m_ifSelected;
        }
    }


    //是否有图标
     bool m_ifSelected = false;
	// Use this for initialization
	protected void Start () 
    {
        m_towerType = TowerType.None;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void SetSprite(TowerType towerType)
    {
        string spriteName = towerType.ToString();
        m_spriteObj.SetActive(true);
        m_spriteObj.GetComponent<UISprite>().spriteName = spriteName + "1";
        m_ifSelected = true;
        m_towerType = towerType;
    }
   

    public void DeleteSprite()
    {
        m_spriteObj.SetActive(false);
        m_ifSelected = false;
        m_towerType = TowerType.None;
    }

    
}
