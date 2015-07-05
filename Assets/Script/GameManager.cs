using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameStatus
{
    GameStart,
    GamePause,
    GameIsFininshed,
    GameOver,
}
//游戏逻辑
public class GameManager : MonoBehaviour 
{
    [HideInInspector]
   public  GameStatus m_gameStatus;
    //particle system
    public GameObject m_BuildingEffect;
    public GameObject m_BuildingDoneEffect;
    //UI管理
    //当前的金币
    public int m_curCoins = 10;
    //与地面的碰撞层
    public LayerMask m_groundLayer;
    //基地的生命
    public int m_Life = 100;


    public GameObject m_processBarPreb;


    EnemyFactory m_enemyFactory;
    //当前选中的塔(升级或者销毁)
   // private Tower m_SelectTower= null;

    //当前要选中的图标
    //private MyButton m_SelectButton = null;

    void Awake()
    { 
        m_Instance = this;
    }

	void Start () 
    {
        Init();
	}



    void Init()
    {
        Coins = 10;
        Life = 100;
        //开始产生怪物
        GameObject enemyFactory = GameObject.Find("EnemyFactory");
        m_enemyFactory = enemyFactory.GetComponent<EnemyFactory>();
        ProductEnemy(true);
    }



    public void ProductEnemy(bool control)
    {
        m_enemyFactory.ifProductMonster = control;
    }
	
    //当前的进攻波数
   private  int m_Wave=1;
   public int CurrentWave
    {
        set
        {
            m_Wave = value;
            //在ui上显示
            UIManager.GetInstance().CurWave = m_Wave;
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
           UIManager.GetInstance().TotalWaves = m_TotalWaves;
       }
        get
       {
           return m_TotalWaves;
       }
   }

   

	// Update is called once per frame
	void Update () 
    {
        
	}

    //在场景中点击到了塔的模型
    public void HitTower(Tower hitTower,Vector3 hitPoint)
    {
        //正在建
        if(hitTower.m_towerState == TowerState.Building)
        {
            return;
        }
        //否则面板显示 
        else
        {
            UIManager.GetInstance().ShowPanel(hitPoint);  
        }
    }
    
    public void HitPlane()
    {
        
    }


    public bool CreateTower(Vector3 hitPos,TowerType towerType)
    {
        string strKey = towerType.ToString() + "1";
          //查找要创建的塔的价钱
          TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(strKey);
          int needCost = newTowerDate.m_Cost;
          //金钱不够
          if (m_curCoins < needCost)
          {
              //产生提示
              UIManager.GetInstance().CreateWarning(hitPos, "金钱不够");
              return false;
          }
          //在指定的位置创建tower;
          Tower newTower = TowerFactory.GetInstance().ProduceTower(towerType, 1, hitPos);
          //减少钱
          Coins -= needCost;
          return true;
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
        UIManager.GetInstance().Coins = m_curCoins;
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
            UIManager.GetInstance().Coins = m_curCoins;
        }
    }

    public int Life
    {
        get
        {
            return m_Life;
        }

        set
        {
            if(value<=0)
            {
                value = 0;
            }
            m_Life = value;
            UIManager.GetInstance().Life = m_Life;
            if (m_Life == 0)
            {
                GameOver();
            }
        }
        
    }

    //基地受到伤害
    public void ReduceLife(int reduceLife)
    {
        m_Life -= reduceLife;
        if (m_Life <= 0)
        {
            m_Life = 0;
        }
        // m_uiManger.Life = m_Life;
        UIManager.GetInstance().Life = m_Life;
        if (m_Life == 0)
        {
            GameOver();
        }
    }



    public bool UpdateTower(Tower selectTower)
    {
        Vector3 towerPos = selectTower.gameObject.transform.position;
        //当前是最大等级
        int curLevel = selectTower.m_curLevel;
        if (curLevel >= selectTower.m_maxLevel)
        {
            UIManager.GetInstance().CreateWarning(towerPos, "当前是最大等级");
            //ui显示
            return false;
        }
        int nextLevel = selectTower.m_curLevel + 1;
        string key = selectTower.m_towerType + nextLevel.ToString();
        //查找要创建的塔的价钱
        TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(key);
        int needCost = newTowerDate.m_Cost;
        //金钱不够
        if (m_curCoins < needCost)
        {
            //产生提示
            UIManager.GetInstance().CreateWarning(towerPos, "金钱不够");
            UIManager.GetInstance().HidPanel();
            return false;
        }
        //在指定的位置创建tower;
        Tower newTower = TowerFactory.GetInstance().ProduceTower(selectTower.m_towerType, nextLevel, towerPos);
        //减少钱
        Coins -= needCost;


        TowerFactory.GetInstance().DestoryTower(selectTower.gameObject);

        //销毁原来的塔
       // Destroy(selectTower.gameObject);
        //隐藏uipanel
        UIManager.GetInstance().HidPanel();
        return true;
    }


    public void DestoryTower(Tower selectTower)
    {
        //销毁塔
        TowerFactory.GetInstance().DestoryTower(selectTower.gameObject);
        UIManager.GetInstance().HidPanel();
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

    public void PauseGame()
    {
        if (m_gameStatus == GameStatus.GameOver || m_gameStatus == GameStatus.GameIsFininshed)
        {
            return;
        }
        Time.timeScale = 0;
        m_gameStatus = GameStatus.GamePause;
    }

    public void ResumeGame()
    {
        if (m_gameStatus == GameStatus.GamePause)
        {
            Time.timeScale = 1;
            m_gameStatus = GameStatus.GameStart;
        }
        
    }


    //基地生命为0
    public void GameOver()
    {
        m_gameStatus = GameStatus.GameOver;
        UIManager.GetInstance().ShowGameOverPanel();
    }

    //消灭了所有敌人
    public void GameComplete()
    {
        if(m_gameStatus == GameStatus.GameOver)
        {
            return;
        }
        m_gameStatus = GameStatus.GameIsFininshed;
        // ui
        UIManager.GetInstance().ShowGameCompletePanel();
    }  


    public void RestartGame()
    {
        Debug.Log("RestartGame");
        //清除场上的所有对象
        TowerFactory.GetInstance().DestoryAllTower();
        EnemyFactory.GetInstance().DestoryAllEnemy();
        PoolManager.GetInstance().DestoryAllPoolObj();
        UIManager.GetInstance().DestoryAllUIElements();
        
        //清除ui分数
       UIManager.GetInstance().HideGameOverPanel();


       //

       m_enemyFactory.Init();
        Init();
        m_gameStatus = GameStatus.GameStart;
    }



}
