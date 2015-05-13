﻿using UnityEngine;
using System.Collections;

//玩家按键的状态
public enum PressStatus
{
    None,
    PressCreateButton,
    PressTower,
}
//游戏逻辑
public class GameManager : MonoBehaviour 
{
    
    //particle system
    public GameObject m_BuildingEffect;
    public GameObject m_BuildingDoneEffect;
    //UI管理
    //当前的金币
    public int m_curCoins = 10;


    int nUiMask;
    int nTowerMask;
    int nTerrianMask;
    int nPlaneMask;


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

    //当前要选中的图标
    private MyButton m_SelectButton = null;

    void Awake()
    {
        m_Instance = this;
    }

	void Start () 
    {
        //获得碰撞掩码
        nTowerMask = LayerMask.GetMask("Tower");
         nTerrianMask = LayerMask.GetMask("BuildTower");
         nPlaneMask = LayerMask.GetMask("CanNotBuildTower");
         nUiMask = LayerMask.GetMask("UI");

        //开始产生怪物
         GameObject enemyFactory = GameObject.Find("EnemyFactory");
         enemyFactory.GetComponent<EnemyFactory>().ifProductMonster = true;
	}
	
    //当前的进攻波数
   private  int m_Wave=1;
   public int CurrentWave
    {
        set
        {
            m_Wave = value;
            //在ui上显示
            UIManager.Instance().CurWave = m_Wave;

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
           UIManager.Instance().TotalWaves = m_TotalWaves;
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
           if (Physics.Raycast(ray, out hit, 100, nUiMask))
           {
               Debug.Log("nUiMask");
               return;
           }

           //如果点击到了tower
           if (Physics.Raycast(ray, out hit, 100, nTowerMask))
           {
               Debug.Log("hit tower");
               m_SelectTower = hit.transform.gameObject.GetComponent<Tower>();
               //如果当前在建造就返回
               if(m_SelectTower.m_towerState == TowerState.Building )
               {
                   return;
               }
               //显示面板
               UIManager.Instance().ShowPanel(hit.point);  
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
               Debug.Log("hit terrian");
               //如果未点击
               if (!m_SelectButton)
               {
                   return;
               }
               string strKey = m_SelectButton.m_towerType.ToString() + "1";
               int hitPointX = (int)hit.point.x;
               int hitPointY = (int)hit.point.y;
               int hitPointZ = (int)hit.point.z;
               Vector3 hitPos = new Vector3(hitPointX, hitPointY, hitPointZ);
              //查找要创建的塔的价钱
               TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(strKey);
               int needCost = newTowerDate.m_Cost;
               //金钱不够
               if(m_curCoins < needCost)
               {
                   //产生提示
                   UIManager.Instance().CreateWarning(hitPos,"金钱不够");
                   m_SelectButton = null;
                   return;
               }
               //在指定的位置创建tower;
               Tower newTower = TowerFactory.GetInstance().ProduceTower(m_SelectButton.m_towerType,1,hitPos);
               m_TowerList.Add(newTower);             
               //减少钱
               Coins -= needCost;
               m_SelectButton = null;

           }
           else    
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
        //m_uiManger.Coins = m_curCoins;
        UIManager.Instance().Coins = m_curCoins;
    }



    public int Coins
    {
        get
        {
            return m_curCoins;
        }
        set
        {
            if(value < 0)
            {
                value = 0;
            }
            m_curCoins = value;
            //m_uiManger.Coins = m_curCoins;
            UIManager.Instance().Coins = m_curCoins;
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
       // m_uiManger.Life = m_Life;
        UIManager.Instance().Life = m_Life;
        if(m_Life == 0)
        {
            //GameOver;
        }
    }

    #region     回调事件
    //鼠标事件 
    public void CreateButtonOnClick(GameObject button)
    {
        //
        Debug.Log("Create ButtonClick");
        m_SelectButton = button.GetComponent<MyButton>();
        //高亮
        //////////////////////////////////////////////////////////////////////todo
    }

    public void UpdateButtonOnClick(GameObject button)
    {
        //升级塔 
        if(!m_SelectTower)
        {
            return;
        }
        Vector3 towerPos = m_SelectTower.gameObject.transform.position;
        //当前是最大等级
        int curLevel= m_SelectTower.m_curLevel;
        if (curLevel >= m_SelectTower.m_maxLevel)
        {
            UIManager.Instance().CreateWarning(towerPos, "当前是最大等级");
            //ui显示
            return;
        }
        int nextLevel = m_SelectTower.m_curLevel + 1;
        string key = m_SelectTower.m_towerType + nextLevel.ToString();
        //查找要创建的塔的价钱
        TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(key);
        int needCost = newTowerDate.m_Cost;
        //金钱不够
        if (m_curCoins < needCost)
        {
            //产生提示
            UIManager.Instance().CreateWarning(towerPos, "金钱不够");
            UIManager.Instance().HidPanel();
            return;
        }
        //在指定的位置创建tower;
        Tower newTower = TowerFactory.GetInstance().ProduceTower(m_SelectTower.m_towerType, nextLevel, towerPos);
        m_TowerList.Add(newTower);
        //减少钱
        Coins -= needCost;
        //销毁原来的塔
        Destroy(m_SelectTower.gameObject);
        //隐藏uipanel
        UIManager.Instance().HidPanel();
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
        UIManager.Instance().HidPanel();
    }

    public void ExitButtonOnClick(GameObject button)
    {
        //ui回到初始位置
        UIManager.Instance().HidPanel();
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
