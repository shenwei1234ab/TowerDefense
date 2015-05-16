using UnityEngine;
using System.Collections;


//ui管理
public class UIManager : MonoBehaviour 
{
    private static UIManager m_instance;
    public static UIManager Instance()
    {
        return m_instance;
    }
  


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
    public UILabel m_uiLastWave;
    public float m_tweenTime = 4.0f;
    public GameObject m_uiWaringPreb;
    


    //胜利后的ui画面
    public GameObject m_uiGameComplete;



    public GameObject m_uiGameOver;

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
        m_instance = this; 
    }

   

	// Use this for initialization
	void Start () 
    {
        ////记录初始位置
         m_uiTowerButtonInitPos = m_uiTowerButtonPanel.transform.position;
        //等待动画完成开始出最后一波
         m_tweenTime = m_uiLastWave.GetComponent<TweenAlpha>().duration * 2;
	}


    IEnumerator LastWaveStart()
    {
        yield return new WaitForSeconds(m_tweenTime);
        Destroy(m_uiLastWave.gameObject);
        //开始出兵
        GameManager.GetInstance().ProductEnemy(true);
    }


    public void ShowPanel(Vector3 worldPos)
    {
        //目标点在主摄像机的位置
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(worldPos);
        viewPortPos.z = 0;
        //在UI中的位置
        Vector3 uiPos = UICamera.currentCamera.ViewportToWorldPoint(viewPortPos);
        viewPortPos.z = 0;
        //在触摸点产生UIPanel
        m_uiTowerButtonPanel.transform.position = uiPos + m_uiTowerButtonOffset;    
        m_uiTowerButtonPanel.SetActive(true);    
    }

    public void HidPanel()
    {
        m_uiTowerButtonPanel.transform.position = m_uiTowerButtonInitPos;
    }

	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public Camera m_uiCamera;
    public Vector3 UIToWorldPos(Vector3 uiPos)
    {
        Vector3 viewPortPos = m_uiCamera.WorldToViewportPoint(uiPos);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPortPos);
        return worldPos;
    }

    //ui与世界坐标的对应
    public  Vector3  WorldPosToUI(Vector3 worldPos)
    {
        //目标点在主摄像机的位置
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(worldPos);
        viewPortPos.z = 0;
        //在UI中的位置
        Vector3 uiPos = UICamera.currentCamera.ViewportToWorldPoint(viewPortPos);
        viewPortPos.z = 0;
        return uiPos;
    }

    //根据世界坐标在ui产生ui元素
    public GameObject CreateObjInUI(Vector3 worPos, GameObject preb)
    {
        GameObject newObj = (GameObject)GameObject.Instantiate(preb);
        ////添加到ui 中
        newObj.transform.parent = UICamera.currentCamera.transform;
        newObj.transform.localScale = new Vector3(1, 1, 1);
        newObj.transform.position = this.WorldPosToUI(worPos);
        return newObj;
    }


    public void CreateWarning(Vector3 worPos,string strMessage)
    {
        GameObject newObj =CreateObjInUI(worPos, m_uiWaringPreb);
        UiWarning uiWarning = newObj.GetComponent<UiWarning>();
        uiWarning.m_showMessage = strMessage;
    }

    //开始播放最后一波动画
    public void ShowLastWave()
    {
        m_uiLastWave.gameObject.SetActive(true);
        StartCoroutine("LastWaveStart");
    }



    public void ShowGameCompletePanel()
    {
        m_uiGameComplete.SetActive(true);
    }

    public void ShowGameOverPanel()
    {
        m_uiGameOver.SetActive(true);
    }
}
