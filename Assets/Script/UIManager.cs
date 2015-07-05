using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ui管理
public class UIManager : MonoBehaviour 
{


    public GameObject m_towerButtoniconPreb;
    public float m_towerButtoniconAnchor = -67.0f;
    private static UIManager m_instance;
    public static UIManager GetInstance()
    {
        return m_instance;
    }


    public Dictionary<string, GameObject> m_uiElements = new Dictionary<string, GameObject>();


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
        //读取ChoiceScene选择的towertype 并生成相应的uibutton
         int index = 1;
        Vector3 initPos = m_towerButtoniconPreb.transform.localPosition;
        foreach( TowerType type in DataBase.GetInstance().m_selectTowerTypes)
        {
            //
            GameObject newTowerIcon =(GameObject) GameObject.Instantiate(m_towerButtoniconPreb);
            newTowerIcon.transform.parent = m_towerButtoniconPreb.transform.parent;
            newTowerIcon.transform.localScale = new Vector3(1, 1, 1);
            newTowerIcon.transform.localPosition = initPos + new Vector3(0, (index - 1) * m_towerButtoniconAnchor, 0);
            newTowerIcon.GetComponent<UITowerButton>().TowerType = type;
            newTowerIcon.SetActive(true);
            index++;
        }
	}



    IEnumerator LastWaveStart()
    {
        yield return new WaitForSeconds(m_tweenTime);
        //Destroy(m_uiLastWave.gameObject);
        m_uiLastWave.gameObject.SetActive(false);
        //开始出兵
        GameManager.GetInstance().ProductEnemy(true);
    }

    //存放血条预设的文件位置
    public string m_lifePrebFilePath = "Assets/Prefab/LifeBar.prefab";


    uint m_uiElementId = 0;
    //添加血条
    public void AddLifeBar(Enemy enemy)
    {
        //从Prefab文件中载入血条的预设
        GameObject lifeBarPreb = Resources.LoadAssetAtPath<GameObject>(m_lifePrebFilePath);
       //Camera uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
        //在uicamera中生成
       GameObject newLifeBar = (GameObject)GameObject.Instantiate(lifeBarPreb);
        newLifeBar.transform.parent = m_uiCamera.transform;
        newLifeBar.transform.localScale = new Vector3(1, 1, 1);
        newLifeBar.name = newLifeBar.name + m_uiElementId.ToString();
        
        MyUIFollowTarget uiTarget = newLifeBar.GetComponent<MyUIFollowTarget>();
        uiTarget.m_targetTransform = enemy.m_LifeBarPoint.transform;
        //获取血量
        enemy.m_lifeSlider = newLifeBar.GetComponent<UISlider>();

        m_uiElements.Add(newLifeBar.name, newLifeBar);
        m_uiElementId++;
    }


    public void DestoryUIElements(GameObject uiElementObj)
    {
        m_uiElements.Remove(uiElementObj.name);
        Destroy(uiElementObj);
    }


    public void DestoryAllUIElements()
    {
        foreach (KeyValuePair<string, GameObject> element in m_uiElements)
        {
            Destroy(element.Value);
        }
        m_uiElements.Clear();
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

    public void HideGameOverPanel()
    {
        m_uiGameOver.SetActive(false);
    }


}
