using UnityEngine;
using System.Collections;


//游戏逻辑
public class GameManager : MonoBehaviour 
{
    //particle system
    public GameObject m_BuildingEffect;
    public GameObject m_BuildingDoneEffect;


    //UI管理
    public GameObject m_ui;
    private UIManager m_uiManger;
    //ui的升级面板
    public GameObject m_uiTowerButtonPanel;
    
    
    //升级面板离触摸点的偏移距离
    public Vector3 m_uiTowerButtonOffset;
    //初始位置
    private Vector3 m_uiTowerButtonInitPos;

    //当前的金币
    public int m_curCoins = 10;
    int nTowerMask;
    int nTerrianMask;
    int nPlaneMask;
    //是否选中了图标
    private bool m_ifSelected = false;
    private string m_SelectTowerName;

    //与地面的碰撞层
    public LayerMask m_groundLayer;
    //基地的生命
    int m_Life = 10;
    //当前所有的敌人容器
    public ArrayList m_EnemyList = new ArrayList();
	
   //当前所有的tower容器
    public ArrayList m_TowerList = new ArrayList();


    public GameObject m_processBarPreb;


    //当前选中的塔(升级或者销毁)
    private Tower m_SelectTower= null;

    void Awake()
    {
        m_Instance = this;
        //注册所有button点击事件
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject button in buttons)
        {
            //判断button的类型
            switch (button.GetComponent<MyButton>().m_buttonType)
            {
                case ButtonType.CreateTower:
                    UIEventListener.Get(button).onClick = CreateButtonOnClick;
                    break;
                case ButtonType.UpdateTower:
                    UIEventListener.Get(button).onClick = UpdateButtonOnClick;
                    break;
                case ButtonType.DestoryTower:
                    UIEventListener.Get(button).onClick = DestoryButtonOnClick;
                    break;
                case ButtonType.Exit:
                    UIEventListener.Get(button).onClick = ExitButtonOnClick;
                    //什么事都不做
                    break;
            }
        }
          //获取ui控件
        m_uiManger = m_ui.GetComponent<UIManager>(); 
    }

	void Start () 
    {
        //获得碰撞掩码
        nTowerMask = LayerMask.GetMask("Tower");
         nTerrianMask = LayerMask.GetMask("BuildTower");
         nPlaneMask = LayerMask.GetMask("CanNotBuildTower");
        //开始产生怪物
         GameObject enemyFactory = GameObject.Find("EnemyFactory");
         enemyFactory.GetComponent<EnemyFactory>().ifProductMonster = true;
        //记录初始位置
         m_uiTowerButtonInitPos = m_uiTowerButtonPanel.transform.position;
	}
	
    //当前的进攻波数
   private  int m_Wave=1;
   public int CurrentWave
    {
        set
        {
            m_Wave = value;
            //在ui上显示
            //m_uiWave.text = m_Wave.ToString();
            m_uiManger.CurWave = m_Wave;

        }
        get
        {
            return m_Wave;
        }
    }

   //总共的波数
   private int m_TotalWaves = 1;
    public int nTotalWaves
   {
       set
       {
           m_TotalWaves = value;
           //在ui上显示
           m_uiManger.TotalWaves = m_TotalWaves;
       }
        get
       {
           return m_TotalWaves;
       }
   }





	// Update is called once per frame
	void Update () 
    {
        //鼠标事件
       if(m_Life<=0)
       {
           return;
       }
        //鼠标按下
       if(Input.GetMouseButtonDown(0))
       {
           //判断点击到了哪里
           Vector3 mousePos = Input.mousePosition;
           //创建一条从摄像机射出的射线
           Ray ray = Camera.main.ScreenPointToRay(mousePos);
           //计算射线与地面的碰撞
           RaycastHit hit;
           //如果点击到了tower
           if (Physics.Raycast(ray, out hit, 100, nTowerMask))
           {
               m_SelectTower = hit.transform.gameObject.GetComponent<Tower>();
               //如果当前在建造就返回
               if(m_SelectTower.m_towerState == TowerState.Building )
               {
                   return;
               }
               //目标点在主摄像机的位置
               Vector3 viewPortPos = Camera.main.WorldToViewportPoint(hit.point);
               viewPortPos.z = 0;
               //在UI中的位置
               Vector3 uiPos = UICamera.currentCamera.ViewportToWorldPoint(viewPortPos);
               viewPortPos.z = 0;
               //在触摸点产生UIPanel
               m_uiTowerButtonPanel.transform.position = uiPos + m_uiTowerButtonOffset;    
               m_uiTowerButtonPanel.SetActive(true);     
           }
           //如果点击到了plane
           else if (Physics.Raycast(ray, out hit, 100, nPlaneMask))
           {
               Debug.Log("hit Plane");
               //不能造房子
               return;
           }
           //如果点击到了terrian
           else if (Physics.Raycast(ray, out hit, 100, nTerrianMask))
           {
               //如果未点击创建的图标或者金钱不够
               if (!m_ifSelected)
               {
                   return;
               }
               int hitPointX = (int)hit.point.x;
               int hitPointY = (int)hit.point.y;
               int hitPointZ = (int)hit.point.z;
               //在指定的位置创建tower;
               string strKey = m_SelectTowerName + "1";
               Tower newTower = TowerFactory.GetInstance().ProduceTower(strKey, new Vector3(hitPointX, hitPointY, hitPointZ));
               m_TowerList.Add(newTower);
               m_ifSelected = false;

           }
           else    //点击到了ui
           {
               Debug.Log("hitOther");
               return;
           }
       }
	}


    //在指定位置创建粒子动画
    GameObject CreateParticleSystem(GameObject particlePreb,Vector3 crePos)
    {
        GameObject particleSystem = (GameObject)GameObject.Instantiate(particlePreb, crePos,Quaternion.identity);
        return particleSystem;
    }


    private static GameManager m_Instance;
    public static GameManager GetInstance()
    {
        return m_Instance;
    }


    public void AddCoin(int coins)
    {
        m_curCoins += coins;
        //ui
        m_uiManger.Coins = m_curCoins;
    }
    public int Coins
    {
        get
        {
            return m_curCoins;
        }
    }


    //基地受到伤害
    public void ReduceLife(int reduceLife)
    {
        m_Life -= reduceLife;
        if(m_Life <=0)
        {
            m_Life = 0;
        }
        //ui
        //m_uiLife.text = m_Life.ToString();
        m_uiManger.Life = m_Life;
        if(m_Life == 0)
        {
            //GameOver;
        }
    }

    #region     回调事件
    //鼠标事件 
    public void CreateButtonOnClick(GameObject button)
    {
        MyButton pressButton = button.GetComponent<MyButton>();
        m_SelectTowerName = pressButton.m_towerType.ToString();
        //高亮
        //////////////////////////////////////////////////////////////////////todo
        m_ifSelected = true;
    }

    public void UpdateButtonOnClick(GameObject button)
    {
        //升级塔 
        if(!m_SelectTower)
        {
            return;
        }
        //当前是最大等级
        int curLevel= m_SelectTower.m_curLevel;
        if (curLevel >= m_SelectTower.m_maxLevel)
        {
            Debug.Log("当前是最大等级");
            //ui显示
            return;
        }
        int nextLevel = m_SelectTower.m_curLevel + 1;
        //获得下一等级Tower对象
        TowerData newData = DataBase.GetInstance().m_TowerDatas[m_SelectTower.m_towerName + nextLevel.ToString()];
        if(newData!=null)
        {
            Debug.LogError("没有");
            return;
        }
        //如果金钱不够
        if (newData.m_Cost > Coins)
        {
            Debug.LogError("金钱不够");
            return;
        }
        //删除原来的模型
        Destroy(m_SelectTower);
        //在指定的位置创建新的tower

    }


    public void DestoryButtonOnClick(GameObject button)
    {
        if (!m_SelectTower)
        {
            return;
        }
        //销毁塔
        Destroy(m_SelectTower.gameObject);
        m_SelectTower = null;
        //ui回到初始位置
        m_uiTowerButtonPanel.transform.position = m_uiTowerButtonInitPos;
    }

    public void ExitButtonOnClick(GameObject button)
    {
        //ui回到初始位置
        m_uiTowerButtonPanel.transform.position = m_uiTowerButtonInitPos;
    }


    //建造粒子动画完成后执行的动作 
    //void BuildingEffectCompleteEvent(GameObject obj)
    //{
    //    //销毁自己并创建完成完成动画和注册事件
    //    Destroy(obj);
    //    GameObject particleSystem = (GameObject)GameObject.Instantiate(m_BuildingDoneEffect, m_bulidPos, Quaternion.identity);
    //    particleSystem.GetComponent<ParticleSystemControl>().m_particleCompleteEvent += BuildingDoneEffectCompleteEvent;
    //}
    //void BuildingDoneEffectCompleteEvent(GameObject obj)
    //{
    //    Destroy(obj);
    //    //在指定的位置创建tower;
    //    string strKey = m_SelectTowerName + "1";
    //    Tower newTower = TowerFactory.GetInstance().ProduceTower(strKey, m_bulidPos);
    //    m_TowerList.Add(newTower);
    //    m_ifSelected = false;
    //}
    #endregion


   
}
