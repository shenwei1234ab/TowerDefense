using UnityEngine;
using System.Collections;


//ui管理
public class UIManager : MonoBehaviour 
{
    //ui的升级面板
    public GameObject m_uiTowerButtonPanel;
    //升级面板离触摸点的偏移距离
    public Vector3 m_uiTowerButtonOffset;
    //初始位置
    private Vector3 m_uiTowerButtonInitPos;


    //ui
    public UILabel m_uiWave;
    public UILabel m_uiTotalWave;

    public UILabel m_uiLife;
    public UILabel m_uiCoin;

    public int CurWave
    {
        set
        {
            m_uiWave.text = value.ToString();
        }
    }

    public int TotalWaves
    {
        set
        {
            m_uiTotalWave.text = value.ToString();
        }
    }
    public int Coins
    {
        set
        {
            m_uiCoin.text = value.ToString();
        }
    }

    public int Life
    {
        set
        {
            m_uiLife.text = value.ToString();
        }
    }

    void Awake()
    {

  
    }

   

	// Use this for initialization
	void Start () 
    {
	
	}



	
	// Update is called once per frame
	void Update () 
    {
	
	}


    public static Vector3  WorldPosToUI(Vector3 worldPos)
    {
        //目标点在主摄像机的位置
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(worldPos);
        viewPortPos.z = 0;
        //在UI中的位置
        Vector3 uiPos = UICamera.currentCamera.ViewportToWorldPoint(viewPortPos);
        viewPortPos.z = 0;
        return uiPos;
    }
}
